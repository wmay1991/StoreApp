namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DateTimeUpdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Orders", "order_date", c => c.DateTime());
            AlterColumn("dbo.Orders", "shipping_date", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Orders", "shipping_date", c => c.DateTime(nullable: false));
            AlterColumn("dbo.Orders", "order_date", c => c.DateTime(nullable: false));
        }
    }
}
