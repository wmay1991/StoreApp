namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddForeignKeyToOrders : DbMigration
    {
        public override void Up()
        {

            AddForeignKey("dbo.Orders", "customer_id", "dbo.Customers", "customer_id", cascadeDelete: true);
        }

        public override void Down()
        {
            AddColumn("dbo.Orders", "cust_id", c => c.Guid(nullable: false));
            DropForeignKey("dbo.Orders", "customer_id", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "customer_id" });
            AlterColumn("dbo.Orders", "customer_id", c => c.Guid());
            RenameColumn(table: "dbo.Orders", name: "customer_id", newName: "customer_customer_id");
            CreateIndex("dbo.Orders", "customer_customer_id");
            AddForeignKey("dbo.Orders", "customer_customer_id", "dbo.Customers", "customer_id");
        }
    }
}
