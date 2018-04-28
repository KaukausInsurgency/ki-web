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

namespace Tests
{
    [TestFixture]
    class MySqlProcessMessageStrategyTests
    {
        // While this test does not exercise the real behaviour (ie a SQLException would be thrown when trying to call
        // the stored procedure because the argument 'Description' was not found, we are just checking here that no
        // exception is being thrown and that the system handles the call correctly
        [Test]
        public void ProcessMessage_GetServerNoHTMLDescription_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection());

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "GetOrAddServer",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'ServerName':'Dev Kaukasus Insurgency Server'}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "GetOrAddServer");
            Assert.That((int)response.Data[0][0] == 1);
        }

        [Test]
        public void ProcessMessage_GetServerWithHTMLDescription_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection());

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "GetOrAddServer",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'ServerName':'Dev Kaukasus Insurgency Server','Description':'Hello World <p></p>'}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "GetOrAddServer");
            Assert.That((int)response.Data[0][0] == 1);
        }
        
        [Test]
        public void ProcessMessage_SampleCallException_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection(new MockMySqlThrowExceptionBehaviour()));

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'Param1':'Test','Param2':'Hello World <p></p>'}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == false);
            Assert.That(response.Error == ("Error executing query against MySQL (Action: " + request.Action + ") - A sample exception has occurred"));
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 0);
        }

        [Test]
        public void ProcessMessage_EmptyData_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection());

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That((int)response.Data[0][0] == 1);
        }

        [Test]
        public void ProcessMessage_VariousDataTypes_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection());

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'Param1':25,'Param2':true,'Param3':'string','Param4':2.25,'Param5':-9999}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");
            Assert.That((int)response.Data[0][0] == 1);
        }    

        [Test]
        public void ProcessMessage_SingleQueryReturnSingleRow_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection(new MockMySqlSuccessBehaviour()));

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = false,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "{'Param':25}"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");

            Assert.That(response.Data.Count == 1, "SingleQuery should only return a single row of data", null);
            Assert.That((int)response.Data[0][0] == 1);
            Assert.That((bool)response.Data[0][1] == true);
            Assert.That((string)response.Data[0][2] == "string1");
            Assert.That((double)response.Data[0][3] == 12.25);
        }

        [Test]
        public void ProcessMessage_BulkQueryMultipleReturn_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection(new MockMySqlSuccessBehaviour()));

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = true,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "[{'Param1':1},{'Param2':2}]"
            };

            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == true);
            Assert.That(response.Error == "");
            Assert.That(response.Action == "SampleCall");

            Assert.That(response.Data.Count == 2, "BulkQuery should return multiple rows of data", null);
            Assert.That((int)response.Data[0][0] == 1);
            Assert.That((bool)response.Data[0][1] == true);
            Assert.That((string)response.Data[0][2] == "string1");
            Assert.That((double)response.Data[0][3] == 12.25);
            Assert.That((int)response.Data[1][0] == 1);
        }

        [Test]
        public void ProcessMessage_BulkQueryException_Success()
        {
            IProcessMessageStrategy strategy = CreateMySqlProcessStrategyWithMocks(new Mocks.MockDBConnection(new MockMySqlBulkQueryExceptionBehaviour()));

            ProtocolRequest request = new ProtocolRequest
            {
                Action = "SampleCall",
                Destination = "MYSQL",
                IsBulkQuery = true,
                IPAddress = "127.0.0.1",
                Type = Newtonsoft.Json.Linq.JTokenType.Object,
                Data = "[{'Param1':1},{'Param2':2}]"
            };
      
            ProtocolResponse response = strategy.Process(request);

            Assert.That(response.Result == false);
            Assert.That(response.Error == "Error executing query against MySQL (Action: " + request.Action + ") - A sample bulk query exception has occurred");
            Assert.That(response.Action == "SampleCall");
            Assert.That(response.Data.Count == 1);
        }

        private IProcessMessageStrategy CreateMySqlProcessStrategyWithMocks(IDbConnection conn)
        {
            return new MySqlProcessMessageStrategy(conn, new Mocks.MockLogger(), new Mocks.MockConfigReader());
        }
    }
}
