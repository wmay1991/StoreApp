using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Core
{
    public class Order
    {
        [Key]
        public Guid order_id { get; set; }

        public string street_address { get; set; }

        public string city { get; set; }

        public string state { get; set; }

        public DateTime? order_date { get; set; }

        public DateTime? shipping_date { get; set; }


        public decimal total_price { get; set; }

        public int quantity { get; set; }

        public Guid item_id { get; set; }

        public Guid customer_id { get; set; }

        public virtual Item item {get;set;}

        public virtual Customer customer { get; set; }
    }
}
