using Microsoft.VisualStudio.TestTools.UnitTesting;
using KampoviZaDecu.helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampoviZaDecu.helpers.Tests
{
    [TestClass()]
    public class TypeChecksTests
    {
        [TestMethod()]
        public void IsNumberTest()
        {
            Assert.IsTrue(TypeChecks.IsNumber(typeof(double)));
            Assert.IsTrue(TypeChecks.IsNumber(typeof(Double)));
        }
    }
}