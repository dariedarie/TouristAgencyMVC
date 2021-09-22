using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{
    public enum TipAranzmana { NOCENJESADORUCKOM, POLUPANSION, PUNPANSION,ALLINCLUSIVE,NAJAMAPART }
    public enum TipPrevoza { AUTOBUS, AVION, BUSAVION, INDIVUALAN, OSTALO }

    public class Aranzman
    {
        private int id;
        private string naziv;
        private TipAranzmana tipAranzmana;
        private TipPrevoza tipPrevoza;
        private string lokacija;
        private DateTime datumPocetkaPutovanja;
        private DateTime datumZavrsetkaPutovanja;
        private MestoNalazenja mestoNalazenja;
        private DateTime vremeNalazenja;
        private int maxBrojPutnika;
        private string opis;
        private string program;
        private UploadedFile poster;
        private int idSmestaja;
       

        public Aranzman()
        {
            Id = 0;
            Naziv = "";
            TipAranzmana = TipAranzmana.ALLINCLUSIVE;
            TipPrevoza = TipPrevoza.AVION;
            Lokacija = "";
            DatumPocetkaPutovanja = DateTime.Now;
            DatumZavrsetkaPutovanja = DateTime.Now;
            MestoNalazenja = new MestoNalazenja();
            VremeNalazenja = DateTime.Now;
            MaxBrojPutnika = 0;
            Opis = "";
            Program = "";
            Poster = new UploadedFile();
            Obrisan = false;
            IdSmestaja = 0;
            Menadzer = new Korisnik();
        }

        public Aranzman(int id, string naziv, TipAranzmana tipAranzmana, TipPrevoza tipPrevoza, string lokacija, DateTime datumPocetkaPutovanja, DateTime datumZavrsetkaPutovanja, MestoNalazenja mestoNalazenja, DateTime vremeNalazenja, int maxBrojPutnika, string opis, string program, UploadedFile poster, int idSmestaja, bool obrisan,Korisnik menadzer)
        {
            Id = id;
            Naziv = naziv;
            TipAranzmana = tipAranzmana;
            TipPrevoza = tipPrevoza;
            Lokacija = lokacija;
            DatumPocetkaPutovanja = datumPocetkaPutovanja;
            DatumZavrsetkaPutovanja = datumZavrsetkaPutovanja;
            MestoNalazenja = mestoNalazenja;
            VremeNalazenja = vremeNalazenja;
            MaxBrojPutnika = maxBrojPutnika;
            Opis = opis;
            Program = program;
            Poster = poster;
            IdSmestaja = idSmestaja;
            Obrisan = obrisan;
            Menadzer = menadzer;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public TipAranzmana TipAranzmana { get => tipAranzmana; set => tipAranzmana = value; }
        public TipPrevoza TipPrevoza { get => tipPrevoza; set => tipPrevoza = value; }
        public DateTime DatumPocetkaPutovanja { get => datumPocetkaPutovanja; set => datumPocetkaPutovanja = value; }
        public DateTime DatumZavrsetkaPutovanja { get => datumZavrsetkaPutovanja; set => datumZavrsetkaPutovanja = value; }
        
        public DateTime VremeNalazenja { get => vremeNalazenja; set => vremeNalazenja = value; }
        public int MaxBrojPutnika { get => maxBrojPutnika; set => maxBrojPutnika = value; }
        public string Opis { get => opis; set => opis = value; }
        public string Program { get => program; set => program = value; }
        public UploadedFile Poster { get => poster; set => poster = value; }
        
        public int Id { get => id; set => id = value; }
        public bool Obrisan { get; set; } 
        public MestoNalazenja MestoNalazenja { get => mestoNalazenja; set => mestoNalazenja = value; }
        public string Lokacija { get => lokacija; set => lokacija = value; }

        public Korisnik Menadzer { get; set; }
        public int IdSmestaja { get => idSmestaja; set => idSmestaja = value; }
    }
}