using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using JabbR.Models.Mapping;

namespace JabbR.Models
{
    using System;

    public class JabbrContext : DbContext
    {
        public JabbrContext()
            : base("Jabbr")
        {
            ((IObjectContextAdapter)this).ObjectContext.ObjectMaterialized += ObjectMaterialized;
        }

        private void ObjectMaterialized(object sender, ObjectMaterializedEventArgs evt)
        {
            var entityChatUser = evt.Entity as ChatUser;
            if (entityChatUser != null)
            {
                entityChatUser.LastActivity = DateTime.SpecifyKind(entityChatUser.LastActivity, DateTimeKind.Utc);
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new ChatClientMap());

            modelBuilder.Configurations.Add(new ChatMessageMap());

            modelBuilder.Configurations.Add(new ChatRoomMap());

            modelBuilder.Configurations.Add(new ChatUserMap());

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<ChatClient> Clients { get; set; }
        public DbSet<ChatMessage> Messages { get; set; }
        public DbSet<ChatRoom> Rooms { get; set; }
        public DbSet<ChatUser> Users { get; set; }
        public DbSet<ChatUserIdentity> Identities { get; set; }
    }
}