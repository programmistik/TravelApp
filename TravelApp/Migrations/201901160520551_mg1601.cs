namespace TravelApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class mg1601 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CheckItems", "TripId", "dbo.Trips");
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            AddForeignKey("dbo.CheckItems", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropForeignKey("dbo.CheckItems", "TripId", "dbo.Trips");
            AddForeignKey("dbo.Tickets", "TripId", "dbo.Trips", "Id");
            AddForeignKey("dbo.CheckItems", "TripId", "dbo.Trips", "Id");
        }
    }
}
