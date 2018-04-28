using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Tests.Mocks
{
    class MockRedisPublishThrowExceptionBehaviour : IRedisPublishBehaviour
    {
        long IRedisPublishBehaviour.Execute(RedisChannel channel, RedisValue message, CommandFlags flags)
        {
            throw new Exception("A Mock Publish Exception has occurred");
        }
    }
}
