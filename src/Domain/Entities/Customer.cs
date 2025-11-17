namespace Domain.Entities
{
    public class Customer : BaseEntity
    {     
        public string Name{get;set;}=string.Empty;
        public string Email {get;set;} =string.Empty;
        public string Phone = string.Empty;
        public string Address {get;set;}=string.Empty;
        public string CompanyName{get;set;} = string.Empty;
        public string CreatedBy {get;set;}=string.Empty;

        public virtual ICollection<CustomerFeedback>? Feedbacks{get;set;}
        public virtual ICollection<Ticket>? Tickets{get;set;}
    }
}