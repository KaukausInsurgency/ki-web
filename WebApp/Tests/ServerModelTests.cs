using KIWebApp.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class ServerModelTests
    {
        [Test]
        public void ConstructServerModel_PlainData_Success()
        {
            ServerModel model = new ServerModel(CreateMockDataRow());
            Assert.That(model.ServerID == 1);
            Assert.That(model.ServerName == "Mock");
            Assert.That(model.IPAddress == "127.0.0.1");
            Assert.That(model.Status == "Online");
            Assert.That(model.OnlinePlayers == 10);
            Assert.That(model.RestartTime == new TimeSpan(300 * TimeSpan.TicksPerSecond));
            Assert.That(model.StatusImage == "Images/status-green-128x128.png");
        }

        [Test]
        public void ConstructServerModel_RestartTimeNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["RestartTime"] = DBNull.Value;
            ServerModel model = new ServerModel(dr);
            Assert.That(model.RestartTime == new TimeSpan(0,0,0));
        }

        [Test]
        public void ConstructServerModel_StatusNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Status"] = DBNull.Value;
            ServerModel model = new ServerModel(dr);
            Assert.That(model.Status == "Offline");
            Assert.That(model.StatusImage == "Images/status-red-128x128.png");
        }

        [Test]
        public void ConstructServerModel_StatusOffline_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Status"] = "Offline";
            ServerModel model = new ServerModel(dr);
            Assert.That(model.Status == "Offline");
            Assert.That(model.StatusImage == "Images/status-red-128x128.png");
        }

        [Test]
        public void ConstructServerModel_StatusYellow_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Status"] = "Restarting";
            ServerModel model = new ServerModel(dr);
            Assert.That(model.Status == "Restarting");
            Assert.That(model.StatusImage == "Images/status-yellow-128x128.png");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("ServerID", typeof(int));
            table.Columns.Add("ServerName");
            table.Columns.Add("IPAddress");
            table.Columns.Add("Status");
            table.Columns.Add("OnlinePlayers", typeof(long));
            table.Columns.Add("RestartTime", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 1, "Mock", "127.0.0.1", "Online", 10, 300};

            return row;
        }
    }

}
