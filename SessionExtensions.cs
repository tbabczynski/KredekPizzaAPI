using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace JakubKalinaLab7.Extensions
{
    /// <summary>
    /// Rozszerzenia dla sesji
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// Zapisuje obiekt jako JSON w sesji.
        /// </summary>
        /// <param name="session">Sesja</param>
        /// <param name="key">Klucz</param>
        /// <param name="value">Wartość</param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }

        /// <summary>
        /// Pobiera obiekt z sesji jako JSON.
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns>Obiekt deserializowany z JSON.</returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var json = session.GetString(key);
            return json == null ? default(T) : JsonSerializer.Deserialize<T>(json);
        }
    }
}