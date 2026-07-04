using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Students;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class StudentsController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    private readonly IWebHostEnvironment _env;

    public StudentsController(SchoolERPDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var q = _context.Students
            .Include(s => s.User)
            .Include(s => s.Class)
            .Include(s => s.Section)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var search = query.Search.ToLower();
            q = q.Where(s =>
                s.User!.FirstName.ToLower().Contains(search) ||
                s.User!.LastName.ToLower().Contains(search) ||
                s.AdmissionNumber.ToLower().Contains(search) ||
                s.User!.Email.ToLower().Contains(search));
        }

        var total = await q.CountAsync();
        var items = await q
            .OrderByDescending(s => s.AdmissionDate)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync();

        var dtos = items.Select(s => new StudentListDto
        {
            Id = s.Id,
            AdmissionNumber = s.AdmissionNumber,
            FullName = s.User?.FullName ?? "",
            Email = s.User?.Email ?? "",
            Phone = s.User?.PhoneNumber,
            ClassName = s.Class?.Name,
            SectionName = s.Section?.Name,
            ProfilePhoto = s.User?.ProfilePhoto,
            Gender = s.Gender,
            Status = s.Status,
            AdmissionDate = s.AdmissionDate
        });

        return Ok(ApiResponse<PagedResult<StudentListDto>>.Ok(new PagedResult<StudentListDto>
        {
            Items = dtos,
            TotalCount = total,
            Page = query.Page,
            PageSize = query.PageSize
        }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var student = await _context.Students
            .Include(s => s.User)
            .Include(s => s.Class)
            .Include(s => s.Section)
            .Include(s => s.Parent).ThenInclude(p => p!.User)
            .Include(s => s.Documents)
            .FirstOrDefaultAsync(s => s.Id == id);

        if (student is null) return NotFound(ApiResponse<object>.Fail("Student not found."));

        var dto = new StudentDetailDto
        {
            Id = student.Id,
            AdmissionNumber = student.AdmissionNumber,
            FullName = student.User?.FullName ?? "",
            Email = student.User?.Email ?? "",
            Phone = student.User?.PhoneNumber,
            ClassName = student.Class?.Name,
            SectionName = student.Section?.Name,
            ProfilePhoto = student.User?.ProfilePhoto,
            Gender = student.Gender,
            Status = student.Status,
            AdmissionDate = student.AdmissionDate,
            DateOfBirth = student.DateOfBirth,
            BloodGroup = student.BloodGroup,
            Religion = student.Religion,
            Nationality = student.Nationality,
            Address = student.Address,
            City = student.City,
            State = student.State,
            Country = student.Country,
            EmergencyContactName = student.EmergencyContactName,
            EmergencyContactPhone = student.EmergencyContactPhone,
            EmergencyContactRelation = student.EmergencyContactRelation,
            PreviousSchool = student.PreviousSchool,
            ClassId = student.ClassId,
            SectionId = student.SectionId,
            ParentId = student.ParentId,
            ParentName = student.Parent?.User?.FullName,
            ParentPhone = student.Parent?.FatherPhone ?? student.Parent?.MotherPhone,
            Documents = student.Documents.Select(d => new DocumentDto
            {
                Id = d.Id, DocumentName = d.DocumentName,
                DocumentType = d.DocumentType, FilePath = d.FilePath
            }).ToList()
        };

        return Ok(ApiResponse<StudentDetailDto>.Ok(dto));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> Create([FromBody] CreateStudentDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(ApiResponse<object>.Fail("Email already exists."));

        var user = new User
        {
            FirstName = dto.FirstName, LastName = dto.LastName,
            Email = dto.Email, PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Student, IsActive = true
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        // Generate admission number
        var count = await _context.Students.CountAsync();
        var admNo = $"ADM{DateTime.UtcNow.Year}{(count + 1):D4}";

        var student = new Student
        {
            UserId = user.Id,
            AdmissionNumber = admNo,
            AdmissionDate = dto.AdmissionDate,
            DateOfBirth = dto.DateOfBirth,
            Gender = dto.Gender,
            BloodGroup = dto.BloodGroup,
            Religion = dto.Religion,
            Nationality = dto.Nationality,
            Address = dto.Address,
            City = dto.City,
            State = dto.State,
            Country = dto.Country,
            EmergencyContactName = dto.EmergencyContactName,
            EmergencyContactPhone = dto.EmergencyContactPhone,
            EmergencyContactRelation = dto.EmergencyContactRelation,
            PreviousSchool = dto.PreviousSchool,
            ClassId = dto.ClassId,
            SectionId = dto.SectionId,
            ParentId = dto.ParentId
        };
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = student.Id },
            ApiResponse<object>.Ok(new { student.Id, student.AdmissionNumber }, "Student admitted successfully"));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateStudentDto dto)
    {
        var student = await _context.Students.Include(s => s.User).FirstOrDefaultAsync(s => s.Id == id);
        if (student is null) return NotFound(ApiResponse<object>.Fail("Student not found."));

        student.User!.FirstName = dto.FirstName;
        student.User!.LastName = dto.LastName;
        student.User!.PhoneNumber = dto.PhoneNumber;
        student.DateOfBirth = dto.DateOfBirth;
        student.Gender = dto.Gender;
        student.BloodGroup = dto.BloodGroup;
        student.Religion = dto.Religion;
        student.Nationality = dto.Nationality;
        student.Address = dto.Address;
        student.City = dto.City;
        student.State = dto.State;
        student.Country = dto.Country;
        student.EmergencyContactName = dto.EmergencyContactName;
        student.EmergencyContactPhone = dto.EmergencyContactPhone;
        student.ClassId = dto.ClassId;
        student.SectionId = dto.SectionId;
        student.ParentId = dto.ParentId;
        student.Status = dto.Status;
        student.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Student updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var student = await _context.Students.FindAsync(id);
        if (student is null) return NotFound(ApiResponse<object>.Fail("Student not found."));

        student.IsDeleted = true;
        student.Status = StudentStatus.Inactive;
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Student deleted successfully"));
    }

    [HttpGet("{id:guid}/attendance")]
    public async Task<IActionResult> GetAttendance(Guid id, [FromQuery] int? month, [FromQuery] int? year)
    {
        var q = _context.Attendances.Where(a => a.StudentId == id);
        if (month.HasValue) q = q.Where(a => a.Date.Month == month);
        if (year.HasValue) q = q.Where(a => a.Date.Year == year);

        var records = await q.OrderByDescending(a => a.Date).ToListAsync();
        var total = records.Count;
        var present = records.Count(a => a.Status == AttendanceStatus.Present);
        var absent = records.Count(a => a.Status == AttendanceStatus.Absent);
        var late = records.Count(a => a.Status == AttendanceStatus.Late);
        var percentage = total > 0 ? Math.Round((double)(present + late) / total * 100, 2) : 0;

        return Ok(ApiResponse<object>.Ok(new
        {
            TotalDays = total, PresentDays = present, AbsentDays = absent, LateDays = late,
            Percentage = percentage, Records = records
        }));
    }
}
