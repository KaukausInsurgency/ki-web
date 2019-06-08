using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockRedisSuccessBehaviour : IRedisExecuteBehaviour
    {
        bool IRedisExecuteBehaviour.Execute(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
        {
            return true;
        }

        bool IRedisExecuteBehaviour.Execute(KeyValuePair<RedisKey, RedisValue>[] values, When when, CommandFlags flags)
        {
            return true;
        }
    }
}
