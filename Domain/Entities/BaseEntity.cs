using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class BaseEntity
    {
        public int ID { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Active { get; set; }
        public DateTime? ModifiedTime { get; set; }
        public BaseEntity()
        {
            CreatedAt = DateTime.Now;
            Active = true;
        }
    }
}
