namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeRelCustpAndItem : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Items", "CustomerPId", c => c.Int(nullable: false));
            CreateIndex("dbo.Items", "CustomerPId");
            AddForeignKey("dbo.Items", "CustomerPId", "dbo.CustParties", "CustomerPId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Items", "CustomerPId", "dbo.CustParties");
            DropIndex("dbo.Items", new[] { "CustomerPId" });
            DropColumn("dbo.Items", "CustomerPId");
        }
    }
}
