using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class MenadzerFunController : Controller
    {

        public ActionResult MenadzerKomentari()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentar"];
            List<Komentar> komentari_cek = new List<Komentar>();

            foreach (Komentar koment in komentari)  
            {
                if (koment.StatusKomentara.ToString().Equals("CEKA") && koment.Aranzman.Menadzer.KorisnickoIme.Equals(menadzer.KorisnickoIme))  
                {
                    komentari_cek.Add(koment);
                }
            }

            ViewBag.Komentari = komentari_cek;
            return View();

        }

        [HttpPost]
        public ActionResult OdobriKoment(string komentara_id)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentar"];

            foreach (Komentar koment in komentari)
            {
                if (koment.Id == int.Parse(komentara_id))
                {
                    koment.StatusKomentara = (StatusKomentara)Enum.Parse(typeof(StatusKomentara), "ODOBREN");
                    Baza.UpdateKoment(komentari);
                    return RedirectToAction("MenadzerKomentari");

                }
            }

            return RedirectToAction("MenadzerKomentari");

        }

        [HttpPost]
        public ActionResult OdbijKoment(string komentara_id)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Application["komentar"];

            foreach (Komentar koment in komentari)
            {
                if (koment.Id == int.Parse(komentara_id))
                {
                    koment.StatusKomentara = (StatusKomentara)Enum.Parse(typeof(StatusKomentara), "ODBIJEN");
                    Baza.UpdateKoment(komentari);
                    return RedirectToAction("MenadzerKomentari");

                }
            }

            return RedirectToAction("MenadzerKomentari");

        }



        // GET: MenadzerFun
        #region SmestajnaJedinicaFun
        public ActionResult SmestajneJedinice()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<SmestajnaJedinica> smestajJ = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            List<SmestajnaJedinica> neobrisane = new List<SmestajnaJedinica>();

            smestajJ.Where(k => k.Obrisana == false).ToList().ForEach(k => neobrisane.Add(k));
            ViewBag.SmestajJ = neobrisane;

            HttpRuntime.Cache["smestajJ_kes"] = neobrisane;
            return View();
        }


        public ActionResult ObrisiSJ(string id_obrisi) 
        {

            List<SmestajnaJedinica> smestajJ = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            for (int i = 0; i < smestajJ.Count; i++)
            {
                if (smestajJ[i].Id == int.Parse(id_obrisi))
                {
                    smestajJ[i].Obrisana = true;
                }
            }

            Baza.UpdateSJ(smestajJ);

            return RedirectToAction("SmestajneJedinice");
        }




        public ActionResult IzmenaSJ(string id)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<SmestajnaJedinica> smestajJedn = (List<SmestajnaJedinica>)HttpContext.Cache["smestajJ_kes"];
            ViewBag.smestajJedn = smestajJedn.Find(item => item.Id == int.Parse(id));
            //List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            //ViewBag.Smestaj = smestaji;

            return View();
        }


        [HttpPost]
        public ActionResult IzmeniSJ(string id_sj, string dozvoljenoGostiju, string kucniLjubimci, string cena)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            List<SmestajnaJedinica> svi_sj = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            SmestajnaJedinica sj_izmena_original = svi_sj.Find(item => item.Id == int.Parse(id_sj));
            List<SmestajnaJedinica> sj_kopija_izmene = new List<SmestajnaJedinica>() { new SmestajnaJedinica() };
            List<SmestajnaJedinica> sj_izmena_original_lista = new List<SmestajnaJedinica>() { sj_izmena_original };
            sj_kopija_izmene = sj_izmena_original_lista.ConvertAll(a => new SmestajnaJedinica(a.Id, a.DozvoljenoGostiju,a.KucniLjubimci,a.Cena,a.Obrisana));
            SmestajnaJedinica sj_izmena = sj_kopija_izmene[0];
            ViewBag.smestajJedn = svi_sj.Find(item => item.Id == int.Parse(id_sj));

            //List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            //ViewBag.Smestaj = smestaji;

            if (dozvoljenoGostiju == "" || kucniLjubimci == "" || cena == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                //ViewBag.Smestaj = smestaji;
                return View("IzmenaSJ");

            }

            
            try
            {
                int.Parse(dozvoljenoGostiju);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeno gostiju mora biti celobrojna nenegativna vrednost!";
                //ViewBag.Smestaj = smestaji;
                return View("IzmenaSJ");

            }

            if (int.Parse(dozvoljenoGostiju) < 0)
            {
                ViewBag.Message = "Dozvoljeno gostiju mora biti celobrojna nenegativna vrednost!";
               // ViewBag.Smestaj = smestaji;
                return View("IzmenaSJ");
            }



            try
            {
                int.Parse(cena);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeno gostiju mora biti celobrojna nenegativna vrednost!";
                //ViewBag.Smestaj = smestaji;
                return View("IzmenaSJ");

            }

            if (int.Parse(cena) < 0)
            {
                ViewBag.Message = "Dozvoljeno gostiju mora biti celobrojna nenegativna vrednost!";
                // ViewBag.Smestaj = smestaji;
                return View("IzmenaSJ");
            }





            //int id = Baza.ProcitajPoslednjiIDSmestajJ();
            sj_izmena.Id = int.Parse(id_sj);
            sj_izmena.DozvoljenoGostiju = int.Parse(dozvoljenoGostiju);
            sj_izmena.KucniLjubimci = bool.Parse(kucniLjubimci);
            sj_izmena.Cena = int.Parse(cena);
           
           
        

            for (int i = 0; i < svi_sj.Count; i++)
            {
                if (svi_sj[i].Id == int.Parse(id_sj))
                {
                    svi_sj[i] = sj_izmena;
                }
            }

            Baza.UpdateSJ(svi_sj);

            return RedirectToAction("SmestajneJedinice");

        }

        public ActionResult PretragaSJ(string donjagranicaP, string gornjagranicaP, string cena, string kucniLjubimci)
        {
            List<SmestajnaJedinica> svi_sj = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            List<SmestajnaJedinica> svi_sj_neobrisani = new List<SmestajnaJedinica>();
            svi_sj.Where(k => k.Obrisana == false).ToList().ForEach(k => svi_sj_neobrisani.Add(k));

            List<SmestajnaJedinica> search_sj = new List<SmestajnaJedinica>();

            search_sj = svi_sj_neobrisani.ConvertAll(a => new SmestajnaJedinica(a.Id,a.DozvoljenoGostiju,a.KucniLjubimci,a.Cena,a.Obrisana));

            if (donjagranicaP == "" && gornjagranicaP == "" && cena == "" && kucniLjubimci == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";

              
                ViewBag.SmestajJ = svi_sj_neobrisani;
                HttpRuntime.Cache["smestajJ_kes"] = svi_sj_neobrisani;
                return View("SmestajneJedinice");

            }

            if (kucniLjubimci != "")
            {
                search_sj.RemoveAll(item => !item.KucniLjubimci.ToString().ToLower().Equals(kucniLjubimci.ToLower()));
            }

            if (donjagranicaP != "")
            {
                try
                {
                    double.Parse(donjagranicaP);
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Morate uneti broj!";
                    
                    ViewBag.SmestajJ = svi_sj;
                    return View("SmestajneJedinice");
                }

                int granica_od = int.Parse(donjagranicaP);
                search_sj.RemoveAll(m => m.DozvoljenoGostiju < granica_od);

            }

            if (gornjagranicaP != "")
            {
                try
                {
                    double.Parse(gornjagranicaP);
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Morate uneti broj!";
                    
                    ViewBag.SmestajJ = svi_sj;
                    return View("SmestajneJedinice");
                }


                int granica_do = int.Parse(gornjagranicaP);
                search_sj.RemoveAll(m => m.DozvoljenoGostiju > granica_do);

            }


            if (cena != "")
            {
                try
                {
                    double.Parse(cena);
                }
                catch (Exception e)
                {
                    ViewBag.Message = "Morate uneti broj!";
                    ViewBag.SmestajJ = svi_sj;
                    return View("SmestajneJedinice");
                }


                int granica_do = int.Parse(cena);
                search_sj.RemoveAll(m => m.Cena > granica_do);

            }

            HttpRuntime.Cache["smestajJ_kes"] = search_sj;

            ViewBag.SmestajJ = search_sj;
            return View("SmestajneJedinice");
        }

        public ActionResult DodavanjeNoveSJ()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
           
            return View();

        }

        public ActionResult SortirajBrGostijuUp()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderBy(c => c.DozvoljenoGostiju).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJedinice");

        }

        public ActionResult SortirajBrGostijuDown()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderByDescending(c => c.DozvoljenoGostiju).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJedinice");

        }


        public ActionResult SortirajCenaUp()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderBy(c => c.Cena).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJedinice");

        }

        public ActionResult SortirajCenaDown()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderByDescending(c => c.Cena).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJedinice");

        }


        [HttpPost]
        public ActionResult DodajNovuSJ(string dozvoljenoGostiju, string kucniLjubimci, string cena)
        {

            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            SmestajnaJedinica sj = new SmestajnaJedinica();
            List<SmestajnaJedinica> sve_smestajnaJedinicas = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            if (dozvoljenoGostiju == "" || kucniLjubimci == "" || cena == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                return View("DodavanjeNoveSJ");

            }

            try
            {
                int.Parse(dozvoljenoGostiju);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeNoveSJ");

            }

            if (int.Parse(dozvoljenoGostiju) < 0)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeNoveSJ");
            }

            try
            {
                int.Parse(cena);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Cena mora biti pozitivna";
                return View("DodavanjeNoveSJ");

            }

            if (int.Parse(cena) < 0)
            {
                ViewBag.Message = "Cena mora biti pozitivna";
                return View("DodavanjeNoveSJ");
            }


            int id = Baza.ProcitajPoslednjiIDSmestajJ();
            sj.Id = id + 1;
            sj.DozvoljenoGostiju = int.Parse(dozvoljenoGostiju);
            sj.KucniLjubimci = bool.Parse(kucniLjubimci);
            sj.Cena = int.Parse(cena);
            sj.Obrisana = false;
            sve_smestajnaJedinicas.Add(sj);
            Baza.UpisiSmestajnuJedinicu(sj);


            return RedirectToAction("SmestajneJedinice");


        }
        #endregion


        #region SmestajFun

        public ActionResult Smestaji()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Smestaj> smestajs = (List<Smestaj>)HttpContext.Application["smestaj"];
            List<Smestaj> neobrisane = new List<Smestaj>();

            smestajs.Where(k => k.Obrisan == false).ToList().ForEach(k => neobrisane.Add(k));
            ViewBag.Smestaji = neobrisane;

            HttpRuntime.Cache["smestaj_kes"] = neobrisane;

            return View();
        }

        [HttpPost]
        public ActionResult ObrisiS(string id_s)
        {

            List<Smestaj> smestaj = (List<Smestaj>)HttpContext.Application["smestaj"];
            for (int i = 0; i < smestaj.Count; i++)
            {
                if (smestaj[i].Id == int.Parse(id_s))
                {
                    smestaj[i].Obrisan = true;
                }
            }

            Baza.UpdateS(smestaj);

            return RedirectToAction("Smestaji");
        }

        public ActionResult IzmenaS(string id)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Smestaj> smestajs = (List<Smestaj>)HttpContext.Cache["smestaj_kes"];
            ViewBag.smestaj = smestajs.Find(item => item.Id == int.Parse(id));
            //List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            //ViewBag.Smestaj = smestaji;

            return View();
        }


        [HttpPost]
        public ActionResult IzmeniS(string id_s, string naziv, string brojZvezdica, string tipSmestaja,string bazen,string spaCentar,string saInv,string wiFi)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }


            List<Smestaj> svi_sj = (List<Smestaj>)HttpContext.Application["smestaj"];
            Smestaj sj_izmena_original = svi_sj.Find(item => item.Id == int.Parse(id_s));
            List<Smestaj> sj_kopija_izmene = new List<Smestaj>() { new Smestaj() };
            List<Smestaj> sj_izmena_original_lista = new List<Smestaj>() { sj_izmena_original };
            sj_kopija_izmene = sj_izmena_original_lista.ConvertAll(a =>  new Smestaj(a.Id, a.TipSmestaja, a.Naziv, a.BrojZvezdica, a.Bazen, a.SpaCentar, a.ZaOsobeInv, a.WiFi,a.Obrisan));
            Smestaj sj_izmena = sj_kopija_izmene[0];
            ViewBag.smestaj = svi_sj.Find(item => item.Id == int.Parse(id_s));

            //List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            //ViewBag.Smestaj = smestaji;

            if (naziv == "" || brojZvezdica == "" || tipSmestaja == "" || bazen == "" || spaCentar == "" || saInv == "" || wiFi == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                return View("IzmenaS");

            }


            try
            {
                int.Parse(brojZvezdica);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("IzmenaS");

            }

            if (int.Parse(brojZvezdica) < 0)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("IzmenaS");
            }



            //int id = Baza.ProcitajPoslednjiIDSmestajJ();
            sj_izmena.Id = int.Parse(id_s);
            sj_izmena.BrojZvezdica = int.Parse(brojZvezdica);
            sj_izmena.Naziv = naziv;
            sj_izmena.Bazen = bool.Parse(bazen);
            sj_izmena.SpaCentar = bool.Parse(spaCentar);
            sj_izmena.WiFi = bool.Parse(wiFi);
            sj_izmena.ZaOsobeInv = bool.Parse(saInv);
            sj_izmena.TipSmestaja = (TipSmestaja)Enum.Parse(typeof(TipSmestaja), tipSmestaja);



            for (int i = 0; i < svi_sj.Count; i++)
            {
                if (svi_sj[i].Id == int.Parse(id_s))
                {
                    svi_sj[i] = sj_izmena;
                }
            }

            Baza.UpdateS(svi_sj);

            return RedirectToAction("Smestaji");

        }

        public ActionResult SortirajNazivSUp()
        {
            List<Smestaj> smestajs = (List<Smestaj>)HttpRuntime.Cache["smestaj_kes"];

            smestajs = smestajs.OrderBy(c => c.Naziv).ToList();

            ViewBag.Smestaji = smestajs;
            return View("Smestaji");

        }

        public ActionResult SortirajNazivSDown()
        {
            List<Smestaj> smestajs = (List<Smestaj>)HttpRuntime.Cache["smestaj_kes"];

            smestajs = smestajs.OrderByDescending(c => c.Naziv).ToList();

            ViewBag.Smestaji = smestajs;
            return View("Smestaji");

        }


        public ActionResult PretragaS(string tip, string naziv, string spaCentar, string bazen,string saInv,string wiFi)
        {
            List<Smestaj> svi_s = (List<Smestaj>)HttpContext.Application["smestaj"];
            List<Smestaj> svi_s_neobrisani = new List<Smestaj>();
            svi_s.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_s_neobrisani.Add(k));

            List<Smestaj> search_ss = new List<Smestaj>();

            search_ss = svi_s_neobrisani.ConvertAll(a => new Smestaj(a.Id, a.TipSmestaja, a.Naziv, a.BrojZvezdica,a.Bazen,a.SpaCentar,a.ZaOsobeInv,a.WiFi,a.Obrisan));

            if (tip == "" && naziv == "" && bazen == "" && spaCentar == "" && saInv == "" && wiFi == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";


                ViewBag.Smestaji = svi_s_neobrisani;
                HttpRuntime.Cache["smestaj_kes"] = svi_s_neobrisani;
                return View("Smestaji");

            }

            if (bazen != "")
            {
                search_ss.RemoveAll(item => !item.Bazen.ToString().ToLower().Equals(bazen.ToLower()));
            }

            if (spaCentar != "")
            {
                search_ss.RemoveAll(item => !item.SpaCentar.ToString().ToLower().Equals(spaCentar.ToLower()));
            }

            if (saInv != "")
            {
                search_ss.RemoveAll(item => !item.ZaOsobeInv.ToString().ToLower().Equals(saInv.ToLower()));
            }

            if (wiFi != "")
            {
                search_ss.RemoveAll(item => !item.WiFi.ToString().ToLower().Equals(wiFi.ToLower()));
            }


            if (naziv != "")
            {
                search_ss.RemoveAll(item => !item.Naziv.ToLower().Contains(naziv.ToLower()));
            }

            if (tip != "")
            {
                search_ss.RemoveAll(item => !item.TipSmestaja.ToString().Equals(tip));
            }


            HttpRuntime.Cache["smestaj_kes"] = search_ss;

            ViewBag.Smestaji = search_ss;
            return View("Smestaji");
        }


        public ActionResult DodavanjeSmestaja()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            return View();

        }

        [HttpPost]
        public ActionResult DodajNoviS(string naziv,string brojZvezdica,string tipSmestaja, string bazen, string spaCentar,string saInv, string wiFi, string dozvoljenoGostiju, string kucniLjubimci, string cena)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            SmestajnaJedinica sj = new SmestajnaJedinica();
            Smestaj s = new Smestaj();
            List<SmestajnaJedinica> sve_smestajnaJedinicas = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            List<Smestaj> smestajs = (List<Smestaj>)HttpContext.Application["smestaj"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            if (dozvoljenoGostiju == "" || kucniLjubimci == "" || cena == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                return View("DodavanjeSmestaja");

            }

            if (naziv == "" || brojZvezdica == "" || tipSmestaja == "" || bazen =="" || spaCentar=="" || saInv=="" || wiFi=="")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                return View("DodavanjeSmestaja");

            }


            try
            {
                int.Parse(brojZvezdica);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeSmestaja");

            }

            if (int.Parse(brojZvezdica) < 0)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeSmestaja");
            }

            try
            {
                int.Parse(dozvoljenoGostiju);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeSmestaja");

            }

            if (int.Parse(dozvoljenoGostiju) < 0)
            {
                ViewBag.Message = "Dozvoljeni broj gostiju ne moze biti negativan broj!";
                return View("DodavanjeSmestaja");
            }

            try
            {
                int.Parse(cena);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Cena mora biti pozitivna";
                return View("DodavanjeSmestaja");

            }

            if (int.Parse(cena) < 0)
            {
                ViewBag.Message = "Cena mora biti pozitivna";
                return View("DodavanjeSmestaja");
            }


            int id2 = Baza.ProcitajPoslednjiSmestajId();
            s.Id = id2 + 1;
            s.Naziv = naziv;
            s.BrojZvezdica = int.Parse(brojZvezdica);
            s.TipSmestaja= (TipSmestaja)Enum.Parse(typeof(TipSmestaja), tipSmestaja);
            s.Bazen = bool.Parse(bazen);
            s.ZaOsobeInv = bool.Parse(saInv);
            s.WiFi = bool.Parse(wiFi);
            s.SpaCentar = bool.Parse(spaCentar);
            s.Obrisan = false;
            smestajs.Add(s);
            Baza.UpisiSmestaj(s);

            int id = Baza.ProcitajPoslednjiIDSmestajJ();
            sj.Id = id + 1;
            sj.DozvoljenoGostiju = int.Parse(dozvoljenoGostiju);
            sj.KucniLjubimci = bool.Parse(kucniLjubimci);
            sj.Cena = int.Parse(cena);
            sj.Obrisana = false;
            sve_smestajnaJedinicas.Add(sj);
            Baza.UpisiSmestajnuJedinicu(sj);



            return RedirectToAction("Smestaji");
        }

        #endregion

        #region AranzmanFun

        public ActionResult Aranzmani()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Aranzman> aranzmeai = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> aranzmani_trenutnog_menadzera = new List<Aranzman>();

            foreach (Aranzman a in aranzmeai)
            {
                if (a.Menadzer.KorisnickoIme.Equals(menadzer.KorisnickoIme) && a.Obrisan == false)
                {
                    aranzmani_trenutnog_menadzera.Add(a);
                }
            }

            ViewBag.aranzmani = aranzmani_trenutnog_menadzera;

            HttpRuntime.Cache["aranzman_kes"] = aranzmani_trenutnog_menadzera;
            return View();
        }

        [HttpPost]
        public ActionResult ObrisiA(string id_obrisi)   
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            

        

            for (int i = 0; i < svi_aranzmani.Count; i++)
            {
                if (svi_aranzmani[i].Id == int.Parse(id_obrisi))
                {
                    svi_aranzmani[i].Obrisan = true;
                }
            }
           
            Baza.UpdateAranzman(svi_aranzmani);

            return RedirectToAction("Aranzmani");

        }


        public ActionResult DodavanjeAranzmana()
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            List<Smestaj> neobrisane = new List<Smestaj>();

            smestaji.Where(k => k.Obrisan == false).ToList().ForEach(k => neobrisane.Add(k));
            ViewBag.Smestaj = neobrisane;


            return View();

        }
        [HttpPost]   
        public ActionResult DodajNoviA(string naziv, string tipAranzmana, string tipPrevoza, string lokacija, string datumPocetkaPutovanja, string datumZavrsetkaPutovanja, string adresa, string gsirina, string gduzina, string vreme,string brputnika,string opis,string program,HttpPostedFileBase poster,string smestaj)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            Aranzman add_aranzman = new Aranzman();
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            ViewBag.Smestaj = smestaji;

            if (naziv == "" || tipAranzmana == "" || lokacija == "" || datumPocetkaPutovanja == "" || datumZavrsetkaPutovanja == "" || adresa == "" || gsirina == "" || gduzina == "" || vreme == "" || brputnika == "" || opis == "" || program == "" || smestaj == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");

            }

            if (poster == null)
            {
                ViewBag.Message = "Morate odabrati poster!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }

            if (adresa.Split(',').Count() != 3)
            {
                ViewBag.Message = "Morate uneti adresu odrzavanja u navedenom formatu!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }

            try
            {
                int.Parse(brputnika);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Broj putnika mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");

            }

            if (int.Parse(brputnika) < 0)
            {
                ViewBag.Message = "Broj putnika mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }


            if (double.Parse(gsirina) < 0)
            {
                ViewBag.Message = "Geografska sirina mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }

      

            if (double.Parse(gduzina) < 0)
            {
                ViewBag.Message = "Geografska duzina mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }


           

            try
            {
                if (poster.ContentLength > 0)  
                {
                    string ime_slike = poster.FileName;
                    string putanja_Ka_folderu_Photos = Path.Combine(Server.MapPath("~/Photos/"), ime_slike); 

                    poster.SaveAs(putanja_Ka_folderu_Photos); 

                    UploadedFile gotov_poster = new UploadedFile(ime_slike, putanja_Ka_folderu_Photos);
                    add_aranzman.Poster = gotov_poster;

                }

            }
            catch
            {
                ViewBag.Poruka = "Dodavanje postera je neuspesno!";
                ViewBag.Smestaj = smestaji;
                return View("DodavanjeAranzmana");
            }

            int id = Baza.ProcitajAranzmaneZadnjiId();
            add_aranzman.Id = id + 1;
            add_aranzman.Naziv = naziv;
            add_aranzman.TipAranzmana = (TipAranzmana)Enum.Parse(typeof(TipAranzmana), tipAranzmana);
            add_aranzman.TipPrevoza = (TipPrevoza)Enum.Parse(typeof(TipPrevoza), tipPrevoza);
            add_aranzman.Lokacija = lokacija;
            add_aranzman.DatumPocetkaPutovanja = Convert.ToDateTime(datumPocetkaPutovanja);
            add_aranzman.DatumZavrsetkaPutovanja = Convert.ToDateTime(datumZavrsetkaPutovanja);
            add_aranzman.MestoNalazenja.Adresa = adresa;
            add_aranzman.MestoNalazenja.GeografskaSirina = gsirina;
            add_aranzman.MestoNalazenja.GeografskaDuzina = gduzina;
            add_aranzman.VremeNalazenja = Convert.ToDateTime(vreme);
            add_aranzman.MaxBrojPutnika = int.Parse(brputnika);
            add_aranzman.Opis = opis;
            add_aranzman.Program = program;
            add_aranzman.Menadzer = menadzer;
            add_aranzman.IdSmestaja = int.Parse(smestaj);
         
            svi_aranzmani.Add(add_aranzman);
            Baza.UpisiAranzman(add_aranzman);

            return RedirectToAction("Aranzmani");

        }

        public ActionResult IzmenaA(string id)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            List<Aranzman> aranzmani = (List<Aranzman>)HttpContext.Cache["aranzman_kes"];
            ViewBag.aranzmani = aranzmani.Find(item => item.Id == int.Parse(id));
            List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            List<Smestaj> neobrisane = new List<Smestaj>();

            smestaji.Where(k => k.Obrisan == false).ToList().ForEach(k => neobrisane.Add(k));
            ViewBag.Smestaj = neobrisane;

            return View();
        }

        [HttpPost]
        public ActionResult IzmeniA(string id_aranzmana,string naziv, string tipAranzmana, string tipPrevoza, string lokacija, string datumPocetkaPutovanja, string datumZavrsetkaPutovanja, string adresa, string gsirina, string gduzina, string vreme, string brputnika, string opis, string program, HttpPostedFileBase poster, string smestaj)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }

            
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            Aranzman aranzman_izmena_original = svi_aranzmani.Find(item => item.Id == int.Parse(id_aranzmana));
            List<Aranzman> aranzman_kopija_izmene = new List<Aranzman>() { new Aranzman() };
            List<Aranzman> aranznman_izmena_original_lista = new List<Aranzman>() { aranzman_izmena_original };
            aranzman_kopija_izmene = aranznman_izmena_original_lista.ConvertAll(a => new Aranzman(a.Id, a.Naziv,a.TipAranzmana,a.TipPrevoza, a.Lokacija, a.DatumPocetkaPutovanja, a.DatumZavrsetkaPutovanja, a.MestoNalazenja, a.VremeNalazenja, a.MaxBrojPutnika, a.Opis, a.Program, a.Poster, a.IdSmestaja,a.Obrisan,a.Menadzer));
            Aranzman aranzman_izmena = aranzman_kopija_izmene[0];
            ViewBag.aranzmani = svi_aranzmani.Find(item => item.Id == int.Parse(id_aranzmana));

            List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            ViewBag.Smestaj = smestaji;

            if (naziv == "" || tipAranzmana == "" || lokacija == "" || datumPocetkaPutovanja == "" || datumZavrsetkaPutovanja == "" || adresa == "" || gsirina == "" || gduzina == "" || vreme == "" || brputnika == "" || opis == "" || program == "" || smestaj == "")
            {
                ViewBag.Message = "Sva polja moraju biti popunjena!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");

            }

            if (poster == null)
            {
                ViewBag.Message = "Morate odabrati poster!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }

            if (adresa.Split(',').Count() != 3)
            {
                ViewBag.Message = "Morate uneti adresu odrzavanja u navedenom formatu!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }

            try
            {
                int.Parse(brputnika);
            }
            catch (Exception e)
            {
                ViewBag.Message = "Broj putnika mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");

            }

            if (int.Parse(brputnika) < 0)
            {
                ViewBag.Message = "Broj putnika mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }


            if (double.Parse(gsirina) < 0)
            {
                ViewBag.Message = "Geografska sirina mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }



            if (double.Parse(gduzina) < 0)
            {
                ViewBag.Message = "Geografska duzina mora biti celobrojna nenegativna vrednost!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }




            try
            {
                if (poster.ContentLength > 0)
                {
                    string ime_slike = poster.FileName;
                    string putanja_Ka_folderu_Photos = Path.Combine(Server.MapPath("~/Photos/"), ime_slike);

                    poster.SaveAs(putanja_Ka_folderu_Photos);

                    UploadedFile gotov_poster = new UploadedFile(ime_slike, putanja_Ka_folderu_Photos);
                    aranzman_izmena.Poster = gotov_poster;

                }

            }
            catch
            {
                ViewBag.Poruka = "Dodavanje postera je neuspesno!";
                ViewBag.Smestaj = smestaji;
                return View("IzmenaA");
            }

            int id = Baza.ProcitajAranzmaneZadnjiId();
            aranzman_izmena.Id = int.Parse(id_aranzmana);
            aranzman_izmena.Naziv = naziv;
            aranzman_izmena.TipAranzmana = (TipAranzmana)Enum.Parse(typeof(TipAranzmana), tipAranzmana);
            aranzman_izmena.TipPrevoza = (TipPrevoza)Enum.Parse(typeof(TipPrevoza), tipPrevoza);
            aranzman_izmena.Lokacija = lokacija;
            aranzman_izmena.DatumPocetkaPutovanja = Convert.ToDateTime(datumPocetkaPutovanja);
            aranzman_izmena.DatumZavrsetkaPutovanja = Convert.ToDateTime(datumZavrsetkaPutovanja);
            aranzman_izmena.MestoNalazenja.Adresa = adresa;
            aranzman_izmena.MestoNalazenja.GeografskaSirina = gsirina;
            aranzman_izmena.MestoNalazenja.GeografskaDuzina = gduzina;
            aranzman_izmena.VremeNalazenja = Convert.ToDateTime(vreme);
            aranzman_izmena.MaxBrojPutnika = int.Parse(brputnika);
            aranzman_izmena.Opis = opis;
            aranzman_izmena.Program = program;
            aranzman_izmena.Menadzer = menadzer;
            aranzman_izmena.IdSmestaja = int.Parse(smestaj);


            for (int i = 0; i < svi_aranzmani.Count; i++)
            {
                if (svi_aranzmani[i].Id == int.Parse(id_aranzmana))
                {
                    svi_aranzmani[i] = aranzman_izmena;
                }
            }
           
            Baza.UpdateAranzman(svi_aranzmani);

            return RedirectToAction("Aranzmani");

        }
#endregion


        #region Sort&Search Aranzmani
        public ActionResult SortirajNazivUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.Naziv).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }

        public ActionResult SortirajNazivDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.Naziv).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }


        public ActionResult SortirajDatumPDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.DatumPocetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }

        public ActionResult SortirajDatumPUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.DatumPocetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }


        public ActionResult SortirajDatumZDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.DatumZavrsetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }

        public ActionResult SortirajDatumZUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.DatumZavrsetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Aranzmani");

        }
        

        public ActionResult Pretraga(string donjagranicaP, string gornjagranicaP, string donjagranicaZ, string gornjagranicaZ, string tipPrevoza, string tipAranzmana, string naziv)
        {
            Korisnik menadzer = (Korisnik)Session["smenadzer"];
            if (menadzer == null)
            {
                return RedirectToAction("Login", "Registracija");
            }
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> svi_aranzmani_neobrisani = new List<Aranzman>();
            List<Aranzman> aranzmani_trenutnog_menadzera = new List<Aranzman>();
            svi_aranzmani.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_aranzmani_neobrisani.Add(k));

            List<Aranzman> search_aranzmani = new List<Aranzman>();

            search_aranzmani = svi_aranzmani_neobrisani.ConvertAll(a => new Aranzman(a.Id, a.Naziv, a.TipAranzmana, a.TipPrevoza, a.Lokacija, a.DatumPocetkaPutovanja, a.DatumZavrsetkaPutovanja, a.MestoNalazenja, a.VremeNalazenja, a.MaxBrojPutnika, a.Opis, a.Program, a.Poster, a.IdSmestaja, a.Obrisan, a.Menadzer));

            if (donjagranicaP == "" && gornjagranicaP == "" && gornjagranicaZ == "" && donjagranicaZ == "" && tipPrevoza == "" && tipAranzmana == "" && naziv == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";
                

                foreach (Aranzman a in svi_aranzmani_neobrisani)
                {
                    if (a.Menadzer.KorisnickoIme.Equals(menadzer.KorisnickoIme) && a.Obrisan == false)
                    {
                        aranzmani_trenutnog_menadzera.Add(a);
                    }
                }

                aranzmani_trenutnog_menadzera = aranzmani_trenutnog_menadzera.OrderBy(c => c.DatumPocetkaPutovanja).ToList();
                ViewBag.aranzmani = aranzmani_trenutnog_menadzera;
                HttpRuntime.Cache["aranzman_kes"] = aranzmani_trenutnog_menadzera;
                return View("Aranzmani");

            }

            if (naziv != "")
            {
                search_aranzmani.RemoveAll(item => !item.Naziv.ToLower().Contains(naziv.ToLower()));
            }

            if (tipPrevoza != "")
            {
                search_aranzmani.RemoveAll(item => !item.TipPrevoza.ToString().Equals(tipPrevoza));
            }

            if (tipAranzmana != "")
            {
                search_aranzmani.RemoveAll(item => !item.TipAranzmana.ToString().Equals(tipAranzmana));
            }


            if (donjagranicaP != "")
            {
                DateTime donjagranica = Convert.ToDateTime(donjagranicaP);
                search_aranzmani.RemoveAll(a => DateTime.Compare(a.DatumPocetkaPutovanja, donjagranica) < 0);

            }

            if (gornjagranicaP != "")
            {
                DateTime gornjagranica = Convert.ToDateTime(gornjagranicaP);
                search_aranzmani.RemoveAll(a => DateTime.Compare(gornjagranica, a.DatumPocetkaPutovanja) < 0);

            }


            if (donjagranicaZ != "")
            {
                DateTime donjagranica = Convert.ToDateTime(donjagranicaZ);
                search_aranzmani.RemoveAll(a => DateTime.Compare(a.DatumZavrsetkaPutovanja, donjagranica) < 0);

            }

            if (gornjagranicaZ != "")
            {
                DateTime gornjagranica = Convert.ToDateTime(gornjagranicaZ);
                search_aranzmani.RemoveAll(a => DateTime.Compare(gornjagranica, a.DatumZavrsetkaPutovanja) < 0);

            }


            HttpRuntime.Cache["aranzman_kes"] = search_aranzmani;

            ViewBag.aranzmani = search_aranzmani;
            return View("Aranzmani");
        }
        #endregion
    }
}