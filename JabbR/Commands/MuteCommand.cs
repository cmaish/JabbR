using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("mute", "Prevent a user from talking in this room for a specified amount of time. Note this is only valid for owners of the room.", "user minutes", "user")]
    public class MuteCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, Models.ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who are you trying to mute?");
            }

            if (args.Length == 1)
            {
                throw new InvalidOperationException("How many minutes are you trying to mute them for?");
            }

            ChatRoom room = context.Repository.VerifyUserRoom(context.Cache, callingUser, callerContext.RoomName);

            string targetUserName = args[0];
            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            string targetDurationMinutes = args[1];
            double durationMinutes = 0;
            if (!double.TryParse(targetDurationMinutes, out durationMinutes) || durationMinutes < 0 || durationMinutes > 3000)
            {
                throw new InvalidOperationException("You must enter a valid number of minutes (0 to 3000).");
            }

            context.Service.MuteUser(callingUser, targetUser, room, durationMinutes);

            context.NotificationService.MuteUser(targetUser, room, durationMinutes);

            context.Repository.CommitChanges();
        }
    }
}