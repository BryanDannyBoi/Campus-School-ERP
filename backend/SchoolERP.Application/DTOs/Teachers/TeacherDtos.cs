using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Teachers;

public class TeacherListDto
{
    public Guid Id { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? DepartmentName { get; set; }
    public string? Qualification { get; set; }
    public string? Specialization { get; set; }
    public string? ProfilePhoto { get; set; }
    public TeacherStatus Status { get; set; }
    public DateTime JoiningDate { get; set; }
}

public class TeacherDetailDto : TeacherListDto
{
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? BloodGroup { get; set; }
    public Guid? DepartmentId { get; set; }
    public decimal? Salary { get; set; }
    public List<string> Subjects { get; set; } = new();
}

public class CreateTeacherDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Qualification { get; set; }
    public string? Specialization { get; set; }
    public DateTime JoiningDate { get; set; } = DateTime.UtcNow;
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? BloodGroup { get; set; }
    public decimal? Salary { get; set; }
    public Guid? DepartmentId { get; set; }
}

public class UpdateTeacherDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Qualification { get; set; }
    public string? Specialization { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? BloodGroup { get; set; }
    public decimal? Salary { get; set; }
    public Guid? DepartmentId { get; set; }
    public TeacherStatus Status { get; set; }
}
