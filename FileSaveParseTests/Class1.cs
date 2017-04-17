using KampoviZaDecu;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FileSaveParseTests
{
    public class Class1
    {
        [Fact]
        public void Save() {

            var deca = new List<Dete>()
            {
                new Dete{ Ime = "fsdfsdsdf", IznosKampa = "21312", Sport = true},
                new Dete{ Ime = "hgffsdfsdsdf", IznosKampa = "21312", Sport = false},
            };

            FileThings.Save("Desktop\\hhhhhhhhhhh.csv", deca);

            Assert.True(true);
        }
    }
}
