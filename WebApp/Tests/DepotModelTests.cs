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
    class DepotModelTests
    {
        [Test]
        public void ConstructDepotModel_PlainData_Success()
        {
            DepotModel model = new DepotModel(CreateMockDataRow());
            Assert.That(model.ID == 1);
            Assert.That(model.Name == "Mock");
            Assert.That(model.LatLong == "LatLong");
            Assert.That(model.MGRS == "MGRS");
            Assert.That(model.Status == "Online");
            Assert.That(model.StatusChanged == false);
            Assert.That(model.Resources == "Resources");
            Assert.That(model.Pos.X == 1.25);
            Assert.That(model.Pos.Y == 0.25);
            Assert.That(model.Image == "Image");
        }

        [Test]
        public void ConstructDepotModel_InfiniteCapacityCase1_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["CurrentCapacity"] = -1;
            DepotModel model = new DepotModel(dr);
            Assert.That(model.Capacity == "Infinite");
        }

        [Test]
        public void ConstructDepotModel_InfiniteCapacityCase2_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Capacity"] = -1;
            DepotModel model = new DepotModel(dr);
            Assert.That(model.Capacity == "Infinite");
        }

        [Test]
        public void ConstructDepotModel_FiniteCapacity_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["CurrentCapacity"] = 5;
            dr["Capacity"] = 10;
            DepotModel model = new DepotModel(dr);
            Assert.That(model.Capacity == "5 / 10");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("DepotID", typeof(int));
            table.Columns.Add("Name");
            table.Columns.Add("LatLong");
            table.Columns.Add("MGRS");
            table.Columns.Add("Status");
            table.Columns.Add("StatusChanged", typeof(ulong));  // for some reason mySql casts BIT(1) as ulong
            table.Columns.Add("Resources");
            table.Columns.Add("X", typeof(double));
            table.Columns.Add("Y", typeof(double));
            table.Columns.Add("ImagePath");
            table.Columns.Add("CurrentCapacity", typeof(int));
            table.Columns.Add("Capacity", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 1, "Mock", "LatLong", "MGRS", "Online", false, "Resources", 1.25, 0.25, "Image", 0, 0 };

            return row;
        }
    }
}
