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
            Dictionary<string, long> config = new Dictionary<string, long>
            {
                { "First", 1 }
            };

            ThrottleMapper throttler = new ThrottleMapper(new Mocks.MockTimer(), config);
            Assert.That(throttler.ShouldThrottle(action) == expected);
        }

        [Test]
        public void ShouldThrottle_SecondTime_YesThrottle()
        {
            Dictionary<string, long> config = new Dictionary<string, long>
            {
                { "First", 1 }
            };
            ThrottleMapper throttler = new ThrottleMapper(new Mocks.MockTimer(), config);
            Assert.That(!throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
            Assert.That(throttler.ShouldThrottle("First"));
        }

        [Test]
        public void ShouldThrottle_NotInConfig_NoThrottle()
        {
            Dictionary<string, long> config = new Dictionary<string, long>
            {
                { "MockAction", 1 }
            };
            ThrottleMapper throttler = new ThrottleMapper(new Mocks.MockTimer(), config);
            Assert.That(!throttler.ShouldThrottle("First"));
            Assert.That(!throttler.ShouldThrottle("First"));
            Assert.That(!throttler.ShouldThrottle("First"));
        }

        [Test]
        public void ShouldThrottle_FifthTime_NoThrottle()
        {
            Dictionary<string, long> config = new Dictionary<string, long>
            {
                { "First", 4 }
            };
            Mocks.MockTimer mockTimer = new Mocks.MockTimer();
            ThrottleMapper throttler = new ThrottleMapper(mockTimer, config);
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
