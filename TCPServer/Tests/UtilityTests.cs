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
using Newtonsoft.Json.Linq;

namespace Tests
{
    [TestFixture]
    class UtilityTests
    {
        [Test]
        public void CreateRequest_Typical_Success()
        {
            string json = "{'Action':'Request','Destination':'MYSQL','BulkQuery':false,'Data':{}}";
            dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            ProtocolRequest request = Utility.CreateRequest(ref j, "127.0.0.1");
            Assert.That(request.Action == "Request");
            Assert.That(request.Destination == "MYSQL");
            Assert.That(request.IsBulkQuery == false);
            Assert.That(request.IPAddress == "127.0.0.1");
        }

        [Test]
        public void CreateRequest_EmptyData_Success()
        {
            string json = "{'Action':'Request','Destination':'MYSQL','BulkQuery':true,'Data':{}}";
            dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            ProtocolRequest request = Utility.CreateRequest(ref j, "127.0.0.1");
            Assert.That(request.Type == ((JToken)(j.Data)).Type);
            Assert.That(request.Data == "{}");
        }

        [Test]
        public void CreateRequest_ComplexData_Success()
        {
            string json = "{'Action':'','Destination':'','BulkQuery':false,'Data':{'ServerID':1,'Description':'Hello World','SessionID':1}}";
            dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            ProtocolRequest request = Utility.CreateRequest(ref j, "127.0.0.1");
            Assert.That(request.Type == ((JToken)(j.Data)).Type);
            Assert.That(request.Data == "{\"ServerID\":1,\"Description\":\"Hello World\",\"SessionID\":1}");
        }

        [Test]
        public void CreateRequest_MultiData_Success()
        {
            string json = "{'Action':'','Destination':'','BulkQuery':false,'Data':[{'ServerID':1,'SessionID':1},{'ServerID':2,'SessionID':2}]}";
            dynamic j = Newtonsoft.Json.JsonConvert.DeserializeObject(json);

            ProtocolRequest request = Utility.CreateRequest(ref j, "127.0.0.1");
            Assert.That(request.Type == ((JToken)(j.Data)).Type);
            Assert.That(request.Data == "[{\"ServerID\":1,\"SessionID\":1},{\"ServerID\":2,\"SessionID\":2}]");
        }

        [Test]
        public void SanitizeHtml_All_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader();
            string html = "<p><l><something attr='' /></p>this is not sanitized";
            string shtml = Utility.SanitizeHTML(html, ref config);
            Assert.That(shtml == "&lt;p&gt;&lt;l&gt;&lt;something attr=&#39;&#39; /&gt;&lt;/p&gt;this is not sanitized");
        }

        [Test]
        public void SanitizeHtml_Some_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader(new List<string>() { "p", "l" }, new Dictionary<string, string>());
            string html = "<p><l><something attr='' /></p>this is not sanitized";
            string shtml = Utility.SanitizeHTML(html, ref config);
            Assert.That(shtml == "<p><l>&lt;something attr=&#39;&#39; /&gt;</p>this is not sanitized");
        }

        [Test]
        public void SanitizeHtml_None_Success()
        {
            IConfigReader config = new Mocks.MockConfigReader(new List<string>() { "p", "l" }, new Dictionary<string, string>());
            string html = "there is no html characters here to escape";
            string shtml = Utility.SanitizeHTML(html, ref config);
            Assert.That(shtml == html);
        }
    }
}
