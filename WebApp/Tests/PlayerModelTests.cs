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
            PlayerModel model = new PlayerModel(CreateMockDataRow());
            Assert.That(model.UCID == "12345");
            Assert.That(model.Name == "MockPlayer");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("UCID");
            table.Columns.Add("Name");
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { "12345", "MockPlayer" };

            return row;
        }
    }

}
