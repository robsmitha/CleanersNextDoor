using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
        public string ImageUrl { get; set; }
        public string DisplayName { get; set; }
        public bool EmailVerified { get; set; }
        public int UserStatusTypeID { get; set; }
        [ForeignKey("UserStatusTypeID")]
        public UserStatusType UserStatusType { get; set; }
    }
}
