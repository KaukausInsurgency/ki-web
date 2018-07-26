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
using Tests.Mocks;
using StackExchange.Redis;

namespace Tests
{
    [TestFixture]
    class RedisUtilityTests
    {
        [Test]
        public void PerformOperation_InvalidAction_ThrowsException()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{}";
            Assert.Throws<Exception>(() => RedisUtility.PerformOperation("UNKNOWN", false, ref db, ref log, "Env", "1", data));
        }

        [Test]
        public void HSET_Multi_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':[{'ID':1,'Data':'string'},{'ID':2,'Data':'string'},{'ID':3,'Data':'string'}]}";
            RedisUtility.HSET_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockHashStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Count == 3);
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Exists(h => h.Name == "1"));
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Exists(h => h.Name == "2"));
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Exists(h => h.Name == "3"));
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].First(h => h.Name == "1").Value == "{\"ID\":1,\"Data\":\"string\"}");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].First(h => h.Name == "2").Value == "{\"ID\":2,\"Data\":\"string\"}");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].First(h => h.Name == "3").Value == "{\"ID\":3,\"Data\":\"string\"}");
        }

        [Test]
        public void HSET_MultiPublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':[{'ID':1,'Data':'string'},{'ID':2,'Data':'string'},{'ID':3,'Data':'string'}]}";
            RedisUtility.HSET_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannel.ContainsKey("UT:1:Sample"));
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "[{\"ID\":1,\"Data\":\"string\"},{\"ID\":2,\"Data\":\"string\"},{\"ID\":3,\"Data\":\"string\"}]");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void HSET_Single_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'ID':1,'Data':'string'}}";
            RedisUtility.HSET_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockHashStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Count == 1);
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Exists(h => h.Name == "1"));
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].First(h => h.Name == "1").Value == "{\"ID\":1,\"Data\":\"string\"}");
        }

        [Test]
        public void HSET_SinglePublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'ID':1,'Data':'string'}}";
            RedisUtility.HSET_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannel.ContainsKey("UT:1:Sample"));
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"ID\":1,\"Data\":\"string\"}");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void RPUSH_Multi_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':[{'Data':'string'},{'Data':'string'},{'Data':'string'}]}";
            RedisUtility.RPUSH_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockListStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockListStore["UT:1:Sample"].Count == 3);
            Assert.That(mockdb.MockListStore["UT:1:Sample"][0] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockListStore["UT:1:Sample"][1] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockListStore["UT:1:Sample"][2] == "{\"Data\":\"string\"}");
        }

        [Test]
        public void RPUSH_MultiPublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':[{'Data':'string'},{'Data':'string'},{'Data':'string'}]}";
            RedisUtility.RPUSH_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannel.ContainsKey("UT:1:Sample"));
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "[{\"Data\":\"string\"},{\"Data\":\"string\"},{\"Data\":\"string\"}]");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void RPUSH_Single_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'Data':'string'}}";
            RedisUtility.RPUSH_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockListStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockListStore["UT:1:Sample"].Count == 1);
            Assert.That(mockdb.MockListStore["UT:1:Sample"][0] == "{\"Data\":\"string\"}");
        }

        [Test]
        public void RPUSH_SinglePublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'Data':'string'}}";
            RedisUtility.RPUSH_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannel.ContainsKey("UT:1:Sample"));
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void SSET_Multi_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "[{'1':{'Data':'string'}},{'2':{'Data':'string'}},{'3':{'Data':'string'}}]";
            RedisUtility.SSET_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockStringSetStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockStringSetStore.ContainsKey("UT:2:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockStringSetStore.ContainsKey("UT:3:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockStringSetStore["UT:1:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockStringSetStore["UT:2:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockStringSetStore["UT:3:Sample"] == "{\"Data\":\"string\"}");
        }

        [Test]
        public void SSET_MultiPublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "[{'1':{'Data':'string'}},{'2':{'Data':'string'}},{'3':{'Data':'string'}}]";
            RedisUtility.SSET_Multi(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
            Assert.That(mockdb.MockChannelCount["UT:2:Sample"] == 1);
            Assert.That(mockdb.MockChannelCount["UT:3:Sample"] == 1);
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockChannel["UT:2:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockChannel["UT:3:Sample"] == "{\"Data\":\"string\"}");
            
        }

        public void SSET_Single_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'Data':'string'}}";
            RedisUtility.SSET_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockStringSetStore.ContainsKey("UT:1:Sample"), "Redis Hash was not created");
            Assert.That(mockdb.MockStringSetStore["UT:1:Sample"] == "{\"Data\":\"string\"}");
        }

        [Test]
        public void SSET_SinglePublish_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            string data = "{'1':{'Data':'string'}}";
            RedisUtility.SSET_Single(ref db, ref log, "UT", "Sample", data);
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);

            Assert.That(mockdb.MockChannel.ContainsKey("UT:1:Sample"));
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Data\":\"string\"}");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void HDEL_Single_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);
            mockdb.MockHashStore.Add("UT:1:Sample", new List<HashEntry>()
                {
                    new HashEntry("1", "data"),
                    new HashEntry("2", "data")
                });
            string data = "{'1':{'ID':2}}";
            RedisUtility.HDEL_Single(ref db, ref log, "UT", "Sample", data);

            Assert.That(mockdb.MockHashStore.ContainsKey("UT:1:Sample"), "Redis Hash must exist");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Exists(k => k.Name == 1), "Redis Key 1 must not be deleted");
            Assert.That(!mockdb.MockHashStore["UT:1:Sample"].Exists(k => k.Name == 2), "Redis Key 2 must be deleted");
        }

        [Test]
        public void HDEL_Multi_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            ILogger log = new Mocks.MockLogger();
            IDatabase db = conn.GetDatabase();
            MockRedisDatabase mockdb = (MockRedisDatabase)(db);
            mockdb.MockHashStore.Add("UT:1:Sample", new List<HashEntry>()
                {
                    new HashEntry("1", "data"),
                    new HashEntry("2", "data")
                });
            string data = "{'1':[{'ID':2},{'ID':1}]}";
            RedisUtility.HDEL_Multi(ref db, ref log, "UT", "Sample", data);

            Assert.That(mockdb.MockHashStore.ContainsKey("UT:1:Sample"), "Redis Hash must exist");
            Assert.That(mockdb.MockHashStore["UT:1:Sample"].Count == 0, "All keys must be deleted from hash set");
        }
    }
}
