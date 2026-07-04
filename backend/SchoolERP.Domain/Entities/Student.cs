using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Student : BaseEntity
{
    public Guid UserId { get; set; }
    public string AdmissionNumber { get; set; } = string.Empty;
    public DateTime AdmissionDate { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? BloodGroup { get; set; }
    public string? Religion { get; set; }
    public string? Nationality { get; set; } = "Indian";
    public string? MotherTongue { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? ZipCode { get; set; }
    public string? Country { get; set; } = "India";
    public string? EmergencyContactName { get; set; }
    public string? EmergencyContactPhone { get; set; }
    public string? EmergencyContactRelation { get; set; }
    public string? PreviousSchool { get; set; }
    public StudentStatus Status { get; set; } = StudentStatus.Active;
    public Guid? ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? ParentId { get; set; }

    // Navigation
    public User? User { get; set; }
    public Class? Class { get; set; }
    public Section? Section { get; set; }
    public Parent? Parent { get; set; }
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
    public ICollection<Mark> Marks { get; set; } = new List<Mark>();
    public ICollection<FeePayment> FeePayments { get; set; } = new List<FeePayment>();
    public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
    public ICollection<StudentDocument> Documents { get; set; } = new List<StudentDocument>();
}

public class StudentDocument : BaseEntity
{
    public Guid StudentId { get; set; }
    public string DocumentName { get; set; } = string.Empty;
    public string DocumentType { get; set; } = string.Empty;
    public string FilePath { get; set; } = string.Empty;
    public long FileSizeBytes { get; set; }

    // Navigation
    public Student? Student { get; set; }
}
