using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampoviZaDecu
{
    class DecaGenerator
    {
        public static IEnumerable<Dete> Generate(int num) {
            for (int i = 0; i < num; i++) {
                yield return new Dete() { Ime = "asdfasdf", Prezime = "dfsgdf", IznosKampa = "324532" };
            }
        }
    }
}
