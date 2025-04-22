namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ForYear : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.YearAccounts", "StartDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.YearAccounts", "EndDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.YearAccounts", "EndDate", c => c.DateTime());
            AlterColumn("dbo.YearAccounts", "StartDate", c => c.DateTime());
        }
    }
}
