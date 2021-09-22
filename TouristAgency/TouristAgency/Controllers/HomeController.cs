using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
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
            

            ViewBag.aranzmani = aranzmani_buduce;

            HttpRuntime.Cache["aranzman_kes"] = aranzmani_buduce;

            return View();
        }

       
        #region Sortiranja
        public ActionResult SortirajNazivUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.Naziv).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }

        public ActionResult SortirajNazivDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.Naziv).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }


        public ActionResult SortirajDatumPDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.DatumPocetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }

        public ActionResult SortirajDatumPUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.DatumPocetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }


        public ActionResult SortirajDatumZDown()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderByDescending(c => c.DatumZavrsetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }

        public ActionResult SortirajDatumZUp()
        {
            List<Aranzman> aranzmani = (List<Aranzman>)HttpRuntime.Cache["aranzman_kes"];

            aranzmani = aranzmani.OrderBy(c => c.DatumZavrsetkaPutovanja).ToList();

            ViewBag.aranzmani = aranzmani;
            return View("Index");

        }



        public ActionResult Pretraga(string donjagranicaP, string gornjagranicaP, string donjagranicaZ, string gornjagranicaZ, string tipPrevoza, string tipAranzmana, string naziv)
        {
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> svi_aranzmani_neobrisani = new List<Aranzman>();
            List<Aranzman> aranzmani_buduce = new List<Aranzman>();
            List<Aranzman> aranzmani_prosli = new List<Aranzman>();
            svi_aranzmani.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_aranzmani_neobrisani.Add(k));

            List<Aranzman> search_aranzmani = new List<Aranzman>();

            search_aranzmani = svi_aranzmani_neobrisani.ConvertAll(a => new Aranzman(a.Id, a.Naziv, a.TipAranzmana, a.TipPrevoza, a.Lokacija, a.DatumPocetkaPutovanja, a.DatumZavrsetkaPutovanja, a.MestoNalazenja, a.VremeNalazenja, a.MaxBrojPutnika, a.Opis, a.Program, a.Poster, a.IdSmestaja, a.Obrisan, a.Menadzer));

            if (donjagranicaP == "" && gornjagranicaP == "" && gornjagranicaZ == "" && donjagranicaZ == "" && tipPrevoza == "" && tipAranzmana == "" && naziv == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";

                foreach (Aranzman man in svi_aranzmani_neobrisani)
                {
                    if (DateTime.Compare(man.DatumPocetkaPutovanja, DateTime.Now) > 0)
                    {
                        aranzmani_buduce.Add(man);
                    }
                    else
                    {
                        aranzmani_prosli.Add(man);
                    }
                }
                aranzmani_buduce = aranzmani_buduce.OrderBy(c => c.DatumPocetkaPutovanja).ToList();
                ViewBag.aranzmani = aranzmani_buduce;
                HttpRuntime.Cache["aranzman_kes"] = aranzmani_buduce;
                return View("Index");

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
            return View("Index");
        }
        #endregion



        [HttpPost]
        public ActionResult About(string id)
        {
            List<Aranzman> aranzmeni = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Smestaj> smestaji = (List<Smestaj>)HttpContext.Application["smestaj"];
            List<Komentar> svi_komentari = (List<Komentar>)HttpContext.Application["komentar"];
            List<Komentar> komentari = new List<Komentar>();
            Aranzman aranz = new Aranzman();
            Smestaj s = new Smestaj();

            foreach (Aranzman a in aranzmeni)
            {
                if (a.Id == int.Parse(id))
                {
                    aranz = a;
                }
            }

            foreach (var item in smestaji)
            {
                if (item.Id == aranz.IdSmestaja)
                {
                    s = item;
                }

            }

                foreach (Komentar k in svi_komentari)   
                {
                    if (k.Aranzman.Id == int.Parse(id) && k.StatusKomentara.ToString().Equals("ODOBREN"))
                    {
                        komentari.Add(k);
                    }
                }

                

            


            ViewBag.Aranzman = aranz;
            ViewBag.Smestaj = s;
            ViewBag.Komentari = komentari;

            return View();
        }

        #region SmestajneJediniceNeReg
        public ActionResult SmestajneJediniceNeReg()
        {
            List<SmestajnaJedinica> smestajJ = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            List<SmestajnaJedinica> neobrisane = new List<SmestajnaJedinica>();

            smestajJ.Where(k => k.Obrisana == false).ToList().ForEach(k => neobrisane.Add(k));
            ViewBag.SmestajJ = neobrisane;

            HttpRuntime.Cache["smestajJ_kes"] = smestajJ;
            return View();
        }


        public ActionResult PretragaSJ(string donjagranicaP, string gornjagranicaP, string cena, string kucniLjubimci)
        {
            List<SmestajnaJedinica> svi_sj = (List<SmestajnaJedinica>)HttpContext.Application["smestajJ"];
            List<SmestajnaJedinica> svi_sj_neobrisani = new List<SmestajnaJedinica>();
            svi_sj.Where(k => k.Obrisana == false).ToList().ForEach(k => svi_sj_neobrisani.Add(k));

            List<SmestajnaJedinica> search_sj = new List<SmestajnaJedinica>();

            search_sj = svi_sj_neobrisani.ConvertAll(a => new SmestajnaJedinica(a.Id, a.DozvoljenoGostiju, a.KucniLjubimci, a.Cena,a.Obrisana));

            if (donjagranicaP == "" && gornjagranicaP == "" && cena == "" && kucniLjubimci == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";


                ViewBag.SmestajJ = svi_sj;
                HttpRuntime.Cache["smestajJ_kes"] = svi_sj;
                return View("SmestajneJediniceNeReg");

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
                    return View("SmestajneJediniceNeReg");
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
                    return View("SmestajneJediniceNeReg");
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
                    return View("SmestajneJediniceNeReg");
                }


                int granica_do = int.Parse(cena);
                search_sj.RemoveAll(m => m.Cena > granica_do);

            }

            HttpRuntime.Cache["smestajJ_kes"] = search_sj;

            ViewBag.SmestajJ = search_sj;
            return View("SmestajneJediniceNeReg");
        }

        public ActionResult SortirajBrGostijuUp()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderBy(c => c.DozvoljenoGostiju).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJediniceNeReg");

        }

        public ActionResult SortirajBrGostijuDown()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderByDescending(c => c.DozvoljenoGostiju).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJediniceNeReg");

        }


        public ActionResult SortirajCenaUp()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderBy(c => c.Cena).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJediniceNeReg");

        }

        public ActionResult SortirajCenaDown()
        {
            List<SmestajnaJedinica> smestajnaJedinicas = (List<SmestajnaJedinica>)HttpRuntime.Cache["smestajJ_kes"];

            smestajnaJedinicas = smestajnaJedinicas.OrderByDescending(c => c.Cena).ToList();

            ViewBag.SmestajJ = smestajnaJedinicas;
            return View("SmestajneJediniceNeReg");

        }
        #endregion
    }
}