namespace SchoolERP.Domain.Enums;

public enum UserRole
{
    SuperAdmin = 1,
    SchoolAdmin = 2,
    Principal = 3,
    Teacher = 4,
    Student = 5,
    Parent = 6,
    Accountant = 7,
    Librarian = 8
}

public enum Gender
{
    Male = 1,
    Female = 2,
    Other = 3
}

public enum AttendanceStatus
{
    Present = 1,
    Absent = 2,
    Late = 3,
    HalfDay = 4
}

public enum ExamType
{
    Internal = 1,
    MidTerm = 2,
    Semester = 3,
    Final = 4
}

public enum AssignmentStatus
{
    Pending = 1,
    Submitted = 2,
    Graded = 3,
    Overdue = 4
}

public enum LeaveStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Cancelled = 4
}

public enum FeeStatus
{
    Pending = 1,
    Paid = 2,
    Partial = 3,
    Overdue = 4,
    Waived = 5
}

public enum BookStatus
{
    Available = 1,
    Issued = 2,
    Reserved = 3,
    Lost = 4,
    Damaged = 5
}

public enum NotificationType
{
    Announcement = 1,
    Holiday = 2,
    Event = 3,
    ExamUpdate = 4,
    FeeReminder = 5,
    AssignmentDeadline = 6,
    General = 7
}

public enum DayOfWeekEnum
{
    Monday = 1,
    Tuesday = 2,
    Wednesday = 3,
    Thursday = 4,
    Friday = 5,
    Saturday = 6,
    Sunday = 7
}

public enum StudentStatus
{
    Active = 1,
    Inactive = 2,
    Transferred = 3,
    Graduated = 4,
    Suspended = 5
}

public enum TeacherStatus
{
    Active = 1,
    Inactive = 2,
    OnLeave = 3,
    Resigned = 4
}
