using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JakubKalinaLab7.Extensions
{
    public static class SessionExtensions
    {
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default(T) : JsonSerializer.Deserialize<T>(json);
        }
    }
}