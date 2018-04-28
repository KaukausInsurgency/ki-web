using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    interface IRedisExecuteBehaviour
    {
        bool Execute(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags);
        bool Execute(KeyValuePair<RedisKey, RedisValue>[] values, When when, CommandFlags flags);
    }
}
