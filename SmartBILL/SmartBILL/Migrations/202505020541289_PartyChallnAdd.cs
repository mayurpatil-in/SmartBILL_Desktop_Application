namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PartyChallnAdd : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PartyChallans",
                c => new
                    {
                        PartyChId = c.Int(nullable: false, identity: true),
                        PartyChNo = c.Int(nullable: false),
                        PartyDate = c.DateTime(nullable: false),
                        WorkDay = c.Int(nullable: false),
                        CustomerPId = c.Int(nullable: false),
                        YearId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PartyChId)
                .ForeignKey("dbo.CustParties", t => t.CustomerPId, cascadeDelete: true)
                .ForeignKey("dbo.YearAccounts", t => t.YearId, cascadeDelete: true)
                .Index(t => t.CustomerPId)
                .Index(t => t.YearId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PartyChallans", "YearId", "dbo.YearAccounts");
            DropForeignKey("dbo.PartyChallans", "CustomerPId", "dbo.CustParties");
            DropIndex("dbo.PartyChallans", new[] { "YearId" });
            DropIndex("dbo.PartyChallans", new[] { "CustomerPId" });
            DropTable("dbo.PartyChallans");
        }
    }
}
