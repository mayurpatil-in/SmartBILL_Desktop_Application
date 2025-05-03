namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPartyChallanItemsRelProcessItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PartyChallanItems", "ProcessId", c => c.Int(nullable: false));
            CreateIndex("dbo.PartyChallanItems", "ProcessId");
            AddForeignKey("dbo.PartyChallanItems", "ProcessId", "dbo.ProcessItems", "ProcessId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyChallanItems", "ProcessId", "dbo.ProcessItems");
            DropIndex("dbo.PartyChallanItems", new[] { "ProcessId" });
            DropColumn("dbo.PartyChallanItems", "ProcessId");
        }
    }
}
