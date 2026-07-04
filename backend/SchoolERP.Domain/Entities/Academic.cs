using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Department : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<Teacher> Teachers { get; set; } = new List<Teacher>();
    public ICollection<Course> Courses { get; set; } = new List<Course>();
}

public class Course : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public int DurationYears { get; set; }
    public Guid DepartmentId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Department? Department { get; set; }
    public ICollection<Class> Classes { get; set; } = new List<Class>();
}

public class Class : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int AcademicYear { get; set; }
    public Guid? CourseId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Course? Course { get; set; }
    public ICollection<Section> Sections { get; set; } = new List<Section>();
    public ICollection<Subject> Subjects { get; set; } = new List<Subject>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
}

public class Section : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public Guid? ClassTeacherId { get; set; }
    public int MaxStudents { get; set; } = 40;
    public bool IsActive { get; set; } = true;

    // Navigation
    public Class? Class { get; set; }
    public Teacher? ClassTeacher { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
