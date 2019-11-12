using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace SportsStore.Infrastructure
{
    public static class SessionExtensions
    {
        // save the cart information as Json to the session memory
        public static void SetJson(this ISession session, string key, object value)
        {
            session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetJson<T>(this ISession session, string key)
        {
            var sessionData = session.GetString(key);

            return sessionData == null 
                ? default(T) // if there's no cart in the session memory, return a new Cart
                : JsonConvert.DeserializeObject<T>(sessionData);
        }
    }
}
