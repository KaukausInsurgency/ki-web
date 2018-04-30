using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Interfaces;

namespace Tests.Mocks
{
    class MockDBConnection : IDbConnection
    {
        private IExecuteReader _readerBehaviour;

        public MockDBConnection(IExecuteReader behaviour)
        {
            _readerBehaviour = behaviour;
        }

        void IDbConnection.Close()
        {
        }

        IDbCommand IDbConnection.CreateCommand()
        {
            return new Mocks.MockDBCommand(_readerBehaviour);
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

        #region unimplemented
        string IDbConnection.ConnectionString { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        int IDbConnection.ConnectionTimeout => throw new NotImplementedException();

        string IDbConnection.Database => throw new NotImplementedException();

        ConnectionState IDbConnection.State => throw new NotImplementedException();

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

        void IDbConnection.Open()
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
