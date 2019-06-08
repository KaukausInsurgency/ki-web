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
    class ServerViewModelTests
    {
        [Test]
        public void ConstructServerViewModel_PlainData_Success()
        {
            ServerViewModel model = new ServerViewModel(CreateMockDataRow(), 1);
            Assert.That(model.ServerID == 1);
            Assert.That(model.Status == "Online");
            Assert.That(model.RestartTimeString == "00:05:00");
        }

        [Test]
        public void ConstructServerViewModel_RestartTimeNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["RestartTime"] = DBNull.Value;
            ServerViewModel model = new ServerViewModel(dr, 1);
            Assert.That(model.RestartTimeString == "00:00:00");
        }

        [Test]
        public void ConstructServerViewModel_StatusNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Status"] = DBNull.Value;
            ServerViewModel model = new ServerViewModel(dr, 1);
            Assert.That(model.Status == "Offline");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("Status");
            table.Columns.Add("RestartTime", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { "Online", 300 };

            return row;
        }

    }
}
