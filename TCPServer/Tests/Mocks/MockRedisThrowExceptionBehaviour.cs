using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockRedisThrowExceptionBehaviour : IRedisExecuteBehaviour
    {
        bool IRedisExecuteBehaviour.Execute(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
        {
            throw new Exception("Mock Redis Exception");
        }

        bool IRedisExecuteBehaviour.Execute(KeyValuePair<RedisKey, RedisValue>[] values, When when, CommandFlags flags)
        {
            throw new Exception("Mock Redis Exception");
        }
    }
}
