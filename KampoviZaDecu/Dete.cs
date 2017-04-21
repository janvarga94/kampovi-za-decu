using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace KampoviZaDecu
{
    public class Dete
    {
        [Required]
        public string Ime { get; set; }

        [Required]
        public string Prezime { get; set; }

        public string Razred { get; set; }
        public string Adresa { get; set; }
        public string TelRod { get; set; }
        public bool Engleski { get; set; }
        public bool Sport { get; set; }

        [EnabledFor(nameof(Sport))]
        public string KojiSport { get; set; }
        public string Plivanje { get; set; }
        public string IznosKampa { get; set; }
    }
}
