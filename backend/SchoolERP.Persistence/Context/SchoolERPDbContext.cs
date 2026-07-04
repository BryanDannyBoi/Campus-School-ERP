using Microsoft.EntityFrameworkCore;
using SchoolERP.Domain.Entities;

namespace SchoolERP.Persistence.Context;

public class SchoolERPDbContext : DbContext
{
    public SchoolERPDbContext(DbContextOptions<SchoolERPDbContext> options) : base(options) { }

    // Users & Auth
    public DbSet<User> Users => Set<User>();

    // People
    public DbSet<Student> Students => Set<Student>();
    public DbSet<Teacher> Teachers => Set<Teacher>();
    public DbSet<Parent> Parents => Set<Parent>();
    public DbSet<StudentDocument> StudentDocuments => Set<StudentDocument>();

    // Academic Structure
    public DbSet<Department> Departments => Set<Department>();
    public DbSet<Course> Courses => Set<Course>();
    public DbSet<Class> Classes => Set<Class>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Subject> Subjects => Set<Subject>();
    public DbSet<SubjectTeacher> SubjectTeachers => Set<SubjectTeacher>();
    public DbSet<Timetable> Timetables => Set<Timetable>();

    // Attendance
    public DbSet<Attendance> Attendances => Set<Attendance>();

    // Assignments & Exams
    public DbSet<Assignment> Assignments => Set<Assignment>();
    public DbSet<AssignmentSubmission> AssignmentSubmissions => Set<AssignmentSubmission>();
    public DbSet<Exam> Exams => Set<Exam>();
    public DbSet<Mark> Marks => Set<Mark>();

    // Fee
    public DbSet<FeeStructure> FeeStructures => Set<FeeStructure>();
    public DbSet<FeePayment> FeePayments => Set<FeePayment>();

    // Library
    public DbSet<LibraryBook> LibraryBooks => Set<LibraryBook>();
    public DbSet<BookIssue> BookIssues => Set<BookIssue>();

    // Communication
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<Message> Messages => Set<Message>();

    // Leaves
    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    // Audit
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SchoolERPDbContext).Assembly);

        // Global query filter for soft delete
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Student>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Teacher>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Parent>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Class>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Section>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Subject>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Department>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<LibraryBook>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Assignment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Exam>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<FeeStructure>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);

        // Message thread self-referencing
        modelBuilder.Entity<Message>()
            .HasMany(m => m.Replies)
            .WithOne(m => m.ParentMessage)
            .HasForeignKey(m => m.ParentMessageId)
            .OnDelete(DeleteBehavior.Restrict);

        // User -> sent messages
        modelBuilder.Entity<Message>()
            .HasOne(m => m.Sender)
            .WithMany(u => u.SentMessages)
            .HasForeignKey(m => m.SenderId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(m => m.Receiver)
            .WithMany(u => u.ReceivedMessages)
            .HasForeignKey(m => m.ReceiverId)
            .OnDelete(DeleteBehavior.Restrict);

        // Notification -> sender
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.Sender)
            .WithMany(u => u.SentNotifications)
            .HasForeignKey(n => n.SenderId)
            .OnDelete(DeleteBehavior.SetNull);

        // Section -> ClassTeacher
        modelBuilder.Entity<Section>()
            .HasOne(s => s.ClassTeacher)
            .WithMany(t => t.ClassSections)
            .HasForeignKey(s => s.ClassTeacherId)
            .OnDelete(DeleteBehavior.SetNull);

        // Unique constraints
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<Student>().HasIndex(s => s.AdmissionNumber).IsUnique();
        modelBuilder.Entity<Teacher>().HasIndex(t => t.EmployeeId).IsUnique();
        modelBuilder.Entity<Subject>().HasIndex(s => s.Code).IsUnique();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is Domain.Common.BaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            if (entry.Entity is Domain.Common.BaseEntity entity)
            {
                if (entry.State == EntityState.Added)
                    entity.CreatedAt = DateTime.UtcNow;
                else
                    entity.UpdatedAt = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
