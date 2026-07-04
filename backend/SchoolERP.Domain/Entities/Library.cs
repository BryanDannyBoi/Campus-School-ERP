using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class LibraryBook : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Author { get; set; } = string.Empty;
    public string? ISBN { get; set; }
    public string? Publisher { get; set; }
    public int? PublishedYear { get; set; }
    public string? Category { get; set; }
    public string? Language { get; set; } = "English";
    public int TotalCopies { get; set; } = 1;
    public int AvailableCopies { get; set; } = 1;
    public decimal? Price { get; set; }
    public string? Location { get; set; }
    public string? CoverImage { get; set; }
    public BookStatus Status { get; set; } = BookStatus.Available;
    public bool IsActive { get; set; } = true;

    // Navigation
    public ICollection<BookIssue> BookIssues { get; set; } = new List<BookIssue>();
}

public class BookIssue : BaseEntity
{
    public Guid BookId { get; set; }
    public Guid StudentId { get; set; }
    public DateTime IssuedDate { get; set; }
    public DateTime DueDate { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public decimal? Fine { get; set; }
    public bool IsReturned { get; set; } = false;
    public Guid IssuedById { get; set; }
    public string? Remarks { get; set; }

    // Navigation
    public LibraryBook? Book { get; set; }
    public Student? Student { get; set; }
}
