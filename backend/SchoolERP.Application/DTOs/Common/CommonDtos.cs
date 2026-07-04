using SchoolERP.Domain.Enums;

namespace SchoolERP.Application.DTOs.Common;

// Generic paginated result
public class PagedResult<T>
{
    public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling(TotalCount / (double)PageSize);
    public bool HasPrevious => Page > 1;
    public bool HasNext => Page < TotalPages;
}

// Generic API response wrapper
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public T? Data { get; set; }
    public List<string> Errors { get; set; } = new();

    public static ApiResponse<T> Ok(T data, string? message = null) =>
        new() { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string error) =>
        new() { Success = false, Errors = new List<string> { error } };

    public static ApiResponse<T> Fail(List<string> errors) =>
        new() { Success = false, Errors = errors };
}

// Pagination query
public class PaginationQuery
{
    private int _pageSize = 10;
    public int Page { get; set; } = 1;
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > 100 ? 100 : value;
    }
    public string? Search { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

// Dashboard DTOs
public class DashboardStatsDto
{
    public int TotalStudents { get; set; }
    public int TotalTeachers { get; set; }
    public int TotalParents { get; set; }
    public int TotalClasses { get; set; }
    public decimal TotalFeeCollected { get; set; }
    public decimal TotalFeePending { get; set; }
    public double AttendancePercentage { get; set; }
    public int TotalNotifications { get; set; }
    public int PendingLeaves { get; set; }
    public int BooksIssued { get; set; }
    public List<MonthlyAttendanceDto> MonthlyAttendance { get; set; } = new();
    public List<FeeCollectionDto> FeeChart { get; set; } = new();
    public List<RecentActivityDto> RecentActivities { get; set; } = new();
}

public class MonthlyAttendanceDto
{
    public string Month { get; set; } = string.Empty;
    public double Present { get; set; }
    public double Absent { get; set; }
}

public class FeeCollectionDto
{
    public string Month { get; set; } = string.Empty;
    public decimal Collected { get; set; }
    public decimal Pending { get; set; }
}

public class RecentActivityDto
{
    public string Action { get; set; } = string.Empty;
    public string User { get; set; } = string.Empty;
    public DateTime Time { get; set; }
    public string Type { get; set; } = string.Empty;
}

// Department, Class, Section DTOs
public class DepartmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Code { get; set; }
    public int TeacherCount { get; set; }
    public bool IsActive { get; set; }
}

public class ClassDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int AcademicYear { get; set; }
    public int StudentCount { get; set; }
    public List<SectionDto> Sections { get; set; } = new();
}

public class SectionDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid ClassId { get; set; }
    public string? ClassTeacherName { get; set; }
    public int MaxStudents { get; set; }
    public int CurrentStudents { get; set; }
}

// Subject DTOs
public class SubjectDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Code { get; set; } = string.Empty;
    public int Credits { get; set; }
    public string? ClassName { get; set; }
    public List<string> Teachers { get; set; } = new();
}

// Attendance DTOs
public class MarkAttendanceDto
{
    public Guid ClassId { get; set; }
    public Guid? SectionId { get; set; }
    public Guid? SubjectId { get; set; }
    public DateTime Date { get; set; }
    public List<StudentAttendanceDto> Attendances { get; set; } = new();
}

public class StudentAttendanceDto
{
    public Guid StudentId { get; set; }
    public AttendanceStatus Status { get; set; }
    public string? Remarks { get; set; }
}

public class AttendanceReportDto
{
    public Guid StudentId { get; set; }
    public string StudentName { get; set; } = string.Empty;
    public string AdmissionNumber { get; set; } = string.Empty;
    public int TotalDays { get; set; }
    public int PresentDays { get; set; }
    public int AbsentDays { get; set; }
    public int LateDays { get; set; }
    public double Percentage { get; set; }
}

// Notification DTOs
public class NotificationDto
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateNotificationDto
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? TargetClassId { get; set; }
    public string? TargetRole { get; set; }
    public bool IsGlobal { get; set; } = false;
}

// Message DTOs
public class MessageDto
{
    public Guid Id { get; set; }
    public string SenderName { get; set; } = string.Empty;
    public string? SenderPhoto { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class SendMessageDto
{
    public Guid ReceiverId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public Guid? ParentMessageId { get; set; }
}
