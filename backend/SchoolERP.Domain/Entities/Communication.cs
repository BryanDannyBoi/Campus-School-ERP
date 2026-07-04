using SchoolERP.Domain.Common;
using SchoolERP.Domain.Enums;

namespace SchoolERP.Domain.Entities;

public class Notification : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public Guid? SenderId { get; set; }
    public string? TargetRole { get; set; } // null = all
    public Guid? TargetUserId { get; set; }
    public Guid? TargetClassId { get; set; }
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public DateTime? ScheduledAt { get; set; }
    public bool IsGlobal { get; set; } = false;

    // Navigation
    public User? Sender { get; set; }
    public Class? TargetClass { get; set; }
}

public class Message : BaseEntity
{
    public Guid SenderId { get; set; }
    public Guid ReceiverId { get; set; }
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public bool IsRead { get; set; } = false;
    public DateTime? ReadAt { get; set; }
    public Guid? ParentMessageId { get; set; }
    public string? AttachmentPath { get; set; }

    // Navigation
    public User? Sender { get; set; }
    public User? Receiver { get; set; }
    public Message? ParentMessage { get; set; }
    public ICollection<Message> Replies { get; set; } = new List<Message>();
}

public class AuditLog : BaseEntity
{
    public Guid? UserId { get; set; }
    public string Action { get; set; } = string.Empty;
    public string EntityName { get; set; } = string.Empty;
    public string? EntityId { get; set; }
    public string? OldValues { get; set; }
    public string? NewValues { get; set; }
    public string? IpAddress { get; set; }
    public string? UserAgent { get; set; }

    // Navigation
    public User? User { get; set; }
}
