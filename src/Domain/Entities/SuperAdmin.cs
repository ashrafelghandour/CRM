using Domain.Enums;

namespace Domain.Entities
{
    public class SuperAdmin : User
    {
        public SuperAdmin()
        {
            Role = UserRole.SuperAdmin;
        }
    }
}