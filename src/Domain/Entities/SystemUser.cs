namespace Domain.Entities
{
    public class SystemUser : User
    {
        public string EmployeeId{get;set;}=string.Empty;
        public string Department {get;set;}=string.Empty;
        
        public DateTime HireDate{get;set;}
        //Navigation properties
        public virtual ICollection<Ticket>? AssignedTickets {get;set;}

    }
}