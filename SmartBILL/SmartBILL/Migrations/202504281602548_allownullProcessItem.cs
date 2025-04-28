namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class allownullProcessItem : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.ProcessItems", "ProcessName", c => c.String(maxLength: 100));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.ProcessItems", "ProcessName", c => c.String(nullable: false, maxLength: 100));
        }
    }
}
