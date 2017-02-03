namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddOrders : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        order_id = c.Guid(nullable: false),
                        street_address = c.String(maxLength: 4000),
                        city = c.String(maxLength: 4000),
                        state = c.String(maxLength: 4000),
                        order_date = c.DateTime(nullable: false),
                        shipping_date = c.DateTime(nullable: false),
                        total_price = c.Decimal(nullable: false, precision: 18, scale: 2),
                        quantity = c.Int(nullable: false),
                        item_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.order_id)
                .ForeignKey("dbo.Items", t => t.item_id, cascadeDelete: true)
                .Index(t => t.item_id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "item_id", "dbo.Items");
            DropIndex("dbo.Orders", new[] { "item_id" });
            DropTable("dbo.Orders");
        }
    }
}
