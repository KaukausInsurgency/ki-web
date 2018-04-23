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

namespace Tests
{
    [TestFixture]
    public class SqlUtilityTests
    {

        [Test]
        public void CreateCommand_NoParameters_Success()
        {
            IDbCommand cmd = SqlUtility.CreateCommand(new Mocks.MockDBConnection(), "MockCommand", new Dictionary<string, object>());

            Assert.That(cmd.CommandText == "MockCommand");
            Assert.That(cmd.CommandType == CommandType.StoredProcedure);
            Assert.That(cmd.Parameters.Count == 0);
        }

        [Test]
        public void CreateCommand_LuaNull_Success()
        {
            Int64 luanull = -9999;
            IDbCommand cmd = SqlUtility.CreateCommand(new Mocks.MockDBConnection(), "MockCommand", new Dictionary<string, object>() { { "param1", luanull } });

            Assert.That(cmd.CommandText == "MockCommand");
            Assert.That(cmd.CommandType == CommandType.StoredProcedure);
            Assert.That(GetParameterValue(cmd.Parameters["param1"]) == null);
        }

        [Test]
        [Ignore("Real Nulls not supported")]
        public void CreateCommand_RealNull_Success()
        {
            IDbCommand cmd = SqlUtility.CreateCommand(new Mocks.MockDBConnection(), "MockCommand", new Dictionary<string, object>() { { "param1", null } });

            Assert.That(cmd.CommandText == "MockCommand");
            Assert.That(cmd.CommandType == CommandType.StoredProcedure);
            Assert.That(GetParameterValue(cmd.Parameters["param1"]) == null);
        }

        [Test]
        public void CreateCommand_DifferentParamTypes_Success()
        {
            Dictionary<string, object> dataparams = new Dictionary<string, object>
            {
                { "param1", "string" },
                { "param2", 25 },
                { "param3", true },
                { "param4", 12.25 },
                { "param5", -100 }
            };

            IDbCommand cmd = SqlUtility.CreateCommand(new Mocks.MockDBConnection(), "MockCommand", dataparams);

            Assert.That(cmd.CommandText == "MockCommand");
            Assert.That(cmd.CommandType == CommandType.StoredProcedure);
            Assert.That((string)GetParameterValue(cmd.Parameters["param1"]) == "string");
            Assert.That((int)GetParameterValue(cmd.Parameters["param2"]) == 25);
            Assert.That((bool)GetParameterValue(cmd.Parameters["param3"]) == true);
            Assert.That((double)GetParameterValue(cmd.Parameters["param4"]) == 12.25);
            Assert.That((int)GetParameterValue(cmd.Parameters["param5"]) == -100);
        }

        [Test]
        public void InvokeCommand_NoResults_Success()
        {
            DataTable dt = new DataTable();
            IDbCommand cmd = new Mocks.MockDBCommand(dt);
            IDbDataParameter p = cmd.CreateParameter();
            p.ParameterName = "param1";
            p.Value = 1;

            List<object> results = SqlUtility.InvokeCommand(cmd, out string error);

            Assert.That(error == "No Results Returned\n");
            Assert.That(results == null);
        }

        [Test]
        public void InvokeCommand_Results_Success()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Col1", typeof(int));
            dt.Columns.Add("Col2", typeof(bool));
            dt.Columns.Add("Col3", typeof(string));
            dt.Columns.Add("Col4", typeof(double));
            dt.Rows.Add(new object[] { 1, true, "string", 12.25 });
            IDbCommand cmd = new Mocks.MockDBCommand(dt);
            IDbDataParameter p = cmd.CreateParameter();
            p.ParameterName = "param1";
            p.Value = 1;

            List<object> results = SqlUtility.InvokeCommand(cmd, out string error);

            Assert.That(error == "");
            Assert.That((int)results[0] == 1);
            Assert.That((bool)results[1] == true);
            Assert.That((string)results[2] == "string");
            Assert.That((double)results[3] == 12.25);
        }

        private object GetParameterValue(object p)
        {
            return ((System.Data.SqlClient.SqlParameter)(p)).Value;
        }
    }
}
