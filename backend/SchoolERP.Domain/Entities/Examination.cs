using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Exam : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ExamType ExamType { get; set; }
    public Guid SubjectId { get; set; }
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public DateTime ExamDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public int TotalMarks { get; set; }
    public int PassingMarks { get; set; }
    public string? Venue { get; set; }
    public int AcademicYear { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Subject? Subject { get; set; }
    public Class? Class { get; set; }
    public Section? Section { get; set; }
    public ICollection<Mark> Marks { get; set; } = new List<Mark>();
}

public class Mark : BaseEntity
{
    public Guid ExamId { get; set; }
    public Guid StudentId { get; set; }
    public decimal ObtainedMarks { get; set; }
    public bool IsAbsent { get; set; } = false;
    public string? Grade { get; set; }
    public string? Remarks { get; set; }

    // Navigation
    public Exam? Exam { get; set; }
    public Student? Student { get; set; }
}
