namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPartyChallanItemsRelUpdate : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.PartyChallanItems", name: "PartyChallanId", newName: "PartychId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "ProcessItemId", newName: "ProcessId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_PartyChallanId", newName: "IX_PartychId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_ProcessItemId", newName: "IX_ProcessId");
            AddColumn("dbo.PartyChallanItems", "CustomerPId", c => c.Int(nullable: false));
            AddColumn("dbo.PartyChallanItems", "YearId", c => c.Int(nullable: false));
            CreateIndex("dbo.PartyChallanItems", "CustomerPId");
            CreateIndex("dbo.PartyChallanItems", "YearId");
            AddForeignKey("dbo.PartyChallanItems", "CustomerPId", "dbo.CustParties", "CustomerPId", cascadeDelete: false);
            AddForeignKey("dbo.PartyChallanItems", "YearId", "dbo.YearAccounts", "YearId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyChallanItems", "YearId", "dbo.YearAccounts");
            DropForeignKey("dbo.PartyChallanItems", "CustomerPId", "dbo.CustParties");
            DropIndex("dbo.PartyChallanItems", new[] { "YearId" });
            DropIndex("dbo.PartyChallanItems", new[] { "CustomerPId" });
            DropColumn("dbo.PartyChallanItems", "YearId");
            DropColumn("dbo.PartyChallanItems", "CustomerPId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_ProcessId", newName: "IX_ProcessItemId");
            RenameIndex(table: "dbo.PartyChallanItems", name: "IX_PartychId", newName: "IX_PartyChallanId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "ProcessId", newName: "ProcessItemId");
            RenameColumn(table: "dbo.PartyChallanItems", name: "PartychId", newName: "PartyChallanId");
        }
    }
}
