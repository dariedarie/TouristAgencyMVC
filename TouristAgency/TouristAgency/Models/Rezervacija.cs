using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{

    public enum StatusRezervacije { AKTIVNA, OTKAZANA }
    public class Rezervacija
    {
        private string id;
        private Korisnik turista;
        private StatusRezervacije status;
        private int idAranzmana;
        private Aranzman aranzman;
        private SmestajnaJedinica izabranaSmestajnaJedinica;

        public Rezervacija()
        {
            Id = "";
            Turista = new Korisnik();
            Status = StatusRezervacije.AKTIVNA;
            IdAranzmana = 0;
            Aranzman = new Aranzman();
            IzabranaSmestajnaJedinica = new SmestajnaJedinica();
        }

        public Rezervacija(Aranzman aranzman,string id, Korisnik turista, StatusRezervacije status)
        {
            Id = id;
            Turista = turista;
            Status = status;
            Aranzman = aranzman;
        }

        public string Id { get => id; set => id = value; }


        public Korisnik Turista { get => turista; set => turista = value; }
        public StatusRezervacije Status { get => status; set => status = value; }
        public SmestajnaJedinica IzabranaSmestajnaJedinica { get => izabranaSmestajnaJedinica; set => izabranaSmestajnaJedinica = value; }
        public int IdAranzmana { get => idAranzmana; set => idAranzmana = value; }
        public Aranzman Aranzman { get => aranzman; set => aranzman = value; }
    }
}