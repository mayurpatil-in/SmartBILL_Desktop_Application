namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddItem : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Items",
                c => new
                    {
                        ItemId = c.Int(nullable: false, identity: true),
                        ItemName = c.String(nullable: false, maxLength: 100),
                        ItemRate = c.Decimal(nullable: false, storeType: "money"),
                        PopNo = c.String(maxLength: 50),
                        HsnCode = c.String(maxLength: 50),
                        CastWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ScrapWeight = c.Decimal(nullable: false, precision: 18, scale: 2),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ItemId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Items");
        }
    }
}
