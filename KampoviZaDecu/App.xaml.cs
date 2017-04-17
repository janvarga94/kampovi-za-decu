using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace KampoviZaDecu
{
    public partial class App : Application
    {
        public static readonly string[] DecaHeader = new string[] {
            "Ime",
            "Prezime",
            "Razred",
            "Adresa",
            "Tel. roditelja",
            "Engleski",
            "Sport",
            "Koji sport",
            "Plivanje",
            "Iznos kampa"
        };
    }
}
