namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveInventory : DbMigration
    {
        public override void Up()
        {
            //DropForeignKey("dbo.Inventories", "item_id", "dbo.Items");
            //DropIndex("dbo.Inventories", new[] { "item_id" });
            ////AddColumn("dbo.Items", "quantity", c => c.Int(nullable: false));
            //DropTable("dbo.Inventories");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Inventories",
                c => new
                    {
                        inv_id = c.Guid(nullable: false),
                        quantity = c.Int(nullable: false),
                        item_id = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.inv_id);
            
            DropColumn("dbo.Items", "quantity");
            CreateIndex("dbo.Inventories", "item_id");
            AddForeignKey("dbo.Inventories", "item_id", "dbo.Items", "item_id", cascadeDelete: true);
        }
    }
}
