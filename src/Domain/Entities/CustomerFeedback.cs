namespace Domain.Entities
{
    public class CustomerFeedback : BaseEntity
    {
        public string Feedbacks {get;set;} = string.Empty;
        public short Rating {get;set;}
        public DateTime FeedbackDate {get;set;}
         // Foreign keys
    public int CustomerId { get; set; }
    
    // Navigation properties
    public virtual Customer Customer { get; set; } = null!;
    }
}