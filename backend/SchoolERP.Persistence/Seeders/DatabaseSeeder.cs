using SchoolERP.Domain.Entities;
using SchoolERP.Domain.Enums;
using SchoolERP.Persistence.Context;
using BCrypt.Net;

namespace SchoolERP.Persistence.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(SchoolERPDbContext context)
    {
        if (context.Users.Any()) return; // already seeded

        // Departments
        var csDept = new Department { Name = "Computer Science", Code = "CS", Description = "CS Department" };
        var mathDept = new Department { Name = "Mathematics", Code = "MATH", Description = "Mathematics Department" };
        var scienceDept = new Department { Name = "Science", Code = "SCI", Description = "Science Department" };
        var englishDept = new Department { Name = "English", Code = "ENG", Description = "English Department" };
        await context.Departments.AddRangeAsync(csDept, mathDept, scienceDept, englishDept);

        // Users
        var superAdminUser = new User
        {
            FirstName = "Super", LastName = "Admin",
            Email = "superadmin@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.SuperAdmin, IsActive = true
        };
        var adminUser = new User
        {
            FirstName = "School", LastName = "Admin",
            Email = "admin@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.SchoolAdmin, IsActive = true
        };
        var principalUser = new User
        {
            FirstName = "Dr. Rajesh", LastName = "Kumar",
            Email = "principal@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Principal, IsActive = true, PhoneNumber = "9876543210"
        };

        // Teacher users
        var teacher1User = new User
        {
            FirstName = "Priya", LastName = "Sharma",
            Email = "priya.sharma@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
            Role = UserRole.Teacher, IsActive = true, PhoneNumber = "9876543211"
        };
        var teacher2User = new User
        {
            FirstName = "Rahul", LastName = "Verma",
            Email = "rahul.verma@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
            Role = UserRole.Teacher, IsActive = true, PhoneNumber = "9876543212"
        };
        var teacher3User = new User
        {
            FirstName = "Anita", LastName = "Singh",
            Email = "anita.singh@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
            Role = UserRole.Teacher, IsActive = true
        };
        var teacher4User = new User
        {
            FirstName = "Deepak", LastName = "Patel",
            Email = "deepak.patel@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Teacher@123"),
            Role = UserRole.Teacher, IsActive = true
        };

        // Student users
        var student1User = new User
        {
            FirstName = "Arjun", LastName = "Mehta",
            Email = "arjun.mehta@student.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = UserRole.Student, IsActive = true
        };
        var student2User = new User
        {
            FirstName = "Priyanka", LastName = "Gupta",
            Email = "priyanka.gupta@student.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = UserRole.Student, IsActive = true
        };
        var student3User = new User
        {
            FirstName = "Rohan", LastName = "Joshi",
            Email = "rohan.joshi@student.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = UserRole.Student, IsActive = true
        };
        var student4User = new User
        {
            FirstName = "Sneha", LastName = "Patel",
            Email = "sneha.patel@student.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = UserRole.Student, IsActive = true
        };
        var student5User = new User
        {
            FirstName = "Karan", LastName = "Sharma",
            Email = "karan.sharma@student.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Student@123"),
            Role = UserRole.Student, IsActive = true
        };

        // Parent users
        var parent1User = new User
        {
            FirstName = "Suresh", LastName = "Mehta",
            Email = "suresh.mehta@parent.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Parent@123"),
            Role = UserRole.Parent, IsActive = true, PhoneNumber = "9876500001"
        };
        var parent2User = new User
        {
            FirstName = "Kavita", LastName = "Gupta",
            Email = "kavita.gupta@parent.schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Parent@123"),
            Role = UserRole.Parent, IsActive = true
        };

        var accountantUser = new User
        {
            FirstName = "Ravi", LastName = "Accountant",
            Email = "accountant@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Accountant, IsActive = true
        };
        var librarianUser = new User
        {
            FirstName = "Meena", LastName = "Librarian",
            Email = "librarian@schoolerp.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
            Role = UserRole.Librarian, IsActive = true
        };

        await context.Users.AddRangeAsync(superAdminUser, adminUser, principalUser,
            teacher1User, teacher2User, teacher3User, teacher4User,
            student1User, student2User, student3User, student4User, student5User,
            parent1User, parent2User, accountantUser, librarianUser);

        await context.SaveChangesAsync();

        // Teachers
        var teacher1 = new Teacher
        {
            UserId = teacher1User.Id,
            EmployeeId = "EMP001",
            DateOfBirth = new DateTime(1985, 5, 15),
            Gender = Gender.Female,
            Qualification = "M.Tech Computer Science",
            Specialization = "Data Structures, Algorithms",
            JoiningDate = new DateTime(2018, 6, 1),
            DepartmentId = csDept.Id,
            Salary = 55000,
            Address = "123 Teacher Colony", City = "Mumbai", State = "Maharashtra"
        };
        var teacher2 = new Teacher
        {
            UserId = teacher2User.Id,
            EmployeeId = "EMP002",
            DateOfBirth = new DateTime(1980, 3, 20),
            Gender = Gender.Male,
            Qualification = "M.Sc Mathematics",
            Specialization = "Calculus, Linear Algebra",
            JoiningDate = new DateTime(2015, 7, 1),
            DepartmentId = mathDept.Id,
            Salary = 48000,
            Address = "456 Math Street", City = "Mumbai", State = "Maharashtra"
        };
        var teacher3 = new Teacher
        {
            UserId = teacher3User.Id,
            EmployeeId = "EMP003",
            DateOfBirth = new DateTime(1988, 8, 10),
            Gender = Gender.Female,
            Qualification = "M.Sc Physics",
            Specialization = "Quantum Mechanics",
            JoiningDate = new DateTime(2019, 4, 1),
            DepartmentId = scienceDept.Id,
            Salary = 50000
        };
        var teacher4 = new Teacher
        {
            UserId = teacher4User.Id,
            EmployeeId = "EMP004",
            DateOfBirth = new DateTime(1982, 12, 5),
            Gender = Gender.Male,
            Qualification = "M.A English Literature",
            Specialization = "English Grammar, Literature",
            JoiningDate = new DateTime(2016, 7, 1),
            DepartmentId = englishDept.Id,
            Salary = 45000
        };
        await context.Teachers.AddRangeAsync(teacher1, teacher2, teacher3, teacher4);

        // Classes
        var class10 = new Class { Name = "Class 10", AcademicYear = 2025 };
        var class11 = new Class { Name = "Class 11", AcademicYear = 2025 };
        var class12 = new Class { Name = "Class 12", AcademicYear = 2025 };
        await context.Classes.AddRangeAsync(class10, class11, class12);

        await context.SaveChangesAsync();

        // Sections
        var section10A = new Section { Name = "A", ClassId = class10.Id, ClassTeacherId = teacher1.Id, MaxStudents = 40 };
        var section10B = new Section { Name = "B", ClassId = class10.Id, ClassTeacherId = teacher2.Id, MaxStudents = 40 };
        var section11A = new Section { Name = "A", ClassId = class11.Id, ClassTeacherId = teacher3.Id, MaxStudents = 40 };
        var section12A = new Section { Name = "A", ClassId = class12.Id, ClassTeacherId = teacher4.Id, MaxStudents = 40 };
        await context.Sections.AddRangeAsync(section10A, section10B, section11A, section12A);

        // Subjects
        var mathSubject = new Subject { Name = "Mathematics", Code = "MATH10", Credits = 5, ClassId = class10.Id };
        var scienceSubject = new Subject { Name = "Science", Code = "SCI10", Credits = 5, ClassId = class10.Id };
        var englishSubject = new Subject { Name = "English", Code = "ENG10", Credits = 4, ClassId = class10.Id };
        var csSubject = new Subject { Name = "Computer Science", Code = "CS11", Credits = 5, ClassId = class11.Id };
        var physicsSubject = new Subject { Name = "Physics", Code = "PHY12", Credits = 5, ClassId = class12.Id };
        await context.Subjects.AddRangeAsync(mathSubject, scienceSubject, englishSubject, csSubject, physicsSubject);

        // Parents
        var parent1 = new Parent { UserId = parent1User.Id, FatherName = "Suresh Mehta", FatherPhone = "9876500001", Occupation = "Engineer" };
        var parent2 = new Parent { UserId = parent2User.Id, MotherName = "Kavita Gupta", MotherPhone = "9876500002", Occupation = "Doctor" };
        await context.Parents.AddRangeAsync(parent1, parent2);

        await context.SaveChangesAsync();

        // Students
        var student1 = new Student
        {
            UserId = student1User.Id, AdmissionNumber = "ADM2025001",
            AdmissionDate = new DateTime(2023, 6, 1), DateOfBirth = new DateTime(2008, 3, 15),
            Gender = Gender.Male, ClassId = class10.Id, SectionId = section10A.Id, ParentId = parent1.Id,
            Address = "789 Student Road", City = "Mumbai", State = "Maharashtra",
            BloodGroup = "O+", Nationality = "Indian"
        };
        var student2 = new Student
        {
            UserId = student2User.Id, AdmissionNumber = "ADM2025002",
            AdmissionDate = new DateTime(2023, 6, 1), DateOfBirth = new DateTime(2008, 7, 22),
            Gender = Gender.Female, ClassId = class10.Id, SectionId = section10A.Id, ParentId = parent2.Id,
            Address = "234 Student Lane", City = "Mumbai", State = "Maharashtra",
            BloodGroup = "A+", Nationality = "Indian"
        };
        var student3 = new Student
        {
            UserId = student3User.Id, AdmissionNumber = "ADM2025003",
            AdmissionDate = new DateTime(2022, 6, 1), DateOfBirth = new DateTime(2007, 11, 10),
            Gender = Gender.Male, ClassId = class11.Id, SectionId = section11A.Id,
            Address = "345 Green Street", City = "Pune", State = "Maharashtra",
            BloodGroup = "B+", Nationality = "Indian"
        };
        var student4 = new Student
        {
            UserId = student4User.Id, AdmissionNumber = "ADM2025004",
            AdmissionDate = new DateTime(2021, 6, 1), DateOfBirth = new DateTime(2006, 4, 5),
            Gender = Gender.Female, ClassId = class12.Id, SectionId = section12A.Id,
            Address = "567 Blue Avenue", City = "Nashik", State = "Maharashtra",
            BloodGroup = "AB+", Nationality = "Indian"
        };
        var student5 = new Student
        {
            UserId = student5User.Id, AdmissionNumber = "ADM2025005",
            AdmissionDate = new DateTime(2023, 6, 1), DateOfBirth = new DateTime(2008, 9, 18),
            Gender = Gender.Male, ClassId = class10.Id, SectionId = section10B.Id,
            Address = "890 Orange Road", City = "Mumbai", State = "Maharashtra",
            BloodGroup = "O-", Nationality = "Indian"
        };
        await context.Students.AddRangeAsync(student1, student2, student3, student4, student5);

        await context.SaveChangesAsync();

        // SubjectTeachers
        await context.SubjectTeachers.AddRangeAsync(
            new SubjectTeacher { SubjectId = mathSubject.Id, TeacherId = teacher2.Id, ClassId = class10.Id, IsPrimary = true },
            new SubjectTeacher { SubjectId = scienceSubject.Id, TeacherId = teacher3.Id, ClassId = class10.Id, IsPrimary = true },
            new SubjectTeacher { SubjectId = englishSubject.Id, TeacherId = teacher4.Id, ClassId = class10.Id, IsPrimary = true },
            new SubjectTeacher { SubjectId = csSubject.Id, TeacherId = teacher1.Id, ClassId = class11.Id, IsPrimary = true },
            new SubjectTeacher { SubjectId = physicsSubject.Id, TeacherId = teacher3.Id, ClassId = class12.Id, IsPrimary = true }
        );

        // Attendance for last 30 days
        var today = DateTime.UtcNow.Date;
        var attendances = new List<Attendance>();
        var students = new[] { student1, student2, student3, student4, student5 };
        var statuses = new[] { AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Present, AttendanceStatus.Absent, AttendanceStatus.Late };
        var rand = new Random(42);
        for (int d = 30; d >= 1; d--)
        {
            var date = today.AddDays(-d);
            if (date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday) continue;
            foreach (var s in students)
            {
                attendances.Add(new Attendance
                {
                    StudentId = s.Id,
                    ClassId = s.ClassId,
                    Date = date,
                    Status = statuses[rand.Next(statuses.Length)],
                    MarkedById = teacher1.UserId,
                    IsTeacherAttendance = false
                });
            }
        }
        await context.Attendances.AddRangeAsync(attendances);

        // Fee Structures
        var feeStructure10 = new FeeStructure
        {
            Name = "Class 10 - Tuition Fee", FeeType = "Tuition", Amount = 15000,
            ClassId = class10.Id, AcademicYear = 2025, DueDate = DateTime.UtcNow.AddDays(30),
            Frequency = "Quarterly", LateFine = 500
        };
        var feeStructure11 = new FeeStructure
        {
            Name = "Class 11 - Tuition Fee", FeeType = "Tuition", Amount = 18000,
            ClassId = class11.Id, AcademicYear = 2025, DueDate = DateTime.UtcNow.AddDays(30),
            Frequency = "Quarterly", LateFine = 500
        };
        var feeStructure12 = new FeeStructure
        {
            Name = "Class 12 - Tuition Fee", FeeType = "Tuition", Amount = 20000,
            ClassId = class12.Id, AcademicYear = 2025, DueDate = DateTime.UtcNow.AddDays(30),
            Frequency = "Quarterly", LateFine = 500
        };
        await context.FeeStructures.AddRangeAsync(feeStructure10, feeStructure11, feeStructure12);

        await context.SaveChangesAsync();

        // Fee Payments
        var payments = new List<FeePayment>
        {
            new() { StudentId = student1.Id, FeeStructureId = feeStructure10.Id, AmountPaid = 15000, TotalAmount = 15000, Status = FeeStatus.Paid, PaidAt = DateTime.UtcNow.AddDays(-10), PaymentMode = "Online", ReceiptNumber = "RCP001", CollectedById = accountantUser.Id },
            new() { StudentId = student2.Id, FeeStructureId = feeStructure10.Id, AmountPaid = 15000, TotalAmount = 15000, Status = FeeStatus.Paid, PaidAt = DateTime.UtcNow.AddDays(-5), PaymentMode = "Cash", ReceiptNumber = "RCP002", CollectedById = accountantUser.Id },
            new() { StudentId = student3.Id, FeeStructureId = feeStructure11.Id, AmountPaid = 9000, TotalAmount = 18000, Status = FeeStatus.Partial, PaymentMode = "Cheque", ReceiptNumber = "RCP003", CollectedById = accountantUser.Id },
            new() { StudentId = student4.Id, FeeStructureId = feeStructure12.Id, AmountPaid = 0, TotalAmount = 20000, Status = FeeStatus.Pending, CollectedById = accountantUser.Id },
            new() { StudentId = student5.Id, FeeStructureId = feeStructure10.Id, AmountPaid = 15000, TotalAmount = 15000, Status = FeeStatus.Paid, PaidAt = DateTime.UtcNow.AddDays(-3), PaymentMode = "Online", ReceiptNumber = "RCP004", CollectedById = accountantUser.Id },
        };
        await context.FeePayments.AddRangeAsync(payments);

        // Library Books
        var books = new List<LibraryBook>
        {
            new() { Title = "Introduction to Algorithms", Author = "Cormen", ISBN = "978-0262033848", Category = "Computer Science", TotalCopies = 5, AvailableCopies = 3, Price = 1500 },
            new() { Title = "Higher Mathematics", Author = "S.L. Loney", ISBN = "978-0230394116", Category = "Mathematics", TotalCopies = 8, AvailableCopies = 6, Price = 800 },
            new() { Title = "Concepts of Physics Vol 1", Author = "H.C. Verma", ISBN = "978-8177091878", Category = "Physics", TotalCopies = 10, AvailableCopies = 7, Price = 600 },
            new() { Title = "English Grammar in Use", Author = "Raymond Murphy", ISBN = "978-1107539334", Category = "English", TotalCopies = 6, AvailableCopies = 4, Price = 900 },
            new() { Title = "The Alchemist", Author = "Paulo Coelho", ISBN = "978-0062315007", Category = "Fiction", TotalCopies = 4, AvailableCopies = 4, Price = 350 },
            new() { Title = "Wings of Fire", Author = "A.P.J. Abdul Kalam", ISBN = "978-8173711466", Category = "Biography", TotalCopies = 3, AvailableCopies = 2, Price = 250 },
        };
        await context.LibraryBooks.AddRangeAsync(books);

        // Notifications
        var notifications = new List<Notification>
        {
            new() { Title = "Welcome to School ERP", Message = "Welcome to our new School ERP System! All your academic activities are now managed here.", Type = NotificationType.Announcement, SenderId = adminUser.Id, IsGlobal = true },
            new() { Title = "Mid-Term Exams Schedule", Message = "Mid-term examinations will be held from 15th July 2025. Please check your exam schedule.", Type = NotificationType.ExamUpdate, SenderId = principalUser.Id, IsGlobal = true },
            new() { Title = "Fee Due Reminder", Message = "Your quarterly fee payment is due. Please pay before 30th July to avoid late fine.", Type = NotificationType.FeeReminder, SenderId = accountantUser.Id, IsGlobal = true },
            new() { Title = "Independence Day Holiday", Message = "School will remain closed on 15th August 2025 on the occasion of Independence Day.", Type = NotificationType.Holiday, SenderId = adminUser.Id, IsGlobal = true },
            new() { Title = "Science Exhibition", Message = "Annual Science Exhibition will be held on 25th July 2025. All Class 10 and 11 students must participate.", Type = NotificationType.Event, SenderId = principalUser.Id, IsGlobal = true },
        };
        await context.Notifications.AddRangeAsync(notifications);

        // Timetables for Class 10 A
        var timetables = new List<Timetable>
        {
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = mathSubject.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeekEnum.Monday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), Room = "R101", AcademicYear = 2025 },
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = scienceSubject.Id, TeacherId = teacher3.Id, DayOfWeek = DayOfWeekEnum.Monday, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0), Room = "R102", AcademicYear = 2025 },
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = englishSubject.Id, TeacherId = teacher4.Id, DayOfWeek = DayOfWeekEnum.Tuesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), Room = "R101", AcademicYear = 2025 },
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = mathSubject.Id, TeacherId = teacher2.Id, DayOfWeek = DayOfWeekEnum.Wednesday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), Room = "R101", AcademicYear = 2025 },
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = scienceSubject.Id, TeacherId = teacher3.Id, DayOfWeek = DayOfWeekEnum.Thursday, StartTime = new TimeSpan(10, 0, 0), EndTime = new TimeSpan(11, 0, 0), Room = "Lab1", AcademicYear = 2025 },
            new() { ClassId = class10.Id, SectionId = section10A.Id, SubjectId = englishSubject.Id, TeacherId = teacher4.Id, DayOfWeek = DayOfWeekEnum.Friday, StartTime = new TimeSpan(9, 0, 0), EndTime = new TimeSpan(10, 0, 0), Room = "R101", AcademicYear = 2025 },
        };
        await context.Timetables.AddRangeAsync(timetables);

        // Assignments
        var assignment1 = new Assignment
        {
            Title = "Algebra Problem Set", Description = "Solve the given set of algebraic equations from Chapter 5.",
            SubjectId = mathSubject.Id, ClassId = class10.Id, SectionId = section10A.Id,
            TeacherId = teacher2.Id, DueDate = DateTime.UtcNow.AddDays(7), MaxMarks = 20
        };
        var assignment2 = new Assignment
        {
            Title = "Essay on Climate Change", Description = "Write a 500-word essay on the effects of climate change.",
            SubjectId = englishSubject.Id, ClassId = class10.Id, SectionId = section10A.Id,
            TeacherId = teacher4.Id, DueDate = DateTime.UtcNow.AddDays(5), MaxMarks = 15
        };
        await context.Assignments.AddRangeAsync(assignment1, assignment2);

        await context.SaveChangesAsync();

        // Exams & Marks
        var exam1 = new Exam
        {
            Name = "Mid-Term Mathematics", ExamType = ExamType.MidTerm,
            SubjectId = mathSubject.Id, ClassId = class10.Id, SectionId = section10A.Id,
            ExamDate = DateTime.UtcNow.AddDays(15), StartTime = new TimeSpan(10, 0, 0),
            EndTime = new TimeSpan(12, 0, 0), TotalMarks = 100, PassingMarks = 35, AcademicYear = 2025, Venue = "Hall A"
        };
        await context.Exams.AddAsync(exam1);
        await context.SaveChangesAsync();

        await context.SaveChangesAsync();
    }
}
