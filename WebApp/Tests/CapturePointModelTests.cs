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
    class CapturePointModelTests
    {
        [Test]
        public void ConstructCapturePointModel_PlainData_Success()
        {
            CapturePointModel model = new CapturePointModel(CreateMockDataRow());
            Assert.That(model.ID == 1);
            Assert.That(model.Type == "AIRPORT");
            Assert.That(model.Name == "Mock");
            Assert.That(model.LatLong == "LatLong");
            Assert.That(model.MGRS == "MGRS");
            Assert.That(model.MaxCapacity == 30);
            Assert.That(model.Status == "Red");
            Assert.That(model.StatusChanged == false);
            Assert.That(model.BlueUnits == 0);
            Assert.That(model.RedUnits == 20);
            Assert.That(model.Image == "Image");
            Assert.That(model.Text == "Text"); 
        }

        [Test]
        public void ConstructCapturePointModel_TextNull_Success()
        {
            DataRow dr = CreateMockDataRow();
            dr["Text"] = DBNull.Value;
            CapturePointModel model = new CapturePointModel(dr);
            Assert.That(model.Text == "");
        }

        private DataRow CreateMockDataRow()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("CapturePointID", typeof(int));
            table.Columns.Add("Type");
            table.Columns.Add("Name");
            table.Columns.Add("LatLong");
            table.Columns.Add("MGRS");
            table.Columns.Add("MaxCapacity", typeof(int));
            table.Columns.Add("Status");
            table.Columns.Add("StatusChanged", typeof(ulong));  // for some reason mySql casts BIT(1) as ulong
            table.Columns.Add("BlueUnits", typeof(int));
            table.Columns.Add("RedUnits", typeof(int));
            table.Columns.Add("ImagePath");
            table.Columns.Add("Text");
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 1, "AIRPORT", "Mock", "LatLong", "MGRS", 30, "Red", false, 0, 20, "Image", "Text" };

            return row;
        }

    }
}
