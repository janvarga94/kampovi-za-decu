using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KampoviZaDecu
{
    class CsvToObservableCollectionDece
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
                        Engleski = splited[5],
                        Sport = splited[6],
                        Plivanje = splited[7],
                        IznosKampa = splited[8]
                    };
                }
            }
        }

        public static bool Save(string filePath, IEnumerable<Dete> deca) {
            var csvHeader = string.Join(";",App.DecaHeader);
            var csvContentBody = string.Join("\n", deca.Select(dete => dete.Ime + ";" + dete.Prezime + ";" + dete.Razred + ";" + dete.Adresa + ";"
                                               + dete.TelRod + ";" + dete.Engleski + ";" + dete.Sport + ";" + dete.Plivanje + ";" +
                                                 dete.IznosKampa));
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
    }
}
