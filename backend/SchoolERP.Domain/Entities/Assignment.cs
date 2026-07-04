using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Assignment : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid SubjectId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid TeacherId { get; set; }
    public DateTime DueDate { get; set; }
    public int MaxMarks { get; set; }
    public string? AttachmentPath { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Subject? Subject { get; set; }
    public Class? Class { get; set; }
    public Section? Section { get; set; }
    public Teacher? Teacher { get; set; }
    public ICollection<AssignmentSubmission> Submissions { get; set; } = new List<AssignmentSubmission>();
}

public class AssignmentSubmission : BaseEntity
{
    public Guid AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public string? SubmissionText { get; set; }
    public string? FilePath { get; set; }
    public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    public AssignmentStatus Status { get; set; } = AssignmentStatus.Submitted;
    public int? ObtainedMarks { get; set; }
    public string? Feedback { get; set; }
    public DateTime? GradedAt { get; set; }

    // Navigation
    public Assignment? Assignment { get; set; }
    public Student? Student { get; set; }
}
