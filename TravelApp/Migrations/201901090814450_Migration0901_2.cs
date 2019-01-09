namespace TravelApp.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migration0901_2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Cities", "Trip_Id", "dbo.Trips");
            DropIndex("dbo.Cities", new[] { "Trip_Id" });
            CreateTable(
                "dbo.CityLists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CityId = c.Int(nullable: false),
                        Trip_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cities", t => t.CityId, cascadeDelete: true)
                .ForeignKey("dbo.Trips", t => t.Trip_Id)
                .Index(t => t.CityId)
                .Index(t => t.Trip_Id);
            
            DropColumn("dbo.Cities", "Trip_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Cities", "Trip_Id", c => c.Int());
            DropForeignKey("dbo.CityLists", "Trip_Id", "dbo.Trips");
            DropForeignKey("dbo.CityLists", "CityId", "dbo.Cities");
            DropIndex("dbo.CityLists", new[] { "Trip_Id" });
            DropIndex("dbo.CityLists", new[] { "CityId" });
            DropTable("dbo.CityLists");
            CreateIndex("dbo.Cities", "Trip_Id");
            AddForeignKey("dbo.Cities", "Trip_Id", "dbo.Trips", "Id");
        }
    }
}
