using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class RolePermission : BaseEntity
    {
        public int RoleID { get; set; }
        public int PermissionID { get; set; }
        [ForeignKey("PermissionID")]
        public Permission Permission { get; set; }
        [ForeignKey("RoleID")]
        public Role Role { get; set; }
    }
}
