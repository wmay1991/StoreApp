using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Core
{
    public class Item
    {
        public Item()
        {
          
        }

        [Key]
        public Guid item_id { get; set; }

        public string name { get; set; }

        // decimals are best for financial information because it is easier to round
        public decimal price { get; set; }

        public int quantity { get; set; }


        public void CalculateItemQuantity(Order order, Item item, string action)
        {
            if (action == "Add")
            {
                item.quantity -= order.quantity;
            }
            else if (action == "Delete")
            {
                item.quantity += order.quantity;
            }

        }
    }
}
