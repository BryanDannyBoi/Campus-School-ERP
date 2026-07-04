using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Timetable : BaseEntity
{
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid SubjectId { get; set; }
    public Guid TeacherId { get; set; }
    public DayOfWeekEnum DayOfWeek { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public string? Room { get; set; }
    public int AcademicYear { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Class? Class { get; set; }
    public Section? Section { get; set; }
    public Subject? Subject { get; set; }
    public Teacher? Teacher { get; set; }
}
