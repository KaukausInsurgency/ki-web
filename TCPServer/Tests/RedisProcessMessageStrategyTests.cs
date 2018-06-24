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
        public void ProcessMessage_RedisPublishBulkSingle_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(), 
                JTokenType.Array, "[{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}]", true);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "[{\"Name\":\"DepotA\"},{\"Name\":\"DepotB\"},{\"Name\":\"DepotC\"}]");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 1);
        }

        [Test]
        public void ProcessMessage_RedisPublishBulkMulti_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':{'Name':'DepotA'}},{'1':{'Name':'DepotB'}},{'1':{'Name':'DepotC'}}]", true);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());

            // should be the last item published
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Name\":\"DepotC\"}", "Last published message is incorrect");
            Assert.That(mockdb.MockChannelCount["UT:1:Sample"] == 3, "3 Messages must be published through Redis for this key");

            Assert.That(mockdb.MockListStore["UT:1:Sample"].Count == 3);
            Assert.That(mockdb.MockListStore["UT:1:Sample"][0] == "{\"Name\":\"DepotA\"}");
            Assert.That(mockdb.MockListStore["UT:1:Sample"][1] == "{\"Name\":\"DepotB\"}");
            Assert.That(mockdb.MockListStore["UT:1:Sample"][2] == "{\"Name\":\"DepotC\"}");
        }


        [Test]
        public void ProcessMessage_BulkPublishSingleResponse_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':[{'Name':'DepotA'},{'Name':'DepotB'},{'Name':'DepotC'}]}]", true);

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 1);
            Assert.That((string)response.Data[0][0] == "UT:1:Sample");
        }

        [Test]
        public void ProcessMessage_BulkPublishMultiResponse_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Array, "[{'1':{'Name':'DepotA'}},{'1':{'Name':'DepotB'}},{'1':{'Name':'DepotC'}}]", true);

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 3);
            foreach (List<object> r in response.Data)
                Assert.That((string)r[0] == "UT:1:Sample");
        }

        [Test]
        public void ProcessMessage_RedisPublishSingle_Success()
        {
            IConnectionMultiplexer conn = null;
            ProtocolResponse response = GenerateMockStrategyResponse(out conn, new MockRedisPublishSuccessBehaviour(),
                JTokenType.Object, "{'1':{'Name':'CapturePointA','ServerID':1}}", false);

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockChannel["UT:1:Sample"] == "{\"Name\":\"CapturePointA\",\"ServerID\":1}");
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
            Assert.That((string)response.Data[0][0] == "UT:1:Sample");
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
