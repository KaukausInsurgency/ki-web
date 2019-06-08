using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    interface IRedisPublishBehaviour
    {
        long Execute(RedisChannel channel, RedisValue message, CommandFlags flags);
    }
}
