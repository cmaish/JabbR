using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JabbR.Models
{
    public class ChatUserRoomSettings
    {
        [Key, Column(Order = 0)]
        public virtual int RoomKey { get; set; }

        [Key, Column(Order = 1)]
        public virtual int UserKey { get; set; }

        [ForeignKey("RoomKey")]
        public virtual ChatRoom Room { get; set; }

        [ForeignKey("UserKey")]
        public virtual ChatUser User { get; set; }

        public DateTimeOffset? MuteExpiry { get; set; }
    }
}