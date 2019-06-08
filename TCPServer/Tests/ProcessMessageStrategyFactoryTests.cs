using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAWKI_TCPServer;
using TAWKI_TCPServer.Interfaces;
using TAWKI_TCPServer.Implementations;
using NUnit.Framework;
using System.Data;
using StackExchange.Redis;

namespace Tests
{
    [TestFixture]
    class ProcessMessageStrategyFactoryTests
    {
        [Test]
        public void GetSource_MySql_Success()
        {
            Assert.That(ProcessMessageStrategyFactory.GetSource("MYSQL") == EDbSource.MySQL);
        }

        [Test]
        public void GetSource_Redis_Success()
        {
            Assert.That(ProcessMessageStrategyFactory.GetSource("REDIS") == EDbSource.Redis);
        }

        [Test]
        public void GetSource_Invalid_Success()
        {
            Assert.That(ProcessMessageStrategyFactory.GetSource("Regi") == EDbSource.Invalid);
        }

        [Test]
        public void CreateStrategy_MySql_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader();
            ILogger logger = new Mocks.MockLogger();
            IConnectionMultiplexer redisconn = new Mocks.MockConnectionMultiplexer(new Mocks.MockRedisSuccessBehaviour(),
                                                                                   new Mocks.MockRedisPublishSuccessBehaviour());
            IDbConnection dbconn = new Mocks.MockDBConnection();
            Assert.IsInstanceOf(typeof(MySqlProcessMessageStrategy), ProcessMessageStrategyFactory.Create(config, logger, EDbSource.MySQL, redisconn, dbconn));
        }

        [Test]
        public void CreateStrategy_Redis_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader();
            ILogger logger = new Mocks.MockLogger();
            IConnectionMultiplexer redisconn = new Mocks.MockConnectionMultiplexer(new Mocks.MockRedisSuccessBehaviour(),
                                                                                   new Mocks.MockRedisPublishSuccessBehaviour());
            IDbConnection dbconn = new Mocks.MockDBConnection();

            Assert.IsInstanceOf(typeof(RedisProcessMessageStrategy), ProcessMessageStrategyFactory.Create(config, logger, EDbSource.Redis, redisconn, dbconn));
        }

        [Test]
        public void CreateStrategy_Invalid_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader();
            ILogger logger = new Mocks.MockLogger();
            IConnectionMultiplexer redisconn = new Mocks.MockConnectionMultiplexer(new Mocks.MockRedisSuccessBehaviour(),
                                                                                   new Mocks.MockRedisPublishSuccessBehaviour());
            IDbConnection dbconn = new Mocks.MockDBConnection();

            Assert.IsInstanceOf(typeof(InvalidProcessMessageStrategy), ProcessMessageStrategyFactory.Create(config, logger, EDbSource.Invalid, redisconn, dbconn));
        }
    }
}
