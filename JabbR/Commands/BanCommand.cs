using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("ban", "Ban a user from JabbR!", "user", "admin")]
    public class BanCommand : AdminCommand
    {
        public override void ExecuteAdminOperation(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who do you want to ban?");
            }

            string targetUserName = args[0];

            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            if (targetUser.BanStatus == UserBanStatus.Banned)
            {
                throw new InvalidOperationException(String.Format("{0} is already banned.", targetUser.Name));
            }

            context.Service.BanUser(callingUser, targetUser, silent: false);
            context.NotificationService.BanUser(targetUser, silent: false);
            context.Repository.CommitChanges();
        }
    }
}