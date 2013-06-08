using System;

using Moq;

using Xunit;

namespace JabbR.Tests
{
    public class GlobalCommandFacts
    {
        public class AddAdminCommand
        {
            [Fact]
            public void AdminMissingUserNameThrows()
            {
                var sut = new ChatTestEnvironment();
                string command = "/addadmin";
                string expectedError = "Who do you want to make an admin?";

                sut.AssertInvalidOperationMessage(sut.Admin, sut.CommandSourceRoom, command, expectedError);
            }

            [Fact]
            public void UserMissingUserNameThrows()
            {
                var sut = new ChatTestEnvironment();
                string command = "/addadmin";
                string expectedError = "Who do you want to make an admin?";

                sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
            }

            [Fact]
            public void UserAddAdminThrows()
            {
                var sut = new ChatTestEnvironment();
                string command = String.Format("/addadmin {0}", sut.NewUser2.Name);
                string expectedError = "You are not an admin.";

                sut.AssertInvalidOperationMessage(sut.NewUser, sut.CommandSourceRoom, command, expectedError);
                Assert.False(sut.NewUser2.IsAdmin);
                sut.NotificationServiceMock.Verify(x => x.AddAdmin(sut.NewUser2), Times.Never());
            }

            [Fact]
            public void AdminAddAdminSucceeds()
            {
                var sut = new ChatTestEnvironment();
                string command = String.Format("/addadmin {0}", sut.NewUser2.Name);

                sut.AssertSuccess(sut.Admin, sut.CommandSourceRoom, command);
                Assert.True(sut.NewUser2.IsAdmin);
                sut.NotificationServiceMock.Verify(x => x.AddAdmin(sut.NewUser2), Times.Once());
            }
        }

        public class AfkCommand
        {
            [Fact]
            public void UserSetNoteSucceeds()
            {
                var sut = new ChatTestEnvironment();
                string afkNote = "User afk note goes here!";
                string command = String.Format("/afk {0}", afkNote);

                sut.AssertSuccess(sut.NewUser, null, command);
                Assert.Equal(afkNote, sut.NewUser.AfkNote);
                Assert.True(sut.NewUser.IsAfk);
                sut.NotificationServiceMock.Verify(x => x.ChangeNote(sut.NewUser), Times.Once());
            }

            [Fact]
            public void UserClearNoteSucceeds()
            {
                var sut = new ChatTestEnvironment();
                sut.NewUser.AfkNote = "Old user afk note goes here!";
                string command = String.Format("/afk");

                sut.AssertSuccess(sut.NewUser, null, command);
                Assert.Null(sut.NewUser.AfkNote);
                Assert.True(sut.NewUser.IsAfk);
                sut.NotificationServiceMock.Verify(x => x.ChangeNote(sut.NewUser), Times.Once());
            }

            [Fact]
            public void UserSetInvalidNoteThrows()
            {
                var sut = new ChatTestEnvironment();
                string oldAfkNote = "Old user afk note goes here!";
                sut.NewUser.AfkNote = oldAfkNote;
                string afkNote = new String('A', 141);
                string command = String.Format("/afk {0}", afkNote);
                string expectedError = "Sorry, but your note is too long. Please keep it under 140 characters.";

                sut.AssertInvalidOperationMessage(sut.NewUser, null, command, expectedError);
                Assert.Equal(oldAfkNote, sut.NewUser.AfkNote);
                Assert.False(sut.NewUser.IsAfk);
            }
        }
    }
}
