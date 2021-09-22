using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{

    public enum TipSmestaja { HOTEL, MOTEL, VILA }
    public class Smestaj
    {
        private int id;
        private TipSmestaja tipSmestaja;
        private string naziv;
        private int brojZvezdica;
        private bool bazen;
        private bool spaCentar;
        private bool zaOsobeInv;
        private bool wiFi;
        private List<SmestajnaJedinica> smestajnaJedinice = new List<SmestajnaJedinica>();

        public Smestaj()
        {
            Id = 0;
            TipSmestaja = TipSmestaja.MOTEL;
            Naziv = "";
            BrojZvezdica = 0;
            Bazen = false;
            SpaCentar = false;
            ZaOsobeInv = false;
            WiFi = false;
            Obrisan = false;
            smestajnaJedinice = new List<SmestajnaJedinica>();
        }

        public Smestaj(int id,TipSmestaja tipSmestaja, string naziv, int brojZvezdica, bool bazen, bool spaCentar, bool zaOsobeInv, bool wiFi,bool obrisan)
        {
            TipSmestaja = tipSmestaja;
            Naziv = naziv;
            BrojZvezdica = brojZvezdica;
            Bazen = bazen;
            SpaCentar = spaCentar;
            ZaOsobeInv = zaOsobeInv;
            WiFi = wiFi;
            //SmestajnaJedinice = smestajnaJedinice;
            Id = id;
            Obrisan = obrisan;
        }

        public TipSmestaja TipSmestaja { get => tipSmestaja; set => tipSmestaja = value; }
        public string Naziv { get => naziv; set => naziv = value; }
        public int BrojZvezdica { get => brojZvezdica; set => brojZvezdica = value; }
        public bool Bazen { get => bazen; set => bazen = value; }
        public bool SpaCentar { get => spaCentar; set => spaCentar = value; }
        public bool ZaOsobeInv { get => zaOsobeInv; set => zaOsobeInv = value; }
        public bool WiFi { get => wiFi; set => wiFi = value; }
        public List<SmestajnaJedinica> SmestajnaJedinice { get => smestajnaJedinice; set => smestajnaJedinice = value; }
        public int Id { get => id; set => id = value; }

        public bool Obrisan { get; set; }
    }
}