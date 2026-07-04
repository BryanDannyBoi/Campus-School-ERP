using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public AttendanceController(SchoolERPDbContext context) => _context = context;

    [HttpPost("mark")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal,Teacher")]
    public async Task<IActionResult> Mark([FromBody] MarkAttendanceDto dto)
    {
        // Remove existing for that date/class
        var existing = await _context.Attendances
            .Where(a => a.ClassId == dto.ClassId && a.Date.Date == dto.Date.Date && !a.IsTeacherAttendance)
            .ToListAsync();
        _context.Attendances.RemoveRange(existing);

        var markedById = GetCurrentUserId();
        var records = dto.Attendances.Select(a => new Attendance
        {
            StudentId = a.StudentId,
            ClassId = dto.ClassId,
            SectionId = dto.SectionId,
            SubjectId = dto.SubjectId,
            Date = dto.Date.Date,
            Status = a.Status,
            Remarks = a.Remarks,
            MarkedById = markedById,
            IsTeacherAttendance = false
        });

        await _context.Attendances.AddRangeAsync(records);
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Attendance marked successfully"));
    }

    [HttpGet("class/{classId:guid}")]
    public async Task<IActionResult> GetClassAttendance(Guid classId, [FromQuery] DateTime? date, [FromQuery] Guid? sectionId)
    {
        var targetDate = date?.Date ?? DateTime.UtcNow.Date;

        var attendances = await _context.Attendances
            .Include(a => a.Student).ThenInclude(s => s!.User)
            .Where(a => a.ClassId == classId && a.Date.Date == targetDate && !a.IsTeacherAttendance)
            .Where(a => !sectionId.HasValue || a.SectionId == sectionId)
            .ToListAsync();

        return Ok(ApiResponse<object>.Ok(attendances.Select(a => new
        {
            a.Id,
            StudentId = a.StudentId,
            StudentName = a.Student?.User?.FullName,
            AdmissionNumber = a.Student?.AdmissionNumber,
            a.Status,
            a.Date,
            a.Remarks
        })));
    }

    [HttpGet("report")]
    public async Task<IActionResult> GetReport([FromQuery] Guid? classId, [FromQuery] Guid? sectionId,
        [FromQuery] int? month, [FromQuery] int? year)
    {
        var q = _context.Attendances
            .Include(a => a.Student).ThenInclude(s => s!.User)
            .Where(a => !a.IsTeacherAttendance)
            .AsQueryable();

        if (classId.HasValue) q = q.Where(a => a.ClassId == classId);
        if (sectionId.HasValue) q = q.Where(a => a.SectionId == sectionId);
        if (month.HasValue) q = q.Where(a => a.Date.Month == month);
        if (year.HasValue) q = q.Where(a => a.Date.Year == year);

        var records = await q.ToListAsync();

        var report = records
            .GroupBy(a => new { a.StudentId, Name = a.Student?.User?.FullName, AdmNo = a.Student?.AdmissionNumber })
            .Select(g => new AttendanceReportDto
            {
                StudentId = g.Key.StudentId ?? Guid.Empty,
                StudentName = g.Key.Name ?? "",
                AdmissionNumber = g.Key.AdmNo ?? "",
                TotalDays = g.Count(),
                PresentDays = g.Count(a => a.Status == AttendanceStatus.Present),
                AbsentDays = g.Count(a => a.Status == AttendanceStatus.Absent),
                LateDays = g.Count(a => a.Status == AttendanceStatus.Late),
                Percentage = g.Count() > 0
                    ? Math.Round((double)g.Count(a => a.Status == AttendanceStatus.Present || a.Status == AttendanceStatus.Late) / g.Count() * 100, 2)
                    : 0
            }).ToList();

        return Ok(ApiResponse<List<AttendanceReportDto>>.Ok(report));
    }

    private Guid GetCurrentUserId()
    {
        var claim = User.FindFirst("sub") ?? User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
        return claim is not null && Guid.TryParse(claim.Value, out var id) ? id : Guid.Empty;
    }
}
