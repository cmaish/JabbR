using System;
using JabbR.Models;

namespace JabbR.Services
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cache, string key)
        {
            return (T)cache.Get(key);
        }

        public static bool? IsUserInRoom(this ICache cache, ChatUser user, ChatRoom room)
        {
            string key = CacheKeys.GetUserInRoom(user, room);

            return (bool?)cache.Get(key);
        }

        public static void SetUserInRoom(this ICache cache, ChatUser user, ChatRoom room, bool value)
        {
            string key = CacheKeys.GetUserInRoom(user, room);

            // Cache this forever since people don't leave rooms often
            cache.Set(key, value, TimeSpan.FromDays(365));
        }

        public static void RemoveUserInRoom(this ICache cache, ChatUser user, ChatRoom room)
        {
            cache.Remove(CacheKeys.GetUserInRoom(user, room));
            cache.Remove(CacheKeys.GetUserInRoomMuted(user, room));
        }

        public static DateTime? UserInRoomMuteExpiry(this ICache cache, ChatUser user, ChatRoom room)
        {
            string key = CacheKeys.GetUserInRoomMuted(user, room);

            return (DateTime?)cache.Get(key);
        }

        public static void MuteUserInRoom(this ICache cache, ChatUser user, ChatRoom room, DateTime muteExpiry)
        {
            string key = CacheKeys.GetUserInRoomMuted(user, room);

            // Cache this forever so we don't have to hit the database much
            cache.Set(key, muteExpiry, TimeSpan.FromDays(365));
        }

        public static void UnmuteUserInRoom(this ICache cache, ChatUser user, ChatRoom room)
        {
            MuteUserInRoom(cache, user, room, DateTime.Now);
        }

        private static class CacheKeys
        {
            public static string GetUserInRoom(ChatUser user, ChatRoom room)
            {
                return "UserInRoom_" + user.Key + "_" + room.Key;
            }

            public static string GetUserInRoomMuted(ChatUser user, ChatRoom room)
            {
                return "UserInRoom_Muted_" + user.Key + "_" + room.Key;
            }
        }
    }
}