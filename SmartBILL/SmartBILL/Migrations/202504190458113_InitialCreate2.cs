namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate2 : DbMigration
    {
        public override void Up()
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
        
        public override void Down()
        {
            DropTable("dbo.CustomerUsers");
        }
    }
}
