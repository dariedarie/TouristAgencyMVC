using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace TouristAgency.Models
{
    public class Baza
    {
        public static List<Aranzman> ProcitajAranzmane(string putanja)
        {
            List<Aranzman> aranzmani = new List<Aranzman>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);  
            StreamReader sr = new StreamReader(stream);
           
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7],token[8],token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]),bool.Parse(token[16]),menadzer);
                aranzmani.Add(a);

            }
            sr.Close();
            stream.Close();

            return aranzmani;

        }

        public static int ProcitajAranzmaneZadnjiId()
        {
            string putanja = "~/App_Data/aranzmani.txt";
            List<Aranzman> aranzmani = new List<Aranzman>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            int id;

            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7], token[8], token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]), bool.Parse(token[16]), menadzer);
                aranzmani.Add(a);

            }
            if (aranzmani.Count == 0)
            {
                id = -1;
            }
            else
            {
                id = aranzmani[aranzmani.Count - 1].Id;
            }
            sr.Close();
            stream.Close();

            return id;

        }


        public static void UpisiAranzman(Aranzman a)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/aranzmani.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string datum = a.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string datum2 = a.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string vreme = a.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

            sw.WriteLine(a.Id + "|" + a.Naziv + "|" + a.TipAranzmana.ToString() + "|" + a.TipPrevoza.ToString() + "|" + a.Lokacija + "|" + datum + "|" + datum2 + "|" + a.MestoNalazenja.Adresa 
                + "|" + a.MestoNalazenja.GeografskaDuzina + "|" + a.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + a.MaxBrojPutnika + "|" + a.Opis 
                + "|" + a.Program + "|" + a.Poster.Name + "|" + a.IdSmestaja + "|" + a.Obrisan.ToString() + "|" + a.Menadzer.KorisnickoIme );


            sw.Close();
            stream.Close();

        }

        public static void UpdateAranzman(List<Aranzman> aran)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/aranzmani.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Aranzman a in aran)
            {
                string datum = a.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string datum2 = a.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string vreme = a.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

                sw.WriteLine(a.Id + "|" + a.Naziv + "|" + a.TipAranzmana.ToString() + "|" + a.TipPrevoza.ToString() + "|" + a.Lokacija + "|" + datum + "|" + datum2 + "|" + a.MestoNalazenja.Adresa
                    + "|" + a.MestoNalazenja.GeografskaDuzina + "|" + a.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + a.MaxBrojPutnika + "|" + a.Opis
                    + "|" + a.Program + "|" + a.Poster.Name + "|" + a.IdSmestaja + "|" + a.Obrisan.ToString() + "|" + a.Menadzer.KorisnickoIme);


            }

            sw.Close();
            stream.Close();

        }


        public static List<Komentar> ProcitajKomentar(string putanja)
        {
            List<Komentar> komentars = new List<Komentar>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7], token[8], token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]), bool.Parse(token[16]), menadzer);


                Korisnik turista = new Korisnik();
                turista.KorisnickoIme = token[19];

                Komentar k = new Komentar(a, int.Parse(token[18]), turista, (StatusKomentara)Enum.Parse(typeof(StatusKomentara), token[20]),token[21],int.Parse(token[22]));
                komentars.Add(k);

            }
            sr.Close();
            stream.Close();

            return komentars;

        }


        public static int ProcitajKomentarZadnjiId()
        {
            string putanja = "~/App_Data/komentari.txt";
            List<Komentar> komentars = new List<Komentar>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            int id;
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7], token[8], token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]), bool.Parse(token[16]), menadzer);


                Korisnik turista = new Korisnik();
                turista.KorisnickoIme = token[19];

                Komentar k = new Komentar(a, int.Parse(token[18]), turista, (StatusKomentara)Enum.Parse(typeof(StatusKomentara), token[20]), token[21], int.Parse(token[22]));
                komentars.Add(k);

            }
            if (komentars.Count == 0)
            {
                id = -1;
            }
            else
            {
                id = komentars[komentars.Count - 1].Id;
            }

            sr.Close();
            stream.Close();

            return id;

        }

        public static List<Rezervacija> ProcitajRezervaciju(string putanja)
        {
            List<Rezervacija> rezervacijas = new List<Rezervacija>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7], token[8], token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]), bool.Parse(token[16]), menadzer);


                Korisnik turista = new Korisnik();
                turista.KorisnickoIme = token[19];

                Rezervacija r = new Rezervacija(a,token[18],turista, (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), token[20]));
                rezervacijas.Add(r);

            }

            sr.Close();
            stream.Close();

            return rezervacijas;

        }


        public static string  ProcitajRezervacijuIdZadnji()
        {
            string putanja = "~/App_Data/rezervacije.txt";
            List<Rezervacija> rezervacijas = new List<Rezervacija>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string id;
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');
                DateTime datum = DateTime.ParseExact(token[5], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime datum2 = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));
                DateTime vreme = DateTime.ParseExact(token[10], "HH:mm", new CultureInfo("en-US"));
                UploadedFile slika = new UploadedFile();
                slika.Name = token[14];
                MestoNalazenja mesto = new MestoNalazenja(token[7], token[8], token[9]);
                Korisnik menadzer = new Korisnik();
                menadzer.KorisnickoIme = token[17];



                Aranzman a = new Aranzman(int.Parse(token[0]), token[1], (TipAranzmana)Enum.Parse(typeof(TipAranzmana), token[2]), (TipPrevoza)Enum.Parse(typeof(TipPrevoza), token[3]),
                   token[4], datum, datum2, mesto, vreme, int.Parse(token[11]), token[12], token[13], slika, int.Parse(token[15]), bool.Parse(token[16]), menadzer);


                Korisnik turista = new Korisnik();
                turista.KorisnickoIme = token[19];

                Rezervacija r = new Rezervacija(a, token[18], turista, (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), token[20]));
                rezervacijas.Add(r);

            }

            if (rezervacijas.Count == 0)
            {
                id = "";
            }
            else
            {
                id = rezervacijas[rezervacijas.Count - 1].Id;
            }
            sr.Close();
            stream.Close();

            return id;

        }


        public static void UpisiKomentar(Komentar r)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);
            string datum = r.Aranzman.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string datum2 = r.Aranzman.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string vreme = r.Aranzman.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

            string aranzman = r.Aranzman.Id + "|" + r.Aranzman.Naziv + "|" + r.Aranzman.TipAranzmana.ToString() + "|" + r.Aranzman.TipPrevoza.ToString() + "|" + r.Aranzman.Lokacija + "|" + datum + "|" + datum2 + "|" + r.Aranzman.MestoNalazenja.Adresa
                + "|" + r.Aranzman.MestoNalazenja.GeografskaDuzina + "|" + r.Aranzman.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + r.Aranzman.MaxBrojPutnika + "|" + r.Aranzman.Opis
                + "|" + r.Aranzman.Program + "|" + r.Aranzman.Poster.Name + "|" + r.Aranzman.IdSmestaja + "|" + r.Aranzman.Obrisan.ToString() + "|" + r.Aranzman.Menadzer.KorisnickoIme;

            sw.WriteLine(aranzman + "|" + r.Id + "|" + r.Turista.KorisnickoIme + "|" + r.StatusKomentara.ToString() + "|" + r.TekstKomentara + "|" + r.Ocena);


            sw.Close();
            stream.Close();

        }

        public static void UpdateKoment(List<Komentar> koment)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Komentar r in koment)
            {
                
                string datum = r.Aranzman.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string datum2 = r.Aranzman.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string vreme = r.Aranzman.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

                string aranzman = r.Aranzman.Id + "|" + r.Aranzman.Naziv + "|" + r.Aranzman.TipAranzmana.ToString() + "|" + r.Aranzman.TipPrevoza.ToString() + "|" + r.Aranzman.Lokacija + "|" + datum + "|" + datum2 + "|" + r.Aranzman.MestoNalazenja.Adresa
                    + "|" + r.Aranzman.MestoNalazenja.GeografskaDuzina + "|" + r.Aranzman.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + r.Aranzman.MaxBrojPutnika + "|" + r.Aranzman.Opis
                    + "|" + r.Aranzman.Program + "|" + r.Aranzman.Poster.Name + "|" + r.Aranzman.IdSmestaja + "|" + r.Aranzman.Obrisan.ToString() + "|" + r.Aranzman.Menadzer.KorisnickoIme;

                sw.WriteLine(aranzman + "|" + r.Id + "|" + r.Turista.KorisnickoIme + "|" + r.StatusKomentara.ToString() + "|" + r.TekstKomentara + "|" + r.Ocena);

            }

            sw.Close();
            stream.Close();

        }



        public static void UpisiRezervaciju(Rezervacija r)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/rezervacije.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);
            string datum = r.Aranzman.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string datum2 = r.Aranzman.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            string vreme = r.Aranzman.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

            string aranzman= r.Aranzman.Id + "|" + r.Aranzman.Naziv + "|" + r.Aranzman.TipAranzmana.ToString() + "|" + r.Aranzman.TipPrevoza.ToString() + "|" + r.Aranzman.Lokacija + "|" + datum + "|" + datum2 + "|" + r.Aranzman.MestoNalazenja.Adresa
                + "|" + r.Aranzman.MestoNalazenja.GeografskaDuzina + "|" + r.Aranzman.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + r.Aranzman.MaxBrojPutnika + "|" + r.Aranzman.Opis
                + "|" + r.Aranzman.Program + "|" + r.Aranzman.Poster.Name + "|" + r.Aranzman.IdSmestaja + "|" + r.Aranzman.Obrisan.ToString() + "|" + r.Aranzman.Menadzer.KorisnickoIme;

            sw.WriteLine(aranzman +"|" + r.Id + "|" + r.Turista.KorisnickoIme + "|" + r.Status.ToString());


            sw.Close();
            stream.Close();

        }



        public static void UpdateRezervaciju(List<Rezervacija> rez)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/rezervacije.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Rezervacija r in rez)
            {
                string datum = r.Aranzman.DatumPocetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string datum2 = r.Aranzman.DatumZavrsetkaPutovanja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                string vreme = r.Aranzman.VremeNalazenja.ToString("HH:mm", DateTimeFormatInfo.InvariantInfo);

                string aranzman = r.Aranzman.Id + "|" + r.Aranzman.Naziv + "|" + r.Aranzman.TipAranzmana.ToString() + "|" + r.Aranzman.TipPrevoza.ToString() + "|" + r.Aranzman.Lokacija + "|" + datum + "|" + datum2 + "|" + r.Aranzman.MestoNalazenja.Adresa
                    + "|" + r.Aranzman.MestoNalazenja.GeografskaDuzina + "|" + r.Aranzman.MestoNalazenja.GeografskaSirina + "|" + vreme + "|" + r.Aranzman.MaxBrojPutnika + "|" + r.Aranzman.Opis
                    + "|" + r.Aranzman.Program + "|" + r.Aranzman.Poster.Name + "|" + r.Aranzman.IdSmestaja + "|" + r.Aranzman.Obrisan.ToString() + "|" + r.Aranzman.Menadzer.KorisnickoIme;

                sw.WriteLine(aranzman + "|" + r.Id + "|" + r.Turista.KorisnickoIme + "|" + r.Status.ToString());
            }

            sw.Close();
            stream.Close();

        }

        public static List<Smestaj> ProcitajSmestaj(string putanja)
        {
            List<Smestaj> smestajs = new List<Smestaj>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');

                Smestaj s = new Smestaj(int.Parse(token[0]), (TipSmestaja)Enum.Parse(typeof(TipSmestaja), token[1]),token[2],int.Parse(token[3]),bool.Parse(token[4]),bool.Parse(token[5]), bool.Parse(token[6]), bool.Parse(token[7]),bool.Parse(token[8]));
                smestajs.Add(s);

            }
            sr.Close();
            stream.Close();

            return smestajs;

        }

        public static void UpisiSmestaj(Smestaj s)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/smestaji.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);


            sw.WriteLine(s.Id + "|" + s.TipSmestaja.ToString() + "|" + s.Naziv + "|" + s.BrojZvezdica + "|" + s.Bazen.ToString() + "|" + s.SpaCentar.ToString() + "|" + s.ZaOsobeInv.ToString() + "|" + s.WiFi.ToString() + "|" + s.Obrisan.ToString());


            sw.Close();
            stream.Close();

        }

        public static void UpdateS(List<Smestaj> ss)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/smestaji.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Smestaj s in ss)
            {

                sw.WriteLine(s.Id + "|" + s.TipSmestaja.ToString() + "|" + s.Naziv + "|" + s.BrojZvezdica + "|" + s.Bazen.ToString() + "|" + s.SpaCentar.ToString() + "|" + s.ZaOsobeInv.ToString() + "|" + s.WiFi.ToString() + "|" + s.Obrisan.ToString());

            }

            sw.Close();
            stream.Close();

        }




        public static List<SmestajnaJedinica> ProcitajSmestajneJedinice(string putanja)
        {
            List<SmestajnaJedinica> smestajnaJedinicas = new List<SmestajnaJedinica>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');

                SmestajnaJedinica sj = new SmestajnaJedinica(int.Parse(token[0]), int.Parse(token[1]), bool.Parse(token[2]), int.Parse(token[3]),bool.Parse(token[4]));
                smestajnaJedinicas.Add(sj);

            }

            sr.Close();
            stream.Close();

            return smestajnaJedinicas;

        }

        public static int ProcitajPoslednjiIDSmestajJ()
        {
            string putanja = "~/App_Data/smestajnejedinice.txt";
            List<SmestajnaJedinica> smestajnaJedinicas = new List<SmestajnaJedinica>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            int id;
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');

                SmestajnaJedinica sj = new SmestajnaJedinica(int.Parse(token[0]), int.Parse(token[1]), bool.Parse(token[2]), int.Parse(token[3]),bool.Parse(token[4]));
                smestajnaJedinicas.Add(sj);

            }


            if (smestajnaJedinicas.Count == 0)
            {
                 id= -1;
            }
            else
            {
                id = smestajnaJedinicas[smestajnaJedinicas.Count - 1].Id;
            }
            sr.Close();
            stream.Close();

            return id;

        }

        public static int ProcitajPoslednjiSmestajId()
        {
            string putanja = "~/App_Data/smestaji.txt";
            List<Smestaj> smestajs = new List<Smestaj>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            int id;
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');

                Smestaj s = new Smestaj(int.Parse(token[0]), (TipSmestaja)Enum.Parse(typeof(TipSmestaja), token[1]), token[2], int.Parse(token[3]), bool.Parse(token[4]), bool.Parse(token[5]), bool.Parse(token[6]), bool.Parse(token[7]),bool.Parse(token[8]));
                smestajs.Add(s);

            }

            if (smestajs.Count == 0)
            {
                id = -1;
            }
            else
            {
                id = smestajs[smestajs.Count - 1].Id;
            }
            sr.Close();
            stream.Close();

            return id;

        }

        public static void UpisiSmestajnuJedinicu(SmestajnaJedinica j)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/smestajnejedinice.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);



            sw.WriteLine(j.Id + "|" + j.DozvoljenoGostiju + "|" + j.KucniLjubimci.ToString() + "|" + j.Cena + "|" + j.Obrisana.ToString());

            sw.Close();
            stream.Close();

        }

        public static void UpdateSJ(List<SmestajnaJedinica> jedn)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/smestajnejedinice.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (SmestajnaJedinica j in jedn)
            {
                
                sw.WriteLine(j.Id + "|" + j.DozvoljenoGostiju + "|" + j.KucniLjubimci.ToString() + "|" + j.Cena + "|" + j.Obrisana.ToString());


            }

            sw.Close();
            stream.Close();

        }


        #region KorisniciBaza
        public static void UpisiTuristu(Korisnik k) 
        {
            string path = HostingEnvironment.MapPath("~/App_Data/turisti.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string datum = k.DatumRodjenja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            sw.WriteLine(k.KorisnickoIme + "|" + k.Ime + "|" + k.Prezime + "|" + k.Lozinka + "|" + k.Pol.ToString() + "|" + k.Email + "|" + datum + "|" + k.Uloga.ToString()+ "|" + k.Blokiran.ToString() + "|" + k.OtkazRez);
            

            sw.Close();
            stream.Close();

        }

        public static void UpdateTuristu(List<Korisnik> users)  
        {
            string path = HostingEnvironment.MapPath("~/App_Data/turisti.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Korisnik k in users)
            {
                string datum = k.DatumRodjenja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                sw.WriteLine(k.KorisnickoIme + "|" + k.Ime + "|" + k.Prezime + "|" + k.Lozinka + "|" + k.Pol.ToString() + "|" + k.Email + "|" + datum + "|" + k.Uloga.ToString() + "|" + k.Blokiran.ToString() + "|" + k.OtkazRez);

            }

            sw.Close();
            stream.Close();

        }


        public static List<Korisnik> ProcitajTuristu(string putanja)  
        {
            List<Korisnik> kupci = new List<Korisnik>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open); 
            StreamReader sr = new StreamReader(stream);
            
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;


                string[] token = line.Split('|');
              
                DateTime datum = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));

                Korisnik k = new Korisnik(token[0],token[1],token[2],token[3],(PolType)Enum.Parse(typeof(PolType), token[4]),token[5],datum, (UserType)Enum.Parse(typeof(UserType), token[7])
                    ,new List<Aranzman>(),new List<Rezervacija>(),bool.Parse(token[8]), int.Parse(token[9]));
                kupci.Add(k);

            }
            sr.Close();
            stream.Close();

            return kupci;

        }


        public static List<Korisnik> ProcitajAdmina(string putanja)  
        {
            List<Korisnik> kupci = new List<Korisnik>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);  
            StreamReader sr = new StreamReader(stream);
            
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;


                string[] token = line.Split('|');

                DateTime datum = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));

                Korisnik k = new Korisnik(token[0], token[1], token[2], token[3], (PolType)Enum.Parse(typeof(PolType), token[4]), token[5], datum, (UserType)Enum.Parse(typeof(UserType), token[7])
                    , new List<Aranzman>(), new List<Rezervacija>(), bool.Parse(token[8]), int.Parse(token[9]));
                kupci.Add(k);

            }
            sr.Close();
            stream.Close();

            return kupci;

        }

        public static void UpdateAdmina(List<Korisnik> users)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/admins.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Korisnik k in users)
            {
                string datum = k.DatumRodjenja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                sw.WriteLine(k.KorisnickoIme + "|" + k.Ime + "|" + k.Prezime + "|" + k.Lozinka + "|" + k.Pol.ToString() + "|" + k.Email + "|" + datum + "|" + k.Uloga.ToString() + "|" + k.Blokiran.ToString() + "|" + k.OtkazRez);

            }

            sw.Close();
            stream.Close();

        }


        public static List<Korisnik> ProcitajMenadzera(string putanja)
        {
            List<Korisnik> kupci = new List<Korisnik>();
            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
           
            while (true)
            {
                string line = sr.ReadLine();
                if (line == null)
                    break;

                string[] token = line.Split('|');

                DateTime datum = DateTime.ParseExact(token[6], "dd/MM/yyyy", new CultureInfo("en-US"));

                Korisnik k = new Korisnik(token[0], token[1], token[2], token[3], (PolType)Enum.Parse(typeof(PolType), token[4]), token[5], datum, (UserType)Enum.Parse(typeof(UserType), token[7])
                    , new List<Aranzman>(), new List<Rezervacija>(), bool.Parse(token[8]),int.Parse(token[9]));
                kupci.Add(k);

            }
            sr.Close();
            stream.Close();

            return kupci;

        }

        public static void UpdateMenadzera(List<Korisnik> users)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/menadzeri.txt");
            FileStream stream = new FileStream(path, FileMode.Create);
            StreamWriter sw = new StreamWriter(stream);

            foreach (Korisnik k in users)
            {
                string datum = k.DatumRodjenja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
                sw.WriteLine(k.KorisnickoIme + "|" + k.Ime + "|" + k.Prezime + "|" + k.Lozinka + "|" + k.Pol.ToString() + "|" + k.Email + "|" + datum + "|" + k.Uloga.ToString() + "|" + k.Blokiran.ToString() + "|" + k.OtkazRez);

            }

            sw.Close();
            stream.Close();

        }

        public static void UpisMenadzera(Korisnik k)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/menadzeri.txt");
            FileStream stream = new FileStream(path, FileMode.Append);
            StreamWriter sw = new StreamWriter(stream);

            string datum = k.DatumRodjenja.ToString("dd/MM/yyyy", DateTimeFormatInfo.InvariantInfo);
            sw.WriteLine(k.KorisnickoIme + "|" + k.Ime + "|" + k.Prezime + "|" + k.Lozinka + "|" + k.Pol.ToString() + "|" + k.Email + "|" + datum + "|" + k.Uloga.ToString() + "|" + k.Blokiran.ToString() + "|" + k.OtkazRez);


            sw.Close();
            stream.Close();

        }

        #endregion

    }
}