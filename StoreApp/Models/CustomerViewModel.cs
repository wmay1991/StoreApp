using StoreApp.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StoreApp.Models
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {

        }

        public CustomerViewModel(Customer model)
        {
            customer_id = model.customer_id;
            isActive = model.isActive;
            name = model.name;
            billing_address = model.billing_address;
            billing_city = model.billing_city;
            billing_state = model.billing_state;
            billing_zip = model.billing_zip;
        }


        public CustomerViewModel(Customer model, CustomerViewModel vm)
        {
            model.customer_id = vm.customer_id;
            model.isActive = vm.isActive;
            model.name = vm.name;
            model.billing_address = vm.billing_address;
            model.billing_city = vm.billing_city;
            model.billing_state = vm.billing_state;
            model.billing_zip = vm.billing_zip;
        }

        public Guid customer_id { get; set; }

        public bool isActive { get; set; }

        public string name { get; set; }

        public string billing_address { get; set; }

        public string billing_city { get; set; }
        public string billing_state { get; set; }

        // could have dashes so leaving it a string
        public string billing_zip { get; set; }

        // a customer can have many orders
        public IList<Order> order { get; set; }
    }
}