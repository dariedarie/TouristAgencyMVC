using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{
    public class SmestajnaJedinica
    {
        private int id;
        private int dozvoljenoGostiju;
        private bool kucniLjubimci;
        private int cena;
        private int smestajId;

        public SmestajnaJedinica()
        {
            DozvoljenoGostiju = 0;
            KucniLjubimci = false;
            Cena = 0;
            Id = 0;
            Obrisana = false;
        }

        public SmestajnaJedinica(int id,int dozvoljenoGostiju, bool kucniLjubimci, int cena,bool obrisana)
        {
            DozvoljenoGostiju = dozvoljenoGostiju;
            KucniLjubimci = kucniLjubimci;
            Cena = cena;
            Id = id;
            Obrisana = obrisana;
        }

        public int DozvoljenoGostiju { get => dozvoljenoGostiju; set => dozvoljenoGostiju = value; }
        public bool KucniLjubimci { get => kucniLjubimci; set => kucniLjubimci = value; }
        public int Cena { get => cena; set => cena = value; }
        public int Id { get => id; set => id = value; }

        public bool Obrisana { get; set; }
    }
}