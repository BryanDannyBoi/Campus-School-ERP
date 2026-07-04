using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Parent : BaseEntity
{
    public Guid UserId { get; set; }
    public string? Occupation { get; set; }
    public string? Address { get; set; }
    public string? AnnualIncome { get; set; }
    public string? FatherName { get; set; }
    public string? FatherPhone { get; set; }
    public string? FatherOccupation { get; set; }
    public string? MotherName { get; set; }
    public string? MotherPhone { get; set; }
    public string? MotherOccupation { get; set; }

    // Navigation
    public User? User { get; set; }
    public ICollection<Student> Students { get; set; } = new List<Student>();
}
