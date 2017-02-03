namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Customer : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        customer_id = c.Guid(nullable: false),
                        isActive = c.Boolean(nullable: false),
                        name = c.String(),
                        billing_address = c.String(),
                        billing_city = c.String(),
                        billing_state = c.String(),
                        billing_zip = c.String(),
                    })
                .PrimaryKey(t => t.customer_id);
            
            //AddColumn("dbo.Orders", "customer_id", c => c.Guid(nullable: false));
            //AddColumn("dbo.Orders", "customer_customer_id", c => c.Guid());
            //CreateIndex("dbo.Orders", "customer_id");
            //AddForeignKey("dbo.Orders", "customer_id", "dbo.Customers", "customer_id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Orders", "customer_customer_id", "dbo.Customers");
            DropIndex("dbo.Orders", new[] { "customer_customer_id" });
            DropColumn("dbo.Orders", "customer_customer_id");
            DropColumn("dbo.Orders", "cust_id");
            DropTable("dbo.Customers");
        }
    }
}
