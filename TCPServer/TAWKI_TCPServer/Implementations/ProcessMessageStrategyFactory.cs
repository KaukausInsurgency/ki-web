using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer.Interfaces;
using System.Data;
using StackExchange.Redis;
using MySql.Data.MySqlClient;

namespace TAWKI_TCPServer.Implementations
{
    public class ProcessMessageStrategyFactory
    {

        public static EDbSource GetSource(string src)
        {
            if (src == "MYSQL")
            {
                return EDbSource.MySQL;
            }
            else if (src == "REDIS")
            {
                return EDbSource.Redis;
            }
            else
            {
                return EDbSource.Invalid;
            }
        }

        public static IProcessMessageStrategy Create(IConfigReader config, ILogger logger, EDbSource source, IConnectionMultiplexer redisconn, IDbConnection dbconn)
        {
            switch (source)
            {
                case EDbSource.MySQL:
                    {
                        return new MySqlProcessMessageStrategy(dbconn, logger, config);
                    }
                case EDbSource.Redis:
                    {
                        return new RedisProcessMessageStrategy(redisconn, logger, config);
                    }
                default:
                    {
                        return new InvalidProcessMessageStrategy();
                    }
            }
        }
    }
}
