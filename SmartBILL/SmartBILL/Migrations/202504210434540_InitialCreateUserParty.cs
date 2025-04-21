namespace SmartBILL.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreateUserParty : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CustParties",
                c => new
                    {
                        CustomerPId = c.Int(nullable: false, identity: true),
                        TitleP = c.String(),
                        CompanyP = c.String(),
                        MobileP = c.String(),
                        GstNoP = c.String(),
                        HouseP = c.String(),
                        PlaceP = c.String(),
                        TalP = c.String(),
                        DistP = c.String(),
                        StateP = c.String(),
                        PincodeP = c.String(),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CustomerPId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.CustParties");
        }
    }
}
