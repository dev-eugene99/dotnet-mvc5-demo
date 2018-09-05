namespace GigHub.Migrations
{
    using System.Data.Entity.Migrations;

    public partial class AddFollowings1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers");
            RenameColumn(table: "dbo.Followings", name: "ArtistId", newName: "FolloweeId");
            RenameIndex(table: "dbo.Followings", name: "IX_ArtistId", newName: "IX_FolloweeId");
            AddForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers");
            RenameIndex(table: "dbo.Followings", name: "IX_FolloweeId", newName: "IX_ArtistId");
            RenameColumn(table: "dbo.Followings", name: "FolloweeId", newName: "ArtistId");
            AddForeignKey("dbo.Followings", "FollowerId", "dbo.AspNetUsers", "Id", cascadeDelete: true);
        }
    }
}
