using System;
using System.Globalization;

using JabbR.Commands;
using JabbR.Models;
using JabbR.Services;

using Moq;

using Xunit;

namespace JabbR.Tests
{
    public class ChatTestEnvironment
    {
        public IJabbrRepository Repository { get; private set; }

        public Mock<INotificationService> NotificationServiceMock { get; private set; }
        public Mock<ICache> CacheMock { get; private set; }

        public ChatService ChatService { get; private set; }

        public ChatRoom CommandSourceRoom { get; private set; }

        public ChatRoom PublicRoom { get; private set; }
        public ChatRoom PrivateRoom { get; private set; }
        public ChatRoom ClosedPublicRoom { get; private set; }
        public ChatRoom ClosedPrivateRoom { get; private set; }
        public string NotExistingRoomName { get; private set; }

        public ChatUser Admin { get; private set; }
        
        public ChatUser EveryRoomOwner { get; private set; }
        public ChatUser PublicRoomOwner { get; private set; }
        public ChatUser PrivateRoomOwner { get; private set; }

        public ChatUser NewUser { get; private set; }
        public ChatUser NewUser2 { get; private set; }
        public ChatUser BannedUser { get; private set; }
        public string NotExistingUsername { get; private set; }

        public ChatTestEnvironment()
        {
            Repository = new InMemoryRepository();
            NotificationServiceMock = new Mock<INotificationService>();
            CacheMock = new Mock<ICache>();

            CommandSourceRoom = new ChatRoom { Name = "CommandSourceRoom" };
            PublicRoom = new ChatRoom { Name = "PublicRoom" };
            PrivateRoom = new ChatRoom { Name = "PrivateRoom", Private = true };
            ClosedPublicRoom = new ChatRoom { Name = "ClosedPublicRoom", Closed = true };
            ClosedPrivateRoom = new ChatRoom { Name = "ClosedPrivateRoom", Closed = true, Private = true };

            Admin = new ChatUser { Id = "1", Name = "admin", IsAdmin = true };

            EveryRoomOwner = new ChatUser { Id = "2", Name = "everyRoomOwner" };
            PublicRoomOwner = new ChatUser { Id = "3", Name = "publicRoomOwner" };
            PrivateRoomOwner = new ChatUser { Id = "4", Name = "privateRoomOwner" };

            NewUser = new ChatUser { Id = "5", Name = "newUser", };
            NewUser2 = new ChatUser { Id = "6", Name = "newUser2" };
            BannedUser = new ChatUser { Id = "7", Name = "bannedUser", IsBanned = true };

            NotExistingUsername = "NotExistingUser";
            NotExistingRoomName = "NotExistingRoom";

            // wire up users and rooms
            Repository.Add(CommandSourceRoom);
            Repository.Add(PublicRoom);
            Repository.Add(PrivateRoom);
            Repository.Add(ClosedPublicRoom);
            Repository.Add(ClosedPrivateRoom);

            Repository.Add(Admin);
            Repository.Add(EveryRoomOwner);
            Repository.Add(PrivateRoomOwner);
            Repository.Add(PublicRoomOwner);
            Repository.Add(NewUser);
            Repository.Add(NewUser2);
            Repository.Add(BannedUser);

            EveryRoomOwner.OwnedRooms.Add(PublicRoom);
            EveryRoomOwner.OwnedRooms.Add(PrivateRoom);
            EveryRoomOwner.OwnedRooms.Add(ClosedPublicRoom);
            EveryRoomOwner.OwnedRooms.Add(ClosedPrivateRoom);

            PrivateRoomOwner.OwnedRooms.Add(PrivateRoom);
            PrivateRoomOwner.OwnedRooms.Add(ClosedPrivateRoom);

            PublicRoomOwner.OwnedRooms.Add(PublicRoom);
            PublicRoomOwner.OwnedRooms.Add(ClosedPublicRoom);

            PublicRoom.Owners.Add(EveryRoomOwner);
            PublicRoom.Owners.Add(PublicRoomOwner);

            ClosedPublicRoom.Owners.Add(EveryRoomOwner);
            ClosedPublicRoom.Owners.Add(PublicRoomOwner);

            PrivateRoom.Owners.Add(EveryRoomOwner);
            PrivateRoom.Owners.Add(PrivateRoomOwner);

            ClosedPrivateRoom.Owners.Add(EveryRoomOwner);
            ClosedPrivateRoom.Owners.Add(PrivateRoomOwner);

            CommandSourceRoom.Users.Add(Admin);
            CommandSourceRoom.Users.Add(EveryRoomOwner);
            CommandSourceRoom.Users.Add(PrivateRoomOwner);
            CommandSourceRoom.Users.Add(PublicRoomOwner);
            CommandSourceRoom.Users.Add(NewUser);
            CommandSourceRoom.Users.Add(NewUser2);

            ChatService = new ChatService(CacheMock.Object, Repository);
        }

        private CommandManager BuildCommandManager(ChatUser user, ChatRoom room)
        {
            return new CommandManager(
                "clientid",
                user.Id,
                room != null ? room.Name : null,
                ChatService,
                Repository,
                CacheMock.Object,
                NotificationServiceMock.Object);
        }

        public void AssertInvalidOperationMessage(ChatUser user, ChatRoom room, string command, string expectedExceptionMessage)
        {
            CommandManager commandManager = BuildCommandManager(user, room);
            var exception = Assert.Throws<InvalidOperationException>(() => commandManager.TryHandleCommand(command));
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        public void AssertSuccess(ChatUser user, ChatRoom room, string command)
        {
            CommandManager commandManager = BuildCommandManager(user, room);
            Assert.True(commandManager.TryHandleCommand(command));
        }

        public void AssertNoMatch(ChatUser user, ChatRoom room, string command)
        {
            CommandManager commandManager = this.BuildCommandManager(user, room);
            Assert.False(commandManager.TryHandleCommand(command));
        }
    }
}
