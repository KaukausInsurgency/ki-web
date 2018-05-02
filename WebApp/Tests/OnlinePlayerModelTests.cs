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
    class OnlinePlayerModelTests
    {
        [Test]
        public void ConstructOnlinePlayerModel_PlainData_Success()
        {
            OnlinePlayerModel model = new OnlinePlayerModel(CreateMockDataRow());
            Assert.That(model.UCID == "12345");
            Assert.That(model.Name == "MockPlayer");
            Assert.That(model.Role == "Mock");
            Assert.That(model.RoleImage == "Image");
            Assert.That(model.Ping == "100 ms");
            Assert.That(model.Side == "Red");
            Assert.That(model.Lives == "5");
        }

        [Test]
        public void ConstructOnlinePlayerModel_LivesNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Lives"] = DBNull.Value;
            OnlinePlayerModel model = new OnlinePlayerModel(dr);
            Assert.That(model.Lives == "");
        }

        [Test]
        public void ConstructOnlinePlayerModel_BlueSide_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Side"] = 2;
            OnlinePlayerModel model = new OnlinePlayerModel(dr);
            Assert.That(model.Side == "Blue");
        }

        [Test]
        public void ConstructOnlinePlayerModel_NeutralSide_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Side"] = 0;
            OnlinePlayerModel model = new OnlinePlayerModel(dr);
            Assert.That(model.Side == "Neutral");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("UCID");
            table.Columns.Add("Name");
            table.Columns.Add("Role");
            table.Columns.Add("RoleImage");
            table.Columns.Add("Ping");
            table.Columns.Add("Side", typeof(int));
            table.Columns.Add("Lives", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { "12345", "MockPlayer", "Mock", "Image", "100 ms", 1, 5 };

            return row;
        }

    }
}
