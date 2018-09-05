namespace GigHub.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddFollowings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Followings",
                c => new
                    {
                        FollowerId = c.String(nullable: false, maxLength: 128),
                        ArtistId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.FollowerId, t.ArtistId })
                .ForeignKey("dbo.AspNetUsers", t => t.ArtistId)
                .ForeignKey("dbo.AspNetUsers", t => t.FollowerId, cascadeDelete: true)
                .Index(t => t.FollowerId)
                .Index(t => t.ArtistId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers");
            DropForeignKey("dbo.Followings", "ArtistId", "dbo.AspNetUsers");
            DropIndex("dbo.Followings", new[] { "ArtistId" });
            DropIndex("dbo.Followings", new[] { "FollowerId" });
            DropTable("dbo.Followings");
        }
    }
}
