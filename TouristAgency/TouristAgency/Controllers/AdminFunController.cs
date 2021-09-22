using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class AdminFunController : Controller
    {
        // GET: AdminFun
        public ActionResult NoviMenadzer()
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            return View();
        }


        
        public ActionResult BlokirajTuristu(string korisnickoIme)
        {

            List<Korisnik> turisti= (List<Korisnik>)HttpContext.Application["turista"];
            for (int i = 0; i < turisti.Count; i++)
            {
                if (turisti[i].KorisnickoIme == korisnickoIme)
                {
                    turisti[i].Blokiran = true;
                }
            }

            Baza.UpdateTuristu(turisti);

            return RedirectToAction("SviKorisnici");
        }


        [HttpPost]
        public ActionResult RegMenadzera(Korisnik k)
        {
            List<Korisnik> administratori = (List<Korisnik>)HttpContext.Application["admin"];
            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];


            if (k.KorisnickoIme == null || k.Ime == null || k.Prezime == null || k.Lozinka == null)
            {
                ViewBag.Message = $"Sva tekstualna polja moraju biti popunjena!";
                return View("~/Views/AdminFun/NoviMenadzer.cshtml");
            }

            if (k.DatumRodjenja.Equals(DateTime.MinValue))
            {
                ViewBag.Message = $"Morate uneti datum rodjenja!";
                return View("~/Views/AdminFun/NoviMenadzer.cshtml");
            }

            if (DateTime.Compare(k.DatumRodjenja, DateTime.Now) >= 0)
            {
                ViewBag.Message = "Datum rodjenja mora biti u proslosti!";
                return View("~/Views/AdminFun/NoviMenadzer.cshtml");
            }


            if (k.Lozinka.Length < 5)
            {
                ViewBag.Message = $"Lozinka mora sadrzati najmanje 5 karaktera!";
                return View("~/Views/AdminFun/NoviMenadzer.cshtml");
            }


            foreach (Korisnik user in administratori)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim korisnickim imenom vec postoji!";
                    return View("~/Views/AdminFun/NoviMenadzer.cshtml");
                }
            }

            foreach (Korisnik user in turisti)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim  korisnickim imenom vec postoji!";
                    return View("~/Views/AdminFun/NoviMenadzer.cshtml");
                }
            }

            foreach (Korisnik user in menadzeri)
            {
                if (user.KorisnickoIme.Equals(k.KorisnickoIme))
                {
                    ViewBag.Message = $"Korisnik sa tim  korisnickim imenom vec postoji!";
                    return View("~/Views/AdminFun/NoviMenadzer.cshtml");
                }
            }


            menadzeri.Add(k);
            Baza.UpisMenadzera(k);


            return RedirectToAction("SviKorisnici");

        }

        public ActionResult SviKorisnici()
        {
            Korisnik admin = (Korisnik)Session["sadmin"];
            if (admin == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> admini = (List<Korisnik>)HttpContext.Application["admin"];
            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];
            List<Korisnik> korisnici = new List<Korisnik>();
            Korisnik t = new Korisnik();
            Korisnik a = new Korisnik();
            Korisnik m = new Korisnik();

            foreach (var item in turisti)
            {
                korisnici.Add(item);
            }
            foreach (var item in admini)
            {
                korisnici.Add(item);
            }
            foreach (var item in menadzeri)
            {
                korisnici.Add(item);
            }
            
            
            

            //turisti.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));  
            //menadzeri.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));  
            //admini.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));  


            ViewBag.Korisnici = korisnici;
            HttpRuntime.Cache["korisnici_pretraga"] = korisnici;


            return View();

        }
        #region Pretraga i Sort
        public ActionResult Pretraga(string pIme, string pPrezime,string uloga)
        {
            List<Korisnik> turisti = (List<Korisnik>)HttpContext.Application["turista"];
            List<Korisnik> admini = (List<Korisnik>)HttpContext.Application["admin"];
            List<Korisnik> menadzeri = (List<Korisnik>)HttpContext.Application["menadzer"];
            List<Korisnik> korisnici = new List<Korisnik>();
            Korisnik t = new Korisnik();
            Korisnik a = new Korisnik();
            Korisnik m = new Korisnik();

            foreach (var item in turisti)
            {
                korisnici.Add(item);
            }
            foreach (var item in admini)
            {
                korisnici.Add(item);
            }
            foreach (var item in menadzeri)
            {
                korisnici.Add(item);
            }

            //turisti.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));
            //menadzeri.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));
            //admini.Where(k => k.Blokiran == false).ToList().ForEach(k => korisnici.Add(k));

            List<Korisnik> korinsik_pretraga = new List<Korisnik>();

            if (pIme == "" && pPrezime == "" && uloga == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";
                ViewBag.Korisnici = korisnici;
                HttpRuntime.Cache["korisnici_pretraga"] = korisnici;  
                return View("SviKorisnici");

            }

            if (pIme != "")
            {


                foreach (Korisnik u in admini)
                {
                    if (u.Ime.ToLower().Contains(pIme.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in turisti)
                {
                    if (u.Ime.ToLower().Contains(pIme.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in menadzeri)
                {
                    if (u.Ime.ToLower().Contains(pIme.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                
            }

            if (pPrezime != "")
            {

                foreach (Korisnik u in admini)
                {
                    if (u.Prezime.ToLower().Contains(pPrezime.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in turisti)
                {
                    if (u.Prezime.ToLower().Contains(pPrezime.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in menadzeri)
                {
                    if (u.Prezime.ToLower().Contains(pPrezime.ToLower()) )  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

            }

            if (uloga != "")
            {

                foreach (Korisnik u in admini)
                {
                    if (u.Uloga.ToString().ToLower().Contains(uloga.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in turisti)
                {
                    if (u.Uloga.ToString().ToLower().Contains(uloga.ToLower()))  
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

                foreach (Korisnik u in menadzeri)
                {
                    if (u.Uloga.ToString().ToLower().Contains(uloga.ToLower())) 
                    {
                        korinsik_pretraga.Add(u);
                    }
                }

            }


            ViewBag.korisnici = korinsik_pretraga;
            HttpRuntime.Cache["korisnici_pretraga"] = korinsik_pretraga;
            return View("SviKorisnici");  
        }

        public ActionResult SortImeDown()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"];

            korisnici = korisnici.OrderByDescending(k => k.Ime).ToList();

            ViewBag.Korisnici = korisnici;
            return View("SviKorisnici");

        }

        public ActionResult SortImeUp()
        {
            List<Korisnik> korisnici_kes = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"];  

            korisnici_kes = korisnici_kes.OrderBy(k => k.Ime).ToList();

            ViewBag.Korisnici = korisnici_kes;
            return View("SviKorisnici");

        }


        public ActionResult SortPrezimeDown()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"];  

            korisnici = korisnici.OrderByDescending(k => k.Prezime).ToList();

            ViewBag.Korisnici = korisnici;
            return View("SviKorisnici");

        }

        public ActionResult SortPrezimeUp()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"]; 

            korisnici = korisnici.OrderBy(k => k.Prezime).ToList();

            ViewBag.Korisnici = korisnici;
            return View("SviKorisnici");

        }

        public ActionResult SortUlogaDown()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"];

            korisnici = korisnici.OrderByDescending(k => k.Uloga.ToString()).ToList();

            ViewBag.Korisnici = korisnici;
            return View("SviKorisnici");

        }

        public ActionResult SortUlogaUp()
        {
            List<Korisnik> korisnici = (List<Korisnik>)HttpRuntime.Cache["korisnici_pretraga"];

            korisnici = korisnici.OrderBy(k => k.Uloga.ToString()).ToList();

            ViewBag.Korisnici = korisnici;
            return View("SviKorisnici");

        }
        #endregion

    }
}