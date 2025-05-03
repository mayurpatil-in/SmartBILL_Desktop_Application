namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPartyChallanItems : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartyChallanItems",
                c => new
                    {
                        PartyChallanItemId = c.Int(nullable: false, identity: true),
                        PartychId = c.Int(nullable: false),
                        Quantity = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartyChallanItemId)
                .ForeignKey("dbo.PartyChallans", t => t.PartychId, cascadeDelete: true)
                .Index(t => t.PartychId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyChallanItems", "PartychId", "dbo.PartyChallans");
            DropIndex("dbo.PartyChallanItems", new[] { "PartychId" });
            DropTable("dbo.PartyChallanItems");
        }
    }
}
