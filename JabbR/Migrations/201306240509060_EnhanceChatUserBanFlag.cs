namespace JabbR.Models.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class EnhanceChatUserBanFlag : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ChatUsers", "BanStatus", c => c.Int(nullable: false));
            Sql("UPDATE dbo.ChatUsers SET BanStatus = CASE WHEN IsBanned = '1' THEN '1' ELSE '0' END");
            DropColumn("dbo.ChatUsers", "IsBanned");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ChatUsers", "IsBanned", c => c.Boolean(nullable: false));
            DropColumn("dbo.ChatUsers", "BanStatus");
        }
    }
}
