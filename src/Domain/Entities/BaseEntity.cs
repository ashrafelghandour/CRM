using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public abstract class  BaseEntity
    {
        public int Id {get; set;}
        public DateTime CreatedAt{get; set;} = DateTime.Now;
        public DateTime UpdateAt {get;set;}
        public bool IsActive {get;set;}

    }
}