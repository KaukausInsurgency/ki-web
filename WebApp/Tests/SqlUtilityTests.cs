using KIWebApp.Classes;
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
    class SqlUtilityTests
    {
        [Test]
        public void CreateCommand_Simple_Success()
        {
            IDbCommand cmd = SqlUtility.CreateCommand(new Mocks.MockDBConnection(new Mocks.ExecuteReaderNoAction()), "TestSproc", new Dictionary<string, object>()
            {
                { "Arg1", 25 },
                { "Arg2", "string" },
                { "Arg3", false }
            });

            Assert.That(cmd.CommandText == "TestSproc");
            Assert.That(cmd.CommandType == CommandType.StoredProcedure);
            Assert.That(cmd.Parameters.Count == 3);
        }

        [Test]
        public void ConvertTimeTicksToString_NullData_Success()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("Blah", typeof(int));
            DataRow row = table.NewRow();

            string timestring = SqlUtility.ConvertTimeTicksToStringInt(ref row, "Blah");
            Assert.That(timestring == "00:00:00");
        }

        [Test]
        public void ConvertTimeTicksToString_WithData_Success()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("Time", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 90330 };    // 25 hours, 5 minutes, 30 seconds

            string timestring = SqlUtility.ConvertTimeTicksToStringInt(ref row, "Time");
            Assert.That(timestring == "25:05:30");
        }

        [Test]
        public void GetValueOrDefault_WithData_Success()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("Time", typeof(int));
            DataRow row = table.NewRow();
            row.ItemArray = new object[] { 300 };    // 25 hours, 5 minutes, 30 seconds

            Assert.That(SqlUtility.GetValueOrDefault(ref row, "Time", 300) == 300);
            
        }

        [Test]
        public void GetValueOrDefault_NullData_Success()
        {
            DataTable table = new DataTable("FakeTable");
            table.Columns.Add("Time", typeof(int));
            DataRow row = table.NewRow();

            Assert.That(SqlUtility.GetValueOrDefault(ref row, "Time", 300) == 300);

        }

    }
}
