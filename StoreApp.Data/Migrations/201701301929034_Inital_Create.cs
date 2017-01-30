namespace StoreApp.Data.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Inital_Create : DbMigration
    {
        public override void Up()
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
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Stores");
        }
    }
}
