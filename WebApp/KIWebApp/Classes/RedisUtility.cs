using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KIWebApp.Classes
{
    public class RedisUtility
    {
        public static string BuildRedisKey<T>(string EnvironmentPrefix, string Key, T Id)
        {
            return EnvironmentPrefix + ":" + Key + ":" + Id.ToString();
        }
    }
}