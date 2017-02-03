using StoreApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class ItemViewModel 
    {
        public ItemViewModel()
        {
            item_id = Guid.NewGuid();
        }

        public ItemViewModel(Item item)
        {
            item_id = item.item_id;
            name = item.name;
            price = item.price;
            quantity = item.quantity;
        }

        public ItemViewModel(ItemViewModel vm, Item model)
        {
            model.item_id = vm.item_id;
            model.name = vm.name;
            model.price = vm.price;
            model.quantity = vm.quantity;
        }

        public Guid item_id { get; set; }

        [Required]
        [DisplayName("Item Name")]
        public string name { get; set; }

        [Required]
        [DisplayName("Price Per Unit")]
        public decimal price { get; set; }

        [Required]
        [DisplayName("Total Quantity in Stock")]
        public int quantity { get; set; }
    }
}