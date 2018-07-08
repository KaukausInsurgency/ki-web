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
    class DateTimeJavaScriptTests
    {
        [Test]
        public void ConvertDateTimeToJSMilliseconds_Success()
        {
            // Date.UTC(2018, 4, 18) = 1526601600000 
            // In Date.UTC, month is 0 - 11
            Assert.That(DateTimeJavaScript.ToJavaScriptMilliseconds(new DateTime(2018, 5, 18)) == 1526616000000);
        }
    }
}
