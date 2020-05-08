using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Application.Merchants.Queries.GetMerchantItems
{
    public class GetMerchantItemsResponse 
    {
        public GetMerchantItemsResponse(List<GetMerchantItemModel> items = null)
        {
            Items = items ?? new List<GetMerchantItemModel>();
        }
        public List<GetMerchantItemModel> Items { get; set; }


        public List<ItemType> ItemTypes => Items == null || !Items.Any()
            ? new List<ItemType>()
            : Items.GroupBy(it => it.ItemTypeID)
            .Select(g => new ItemType { ID = g.Key, Name = g.First().ItemTypeName })
            .ToList();

        public class ItemType
        {
            public int ID { get; set; }
            public string Name { get; set; }
        }

    }
}
