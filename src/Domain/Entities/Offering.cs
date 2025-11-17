namespace Domain.Entities
{
    public class Offering : BaseEntity
    {
         public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsActive { get; set; } = true;
    public string CreatedBy { get; set; } = string.Empty;
    
    // Foreign keys
    public int? ProductId { get; set; }
    
    // Navigation properties
    public virtual Product? Product { get; set; }
    }
}