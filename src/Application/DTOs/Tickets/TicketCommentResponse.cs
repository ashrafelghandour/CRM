using Domain.Enums;

namespace Application.DTOs.Tickets;

public class TicketResponse
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }
    public TicketPriority Priority { get; set; }
    public int CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public int? AssignedToId { get; set; }
    public string? AssignedToName { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TicketCommentResponse> Comments { get; set; } = new();
}

public class TicketCommentResponse
{
    public int Id { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string UserName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}