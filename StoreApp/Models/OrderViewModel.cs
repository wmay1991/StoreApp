using StoreApp.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class OrderViewModel
    {
        public OrderViewModel()
        {

        }

        public OrderViewModel(Order order)
        {
            order_id = order.order_id;
            street_address = order.street_address;
            city = order.city;
            state = order.state;
            order_date = order.order_date;
            shipping_date = order.shipping_date;
            total_price = order.total_price;
            quantity = order.quantity;
            item_id = order.item_id;
            item = order.item;
            customer_id = order.customer_id;
        }


        public OrderViewModel(Order order, OrderViewModel order_vm)
        {

            order.order_id = order_vm.order_id;
            order.street_address = order_vm.street_address;
            order.city = order_vm.city;
            order.state = order_vm.state;
            order.order_date = order_vm.order_date;
            order.shipping_date = order_vm.shipping_date;
            order.total_price = order_vm.total_price;
            order.quantity = order_vm.quantity;
            order.item_id = order_vm.item_id;
            order.customer_id = order_vm.customer_id;
            //order.item = order_vm.item;
        }

        public Guid order_id { get; set; }

        public string street_address { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        [DataType(DataType.Date)]
        public DateTime? order_date { get; set; }

        [DataType(DataType.Date)]
        public DateTime? shipping_date { get; set; }


        public decimal total_price { get; set; }

        public int quantity { get; set; }

        public Guid item_id { get; set; }

        public Guid customer_id { get; set; }

        public Item item { get; set; }
    }
}