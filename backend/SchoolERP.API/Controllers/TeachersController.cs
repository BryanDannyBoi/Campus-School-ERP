using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Application.DTOs.Teachers;
using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TeachersController : ControllerBase
{
    private readonly SchoolERPDbContext _context;

    public TeachersController(SchoolERPDbContext context) => _context = context;

    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] PaginationQuery query)
    {
        var q = _context.Teachers
            .Include(t => t.User)
            .Include(t => t.Department)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(query.Search))
        {
            var s = query.Search.ToLower();
            q = q.Where(t => t.User!.FirstName.ToLower().Contains(s) ||
                              t.User!.LastName.ToLower().Contains(s) ||
                              t.EmployeeId.ToLower().Contains(s) ||
                              t.User!.Email.ToLower().Contains(s));
        }

        var total = await q.CountAsync();
        var items = await q.Skip((query.Page - 1) * query.PageSize).Take(query.PageSize).ToListAsync();

        var dtos = items.Select(t => new TeacherListDto
        {
            Id = t.Id, EmployeeId = t.EmployeeId,
            FullName = t.User?.FullName ?? "",
            Email = t.User?.Email ?? "",
            Phone = t.User?.PhoneNumber,
            DepartmentName = t.Department?.Name,
            Qualification = t.Qualification,
            Specialization = t.Specialization,
            ProfilePhoto = t.User?.ProfilePhoto,
            Status = t.Status,
            JoiningDate = t.JoiningDate
        });

        return Ok(ApiResponse<PagedResult<TeacherListDto>>.Ok(new PagedResult<TeacherListDto>
        { Items = dtos, TotalCount = total, Page = query.Page, PageSize = query.PageSize }));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var teacher = await _context.Teachers
            .Include(t => t.User)
            .Include(t => t.Department)
            .Include(t => t.SubjectTeachers).ThenInclude(st => st.Subject)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (teacher is null) return NotFound(ApiResponse<object>.Fail("Teacher not found."));

        var dto = new TeacherDetailDto
        {
            Id = teacher.Id, EmployeeId = teacher.EmployeeId,
            FullName = teacher.User?.FullName ?? "",
            Email = teacher.User?.Email ?? "",
            Phone = teacher.User?.PhoneNumber,
            DepartmentName = teacher.Department?.Name,
            DepartmentId = teacher.DepartmentId,
            Qualification = teacher.Qualification,
            Specialization = teacher.Specialization,
            ProfilePhoto = teacher.User?.ProfilePhoto,
            Status = teacher.Status,
            JoiningDate = teacher.JoiningDate,
            DateOfBirth = teacher.DateOfBirth,
            Gender = teacher.Gender,
            Address = teacher.Address,
            City = teacher.City,
            State = teacher.State,
            BloodGroup = teacher.BloodGroup,
            Salary = teacher.Salary,
            Subjects = teacher.SubjectTeachers.Select(st => st.Subject?.Name ?? "").ToList()
        };

        return Ok(ApiResponse<TeacherDetailDto>.Ok(dto));
    }

    [HttpPost]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> Create([FromBody] CreateTeacherDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            return BadRequest(ApiResponse<object>.Fail("Email already exists."));

        var user = new User
        {
            FirstName = dto.FirstName, LastName = dto.LastName,
            Email = dto.Email, PhoneNumber = dto.PhoneNumber,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
            Role = UserRole.Teacher, IsActive = true
        };
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();

        var count = await _context.Teachers.CountAsync();
        var empId = $"EMP{(count + 1):D4}";

        var teacher = new Teacher
        {
            UserId = user.Id, EmployeeId = empId,
            DateOfBirth = dto.DateOfBirth, Gender = dto.Gender,
            Qualification = dto.Qualification, Specialization = dto.Specialization,
            JoiningDate = dto.JoiningDate, Address = dto.Address, City = dto.City,
            State = dto.State, BloodGroup = dto.BloodGroup, Salary = dto.Salary,
            DepartmentId = dto.DepartmentId
        };
        await _context.Teachers.AddAsync(teacher);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = teacher.Id },
            ApiResponse<object>.Ok(new { teacher.Id, teacher.EmployeeId }, "Teacher created successfully"));
    }

    [HttpPut("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin,Principal")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTeacherDto dto)
    {
        var teacher = await _context.Teachers.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        if (teacher is null) return NotFound(ApiResponse<object>.Fail("Teacher not found."));

        teacher.User!.FirstName = dto.FirstName;
        teacher.User!.LastName = dto.LastName;
        teacher.User!.PhoneNumber = dto.PhoneNumber;
        teacher.DateOfBirth = dto.DateOfBirth;
        teacher.Gender = dto.Gender;
        teacher.Qualification = dto.Qualification;
        teacher.Specialization = dto.Specialization;
        teacher.Address = dto.Address;
        teacher.City = dto.City;
        teacher.State = dto.State;
        teacher.BloodGroup = dto.BloodGroup;
        teacher.Salary = dto.Salary;
        teacher.DepartmentId = dto.DepartmentId;
        teacher.Status = dto.Status;
        teacher.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Teacher updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    [Authorize(Roles = "SuperAdmin,SchoolAdmin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var teacher = await _context.Teachers.FindAsync(id);
        if (teacher is null) return NotFound(ApiResponse<object>.Fail("Teacher not found."));
        teacher.IsDeleted = true;
        teacher.Status = TeacherStatus.Resigned;
        await _context.SaveChangesAsync();
        return Ok(ApiResponse<object>.Ok(null, "Teacher deleted successfully"));
    }
}
