﻿using System;
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

           // return DecaGenerator.Generate(40);

            var deca = new List<Dete>();
            foreach (var line in File.ReadAllLines(filePath).Skip(1)) {
                var splited = line.Split(App.CsvDelemiter);
                if(splited.Length >= typeof(Dete).GetProperties().Length)
                {
                    splited = splited.Select(sp => sp.Replace("\"","")).ToArray();
                    deca.Add( new Dete() {
                        Ime = splited[0],
                        Prezime = splited[1],
                        Razred = splited[2],
                        Adresa = splited[3],
                        TelRod = splited[4],
                        Engleski = splited[5] == true.ToString(),
                        Sport = splited[6] == true.ToString(),
                        KojiSport = splited[7],
                        Plivanje = splited[8],
                        IznosKampa = double.Parse(splited[9])
                    });
                }
            }
            return deca;
        }

        public static bool Save(string filePath, IEnumerable<Dete> deca) {
            var csvHeader = string.Join(App.CsvDelemiter.ToString(), App.DecaHeader);
            var csvContentBody = string.Join("\n", deca.Select(GetCommaSeparatedDete));
            var csv = csvHeader + "\n" + csvContentBody;
            try
            {
                File.WriteAllText(filePath, csv);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        private static string GetCommaSeparatedDete(Dete dete) {

            var result = "";
            for (int i = 0; i < typeof(Dete).GetProperties().Length; i++)
            {
                var currentDeteProperty = typeof(Dete).GetProperties().ElementAt(i);
                var value = "\"" + currentDeteProperty.GetValue(dete) + "\"";
                result += value?.ToString() + App.CsvDelemiter.ToString();
            }
            return result;
        }
    }
}
