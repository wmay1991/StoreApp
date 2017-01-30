﻿using StoreApp.Core;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreApp.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext():base("StoreApp")
        {

        }

        public virtual DbSet<Inventory> Inventory { get; set; }

        public virtual DbSet<Item> Items { get; set; }
    }
}
