using System;
using System.Linq;

using Xunit;

namespace JabbR.Tests
{
    public class RoomCommandFacts
    {
        public class AddOwnerCommand
        {
            public class FromLobby
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminMissingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NewUser2.Name);
                    string expectedError = "Which room?";
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserMissingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NewUser2.Name);
                    string expectedError = "Which room?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.Admin, null, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void OwnerAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.EveryRoomOwner, null, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void UserAddOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void AdminAddClosedRoomThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.ClosedPrivateRoom.Name);
                    string expectedError = String.Format("The room '{0}' is closed.", sut.ClosedPrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }
            }

            public class FromCommandRoom
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.Admin, sut.CommandSourceRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void OwnerAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.EveryRoomOwner, sut.CommandSourceRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void UserAddOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }
            }

            public class FromTargetRoom
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/addowner";
                    string expectedError = "Who do you want to make an owner?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void AdminAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NewUser2.Name);
                    sut.AssertSuccess(sut.Admin, sut.PrivateRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void OwnerAddOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NewUser2.Name);
                    sut.AssertSuccess(sut.EveryRoomOwner, sut.PrivateRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void UserAddOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.NewUser2.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserAddExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/addowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }
            }
        }

        public class RemoveOwnerCommand
        {
            public class FromLobby
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminMissingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NewUser2.Name);
                    string expectedError = "Which room?";
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserMissingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NewUser2.Name);
                    string expectedError = "Which room?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                }

                [Fact]
                public void UserNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                }

                [Fact]
                public void AdminRemoveOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.Admin, null, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void OwnerRemoveOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.EveryRoomOwner, null, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void UserRemoveOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void AdminRemoveClosedRoomThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.ClosedPrivateRoom.Name);
                    string expectedError = String.Format("The room '{0}' is closed.", sut.ClosedPrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, null, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }
            }

            public class FromCommandRoom
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingRoomNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.NotExistingRoomName);
                    string expectedError = String.Format("Unable to find room '{0}'.", sut.NotExistingRoomName);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                }

                [Fact]
                public void AdminRemoveOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.Admin, sut.CommandSourceRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void OwnerRemoveOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    sut.AssertSuccess(sut.EveryRoomOwner, sut.CommandSourceRoom, command);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void UserRemoveOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.NewUser2.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("'{0}' is already an owner of '{1}'.", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0} {1}", sut.PrivateRoomOwner.Name, sut.PrivateRoom.Name);
                    string expectedError = String.Format("You are not an owner of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }
            }

            public class FromTargetRoom
            {
                [Fact]
                public void AdminMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void UserMissingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = "/removeowner";
                    string expectedError = "Which owner do you want to remove?";
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void AdminNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void UserNotExistingUserNameThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NotExistingUsername);
                    string expectedError = String.Format("Unable to find user '{0}'.", sut.NotExistingUsername);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                }

                [Fact]
                public void AdminRemoveOwnerSucceeds()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.PrivateRoomOwner.Name);
                    sut.AssertSuccess(sut.Admin, sut.PrivateRoom, command);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.PrivateRoomOwner));
                }

                [Fact]
                public void OwnerRemoveOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.EveryRoomOwner, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Contains(sut.PrivateRoomOwner));
                }

                [Fact]
                public void UserRemoveOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NewUser2.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                    Assert.False(sut.PrivateRoom.Owners.Contains(sut.NewUser2));
                }

                [Fact]
                public void AdminRemoveNonOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.NewUser.Name);
                    string expectedError = String.Format("'{0}' is not an owner of '{1}'.", sut.NewUser.Name, sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.Admin, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void OwnerRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.EveryRoomOwner, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }

                [Fact]
                public void UserRemoveExistingOwnerThrows()
                {
                    var sut = new ChatTestEnvironment();
                    string command = String.Format("/removeowner {0}", sut.PrivateRoomOwner.Name);
                    string expectedError = String.Format("You are not the creator of room '{0}'.", sut.PrivateRoom.Name);
                    sut.AssertInvalidOperationMessage(sut.NewUser, sut.PrivateRoom, command, expectedError);
                    Assert.True(sut.PrivateRoom.Owners.Count(u => u == sut.PrivateRoomOwner) == 1);
                }
            }
        }
    }
}
