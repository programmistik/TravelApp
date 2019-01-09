namespace TravelApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration0901 : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.Users", new[] { "Login" });
            CreateTable(
                "dbo.CheckItems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ItemName = c.String(),
                        ItemStatus = c.Boolean(nullable: false),
                        Trip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.Trip_Id)
                .Index(t => t.Trip_Id);
            
            CreateTable(
                "dbo.Cities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityName = c.String(),
                        Latitude = c.String(),
                        Longitude = c.String(),
                        CountryName = c.String(),
                        CountryCode = c.String(),
                        Currency = c.String(),
                        Mayor = c.String(),
                        TimeZoneShortName = c.String(),
                        TimeZone = c.Int(nullable: false),
                        ImageUri = c.String(),
                        Trip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.Trip_Id)
                .Index(t => t.Trip_Id);
            
            CreateTable(
                "dbo.Tickets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TicketName = c.String(),
                        TicketUri = c.String(),
                        Trip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Trips", t => t.Trip_Id)
                .Index(t => t.Trip_Id);
            
            AddColumn("dbo.Trips", "DepartureDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Trips", "ArrivalDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.Cities", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.CheckItems", "Trip_Id", "dbo.Trips");
            DropIndex("dbo.Tickets", new[] { "Trip_Id" });
            DropIndex("dbo.Cities", new[] { "Trip_Id" });
            DropIndex("dbo.CheckItems", new[] { "Trip_Id" });
            DropColumn("dbo.Trips", "ArrivalDate");
            DropColumn("dbo.Trips", "DepartureDate");
            DropTable("dbo.Tickets");
            DropTable("dbo.Cities");
            DropTable("dbo.CheckItems");
            CreateIndex("dbo.Users", "Login", unique: true);
        }
    }
}
