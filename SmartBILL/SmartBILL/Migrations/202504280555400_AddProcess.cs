namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProcess : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProcessItems",
                c => new
                    {
                        ProcessId = c.Int(nullable: false, identity: true),
                        ProcessName = c.String(nullable: false, maxLength: 100),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.ProcessId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ProcessItems");
        }
    }
}
