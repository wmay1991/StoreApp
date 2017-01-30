using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Core
{
    public class Inventory
    {
        [Key]
        public Guid inv_id { get; set; }
        public int quantity { get; set; }

        public Guid item_id { get; set; }
        public virtual Item item { get; set; }
        //public Guid store_id { get;set }
    }
}
