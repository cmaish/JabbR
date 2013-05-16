namespace JabbR.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UserRoomSettings : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ChatUserRoomSettings",
                c => new
                    {
                        Room_Key = c.Int(nullable: false),
                        User_Key = c.Int(nullable: false),
                        MuteExpiry = c.DateTimeOffset(),
                    })
                .PrimaryKey(t => new { t.Room_Key, t.User_Key })
                .ForeignKey("dbo.ChatRooms", t => t.Room_Key, cascadeDelete: true)
                .ForeignKey("dbo.ChatUsers", t => t.User_Key, cascadeDelete: true)
                .Index(t => t.Room_Key)
                .Index(t => t.User_Key);
            
        }
        
        public override void Down()
        {
            DropIndex("dbo.ChatUserRoomSettings", new[] { "User_Key" });
            DropIndex("dbo.ChatUserRoomSettings", new[] { "Room_Key" });
            DropForeignKey("dbo.ChatUserRoomSettings", "User_Key", "dbo.ChatUsers");
            DropForeignKey("dbo.ChatUserRoomSettings", "Room_Key", "dbo.ChatRooms");
            DropTable("dbo.ChatUserRoomSettings");
        }
    }
}
