namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate4 : DbMigration
    {
        public override void Up()
        {
            DropTable("dbo.CustomerUsers");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.CustomerUsers",
                c => new
                    {
                        CustUserId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        CompanyName = c.String(),
                        Mobile = c.String(),
                        GstNo = c.String(),
                        HouseNo = c.String(),
                        Place = c.String(),
                        Tal = c.String(),
                        Dist = c.String(),
                        State = c.String(),
                        PinCode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustUserId);
            
        }
    }
}
