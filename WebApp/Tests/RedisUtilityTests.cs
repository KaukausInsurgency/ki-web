using KIWebApp.Classes;
using KIWebApp.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    class RedisUtilityTests
    {
        [Test]
        public void BuildRedisKey_Integer_Success()
        {
            Assert.That(RedisUtility.BuildRedisKey<int>("UT", "Key", 1) == "UT:1:Key");
        }

        [Test]
        public void BuildRedisKey_Long_Success()
        {
            Assert.That(RedisUtility.BuildRedisKey<long>("UT", "Key", 1) == "UT:1:Key");
        }

        [Test]
        public void BuildRedisKey_Double_Success()
        {
            Assert.That(RedisUtility.BuildRedisKey<double>("UT", "Key", 1.1) == "UT:1.1:Key");
        }
    }
}
