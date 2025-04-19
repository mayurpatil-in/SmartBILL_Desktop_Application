namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustUsers",
                c => new
                    {
                        CustomerId = c.Int(nullable: false, identity: true),
                        Title = c.String(),
                        Company = c.String(),
                        Mobile = c.String(),
                        GstNo = c.String(),
                        House = c.String(),
                        Place = c.String(),
                        Tal = c.String(),
                        Dist = c.String(),
                        State = c.String(),
                        Pincode = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustUsers");
        }
    }
}
