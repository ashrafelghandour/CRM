
using Domain.Enums;

public class CreateTicketRequest
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public int CustomerId { get; set; }
    public int? AssignedToId { get; set; }
}