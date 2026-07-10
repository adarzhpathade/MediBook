using System.Text.Json;
using Microsoft.AspNetCore.Http;
using MediBook.Models.Entities;

namespace MediBook.Helpers
{
    public static class SessionHelper
    {
        private const string UserSessionKey = "_UserSession";

        public static void SetUserSession(ISession session, User user)
        {
            var sessionUser = new
            {
                user.UserId,
                user.FullName,
                user.Email,
                user.Role
            };
            
            session.SetString(UserSessionKey, JsonSerializer.Serialize(sessionUser));
        }

        public static User? GetUserSession(ISession session)
        {
            var value = session.GetString(UserSessionKey);
            return value == null ? null : JsonSerializer.Deserialize<User>(value);
        }

        public static void ClearSession(ISession session)
        {
            session.Remove(UserSessionKey);
            session.Clear();
        }

        public static bool IsAuthenticated(ISession session)
        {
            return session.GetString(UserSessionKey) != null;
        }

        public static string? GetUserRole(ISession session)
        {
            var user = GetUserSession(session);
            return user?.Role;
        }

        public static int? GetUserId(ISession session)
        {
            var user = GetUserSession(session);
            return user?.UserId;
        }

        public static string? GetUserName(ISession session)
        {
            var user = GetUserSession(session);
            return user?.FullName;
        }
    }
}
