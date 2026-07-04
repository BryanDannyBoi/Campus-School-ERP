using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Students;

public class StudentListDto
{
    public Guid Id { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Phone { get; set; }
    public string? ClassName { get; set; }
    public string? SectionName { get; set; }
    public string? ProfilePhoto { get; set; }
    public Gender Gender { get; set; }
    public StudentStatus Status { get; set; }
    public DateTime AdmissionDate { get; set; }
}

public class StudentDetailDto : StudentListDto
{
    public DateTime DateOfBirth { get; set; }
    public string? BloodGroup { get; set; }
    public string? Religion { get; set; }
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public string? PreviousSchool { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ParentId { get; set; }
    public string? ParentName { get; set; }
    public string? ParentPhone { get; set; }
    public List<DocumentDto> Documents { get; set; } = new();
}

public class CreateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public DateTime AdmissionDate { get; set; } = DateTime.UtcNow;
    public string? BloodGroup { get; set; }
    public string? Religion { get; set; }
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public string? PreviousSchool { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ParentId { get; set; }
}

public class UpdateStudentDto
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string? PhoneNumber { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Religion { get; set; }
    public string? Nationality { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ParentId { get; set; }
    public StudentStatus Status { get; set; }
}

public class DocumentDto
{
    public Guid Id { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
}
