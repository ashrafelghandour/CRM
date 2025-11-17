using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Enums;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username {get;set;} = string.Empty;
        public string Email {get;set;} =string.Empty;
        public string PassworedHash {get;set;}=string.Empty;
        public string FirstName{get;set;}=string.Empty;
        public string LastName {get;set;}=string.Empty;
        public string Phone = string.Empty;
        public UserRole Role  {get;set;}
        public DateTime? LastLogin{get;set;}
    }
}