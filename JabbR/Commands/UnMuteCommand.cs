using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("unmute", "Allow a muted user to start talking again. Note this is only valid for owners of the room.", "user", "user")]
    public class UnMuteCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, Models.ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who are you trying to unmute?");
            }

            ChatRoom room = context.Repository.VerifyUserRoom(context.Cache, callingUser, callerContext.RoomName);

            string targetUserName = args[0];
            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            context.Service.UnmuteUser(callingUser, targetUser, room);

            context.NotificationService.UnmuteUser(targetUser, room);

            context.Repository.CommitChanges();
        }
    }
}