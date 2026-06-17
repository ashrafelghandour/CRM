using Domain.Enums;

namespace Domain.Entities
{
    public class Ticket : BaseEntity
        {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public TicketStatus Status { get; set; } = TicketStatus.Open;
        public TicketPriority Priority { get; set; } = TicketPriority.Medium;
        
        // Foreign keys
        public int CustomerId { get; set; }
        public int? AssignedToId { get; set; }
        public int CreatedById { get; set; }
        
        // Navigation properties
        public  Customer Customer { get; set; } = null!;
        public  SystemUser? AssignedTo { get; set; }
        public  User CreatedBy { get; set; } = null!;
        public  ICollection<TicketComment>? Comments { get; set; }

    }
}