namespace AuctionSiteServer.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class v01 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Title = c.String(),
                        Description = c.String(),
                        StartTime = c.DateTime(nullable: false),
                        EndTime = c.DateTime(nullable: false),
                        StartBid = c.Double(nullable: false),
                        IsItemNew = c.Boolean(nullable: false),
                        UserID = c.String(maxLength: 128),
                        Picture1 = c.String(),
                        Picture2 = c.String(),
                        Picture3 = c.String(),
                        Picture4 = c.String(),
                        CategoryID = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Users", t => t.UserID)
                .ForeignKey("dbo.Categories", t => t.CategoryID, cascadeDelete: true)
                .Index(t => t.UserID)
                .Index(t => t.CategoryID);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        ID = c.Long(nullable: false, identity: true),
                        AuctionID = c.String(nullable: false, maxLength: 128),
                        UserID = c.String(maxLength: 128),
                        Offer = c.Double(nullable: false),
                        BidTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.Auctions", t => t.AuctionID, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserID)
                .Index(t => t.AuctionID)
                .Index(t => t.UserID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        Email = c.String(),
                        Password = c.String(),
                        LastLoginTime = c.DateTime(nullable: false),
                        CreatedOn = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Categories",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "CategoryID", "dbo.Categories");
            DropForeignKey("dbo.Bids", "UserID", "dbo.Users");
            DropForeignKey("dbo.Auctions", "UserID", "dbo.Users");
            DropForeignKey("dbo.Bids", "AuctionID", "dbo.Auctions");
            DropIndex("dbo.Bids", new[] { "UserID" });
            DropIndex("dbo.Bids", new[] { "AuctionID" });
            DropIndex("dbo.Auctions", new[] { "CategoryID" });
            DropIndex("dbo.Auctions", new[] { "UserID" });
            DropTable("dbo.Categories");
            DropTable("dbo.Users");
            DropTable("dbo.Bids");
            DropTable("dbo.Auctions");
        }
    }
}
