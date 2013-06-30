using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("unban", "Unban a user from JabbR!", "user", "admin")]
    public class UnbanCommand : AdminCommand
    {
        public override void ExecuteAdminOperation(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            if (args.Length == 0)
            {
                throw new InvalidOperationException("Who do you want to unban?");
            }

            string targetUserName = args[0];

            ChatUser targetUser = context.Repository.VerifyUser(targetUserName);

            if (targetUser.BanStatus == UserBanStatus.NotBanned)
            {
                throw new InvalidOperationException(String.Format("{0} is not banned.", targetUser.Name));
            }

            context.Service.UnbanUser(callingUser, targetUser);
            context.NotificationService.UnbanUser(targetUser);
            context.Repository.CommitChanges();
        }
    }
}