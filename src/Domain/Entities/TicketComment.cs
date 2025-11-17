namespace Domain.Entities
{
    public class TicketComment : BaseEntity
{
    public string Comment { get; set; } = string.Empty;
    
    public int TicketId { get; set; }
    public int UserId { get; set; }
    
    public virtual Ticket Ticket { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
}