namespace Domain.Entities
{
    public class Product : BaseEntity
    {
        public string Name {get;set;}=string.Empty;
        public string Description{get;set;}=string.Empty;
        public decimal price {get;set;}
        public int StockQuantity {get;set;}
        public string Category { get; set; } = string.Empty;
        public string CreatedBy { get; set; } = string.Empty;
    
    // Navigation properties
         public virtual ICollection<Offering>? Offerings { get; set; }
    }

}