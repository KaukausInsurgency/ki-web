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
        private ProtocolResponse GenerateMockStrategyResponse(out IConnectionMultiplexer conn, IRedisPublishBehaviour behaviour, JTokenType jtype, string data, bool bulkquery)
        {
            conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour(), behaviour);
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, string>
                {
                    { "SampleCall", "Sample" },
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
        public void ProcessMessage_RedisPublishBulk_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(), 
                JTokenType.Array, "[{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}]", true);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["Sample:1"] == "[{\"Name\":\"DepotA\"},{\"Name\":\"DepotB\"},{\"Name\":\"DepotC\"}]");
        }


        [Test]
        public void ProcessMessage_BulkPublishResponse_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}]", true);

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 1);
            Assert.That((string)response.Data[0][0] == "Sample:1");
        }

        [Test]
        public void ProcessMessage_RedisPublishSingle_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["Sample:1"] == "{\"Name\":\"CapturePointA\",\"ServerID\":1}");
        }


        [Test]
        public void ProcessMessage_SinglePublishResponse_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 1);
            Assert.That((string)response.Data[0][0] == "Sample:1");
        }

        [Test]
        public void ProcessMessage_SampleCallException_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishThrowExceptionBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false);

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
        public void ProcessMessage_EmptyData_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{}", false);

            Assert.That(response.Result == false);
            Assert.That(!String.IsNullOrWhiteSpace(response.Error));
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

    }
}
