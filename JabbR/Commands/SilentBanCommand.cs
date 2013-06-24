using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("silentban", "Ban a user from JabbR without them knowing.", "user", "admin")]
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

            context.Service.BanUser(callingUser, targetUser, silent: true);
            context.Repository.CommitChanges();
        }
    }
}