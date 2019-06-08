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
using System.Data.SqlClient;
using StackExchange.Redis;
using Newtonsoft.Json.Linq;

namespace Tests
{
    [TestFixture]
    class RedisProcessMessageStrategyTests
    {
        private ProtocolResponse GenerateMockStrategyResponse(out IConnectionMultiplexer conn, IRedisPublishBehaviour behaviour, JTokenType jtype, string data, bool bulkquery, string redisAction)
        {
            conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), behaviour);
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, RedisAction>
                {
                    { "SampleCall", new RedisAction("Sample", redisAction) },
                });

            IProcessMessageStrategy strategy = new RedisProcessMessageStrategy(conn, new Mocks.MockLogger(), config);

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "REDIS",
                IsBulkQuery = bulkquery,
                IPAddress = "127.0.0.1",
                Type = jtype,
                Data = data
            };

            return strategy.Process(request);
        }

        [Test]
        [TestCase("RPUSH")]
        [TestCase("HSET")]
        public void ProcessMessage_RedisPublishBulk_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(), 
                JTokenType.Array, "{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}", true, redisaction);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "[{\"Name\":\"DepotA\"},{\"Name\":\"DepotB\"},{\"Name\":\"DepotC\"}]");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void ProcessMessage_RedisPublishBulkSSET_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':{'Name':'DepotA'}},{'1':{'Name':'DepotB'}},{'1':{'Name':'DepotC'}}]", true, "SET");

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());

            // should be the last item published
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Name\":\"DepotC\"}", "Last published message is incorrect");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 3, "3 Messages must be published through Redis for this key");
            Assert.That(mockdb.MockStringSetStore["UT:1:Sample"] == "{\"Name\":\"DepotC\"}");
        }


        [Test]
        [TestCase("RPUSH")]
        [TestCase("HSET")]
        public void ProcessMessage_BulkResponse_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}", true, redisaction);

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        public void ProcessMessage_BulkSETResponse_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':{'Name':'DepotA'}},{'1':{'Name':'DepotB'}},{'1':{'Name':'DepotC'}}]", true, "SET");

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        [TestCase("RPUSH")]
        [TestCase("SET")]
        [TestCase("HSET")]
        public void ProcessMessage_RedisPublishSingle_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false, redisaction);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Name\":\"CapturePointA\",\"ServerID\":1}");
        }

        [Test]
        [TestCase("RPUSH")]
        [TestCase("SET")]
        [TestCase("HSET")]
        public void ProcessMessage_RedisPublishSingleNull_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':-9999,'ServerID':1}}", false, redisaction);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Name\":null,\"ServerID\":1}");

            dynamic JsonObject =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<dynamic>(mockdb.MockChannel["UT:1:Sample"]);

            Assert.That(JsonObject.Name == null);
        }

        [Test]
        [TestCase("RPUSH")]
        [TestCase("SET")]
        [TestCase("HSET")]
        public void ProcessMessage_SingleResponse_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false, redisaction);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
            //Assert.That((string)response.Data[0][0] == "UT:1:Sample");
        }

        [Test]
        [TestCase("RPUSH")]
        [TestCase("SET")]
        [TestCase("HSET")]
        public void ProcessMessage_SampleCallException_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishThrowExceptionBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false, redisaction);

            Assert.That(response.Result == false);
            Assert.That(!String.IsNullOrWhiteSpace(response.Error));
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        public void ProcessMessage_InvalidRedisKeyError_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), new MockRedisPublishSuccessBehaviour());
            IConfigReader config = new Mocks.MockConfigReader();

            IProcessMessageStrategy strategy = new RedisProcessMessageStrategy(conn, new Mocks.MockLogger(), config);

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "AddOrUpdateCapturePoint",
                Destination = "REDIS",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'1:1':{'Name':'CapturePointA','ServerID':1}}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == false);
            Assert.That(!String.IsNullOrWhiteSpace(response.Error));
            Assert.That(response.Action == "AddOrUpdateCapturePoint");
            Assert.That(response.Data.Count == 0);

        }


        [Test]
        [TestCase("RPUSH")]
        [TestCase("SET")]
        [TestCase("HSET")]
        public void ProcessMessage_EmptyData_Success(string redisaction)
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{}", false, redisaction);

            Assert.That(response.Result == false);
            Assert.That(!String.IsNullOrWhiteSpace(response.Error));
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

    }
}
