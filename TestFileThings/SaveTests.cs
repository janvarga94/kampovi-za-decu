using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using KampoviZaDecu;
using System.Collections.Generic;

namespace TestFileThings
{
    [TestClass]
    public class SaveTests
    {
        [TestMethod]
        [Ignore]
        public void Save()
        {
            var deca = new List<Dete>()
            {
                new Dete{ Ime = "fsdfsdsdf", IznosKampa = 21312, Sport = true},
                new Dete{ Ime = "hgffsdfsdsdf", IznosKampa = 21312, Sport = false},
            };

            FileThings.Save("C:\\Users\\janva\\Desktop\\hhhhhhhhhhh.csv", deca);

            Assert.IsTrue(true);
        }
    }
}
