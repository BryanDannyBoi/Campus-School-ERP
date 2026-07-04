using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Domain.Entities;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SubjectsController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public SubjectsController(SchoolERPDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Guid? classId)
    {
        var q = _context.Subjects
            .Include(s => s.Class)
            .Include(s => s.SubjectTeachers).ThenInclude(st => st.Teacher).ThenInclude(t => t!.User)
            .AsQueryable();

        if (classId.HasValue) q = q.Where(s => s.ClassId == classId);

        var subjects = await q.OrderBy(s => s.Name).ToListAsync();
        var dtos = subjects.Select(s => new SubjectDto
        {
            Id = s.Id, Name = s.Name, Code = s.Code, Credits = s.Credits,
            ClassName = s.Class?.Name,
            Teachers = s.SubjectTeachers.Select(st => st.Teacher?.User?.FullName ?? "").ToList()
        });

        return Ok(ApiResponse<IEnumerable<SubjectDto>>.Ok(dtos));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> Create([FromBody] CreateSubjectRequest req)
    {
        if (await _context.Subjects.AnyAsync(s => s.Code == req.Code))
            return BadRequest(ApiResponse<object>.Fail("Subject code already exists."));

        var subject = new Subject { Name = req.Name, Code = req.Code, Credits = req.Credits, ClassId = req.ClassId };
        await _context.Subjects.AddAsync(subject);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { subject.Id }, "Subject created"));
    }

    [HttpPost("{id:guid}/assign-teacher")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> AssignTeacher(Guid id, [FromBody] AssignTeacherRequest req)
    {
        var existing = await _context.SubjectTeachers.FirstOrDefaultAsync(st => st.SubjectId == id && st.TeacherId == req.TeacherId);
        if (existing is not null) return BadRequest(ApiResponse<object>.Fail("Teacher already assigned."));

        await _context.SubjectTeachers.AddAsync(new SubjectTeacher { SubjectId = id, TeacherId = req.TeacherId, ClassId = req.ClassId });
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Teacher assigned"));
    }
}

[ApiController]
[Route("api/departments")]
[Authorize]
public class DepartmentsController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public DepartmentsController(SchoolERPDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var depts = await _context.Departments
            .Include(d => d.Teachers)
            .OrderBy(d => d.Name)
            .ToListAsync();

        var dtos = depts.Select(d => new DepartmentDto
        {
            Id = d.Id, Name = d.Name, Description = d.Description,
            Code = d.Code, TeacherCount = d.Teachers.Count, IsActive = d.IsActive
        });

        return Ok(ApiResponse<IEnumerable<DepartmentDto>>.Ok(dtos));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> Create([FromBody] CreateDeptRequest req)
    {
        var dept = new Department { Name = req.Name, Code = req.Code, Description = req.Description };
        await _context.Departments.AddAsync(dept);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { dept.Id }, "Department created"));
    }
}

[ApiController]
[Route("api/timetable")]
[Authorize]
public class TimetableController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public TimetableController(SchoolERPDbContext context) => _context = context;

    [HttpGet("class/{classId:guid}")]
    public async Task<IActionResult> GetClassTimetable(Guid classId, [FromQuery] Guid? sectionId)
    {
        var q = _context.Timetables
            .Include(t => t.Subject)
            .Include(t => t.Teacher).ThenInclude(t => t!.User)
            .Where(t => t.ClassId == classId && t.IsActive);

        if (sectionId.HasValue) q = q.Where(t => t.SectionId == sectionId);

        var tt = await q.OrderBy(t => t.DayOfWeek).ThenBy(t => t.StartTime).ToListAsync();

        return Ok(ApiResponse<object>.Ok(tt.Select(t => new
        {
            t.Id, t.DayOfWeek,
            StartTime = t.StartTime.ToString(@"hh\:mm"),
            EndTime = t.EndTime.ToString(@"hh\:mm"),
            SubjectName = t.Subject?.Name,
            TeacherName = t.Teacher?.User?.FullName,
            t.Room
        })));
    }

    [HttpGet("teacher/{teacherId:guid}")]
    public async Task<IActionResult> GetTeacherTimetable(Guid teacherId)
    {
        var tt = await _context.Timetables
            .Include(t => t.Subject)
            .Include(t => t.Class)
            .Include(t => t.Section)
            .Where(t => t.TeacherId == teacherId && t.IsActive)
            .OrderBy(t => t.DayOfWeek).ThenBy(t => t.StartTime)
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(tt.Select(t => new
        {
            t.Id, t.DayOfWeek,
            StartTime = t.StartTime.ToString(@"hh\:mm"),
            EndTime = t.EndTime.ToString(@"hh\:mm"),
            SubjectName = t.Subject?.Name,
            ClassName = t.Class?.Name,
            SectionName = t.Section?.Name,
            t.Room
        })));
    }
}

[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public NotificationsController(SchoolERPDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
    {
        var q = _context.Notifications.Where(n => n.IsGlobal).OrderByDescending(n => n.CreatedAt);
        var total = await q.CountAsync();
        var items = await q.Skip((page - 1) * pageSize).Take(pageSize)
            .Select(n => new NotificationDto
            { Id = n.Id, Title = n.Title, Message = n.Message, Type = n.Type.ToString(), IsRead = n.IsRead, CreatedAt = n.CreatedAt })
            .ToListAsync();

        return Ok(ApiResponse<PagedResult<NotificationDto>>.Ok(new PagedResult<NotificationDto>
        { Items = items, TotalCount = total, Page = page, PageSize = pageSize }));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal,Teacher")]
    public async Task<IActionResult> Create([FromBody] CreateNotificationDto dto)
    {
        var senderId = GetCurrentUserId();
        var notification = new Notification
        {
            Title = dto.Title, Message = dto.Message, Type = dto.Type,
            SenderId = senderId, TargetUserId = dto.TargetUserId,
            TargetClassId = dto.TargetClassId, TargetRole = dto.TargetRole,
            IsGlobal = dto.IsGlobal
        };
        await _context.Notifications.AddAsync(notification);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(new { notification.Id }, "Notification sent"));
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }
}

public record CreateSubjectRequest(string Name, string Code, int Credits, Guid? ClassId);
public record AssignTeacherRequest(Guid TeacherId, Guid? ClassId);
public record CreateDeptRequest(string Name, string Code, string? Description);
