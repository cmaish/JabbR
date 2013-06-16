﻿using System;
using System.Linq;
using JabbR.Models;

namespace JabbR.Commands
{
    [Command("list", "List_CommandInfo", "[room]", "room")]
    public class ListCommand : UserCommand
    {
        public override void Execute(CommandContext context, CallerContext callerContext, ChatUser callingUser, string[] args)
        {
            string roomName = args.Length > 0 ? args[0] : callerContext.RoomName;

            if (String.IsNullOrEmpty(roomName))
            {
                throw new InvalidOperationException(LanguageResources.List_RoomRequired);
            }

            ChatRoom room = context.Repository.VerifyRoom(roomName);

            // ensure the user could join the room if they wanted to
            callingUser.EnsureAllowed(room);

            var names = context.Repository.GetOnlineUsers(room).Select(s => s.Name);

            context.NotificationService.ListUsers(room, names);
        }
    }
}