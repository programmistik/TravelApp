namespace TravelApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg1501 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CheckItems", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropIndex("dbo.CheckItems", new[] { "TripId" });
            DropIndex("dbo.CityLists", new[] { "TripId" });
            DropIndex("dbo.Tickets", new[] { "TripId" });
            AlterColumn("dbo.CheckItems", "TripId", c => c.Int());
            AlterColumn("dbo.CityLists", "TripId", c => c.Int());
            AlterColumn("dbo.Tickets", "TripId", c => c.Int());
            CreateIndex("dbo.CheckItems", "TripId");
            CreateIndex("dbo.CityLists", "TripId");
            CreateIndex("dbo.Tickets", "TripId");
            AddForeignKey("dbo.CheckItems", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.Tickets", "TripId", "dbo.Trips", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropForeignKey("dbo.CheckItems", "TripId", "dbo.Trips");
            DropIndex("dbo.Tickets", new[] { "TripId" });
            DropIndex("dbo.CityLists", new[] { "TripId" });
            DropIndex("dbo.CheckItems", new[] { "TripId" });
            AlterColumn("dbo.Tickets", "TripId", c => c.Int(nullable: false));
            AlterColumn("dbo.CityLists", "TripId", c => c.Int(nullable: false));
            AlterColumn("dbo.CheckItems", "TripId", c => c.Int(nullable: false));
            CreateIndex("dbo.Tickets", "TripId");
            CreateIndex("dbo.CityLists", "TripId");
            CreateIndex("dbo.CheckItems", "TripId");
            AddForeignKey("dbo.Tickets", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CheckItems", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
        }
    }
}
