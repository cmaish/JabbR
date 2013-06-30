using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("silentban", "Ban a user from JabbR without them knowing.  They can still log in and read messages.", "user", "admin")]
    public class SilentBanCommand : AdminCommand
    {
        public override void ExecuteAdminOperation(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who do you want to ban?");
            }

            string targetUserName = args[0];

            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            if (targetUser.BanStatus == UserBanStatus.SilentlyBanned)
            {
                throw new InvalidOperationException(String.Format("{0} is already silently banned.", targetUser.Name));
            }

            context.Service.BanUser(callingUser, targetUser, silent: true);
            context.NotificationService.BanUser(targetUser, silent: true);
            context.Repository.CommitChanges();
        }
    }
}