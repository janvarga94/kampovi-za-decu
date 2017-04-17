using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampoviZaDecu
{
    public class FileThings
    {
        public static IEnumerable<Dete> Parse(string filePath) {
            foreach (var line in File.ReadAllLines(filePath).Skip(1)) {
                var splited = line.Split(';');
                if(splited.Length >= App.DecaHeader.Length)
                {
                    yield return new Dete() {
                        Ime = splited[0],
                        Prezime = splited[1],
                        Razred = splited[2],
                        Adresa = splited[3],
                        TelRod = splited[4],
                        Engleski = splited[5] == true.ToString(),
                        Sport = splited[6] == true.ToString(),
                        KojiSport = splited[7],
                        Plivanje = splited[8],
                        IznosKampa = splited[9]
                    };
                }
            }
        }

        public static bool Save(string filePath, IEnumerable<Dete> deca) {
            var csvHeader = string.Join(";", App.DecaHeader);
            var csvContentBody = string.Join("\n", deca.Select(GetCommaSeparatedDete));
            var csv = csvHeader + "\n" + csvContentBody;
            try
            {
                File.WriteAllText(filePath, csv);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetCommaSeparatedDete(Dete dete) {

            var result = "";
            for (int i = 0; i < typeof(Dete).GetProperties().Length; i++)
            {
                var currentDeteProperty = typeof(Dete).GetProperties().ElementAt(i);
                var value = currentDeteProperty.GetValue(dete);
                result += value?.ToString() + ";";
            }
            return result;
        }
    }
}
