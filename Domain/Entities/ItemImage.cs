using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain.Entities
{
    public class ItemImage : BaseEntity
    {
        public string ImageUrl { get; set; }
        public int ItemID { get; set; }
        [ForeignKey("ItemID")]
        public Item Item { get; set; }
        public bool IsDefault { get; set; }
    }
}
