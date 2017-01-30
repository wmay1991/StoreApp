namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inventory : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                {
                    inv_id = c.Guid(nullable: false),
                    quantity = c.Int(nullable: false),
                    item_id = c.Guid(nullable: false),

                })
                .PrimaryKey(t => t.inv_id)
               .ForeignKey("dbo.Item", t => t.item_id, cascadeDelete: true);

            CreateTable(
           "dbo.Item",
           c => new
           {
               item_id = c.Guid(nullable: false),
               item_name = c.String(maxLength: 4000),
               item_price = c.Decimal(nullable: false, precision: 18, scale: 2),
           })
           .PrimaryKey(t => t.item_id);


            DropTable("dbo.Stores");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Stores",
                c => new
                    {
                        store_id = c.Guid(nullable: false),
                        name = c.String(maxLength: 4000),
                        address = c.String(maxLength: 4000),
                        phone = c.String(maxLength: 4000),
                    })
                .PrimaryKey(t => t.store_id);
            
            DropTable("dbo.Inventories");
        }
    }
}
