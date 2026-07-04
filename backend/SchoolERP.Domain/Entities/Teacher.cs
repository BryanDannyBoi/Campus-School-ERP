using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Teacher : BaseEntity
{
    public Guid UserId { get; set; }
    public string EmployeeId { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string? Qualification { get; set; }
    public string? Specialization { get; set; }
    public DateTime JoiningDate { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? BloodGroup { get; set; }
    public decimal? Salary { get; set; }
    public string? BankAccountNumber { get; set; }
    public string? BankName { get; set; }
    public string? PanNumber { get; set; }
    public TeacherStatus Status { get; set; } = TeacherStatus.Active;
    public Guid? DepartmentId { get; set; }

    // Navigation
    public User? User { get; set; }
    public Department? Department { get; set; }
    public ICollection<SubjectTeacher> SubjectTeachers { get; set; } = new List<SubjectTeacher>();
    public ICollection<Timetable> Timetables { get; set; } = new List<Timetable>();
    public ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();
    public ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();
    public ICollection<Assignment> Assignments { get; set; } = new List<Assignment>();
    public ICollection<Section> ClassSections { get; set; } = new List<Section>();
}

public class LeaveRequest : BaseEntity
{
    public Guid TeacherId { get; set; }
    public DateTime FromDate { get; set; }
    public DateTime ToDate { get; set; }
    public string LeaveType { get; set; } = string.Empty;
    public string Reason { get; set; } = string.Empty;
    public LeaveStatus Status { get; set; } = LeaveStatus.Pending;
    public string? RejectionReason { get; set; }
    public Guid? ApprovedById { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation
    public Teacher? Teacher { get; set; }
}
