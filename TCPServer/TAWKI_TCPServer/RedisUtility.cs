using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using TAWKI_TCPServer.Interfaces;

namespace TAWKI_TCPServer.Implementations
{
    public class RedisUtility
    {
        public static void HSET_Multi(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            // now deserialize this string into a list of dictionaries for parsing
            Dictionary<string, List<Dictionary<string, object>>> DataDictionary =
                Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(data);

            string ServerID = DataDictionary.First().Key;
            string key = redisEnvironment + ":" + ServerID + ":" + rediskey;
            HashEntry[] entries = new HashEntry[DataDictionary.First().Value.Count];
            int i = 0;

            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(DataDictionary.First().Value);

            foreach (Dictionary<string, object> x in DataDictionary.First().Value)
            {
                string hashkey = "0";
                if (x.ContainsKey("ID"))
                    hashkey = Convert.ToString(x["ID"]);

                string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                entries[i] = new HashEntry(hashkey, jdatastring);
                i++;
            }

            db.HashSet(key, entries);
            log.Log("HSET for key '" + key + "' (Entries: " + entries.Length.ToString() + ")");

            long subs = db.Publish(key, jsonstring, CommandFlags.None);
            log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
        }

        public static void HSET_Single(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            // now deserialize this string into a list of dictionaries for parsing
            Dictionary<string, Dictionary<string, object>> DataDictionary =
                Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(data);
            Dictionary<string, object> d = DataDictionary.First().Value;

            string ServerID = DataDictionary.First().Key;
            string key = redisEnvironment + ":" + ServerID + ":" + rediskey;
            string hashkey = "0";

            if (d.ContainsKey("ID"))
                hashkey = Convert.ToString(d["ID"]);

            string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(d);

            db.HashSet(key, new HashEntry[] { new HashEntry(hashkey, jdatastring) });
            log.Log("HSET for key '" + key + "' (Entries: 1)");

            long subs = db.Publish(key, jdatastring, CommandFlags.None);
            log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
        }

        public static void RPUSH_Multi(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            Dictionary<string, List<Dictionary<string, object>>> DataDictionary =
               Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<Dictionary<string, object>>>>(data);

            string ServerID = DataDictionary.First().Key;
            string key = redisEnvironment + ":" + ServerID + ":" + rediskey;

            foreach (Dictionary<string, object> x in DataDictionary.First().Value)
            {
                string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(x);
                db.ListRightPush(key, jdatastring);
                log.Log("RPUSH data into key '" + key + "'");
            }

            string jsonstring = Newtonsoft.Json.JsonConvert.SerializeObject(DataDictionary.First().Value);
            long subs = db.Publish(key, jsonstring, CommandFlags.None);
            log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
        }

        public static void RPUSH_Single(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            Dictionary<string, Dictionary<string, object>> DataDictionary =
               Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, object>>>(data);

            string ServerID = DataDictionary.First().Key;
            string key = redisEnvironment + ":" + ServerID + ":" + rediskey;
            string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(DataDictionary.First().Value);
            db.ListRightPush(key, jdatastring);
            log.Log("RPUSH data into key '" + key + "'");

            long subs = db.Publish(key, jdatastring, CommandFlags.None);
            log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
        }

        public static void SSET_Multi(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            List<Dictionary<string, object>> DataDictionary =
                Newtonsoft.Json.JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data);
           
            foreach (Dictionary<string, object> x in DataDictionary)
            {
                string key = redisEnvironment + ":" + x.First().Key + ":" + rediskey;
                string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(x.First().Value);
                db.StringSet(key, jdatastring);
                log.Log("SSET data into key '" + key + "'");

                long subs = db.Publish(key, jdatastring, CommandFlags.None);
                log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
            }

            
        }

        public static void SSET_Single(ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            Dictionary<string, object> DataDictionary =
               Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, object>>(data);

            string ServerID = DataDictionary.First().Key;
            string key = redisEnvironment + ":" + ServerID + ":" + rediskey;
            string jdatastring = Newtonsoft.Json.JsonConvert.SerializeObject(DataDictionary.First().Value);
            db.StringSet(key, jdatastring);
            log.Log("SSET data into key '" + key + "'");

            long subs = db.Publish(key, jdatastring, CommandFlags.None);
            log.Log("Published data to channel: '" + key + "' - Subscribers listening: " + subs);
        }

        public static void PerformOperation(string action, bool isBulk, ref IDatabase db, ref ILogger log, string redisEnvironment, string rediskey, string data)
        {
            switch (action)
            {
                case "RPUSH":
                    {
                        if (isBulk)
                            RPUSH_Multi(ref db, ref log, redisEnvironment, rediskey, data);
                        else
                            RPUSH_Single(ref db, ref log, redisEnvironment, rediskey, data);
                        return;
                    }
                case "HSET":
                    {
                        if (isBulk)
                            HSET_Multi(ref db, ref log, redisEnvironment, rediskey, data);
                        else
                            HSET_Single(ref db, ref log, redisEnvironment, rediskey, data);
                        return;
                    }
                case "SET":
                    {
                        if (isBulk)
                            SSET_Multi(ref db, ref log, redisEnvironment, rediskey, data);
                        else
                            SSET_Single(ref db, ref log, redisEnvironment, rediskey, data);
                        return;
                    }
                default:
                    {
                        throw new Exception("Invalid Redis Action Configured (" + action + ") - Cannot perform operation");
                    }
            }
        }

    }
}
