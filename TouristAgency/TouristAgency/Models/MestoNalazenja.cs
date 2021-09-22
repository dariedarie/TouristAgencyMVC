using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{
    public class MestoNalazenja
    {
        public MestoNalazenja()
        {
            Adresa = "";
            GeografskaDuzina = "";
            GeografskaSirina = "";
        }

        public MestoNalazenja(string adresa, string geografskaDuzina, string geografskaSirina)
        {
            Adresa = adresa;
            GeografskaDuzina = geografskaDuzina;
            GeografskaSirina = geografskaSirina;
        }

        public string Adresa { get; set; }  

        public string GeografskaDuzina { get; set; }
        public string GeografskaSirina { get; set; }
    }
}