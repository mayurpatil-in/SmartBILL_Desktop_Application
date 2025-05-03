namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPartyChallanItemsRelItems : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PartyChallanItems", name: "PartychId", newName: "PartyChallanId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "ProcessId", newName: "ProcessItemId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_PartychId", newName: "IX_PartyChallanId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_ProcessId", newName: "IX_ProcessItemId");
            AddColumn("dbo.PartyChallanItems", "ItemId", c => c.Int(nullable: false));
            CreateIndex("dbo.PartyChallanItems", "ItemId");
            AddForeignKey("dbo.PartyChallanItems", "ItemId", "dbo.Items", "ItemId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyChallanItems", "ItemId", "dbo.Items");
            DropIndex("dbo.PartyChallanItems", new[] { "ItemId" });
            DropColumn("dbo.PartyChallanItems", "ItemId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_ProcessItemId", newName: "IX_ProcessId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_PartyChallanId", newName: "IX_PartychId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "ProcessItemId", newName: "ProcessId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "PartyChallanId", newName: "PartychId");
        }
    }
}
