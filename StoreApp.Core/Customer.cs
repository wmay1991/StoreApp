using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StoreApp.Core
{
    public class Customer
    {
        [Key]
        public Guid customer_id { get; set; }

        public bool isActive { get; set; }

        public string name { get; set; }

        public string billing_address { get; set; }

        public string billing_city { get; set; }
        public string billing_state { get; set; }

        // could have dashes so leaving it a string
        public string billing_zip { get; set; }

        // a customer can have many orders
        public IList<Order> order {get;set;}
    }
}
