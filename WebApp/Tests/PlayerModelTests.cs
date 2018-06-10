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
    class PlayerModelTests
    {
        [Test]
        public void ConstructPlayerModel_PlainData_Success()
        {
            PlayerModel model = new PlayerModel(CreateMockDataRow(300));
            Assert.That(model.UCID == "12345");
            Assert.That(model.Name == "MockPlayer");
            Assert.That(!model.Banned);
            Assert.That(model.GameTime == "00:05:00");
            Assert.That(model.Sorties == 20);
            Assert.That(model.Kills == 50);
        }

        [Test]
        public void ConstructPlayerModel_NullGameTime_Success()
        {
            PlayerModel model = new PlayerModel(CreateMockDataRow(DBNull.Value));
            Assert.That(model.UCID == "12345");
            Assert.That(model.Name == "MockPlayer");
            Assert.That(!model.Banned);
            Assert.That(model.GameTime == "00:00:00");
            Assert.That(model.Sorties == 20);
            Assert.That(model.Kills == 50);
        }

        private DataRow CreateMockDataRow(object time)
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("UCID");
            table.Columns.Add("Name");
            table.Columns.Add("Banned", typeof(ulong));
            table.Columns.Add("GameTime", typeof(long));
            table.Columns.Add("Sorties", typeof(long));
            table.Columns.Add("Kills", typeof(long));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { "12345", "MockPlayer", false, time, 20, 50 };

            return row;
        }
    }

}
