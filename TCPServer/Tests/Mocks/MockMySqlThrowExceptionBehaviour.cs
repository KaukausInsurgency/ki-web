using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockMySqlThrowExceptionBehaviour : IExecuteReader
    {
        IDataReader IExecuteReader.Execute()
        {
            throw new Exception("A sample exception has occurred");
        }
    }
}
