using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests.Mocks
{
    class MockDBConnection : IDbConnection
    {
        private string _conn;
        public int OpenCalled { get; private set; }
        private ConnectionState _state;
        string IDbConnection.ConnectionString { get => _conn; set => _conn = value; }
        private IExecuteReader _executeReader;

        public MockDBConnection()
        {
            _executeReader = null;
            OpenCalled = 0;
        }

        public MockDBConnection(IExecuteReader executeReader)
        {
            _executeReader = executeReader;
            OpenCalled = 0;
        }

        int IDbConnection.ConnectionTimeout => 60;

        string IDbConnection.Database => "MockDB";

        ConnectionState IDbConnection.State => _state;

        IDbTransaction IDbConnection.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        IDbTransaction IDbConnection.BeginTransaction(IsolationLevel il)
        {
            throw new NotImplementedException();
        }

        void IDbConnection.ChangeDatabase(string databaseName)
        {
            throw new NotImplementedException();
        }

        void IDbConnection.Close()
        {
            _state = ConnectionState.Closed;
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            if (_executeReader == null)
                return new MockDBCommand();
            else
                return new MockDBCommand(_executeReader);
        }

        void IDbConnection.Open()
        {
            _state = ConnectionState.Open;
            OpenCalled++;
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
        // ~MockDBConnection() {
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
