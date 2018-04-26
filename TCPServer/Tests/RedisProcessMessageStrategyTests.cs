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

namespace Tests
{
    [TestFixture]
    class RedisProcessMessageStrategyTests
    {
        [Test]
        public void ProcessMessage_BulkRedisSet_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour());
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, string>
                {
                    { "AddOrUpdateDepot", "Depot" },
                });

            IProcessMessageStrategy strategy = new RedisProcessMessageStrategy(conn, new Mocks.MockLogger(), config);

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "AddOrUpdateDepot",
                Destination = "REDIS",
                IsBulkQuery = true,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Array,
                Data = "[{'1:1':{'Name':'DepotA'}},{'2:2':{'Name':'DepotB'}},{'3:3':{'Name':'DepotC'}}]"
            };

            ProtocolResponse response = strategy.Process(request);

            // Assert that the response object is correct
            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "AddOrUpdateDepot");
            Assert.That(response.Data.Count == 3);
            Assert.That((string)response.Data[0][0] == "Depot:1:1");
            Assert.That((string)response.Data[1][0] == "Depot:2:2");
            Assert.That((string)response.Data[2][0] == "Depot:3:3");

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockDBStore["Depot:1:1"] != "{'Name':'DepotA'}");
            Assert.That(mockdb.MockDBStore["Depot:2:2"] != "{'Name':'DepotB'}");
            Assert.That(mockdb.MockDBStore["Depot:3:3"] != "{'Name':'DepotC'}");
        }

        [Test]
        public void ProcessMessage_SingleRedisSet_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour());
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, string>
                {
                    { "AddOrUpdateCapturePoint", "CP" },
                });

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

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "AddOrUpdateCapturePoint");
            Assert.That(response.Data.Count == 1);
            Assert.That((string)response.Data[0][0] == "CP:1:1");

            // Confirm that the data stored in the mock redis is correct
            MockRedisDatabase mockdb = (MockRedisDatabase)(conn.GetDatabase());
            Assert.That(mockdb.MockDBStore["CP:1:1"] == "{\"Name\":\"CapturePointA\",\"ServerID\":1}");
        }

        [Test]
        public void ProcessMessage_SampleCallException_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisThrowExceptionBehaviour());
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, string>
                {
                    { "AddOrUpdateCapturePoint", "CP" },
                });

            IProcessMessageStrategy strategy = new RedisProcessMessageStrategy(conn, new Mocks.MockLogger(), config);

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "AddOrUpdateCapturePoint",
                Destination = "REDIS",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'Name':'CapturePointA','ServerID':1}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == false);
            Assert.That(response.Error == ("Error executing query against Redis (Action: " + request.Action + ") - Mock Redis Exception"));
            Assert.That(response.Action == "AddOrUpdateCapturePoint");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        public void ProcessMessage_RedisSetFail_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisFailBehaviour());
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string>(),
                new Dictionary<string, string>
                {
                    { "AddOrUpdateCapturePoint", "CP" },
                });

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
            Assert.That(response.Error == ("Failed to Set Key in Redis (Key: 'CP:1:1')"));
            Assert.That(response.Action == "AddOrUpdateCapturePoint");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        public void ProcessMessage_InvalidRedisKeyError_Success()
        {
            IConnectionMultiplexer conn = new Mocks.MockConnectionMultiplexer(new MockRedisSuccessBehaviour());
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
            Assert.That(response.Error == ("Error executing query against Redis - Action: '" + request.Action + "' not found in server configuration - please check action message or server configuration."));
            Assert.That(response.Action == "AddOrUpdateCapturePoint");
            Assert.That(response.Data.Count == 0);

        }


        [Test]
        public void ProcessMessage_EmptyDataError_Success()
        {
        }

        private class MockRedisSuccessBehaviour : IRedisExecuteBehaviour
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

        private class MockRedisFailBehaviour : IRedisExecuteBehaviour
        {
            bool IRedisExecuteBehaviour.Execute(RedisKey key, RedisValue value, TimeSpan? expiry, When when, CommandFlags flags)
            {
                return false;
            }

            bool IRedisExecuteBehaviour.Execute(KeyValuePair<RedisKey, RedisValue>[] values, When when, CommandFlags flags)
            {
                return false;
            }
        }

        private class MockRedisThrowExceptionBehaviour : IRedisExecuteBehaviour
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
}
