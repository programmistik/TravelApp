namespace TravelApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class m1501 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.CheckItems", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.CityLists", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.Tickets", "Trip_Id", "dbo.Trips");
            DropIndex("dbo.CheckItems", new[] { "Trip_Id" });
            DropIndex("dbo.CityLists", new[] { "Trip_Id" });
            DropIndex("dbo.Tickets", new[] { "Trip_Id" });
            RenameColumn(table: "dbo.CheckItems", name: "Trip_Id", newName: "TripId");
            RenameColumn(table: "dbo.CityLists", name: "Trip_Id", newName: "TripId");
            RenameColumn(table: "dbo.Tickets", name: "Trip_Id", newName: "TripId");
            AlterColumn("dbo.CheckItems", "TripId", c => c.Int(nullable: false));
            AlterColumn("dbo.CityLists", "TripId", c => c.Int(nullable: false));
            AlterColumn("dbo.Tickets", "TripId", c => c.Int(nullable: false));
            CreateIndex("dbo.CheckItems", "TripId");
            CreateIndex("dbo.CityLists", "TripId");
            CreateIndex("dbo.Tickets", "TripId");
            AddForeignKey("dbo.CheckItems", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            AddForeignKey("dbo.CityLists", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
            AddForeignKey("dbo.Tickets", "TripId", "dbo.Trips", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Tickets", "TripId", "dbo.Trips");
            DropForeignKey("dbo.CityLists", "TripId", "dbo.Trips");
            DropForeignKey("dbo.CheckItems", "TripId", "dbo.Trips");
            DropIndex("dbo.Tickets", new[] { "TripId" });
            DropIndex("dbo.CityLists", new[] { "TripId" });
            DropIndex("dbo.CheckItems", new[] { "TripId" });
            AlterColumn("dbo.Tickets", "TripId", c => c.Int());
            AlterColumn("dbo.CityLists", "TripId", c => c.Int());
            AlterColumn("dbo.CheckItems", "TripId", c => c.Int());
            RenameColumn(table: "dbo.Tickets", name: "TripId", newName: "Trip_Id");
            RenameColumn(table: "dbo.CityLists", name: "TripId", newName: "Trip_Id");
            RenameColumn(table: "dbo.CheckItems", name: "TripId", newName: "Trip_Id");
            CreateIndex("dbo.Tickets", "Trip_Id");
            CreateIndex("dbo.CityLists", "Trip_Id");
            CreateIndex("dbo.CheckItems", "Trip_Id");
            AddForeignKey("dbo.Tickets", "Trip_Id", "dbo.Trips", "Id");
            AddForeignKey("dbo.CityLists", "Trip_Id", "dbo.Trips", "Id");
            AddForeignKey("dbo.CheckItems", "Trip_Id", "dbo.Trips", "Id");
        }
    }
}
