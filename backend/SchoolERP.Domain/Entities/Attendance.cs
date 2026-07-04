using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Attendance : BaseEntity
{
    public Guid? StudentId { get; set; }
    public Guid? TeacherId { get; set; }
    public Guid? ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? SubjectId { get; set; }
    public DateTime Date { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
    public Guid MarkedById { get; set; }
    public bool IsTeacherAttendance { get; set; } = false;

    // Navigation
    public Student? Student { get; set; }
    public Teacher? Teacher { get; set; }
    public Class? Class { get; set; }
    public Subject? Subject { get; set; }
}
