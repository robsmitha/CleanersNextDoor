using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Authorization : BaseEntity
    {
        public string Last4 { get; set; }
        public string Note { get; set; }
        public string CardType { get; set; }
        public string AuthorizationCode { get; set; }
        public string Token { get; set; }
        public int AuthorizationTypeID { get; set; }

        [ForeignKey("AuthorizationTypeID")]
        public AuthorizationType AuthorizationType { get; set; }
    }
}
