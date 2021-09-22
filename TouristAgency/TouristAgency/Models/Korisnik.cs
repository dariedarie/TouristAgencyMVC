using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{

    public enum PolType { M, F }
    public enum UserType { ADMINISTRATOR, MENADZER, TURISTA }
    public class Korisnik
    {
        private List<Aranzman> aranzmani = new List<Aranzman>();
        private List<Rezervacija> rezervacije = new List<Rezervacija>();
        public string KorisnickoIme { get; set; }   
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Lozinka { get; set; }
        public PolType Pol { get; set; }
        public string Email { get; set; }
        public DateTime DatumRodjenja { get; set; }
        public UserType Uloga { get; set; }
        public List<Aranzman> Aranzmani { get => aranzmani; set => aranzmani = value; }
        public List<Rezervacija> Rezervacije { get => rezervacije; set => rezervacije = value; }

        public bool Blokiran { get; set; }

        public int OtkazRez { get; set; }

        public Korisnik()
        {
            KorisnickoIme = "";
            Ime = "";
            Prezime = "";
            Lozinka = "";
            Pol = PolType.M;
            Email = "";
            OtkazRez = 0;
            Uloga = UserType.TURISTA;
            Aranzmani = new List<Aranzman>();
            Rezervacije = new List<Rezervacija>();
            Blokiran = false;



        }

        public Korisnik(string korisnickoIme, string ime, string prezime, string lozinka, PolType pol, string email, DateTime datumRodjenja, UserType uloga, List<Aranzman> aranzmani, List<Rezervacija> rezervacije,bool blokiran,int otkazRez)
        {
            KorisnickoIme = korisnickoIme;
            Ime = ime;
            Prezime = prezime;
            Lozinka = lozinka;
            Pol = pol;
            Email = email;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            Aranzmani = aranzmani;
            Rezervacije = rezervacije;
            Blokiran = blokiran;
            OtkazRez = otkazRez;
        }
    }
}