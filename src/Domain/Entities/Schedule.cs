namespace Domain.Entities
{
    public class Schedule : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string Location { get; set; } = string.Empty;
    
    // Foreign keys
    public int CreatedById { get; set; }
    public int? AssignedToId { get; set; }
    public int? CustomerId { get; set; }
    
    // Navigation properties
    public virtual User CreatedBy { get; set; } = null!;
    public virtual SystemUser? AssignedTo { get; set; }
    public virtual Customer? Customer { get; set; }
}
}