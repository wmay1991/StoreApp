namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Item : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        item_id = c.Guid(nullable: false),
                        name = c.String(maxLength: 4000),
                        price = c.Decimal(nullable: false, precision: 18, scale: 2),
                    })
                .PrimaryKey(t => t.item_id);
            
            CreateIndex("dbo.Inventories", "item_id");
            AddForeignKey("dbo.Inventories", "item_id", "dbo.Items", "item_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            AddColumn("dbo.Inventories", "item_price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.Inventories", "item_name", c => c.String(maxLength: 4000));
            AddColumn("dbo.Inventories", "item_item_id", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Inventories", "item_id", "dbo.Items");
            DropIndex("dbo.Inventories", new[] { "item_id" });
            DropTable("dbo.Items");
        }
    }
}
