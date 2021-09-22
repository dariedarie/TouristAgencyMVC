using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class RegistracijaController : Controller
    {
        public ActionResult Index()
        {
            return View("~/Views/Registracija/Registracija.cshtml");
        }


        [HttpPost]
        public ActionResult RegistracijaKorisnika(Korisnik k)  
        {
            List<Korisnik> administratori = (List<Korisnik>)HttpContext.Application["admin"];
            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];


            if (k.KorisnickoIme == null || k.Ime == null || k.Prezime == null || k.Lozinka == null)
            {
                ViewBag.Message = $"Sva tekstualna polja moraju biti popunjena!";
                return View("~/Views/Registracija/Registracija.cshtml");
            }

            if (k.DatumRodjenja.Equals(DateTime.MinValue))
            {
                ViewBag.Message = $"Morate uneti datum rodjenja!";
                return View("~/Views/Registracija/Registracija.cshtml");
            }

            if (DateTime.Compare(k.DatumRodjenja, DateTime.Now) >= 0)
            {
                ViewBag.Message = "Datum rodjenja mora biti u proslosti!";
                return View("~/Views/Registracija/Registracija.cshtml");
            }


            if (k.Lozinka.Length < 5)
            {
                ViewBag.Message = $"Lozinka mora sadrzati najmanje 5 karaktera!";
                return View("~/Views/Registracija/Registracija.cshtml");
            }


            foreach (Korisnik user in administratori)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim korisnickim imenom vec postoji!";
                    return View("~/Views/Registracija/Registracija.cshtml");
                }
            }

            foreach (Korisnik user in turisti)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim  korisnickim imenom vec postoji!";
                    return View("~/Views/Registracija/Registracija.cshtml");
                }
            }

            foreach (Korisnik user in menadzeri)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim  korisnickim imenom vec postoji!";
                    return View("~/Views/Registracija/Registracija.cshtml");
                }
            }

           
            turisti.Add(k);
            Baza.UpisiTuristu(k);

            Session["sturista"] = k;

            return RedirectToAction("Index", "Home");

        }

        public ActionResult Login()
        {

            return View();
        }


        public ActionResult LogOut()
        {
            Session["sadmin"] = null;
            Session["sturista"] = null;
            Session["smenadzer"] = null;
            return RedirectToAction("Login", "Registracija");
        }


        [HttpPost]
        public ActionResult LogovanjeKorsnika(string korisnickoIme, string lozinka)
        {
            List<Korisnik> administratori = (List<Korisnik>)HttpContext.Application["admin"];
            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];

            if(korisnickoIme == "" || lozinka == "")
            {
                ViewBag.Message = $"Morate popuniti sva polja!";
                return View("Login");
            }


            Korisnik k = new Korisnik();
            k.KorisnickoIme = korisnickoIme;
            k.Lozinka = lozinka;

            bool postoji = false;

            foreach (Korisnik admin in administratori)
            {
                if (admin.KorisnickoIme == korisnickoIme && admin.Lozinka==lozinka)
                {
                    k = admin;
                    postoji = true;
                }

            }

            if (postoji == false)  
            {
                foreach (Korisnik t in turisti)
                {
                    if (t.KorisnickoIme == korisnickoIme && t.Lozinka == lozinka)
                    {
                        k = t;
                        postoji = true;
                    }

                }
            }
            if (postoji == false)  
            {
                foreach (Korisnik m in menadzeri)
                {
                    if (m.KorisnickoIme == korisnickoIme && m.Lozinka == lozinka)
                    {
                        k = m;
                        postoji = true;
                    }

                }
            }


            if (postoji == true)
            {
                if (k.Blokiran == true)   
                {
                    ViewBag.Message = $"BLOKIRANI STE!";
                    return View("Login");
                }

                if (k.Uloga.ToString().Equals("ADMINISTRATOR"))   
                {
                    Session["sadmin"] = k;
                    return RedirectToAction("Admin", "Korisnici");
                }
                else if (k.Uloga.ToString().Equals("MENADZER"))  
                {
                    Session["smenadzer"] = k;
                    return RedirectToAction("Menadzer", "Korisnici");
                }
                else  
                {
                    Session["sturista"] = k;
                    return RedirectToAction("Turista", "Korisnici");
                }

            }

            ViewBag.Message = $"Ne postoji registrovan korisnik sa unesenim username-om i unesenom sifrom!";
            return View("Login");


        }

    }
       
}
