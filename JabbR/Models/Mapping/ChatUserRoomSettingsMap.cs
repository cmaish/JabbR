using System.Data.Entity.ModelConfiguration;

namespace JabbR.Models.Mapping
{
    public class ChatUserRoomSettingsMap : EntityTypeConfiguration<ChatUserRoomSettings>
    {
        public ChatUserRoomSettingsMap()
        {
            // Primary Key
            this.HasKey(c => new { c.RoomKey, c.UserKey });

            // Properties
            // Table & Column Mappings
            this.ToTable("ChatUserRoomSettings");
            this.Property(c => c.RoomKey).HasColumnName("Room_Key");
            this.Property(c => c.UserKey).HasColumnName("User_Key");
            this.Property(c => c.MuteExpiry).HasColumnName("MuteExpiry");

            // Relationships
            this.HasRequired(c => c.User);
            this.HasRequired(c => c.Room);
        }
    }
}