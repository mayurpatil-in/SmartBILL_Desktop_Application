namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAccountingYearP : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.YearAccounts",
                c => new
                    {
                        YearId = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.YearId);
            
            DropTable("dbo.SelectYears");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.SelectYears",
                c => new
                    {
                        yearId = c.Int(nullable: false, identity: true),
                        StartDate = c.DateTime(),
                        EndDate = c.DateTime(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.yearId);
            
            DropTable("dbo.YearAccounts");
        }
    }
}
