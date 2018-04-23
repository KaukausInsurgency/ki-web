using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockDBCommand : IDbCommand
    {
        private IDbConnection _conn;
        private string _cmd;
        private int _timeout;
        private CommandType _type;
        private IDataParameterCollection _params = null;
        private DataTable _dt;

        public MockDBCommand()
        {
            Type type = typeof(SqlParameterCollection);
            _params = (SqlParameterCollection)Activator.CreateInstance(type, true);
            _dt = new DataTable("Mock");
            _dt.Columns.Add("MockCol", typeof(int));
            _dt.Rows.Add(1);
        }

        public MockDBCommand(DataTable dt)
        {
            Type type = typeof(SqlParameterCollection);
            _params = (SqlParameterCollection)Activator.CreateInstance(type, true);
            _dt = dt;
        }

        IDbConnection IDbCommand.Connection { get => _conn; set => _conn = value; }
        IDbTransaction IDbCommand.Transaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        string IDbCommand.CommandText { get => _cmd; set => _cmd = value; }
        int IDbCommand.CommandTimeout { get => _timeout; set => _timeout = value; }
        CommandType IDbCommand.CommandType { get => _type; set => _type = value; }

        IDataParameterCollection IDbCommand.Parameters => _params;

        UpdateRowSource IDbCommand.UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        void IDbCommand.Cancel()
        {
            throw new NotImplementedException();
        }

        IDbDataParameter IDbCommand.CreateParameter()
        {
            return new SqlParameter();
        }

        int IDbCommand.ExecuteNonQuery()
        {
            return 1;
        }

        IDataReader IDbCommand.ExecuteReader()
        {
            return _dt.CreateDataReader();
        }

        IDataReader IDbCommand.ExecuteReader(CommandBehavior behavior)
        {
            return _dt.CreateDataReader();
        }

        object IDbCommand.ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        void IDbCommand.Prepare()
        {
            throw new NotImplementedException();
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~MockDBCommand() {
        //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        //   Dispose(false);
        // }

        // This code added to correctly implement the disposable pattern.
        void IDisposable.Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            // TODO: uncomment the following line if the finalizer is overridden above.
            // GC.SuppressFinalize(this);
        }
        #endregion
    }
}
