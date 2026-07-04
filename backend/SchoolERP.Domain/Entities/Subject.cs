using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Subject : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int Credits { get; set; }
    public Guid? ClassId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Class? Class { get; set; }
    public ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<Exam> Exams { get; set; } = new List<Exam>();
}

public class SubjectTeacher : BaseEntity
{
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public Guid? ClassId { get; set; }
    public bool IsPrimary { get; set; } = true;

    // Navigation
    public Subject? Subject { get; set; }
    public Teacher? Teacher { get; set; }
    public Class? Class { get; set; }
}
