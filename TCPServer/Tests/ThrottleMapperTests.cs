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
using Newtonsoft.Json.Linq;

namespace Tests
{
    [TestFixture]
    class ThrottleMapperTests
    {
        [TestCase("First", false)]
        [TestCase("Second", false)]
        [TestCase("Third", false)]
        public void ShouldThrottle_FirstTime_NoThrottle(string action, bool expected)
        {
            ThrottleMapper throttler = new ThrottleMapper(new Mocks.MockTimer(), 1);
            Assert.That(throttler.ShouldThrottle(action) == expected);
        }

        [Test]
        public void ShouldThrottle_SecondTime_YesThrottle()
        {
            ThrottleMapper throttler = new ThrottleMapper(new Mocks.MockTimer(), 1);
            Assert.That(!throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
        }

        [Test]
        public void ShouldThrottle_FifthTime_NoThrottle()
        {
            Mocks.MockTimer mockTimer = new Mocks.MockTimer();
            ThrottleMapper throttler = new ThrottleMapper(mockTimer, 4);
            Assert.That(!throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            mockTimer.SetCounter(16);
            Assert.That(!throttler.ShouldThrottle("First"));
        }

    }

}
