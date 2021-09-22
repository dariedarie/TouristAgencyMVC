using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{

    public enum StatusKomentara { ODOBREN, ODBIJEN, CEKA }
    public class Komentar
    {
        private int id;
        private Korisnik turista;
        private Aranzman aranzman;
        private string tekstKomentara;
        private int ocena;
        private StatusKomentara statusKomentara;


        public Komentar()
        {
            Id = 0;
            TekstKomentara = "";
            Ocena = 0;

        }

        public Komentar(Aranzman aranzman,int id, Korisnik turista ,StatusKomentara statusKomentara,string tekstKomentara, int ocena)
        {
            Id = id;
            Turista = turista;
            Aranzman = aranzman;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            StatusKomentara = statusKomentara;
        }

        public int Id { get => id; set => id = value; }
        public Korisnik Turista { get => turista; set => turista = value; }
        public Aranzman Aranzman { get => aranzman; set => aranzman = value; }
        public string TekstKomentara { get => tekstKomentara; set => tekstKomentara = value; }
        public int Ocena { get => ocena; set => ocena = value; }
        public StatusKomentara StatusKomentara { get => statusKomentara; set => statusKomentara = value; }
    }
}