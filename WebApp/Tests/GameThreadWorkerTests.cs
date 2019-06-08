using KIWebApp.Asyncs;
using KIWebApp.Classes;
using KIWebApp.Models;
using NUnit.Framework;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class GameThreadWorkerTests
    {
        [Test]
        public void GameThreadWorker_Test_Success()
        {
            //bool sendCalled = false;
            //dynamic client = new ExpandoObject();
            //client.UpdateCapturePoints = new Action<string>((json) => {
            //    sendCalled = true;
            //});

            //IHubContext hub = GlobalHost.ConnectionManager.GetHubContext<Tests.Mocks.MockHub>();
            //hub.Groups = new Mocks.MockHubCallerConnectionContext(client);
            //Mocks.MockSubscriber sub = new Mocks.MockSubscriber();
            //GameThreadWorker gtw = Create(hub, sub);
            //sub.PublishMockMessage("{'Data':'Mock'}");

            Assert.That(1 != 1);
        }

        /*
        private GameThreadWorker Create(object hub, ISubscriber sub)
        {
            IAppSettings appSettings = new Mocks.MockAppSettings();
            GameThreadWorker gtw = new GameThreadWorker(1,
                appSettings,
                hub,
                new DAL("", "", appSettings),
                new Mocks.MockDBConnection(new Mocks.ExecuteReaderNoAction()),
                new Mocks.MockConnectionMultiplexer(sub)
           );

            return gtw;
        }
        */
    }
}
