using StoreApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ItemViewModel 
    {
        public ItemViewModel(Item item)
        {
            item_id = item.item_id;
            name = item.name;
            price = item.price;
        }

        public Guid item_id { get; set; }

        public string name { get; set; }

        public decimal price { get; set; }
    }
}