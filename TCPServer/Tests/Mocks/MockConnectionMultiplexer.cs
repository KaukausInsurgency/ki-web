using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Tests.Mocks
{
    class MockConnectionMultiplexer : IConnectionMultiplexer
    {
        private bool _isConnected = true;

        string IConnectionMultiplexer.ClientName => "MockRedis";

        string IConnectionMultiplexer.Configuration => "MockRedisConfiguration";

        int IConnectionMultiplexer.TimeoutMilliseconds => 60000;

        long IConnectionMultiplexer.OperationCount => 1;

        bool IConnectionMultiplexer.IsConnected => _isConnected;


        IDatabase IConnectionMultiplexer.GetDatabase(int db, object asyncState)
        {
            return new MockRedisDatabase();
        }

        void IConnectionMultiplexer.Close(bool allowCommandsToComplete)
        {
            _isConnected = false;
        }

        IServer IConnectionMultiplexer.GetServer(string host, int port, object asyncState)
        {
            throw new NotImplementedException();
        }

        IServer IConnectionMultiplexer.GetServer(string hostAndPort, object asyncState)
        {
            throw new NotImplementedException();
        }

        IServer IConnectionMultiplexer.GetServer(IPAddress host, int port)
        {
            throw new NotImplementedException();
        }

        IServer IConnectionMultiplexer.GetServer(EndPoint endpoint, object asyncState)
        {
            throw new NotImplementedException();
        }






        #region UnimplementedMockMethods
        bool IConnectionMultiplexer.IncludeDetailInExceptions { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        int IConnectionMultiplexer.StormLogThreshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        bool IConnectionMultiplexer.PreserveAsyncOrder { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }


        event EventHandler<RedisErrorEventArgs> IConnectionMultiplexer.ErrorMessage
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<ConnectionFailedEventArgs> IConnectionMultiplexer.ConnectionFailed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<InternalErrorEventArgs> IConnectionMultiplexer.InternalError
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<ConnectionFailedEventArgs> IConnectionMultiplexer.ConnectionRestored
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<EndPointEventArgs> IConnectionMultiplexer.ConfigurationChanged
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<EndPointEventArgs> IConnectionMultiplexer.ConfigurationChangedBroadcast
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event EventHandler<HashSlotMovedEventArgs> IConnectionMultiplexer.HashSlotMoved
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        void IConnectionMultiplexer.BeginProfiling(object forContext)
        {
            throw new NotImplementedException();
        }

        Task IConnectionMultiplexer.CloseAsync(bool allowCommandsToComplete)
        {
            throw new NotImplementedException();
        }

        bool IConnectionMultiplexer.Configure(TextWriter log)
        {
            throw new NotImplementedException();
        }

        Task<bool> IConnectionMultiplexer.ConfigureAsync(TextWriter log)
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.Dispose()
        {

        }

        ProfiledCommandEnumerable IConnectionMultiplexer.FinishProfiling(object forContext, bool allowCleanupSweep)
        {
            throw new NotImplementedException();
        }

        ServerCounters IConnectionMultiplexer.GetCounters()
        {
            throw new NotImplementedException();
        }

        EndPoint[] IConnectionMultiplexer.GetEndPoints(bool configuredOnly)
        {
            throw new NotImplementedException();
        }

        string IConnectionMultiplexer.GetStatus()
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.GetStatus(TextWriter log)
        {
            throw new NotImplementedException();
        }

        string IConnectionMultiplexer.GetStormLog()
        {
            throw new NotImplementedException();
        }

        ISubscriber IConnectionMultiplexer.GetSubscriber(object asyncState)
        {
            throw new NotImplementedException();
        }

        int IConnectionMultiplexer.HashSlot(RedisKey key)
        {
            throw new NotImplementedException();
        }

        long IConnectionMultiplexer.PublishReconfigure(CommandFlags flags)
        {
            throw new NotImplementedException();
        }

        Task<long> IConnectionMultiplexer.PublishReconfigureAsync(CommandFlags flags)
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.RegisterProfiler(IProfiler profiler)
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.ResetStormLog()
        {
            throw new NotImplementedException();
        }

        string IConnectionMultiplexer.ToString()
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.Wait(Task task)
        {
            throw new NotImplementedException();
        }

        T IConnectionMultiplexer.Wait<T>(Task<T> task)
        {
            throw new NotImplementedException();
        }

        void IConnectionMultiplexer.WaitAll(params Task[] tasks)
        {
            throw new NotImplementedException();
        }

#endregion
    }
}
