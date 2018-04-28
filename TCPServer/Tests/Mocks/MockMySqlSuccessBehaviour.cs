using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockMySqlSuccessBehaviour : IExecuteReader
    {
        IDataReader IExecuteReader.Execute()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Col1", typeof(int));
            dt.Columns.Add("Col2", typeof(bool));
            dt.Columns.Add("Col3", typeof(string));
            dt.Columns.Add("Col4", typeof(double));
            dt.Rows.Add(new object[] { 1, true, "string1", 12.25 });
            return dt.CreateDataReader();
        }
    }
}
