using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class DrivingSession : BaseEntity
    {
        public int UserID { get; set; }
        [ForeignKey("UserID")]
        public User User { get; set; }
        public string Note { get; set; }
    }
}
