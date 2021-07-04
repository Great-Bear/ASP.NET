using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace CookiesAndSessions.Extentions
{
    public static class SessionExtension
    {
        public static void SetObject(this ISession session, string key, object obj, Type type)
        {
            string json = JsonSerializer.Serialize(obj, type);
            session.SetString(key, json);
        }

        public static object GetObject(this ISession session, string key, Type type)
        {
            string json = session.GetString(key);
            if(json == null) 
            {
                return null;
            }
            return JsonSerializer.Deserialize(json, type);
        }
    }
}
