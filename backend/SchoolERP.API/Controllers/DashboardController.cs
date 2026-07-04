using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SchoolERP.Application.DTOs.Common;
using SchoolERP.Persistence.Context;

namespace SchoolERP.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly SchoolERPDbContext _context;
    public DashboardController(SchoolERPDbContext context) => _context = context;

    [HttpGet("stats")]
    public async Task<IActionResult> GetStats()
    {
        var today = DateTime.UtcNow.Date;
        var totalStudents = await _context.Students.CountAsync();
        var totalTeachers = await _context.Teachers.CountAsync();
        var totalParents = await _context.Parents.CountAsync();
        var totalClasses = await _context.Classes.CountAsync();
        var pendingLeaves = await _context.LeaveRequests.CountAsync(l => l.Status == Domain.Enums.LeaveStatus.Pending);
        var booksIssued = await _context.BookIssues.CountAsync(b => !b.IsReturned);

        var totalFeeCollected = await _context.FeePayments
            .Where(f => f.Status == Domain.Enums.FeeStatus.Paid)
            .SumAsync(f => f.AmountPaid);
        var totalFeePending = await _context.FeePayments
            .Where(f => f.Status == Domain.Enums.FeeStatus.Pending || f.Status == Domain.Enums.FeeStatus.Partial)
            .SumAsync(f => f.TotalAmount - f.AmountPaid);

        // Monthly attendance (last 6 months)
        var sixMonthsAgo = today.AddMonths(-6);
        var attendances = await _context.Attendances
            .Where(a => a.Date >= sixMonthsAgo && !a.IsTeacherAttendance)
            .ToListAsync();

        var monthlyAttendance = attendances
            .GroupBy(a => new { a.Date.Year, a.Date.Month })
            .OrderBy(g => g.Key.Year).ThenBy(g => g.Key.Month)
            .Select(g => new MonthlyAttendanceDto
            {
                Month = new DateTime(g.Key.Year, g.Key.Month, 1).ToString("MMM yyyy"),
                Present = g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Present),
                Absent = g.Count(a => a.Status == Domain.Enums.AttendanceStatus.Absent)
            }).ToList();

        // Overall attendance %
        var totalAtt = await _context.Attendances.CountAsync(a => !a.IsTeacherAttendance);
        var presentAtt = await _context.Attendances.CountAsync(a => !a.IsTeacherAttendance &&
            (a.Status == Domain.Enums.AttendanceStatus.Present || a.Status == Domain.Enums.AttendanceStatus.Late));
        var attPct = totalAtt > 0 ? Math.Round((double)presentAtt / totalAtt * 100, 2) : 0;

        // Fee chart (last 4 months)
        var feeChart = Enumerable.Range(0, 4).Select(i =>
        {
            var m = today.AddMonths(-i);
            var collected = _context.FeePayments
                .Where(f => f.PaidAt.HasValue && f.PaidAt.Value.Month == m.Month && f.PaidAt.Value.Year == m.Year)
                .Sum(f => f.AmountPaid);
            return new FeeCollectionDto
            {
                Month = m.ToString("MMM"),
                Collected = collected,
                Pending = 0
            };
        }).Reverse().ToList();

        // Recent activities
        var recentLogs = await _context.AuditLogs
            .Include(a => a.User)
            .OrderByDescending(a => a.CreatedAt)
            .Take(10)
            .Select(a => new RecentActivityDto
            {
                Action = a.Action,
                User = a.User != null ? a.User.FullName : "System",
                Time = a.CreatedAt,
                Type = a.EntityName
            }).ToListAsync();

        var stats = new DashboardStatsDto
        {
            TotalStudents = totalStudents,
            TotalTeachers = totalTeachers,
            TotalParents = totalParents,
            TotalClasses = totalClasses,
            TotalFeeCollected = totalFeeCollected,
            TotalFeePending = totalFeePending,
            AttendancePercentage = attPct,
            PendingLeaves = pendingLeaves,
            BooksIssued = booksIssued,
            MonthlyAttendance = monthlyAttendance,
            FeeChart = feeChart,
            RecentActivities = recentLogs
        };

        return Ok(ApiResponse<DashboardStatsDto>.Ok(stats));
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetNotifications([FromQuery] int take = 10)
    {
        var notifications = await _context.Notifications
            .Where(n => n.IsGlobal)
            .OrderByDescending(n => n.CreatedAt)
            .Take(take)
            .Select(n => new NotificationDto
            {
                Id = n.Id, Title = n.Title, Message = n.Message,
                Type = n.Type.ToString(), IsRead = n.IsRead, CreatedAt = n.CreatedAt
            }).ToListAsync();

        return Ok(ApiResponse<List<NotificationDto>>.Ok(notifications));
    }
}
