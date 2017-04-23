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
                yield return new Dete() { Ime = RandomString(8), Prezime = "dfsgdf", IznosKampa = 324532, Engleski = random.Next() % 2 == 0 };
            }
        }
        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
