using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class FeeStructure : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? ClassId { get; set; }
    public decimal Amount { get; set; }
    public string FeeType { get; set; } = string.Empty; // Tuition, Transport, Library, Lab, etc.
    public int AcademicYear { get; set; }
    public string? Frequency { get; set; } // Monthly, Quarterly, Annual
    public DateTime DueDate { get; set; }
    public decimal? LateFine { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation
    public Class? Class { get; set; }
    public ICollection<FeePayment> Payments { get; set; } = new List<FeePayment>();
}

public class FeePayment : BaseEntity
{
    public Guid StudentId { get; set; }
    public Guid FeeStructureId { get; set; }
    public decimal AmountPaid { get; set; }
    public decimal TotalAmount { get; set; }
    public decimal? Discount { get; set; }
    public decimal? Fine { get; set; }
    public FeeStatus Status { get; set; }
    public string? ReceiptNumber { get; set; }
    public DateTime? PaidAt { get; set; }
    public string? PaymentMode { get; set; } // Cash, Online, Cheque, DD
    public string? TransactionId { get; set; }
    public string? Remarks { get; set; }
    public Guid CollectedById { get; set; }

    // Navigation
    public Student? Student { get; set; }
    public FeeStructure? FeeStructure { get; set; }
}
