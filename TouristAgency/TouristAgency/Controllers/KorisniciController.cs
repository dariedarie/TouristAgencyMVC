using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class KorisniciController : Controller
    {
        // GET: Korisnici

        #region ProfilTurista
        public ActionResult Turista()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            ViewBag.Turista = turista;
            return View();
        }


        public ActionResult OtkazivanjeRez()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("OtkazivanjeRez", "TuristaFun");
        }

        public ActionResult TuristaRezervacije()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("Rezervacije", "TuristaFun");
        }

        public ActionResult PregledRezervacija()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("PregledRezervacija", "TuristaFun");
        }

        public ActionResult NaKomentare()
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("Komentari", "TuristaFun");
        }

        public ActionResult IzmenaTuriste(string korisnickoIme)  
        {
            Korisnik turista = (Korisnik)Session["sturista"];
            if (turista == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];

            foreach (Korisnik k in turisti)
            {
                if (k.KorisnickoIme.Equals(korisnickoIme))
                {
                    ViewBag.Turista = k;
                    return View();
                }
            }

            return RedirectToAction("Turista");

        }

        [HttpPost]
        public ActionResult IzmeniTuristu(string korisnickoIme, Korisnik turista_izmena)
        {
            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
           

            
            Korisnik k = new Korisnik();  
            foreach (Korisnik u in turisti)
            {
                if (u.KorisnickoIme.Equals(korisnickoIme))
                {
                    k = u;
                }
            }

            if (turista_izmena.Ime == null || turista_izmena.Prezime == null || turista_izmena.Lozinka == null)
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                ViewBag.Turista = k;
                return View("IzmenaTuriste");
            }

            
            if (turista_izmena.Lozinka.Length < 5)
            {
                ViewBag.Message = $"Lozinka mora sadrzati najmanje 5 karaktera!";
                ViewBag.Turista = k;
                return View("IzmenaTuriste");
            }

            

            turista_izmena.Uloga = (UserType)Enum.Parse(typeof(UserType), "TURISTA"); 

            for (int i = 0; i < turisti.Count; i++)
            {
                if (turisti[i].KorisnickoIme == korisnickoIme)
                {
                    if (turista_izmena.DatumRodjenja.Equals(DateTime.MinValue))
                    {
                        turista_izmena.DatumRodjenja = turisti[i].DatumRodjenja;
                    }

                    turisti[i] = turista_izmena;

                }
            }

            Baza.UpdateTuristu(turisti);
            
            ViewBag.Turista = turista_izmena;

          
            Session["sturista"] = null;
            Session["sturista"] = turista_izmena;

            return View("Turista");

        }
        #endregion


        #region ProfilAdmin
        public ActionResult Admin()
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            ViewBag.Admin = admin;
            return View();
        }


        public ActionResult IzmenaAdmina(string korisnickoIme)
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Korisnik> admini = (List<Korisnik>)HttpContext.Application["admin"];

            foreach (Korisnik k in admini)
            {
                if (k.KorisnickoIme.Equals(korisnickoIme))
                {
                    ViewBag.Admin = k;
                    return View();
                }
            }

            return RedirectToAction("Admin");

        }

        [HttpPost]
        public ActionResult IzmeniAdmina(string korisnickoIme, Korisnik adminEdit)
        {
           
            List<Korisnik> administratori = (List<Korisnik>)HttpContext.Application["admin"];
            

            
            Korisnik k = new Korisnik();
            foreach (Korisnik u in administratori)
            {
                if (u.KorisnickoIme.Equals(korisnickoIme))
                {
                    k = u;
                }
            }

            if (adminEdit.Ime == null || adminEdit.Prezime == null || adminEdit.Lozinka == null)
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                ViewBag.Admin = k;
                return View("IzmenaAdmina");
            }


            if (adminEdit.Lozinka.Length < 5)
            {
                ViewBag.Message = $"Lozinka mora sadrzati najmanje 5 karaktera!";
                ViewBag.Admin = k;
                return View("IzmenaAdmina");
            }



            adminEdit.Uloga = (UserType)Enum.Parse(typeof(UserType), "ADMINISTRATOR");

            for (int i = 0; i < administratori.Count; i++)
            {
                if (administratori[i].KorisnickoIme == korisnickoIme)
                {
                    if (adminEdit.DatumRodjenja.Equals(DateTime.MinValue))//ako nisam uneo datum rodjenja stavi mi stari
                    {
                        adminEdit.DatumRodjenja = administratori[i].DatumRodjenja;
                    }

                    administratori[i] = adminEdit;

                }
            }

            Baza.UpdateAdmina(administratori);

            ViewBag.Admin = adminEdit;


            Session["sadmin"] = null;
            Session["sadmin"] = adminEdit;

            return View("Admin");

        }


        public ActionResult AdminNewMen()
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

          
            return RedirectToAction("NoviMenadzer","AdminFun");
        }

        public ActionResult AdminSviKorisnici()
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("SviKorisnici", "AdminFun");
        }



        #endregion

        #region ProfilMenadzer
        public ActionResult Menadzer()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            ViewBag.Menadzer = menadzer;
            return View();
        }




        public ActionResult IzmenaMenadzera(string korisnickoIme)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];

            foreach (Korisnik k in menadzeri)
            {
                if (k.KorisnickoIme.Equals(korisnickoIme))
                {
                    ViewBag.Menadzer = k;
                    return View();
                }
            }

            return RedirectToAction("Menadzer");

        }

        [HttpPost]
        public ActionResult IzmeniMenadzera(string korisnickoIme, Korisnik menadzerEdit)
        {

            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];


            //u slucaju greske kada se ponovo ucita stranica
            Korisnik k = new Korisnik();
            foreach (Korisnik u in menadzeri)
            {
                if (u.KorisnickoIme.Equals(korisnickoIme))
                {
                    k = u;
                }
            }

            if (menadzerEdit.Ime == null || menadzerEdit.Prezime == null || menadzerEdit.Lozinka == null)
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                ViewBag.Menadzer = k;
                return View("IzmenaMenadzera");
            }


            if (menadzerEdit.Lozinka.Length < 5)
            {
                ViewBag.Message = $"Lozinka mora sadrzati najmanje 5 karaktera!";
                ViewBag.Menadzer = k;
                return View("IzmenaMenadzera");
            }



            menadzerEdit.Uloga = (UserType)Enum.Parse(typeof(UserType), "MENADZER");

            for (int i = 0; i < menadzeri.Count; i++)
            {
                if (menadzeri[i].KorisnickoIme == korisnickoIme)
                {
                    if (menadzerEdit.DatumRodjenja.Equals(DateTime.MinValue))//ako nisam uneo datum rodjenja stavi mi stari
                    {
                        menadzerEdit.DatumRodjenja = menadzeri[i].DatumRodjenja;
                    }

                    menadzeri[i] = menadzerEdit;

                }
            }

            Baza.UpdateMenadzera(menadzeri);

            ViewBag.Menadzer = menadzerEdit;


            Session["smenadzer"] = null;
            Session["smenadzer"] = menadzerEdit;

            return View("Menadzer");

        }


        public ActionResult MenadzerSmestajnaJedinica()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("SmestajneJedinice", "MenadzerFun");
        }

        public ActionResult MenadzerAranzman()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("Aranzmani", "MenadzerFun");
        }


        public ActionResult MenadzerKoment()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("MenadzerKomentari", "MenadzerFun");
        }

        public ActionResult MenadzerSmestaj()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            return RedirectToAction("Smestaji", "MenadzerFun");
        }
        #endregion


        public ActionResult Profil()    
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            Korisnik admin = (Korisnik)Session["sadmin"];
            Korisnik turista = (Korisnik)Session["sturista"];

            if (admin != null)
            {
                return RedirectToAction("Admin");
            }
            else if (menadzer != null)
            {
                return RedirectToAction("Menadzer");
            }
            else if (turista != null)
            {
                return RedirectToAction("Turista");
            }
            else
            {
                return RedirectToAction("Login", "Registracija");
            }

        }


       
    }
}