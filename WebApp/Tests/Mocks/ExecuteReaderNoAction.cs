using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Interfaces;

namespace Tests.Mocks
{
    class ExecuteReaderNoAction : IExecuteReader
    {
        IDataReader IExecuteReader.Execute()
        {
            DataTable dt = new DataTable();
            return dt.CreateDataReader();
        }
    }
}
