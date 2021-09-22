using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class TuristaFunController : Controller
    {
        // GET: TuristaFun

        public ActionResult Rezervacije()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> svi_aranzmani_neobrisani = new List<Aranzman>();

            //implementirano zbog logickog brisanja
            svi_aranzmani.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_aranzmani_neobrisani.Add(k));

            List<Aranzman> aranzmani_buduce = new List<Aranzman>();
            List<Aranzman> aranzmani_prosle = new List<Aranzman>();

            foreach (Aranzman man in svi_aranzmani_neobrisani)
            {
                if (DateTime.Compare(man.DatumPocetkaPutovanja, DateTime.Now) > 0)
                {
                    aranzmani_buduce.Add(man);
                }
                else
                {
                    aranzmani_prosle.Add(man);
                }
            }
            aranzmani_buduce = aranzmani_buduce.OrderBy(a => a.DatumPocetkaPutovanja).ToList();
            //aranzmani_prosle = aranzmani_prosle.OrderByDescending(m => m.DatumPocetkaPutovanja).ToList();  
            //foreach (Aranzman man in aranzmani_prosle)
            //{
            //    aranzmani_buduce.Add(man);   
            //}

            ViewBag.aranzmani = aranzmani_buduce;

            HttpRuntime.Cache["aranzman_kes"] = aranzmani_buduce;

            return View();
        }


        public ActionResult Komentari()  
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Rezervacija> sve_rez = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            List<Aranzman> aranzmani_prosle = new List<Aranzman>();   
            List<Aranzman> aranzmani_prikaz = new List<Aranzman>();   


            foreach (Aranzman aran in svi_aranzmani)
            {
                if ((DateTime.Compare(aran.DatumZavrsetkaPutovanja, DateTime.Now) < 0) && aran.Obrisan == false) 
                {
                    aranzmani_prosle.Add(aran);
                }
            }


            foreach (Aranzman aran in svi_aranzmani)
            {
                foreach (Rezervacija r in sve_rez)   
                {
                    if (r.Turista.KorisnickoIme.Equals(turista.KorisnickoIme) && r.Status.ToString().Equals("AKTIVNA") && r.Aranzman.Id == aran.Id)
                    {                                                                
                        aranzmani_prikaz.Add(aran);
                        break;   
                    }
                }

            }

            ViewBag.aranzmani = aranzmani_prikaz;
            return View();

        }


        public ActionResult DodajKomentar(string id_aran)  
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            ViewBag.id_aran = id_aran;
            return View();

        }




        [HttpPost]
        public ActionResult DodajComm(string id_aran, string tekst, string a_ocena)
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            
            if (tekst == "" || a_ocena == "")
            {
                ViewBag.Message = "Niste popunili sva polja!";
                ViewBag.id_aran = id_aran;
                return View("DodajKomentar");

            }

            try
            {
                int.Parse(a_ocena);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Ocena mora biti ceo broj u sledecem intervalu: [1,5]!";
                ViewBag.id_aran = id_aran;
                return View("DodajKomentar");
            }

            int ocena = int.Parse(a_ocena);
            if (ocena < 1 || ocena > 5)
            {
                ViewBag.Message = "Ocena mora biti ceo broj u sledecem intervalu: [1,5]!";
                ViewBag.id_aran = id_aran;
                return View("DodajKomentar");
            }

            
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentar"];
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Korisnik> svi_turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> svi_menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];
            List<Rezervacija> sve_karte = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            Komentar comm = new Komentar();
            Aranzman aranzman = new Aranzman();

            foreach (Aranzman aran in svi_aranzmani)
            {
                if (aran.Id == int.Parse(id_aran))
                {
                    aranzman = aran;
                }
            }

            
            int id_zadnji = Baza.ProcitajKomentarZadnjiId();
            comm.Id = id_zadnji + 1;
            comm.Aranzman = aranzman;
            comm.Turista = turista;
            comm.Ocena = int.Parse(a_ocena);
            comm.TekstKomentara = tekst;
            comm.StatusKomentara = (StatusKomentara)Enum.Parse(typeof(StatusKomentara), "CEKA");

            
            komentari.Add(comm);
            Baza.UpisiKomentar(comm);
            
            return RedirectToAction("Turista","Korisnici");

        }

        [HttpPost]
        public ActionResult Otkazi(string id_rez)
        {
            
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            List<Rezervacija> sve_rez = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            List<Korisnik> svi_turisti = (List<Korisnik>)HttpContext.Application["turista"];
            Korisnik u = new Korisnik();
            Rezervacija r = new Rezervacija();

            foreach (Rezervacija rez in sve_rez)
            {
                if (rez.Id.Equals(id_rez))
                {
                    r = rez;
                    u = rez.Turista;  
                }
            }

            foreach (Korisnik kup in svi_turisti)
            {
                if (kup.KorisnickoIme.Equals(u.KorisnickoIme))
                {
                    u = kup;   
                }
            }


            
            

            r.Status = (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), "OTKAZANA");


            for (int i = 0; i < u.Rezervacije.Count; i++)
            {
                if (u.Rezervacije[i].Id.Equals(id_rez))
                {
                    u.Rezervacije[i].Status = (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), "OTKAZANA");
                }
            }

            r.Turista = u;


            u.OtkazRez++;
            for (int i = 0; i < svi_turisti.Count; i++)
            {
                if (svi_turisti[i].KorisnickoIme.Equals(u.KorisnickoIme))
                {
                    svi_turisti[i] = u;
                }
            }

           
            for (int i = 0; i < sve_rez.Count; i++)
            {
                if (sve_rez[i].Id.Equals(r.Id))
                {
                    sve_rez[i] = r;
                }
            }
            Baza.UpdateRezervaciju(sve_rez);
            Baza.UpdateTuristu(svi_turisti);

           
            Session["sturista"] = null;
            Session["sturista"] = u;

            return RedirectToAction("OtkazivanjeRez");

        }




        [HttpPost]
        public ActionResult Rezervisi(string id)
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            List<Rezervacija> sve_rez = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];

            Aranzman aranzman = new Aranzman();

          

            string poslednji_id = Baza.ProcitajRezervacijuIdZadnji();
            double poslednji_id_double = 0;
            if (poslednji_id == "")   
            {
                poslednji_id_double = 999999999;

            }
            else
            {
                poslednji_id_double = double.Parse(poslednji_id);
            }

            double id_koji_mi_treba = poslednji_id_double + 1;
            Rezervacija nova_rez = new Rezervacija();
            nova_rez.Id = id_koji_mi_treba.ToString();
            nova_rez.Status = (StatusRezervacije)Enum.Parse(typeof(StatusRezervacije), "AKTIVNA");
            nova_rez.Turista = turista;

            Rezervacija r = new Rezervacija();
           
            //List<Aranzman> svi_aran = (List<Aranzman>)HttpContext.Application["aranzman"];
            foreach (Aranzman aa in svi_aranzmani)
            {
                if (aa.Id.ToString() == id)
                {
                   nova_rez.Aranzman = aa;
               }
           }

            sve_rez.Add(nova_rez);
            Baza.UpisiRezervaciju(nova_rez);

           
            Session["sturista"] = null;
            Session["sturista"] = turista;

            return RedirectToAction("Rezervacije");  
        }
        public ActionResult PregledRezervacija()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Rezervacija> sve_rez = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            List<Rezervacija> rez_turiste = new List<Rezervacija>();
            

            foreach (Rezervacija r in sve_rez)
            {
                if (r.Turista.KorisnickoIme.Equals(turista.KorisnickoIme))   
                {
                    rez_turiste.Add(r);
                }
        
            }

           
            ViewBag.rez = rez_turiste;
            HttpRuntime.Cache["rez_pretraga"] = rez_turiste;

            return View();
        }


        public ActionResult OtkazivanjeRez()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Rezervacija> sve_rez = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            List<Rezervacija> rez_turiste = new List<Rezervacija>();


            foreach (Rezervacija r in sve_rez)
            {
                if (r.Turista.KorisnickoIme.Equals(turista.KorisnickoIme) && r.Status.ToString().Equals("AKTIVNA"))
                {
                    rez_turiste.Add(r);
                }

            }


            ViewBag.rez = rez_turiste;
            HttpRuntime.Cache["rez_pretraga"] = rez_turiste;

            return View();
        }


        #region Sort&Pretraga
        public ActionResult SortirajNazivUp()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderBy(c => c.Aranzman.Naziv).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }

        public ActionResult SortirajIdDown()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderByDescending(c => c.Id).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }


        public ActionResult SortirajIdUp()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderBy(c => c.Id).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }

        public ActionResult SortirajNazivDown()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderByDescending(c => c.Aranzman.Naziv).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }


        public ActionResult SortirajDatumPDown()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderByDescending(c => c.Aranzman.DatumPocetkaPutovanja).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }

        public ActionResult SortirajDatumPUp()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderBy(c => c.Aranzman.DatumPocetkaPutovanja).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }


        public ActionResult SortirajDatumZDown()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderByDescending(c => c.Aranzman.DatumZavrsetkaPutovanja).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");

        }

        public ActionResult SortirajDatumZUp()
        {
            List<Rezervacija> rez = (List<Rezervacija>)HttpRuntime.Cache["rez_pretraga"];

            rez = rez.OrderBy(c => c.Aranzman.DatumZavrsetkaPutovanja).ToList();

            ViewBag.rez = rez;
            return View("PregledRezervacija");
        }


        public ActionResult Pretraga(string donjagranicaP, string gornjagranicaP, string donjagranicaZ, string gornjagranicaZ, string tipAranzmana, string naziv,string status,string id)
        {
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> svi_aranzmani_neobrisani = new List<Aranzman>();
            svi_aranzmani.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_aranzmani_neobrisani.Add(k));

            List<Rezervacija> svi_rezervacije = (List<Rezervacija>)HttpContext.Application["rezervacija"];
            

            List<Aranzman> search_aranzmani = new List<Aranzman>();
            List<Rezervacija> search_rezervacije = new List<Rezervacija>();

            search_aranzmani = svi_aranzmani_neobrisani.ConvertAll(a => new Aranzman(a.Id, a.Naziv, a.TipAranzmana, a.TipPrevoza, a.Lokacija, a.DatumPocetkaPutovanja, a.DatumZavrsetkaPutovanja, a.MestoNalazenja, a.VremeNalazenja, a.MaxBrojPutnika, a.Opis, a.Program, a.Poster, a.IdSmestaja, a.Obrisan, a.Menadzer));
            search_rezervacije = svi_rezervacije.ConvertAll(a => new Rezervacija(a.Aranzman,a.Id, a.Turista,a.Status));

            if (donjagranicaP == "" && gornjagranicaP == "" && gornjagranicaZ == "" && donjagranicaZ == "" && status == "" && tipAranzmana == "" && naziv == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";

                //svi_aranzmani_neobrisani = svi_aranzmani_neobrisani.OrderBy(c => c.DatumPocetkaPutovanja).ToList();
                ViewBag.rez = svi_rezervacije;
                HttpRuntime.Cache["rez_pretraga"] = svi_rezervacije;
                return View("PregledRezervacija");

            }

            if (naziv != "")
            {
                search_rezervacije.RemoveAll(item => !item.Aranzman.Naziv.ToLower().Contains(naziv.ToLower()));
            }

            //if (id != "")
            //{
            //    search_rezervacije.RemoveAll(item => !item.Id.ToLower().Equals(id.ToLower()));
            //}

            if (status != "")
            {
                search_rezervacije.RemoveAll(item => !item.Status.ToString().Equals(status));
            }

            if (tipAranzmana != "")
            {
                search_rezervacije.RemoveAll(item => !item.Aranzman.TipAranzmana.ToString().Equals(tipAranzmana));
            }


            if (donjagranicaP != "")
            {
                DateTime donjagranica = Convert.ToDateTime(donjagranicaP);
                search_rezervacije.RemoveAll(a => DateTime.Compare(a.Aranzman.DatumPocetkaPutovanja, donjagranica) < 0);

            }

            if (gornjagranicaP != "")
            {
                DateTime gornjagranica = Convert.ToDateTime(gornjagranicaP);
                search_rezervacije.RemoveAll(a => DateTime.Compare(gornjagranica, a.Aranzman.DatumPocetkaPutovanja) < 0);

            }


            if (donjagranicaZ != "")
            {
                DateTime donjagranica = Convert.ToDateTime(donjagranicaZ);
                search_rezervacije.RemoveAll(a => DateTime.Compare(a.Aranzman.DatumZavrsetkaPutovanja, donjagranica) < 0);

            }

            if (gornjagranicaZ != "")
            {
                DateTime gornjagranica = Convert.ToDateTime(gornjagranicaZ);
                search_rezervacije.RemoveAll(a => DateTime.Compare(gornjagranica, a.Aranzman.DatumZavrsetkaPutovanja) < 0);

            }


            HttpRuntime.Cache["rez_pretraga"] = search_rezervacije;

            ViewBag.rez = search_rezervacije;
            return View("PregledRezervacija");
        }

        #endregion
    }
}