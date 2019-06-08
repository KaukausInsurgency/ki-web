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
    class InvalidProcessMessageStrategyTests
    {
        [Test]
        public void ProcessMessage_Default_Success()
        {
            IProcessMessageStrategy strategy = new InvalidProcessMessageStrategy();

            ProtocolRequest request = new ProtocolRequest()
            {
                Action = "SampleAction",
                IsBulkQuery = false,
                Destination = "Unknown",
                Data = "{}",
                IPAddress = "",
                Type = Newtonsoft.Json.Linq.JTokenType.Object
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == false);
            Assert.That(response.Action == "SampleAction");
            Assert.That(response.Error == "Invalid Destination specified - must be either 'REDIS' or 'MYSQL'");
        }
    }
}
