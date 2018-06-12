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
    class SideMissionModelTests
    {
        [Test]
        public void ConstructSideMissionModel_PlainData_Success()
        {
            SideMissionModel model = new SideMissionModel(CreateMockDataRow());
            Assert.That(model.ID == 1);
            Assert.That(model.Name == "Server");
            Assert.That(model.Desc == "Description");
            Assert.That(model.Image == "Image");
            Assert.That(model.Status == "Active");
            Assert.That(model.StatusChanged == false);
            Assert.That(model.LatLong == "LatLong");
            Assert.That(model.MGRS == "MGRS");
            Assert.That(model.TimeRemaining == "00:01:40");
            Assert.That(model.TimeInactive > 0);
        }

        [Test]
        public void ConstructSideMissionModel_TimeInactiveNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["TimeInactive"] = DBNull.Value;
            SideMissionModel model = new SideMissionModel(dr);
            Assert.That(model.TimeInactive == 0);
        }

        [Test]
        public void ConstructSideMissionModel_TimeRemainingNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["TimeRemaining"] = DBNull.Value;
            SideMissionModel model = new SideMissionModel(dr);
            Assert.That(model.TimeRemaining == "00:00:00");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("ServerMissionID", typeof(int));
            table.Columns.Add("Name");
            table.Columns.Add("Description");
            table.Columns.Add("ImagePath");
            table.Columns.Add("Status");
            table.Columns.Add("StatusChanged", typeof(ulong));
            table.Columns.Add("LatLong");
            table.Columns.Add("MGRS");
            table.Columns.Add("TimeRemaining", typeof(double));
            table.Columns.Add("TimeInactive", typeof(DateTime));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 1, "Server", "Description", "Image", "Active", false, "LatLong", "MGRS", 100.5, new DateTime(2018, 1, 1) };

            return row;
        }

    }
}
