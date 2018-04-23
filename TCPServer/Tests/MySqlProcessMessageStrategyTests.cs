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

namespace Tests
{
    [TestFixture]
    class MySqlProcessMessageStrategyTests
    {
        [Test]
        public void ProcessMessage_GetServerNoHTMLDescription_Success()
        {
            IDbConnection connection = new Mocks.MockDBConnection();
            ILogger logger = new Mocks.MockLogger();
            IConfigReader config = new Mocks.MockConfigReader();

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "GetOrAddServer",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'ServerName':'Dev Kaukasus Insurgency Server'}"
            };

            IProcessMessageStrategy strategy = new MySqlProcessMessageStrategy(connection, logger, config);
            ProtocolResponse response = strategy.Process(request, logger);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "GetOrAddServer");
            Assert.That((int)response.Data[0][0] == 1);
        }

        [Test]
        public void ProcessMessage_GetServerWithHTMLDescription_Success()
        {
            IDbConnection connection = new Mocks.MockDBConnection();
            ILogger logger = new Mocks.MockLogger();
            IConfigReader config = new Mocks.MockConfigReader();

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "GetOrAddServer",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'ServerName':'Dev Kaukasus Insurgency Server','Description':'Hello World <p></p>'}"
            };

            IProcessMessageStrategy strategy = new MySqlProcessMessageStrategy(connection, logger, config);
            ProtocolResponse response = strategy.Process(request, logger);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "GetOrAddServer");
            Assert.That((int)response.Data[0][0] == 1);
        }

        [Test]
        public void Stub()
        {
            IDbConnection connection = new Mocks.MockDBConnection();
            ILogger logger = new Mocks.MockLogger();
            IConfigReader config = new Mocks.MockConfigReader(
                new List<string> { "b", "p" },
                new Dictionary<string, string>
                {
                    { "AddOrUpdateCapturePoint", "CP" },
                    { "AddOrUpdateDepot", "Depot" },
                    { "AddOrUpdateSideMission", "SM" }
                });

            IProcessMessageStrategy strategy = new MySqlProcessMessageStrategy(connection, logger, config);
        }
    }
}
