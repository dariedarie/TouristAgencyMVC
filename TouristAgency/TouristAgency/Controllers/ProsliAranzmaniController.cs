using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TouristAgency.Models;

namespace TouristAgency.Controllers
{
    public class ProsliAranzmaniController : Controller
    {
        // GET: ProsliAranzmani
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
            //aranzmani_buduce = aranzmani_buduce.OrderBy(a => a.DatumPocetkaPutovanja).ToList();
            aranzmani_prosle = aranzmani_prosle.OrderBy(m => m.DatumPocetkaPutovanja).ToList();  
            //foreach (Aranzman man in aranzmani_prosle)
            //{
            //    aranzmani_buduce.Add(man);   
            //}

            ViewBag.aranzmani = aranzmani_prosle;

            HttpRuntime.Cache["aranzman_kes"] = aranzmani_prosle;

            return View();
        }
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
        #endregion

        public ActionResult Pretraga(string donjagranicaP, string gornjagranicaP, string donjagranicaZ, string gornjagranicaZ, string tipPrevoza, string tipAranzmana, string naziv)
        {
            List<Aranzman> svi_aranzmani = (List<Aranzman>)HttpContext.Application["aranzman"];
            List<Aranzman> svi_aranzmani_neobrisani = new List<Aranzman>();
            svi_aranzmani.Where(k => k.Obrisan == false).ToList().ForEach(k => svi_aranzmani_neobrisani.Add(k));

            List<Aranzman> search_aranzmani = new List<Aranzman>();

           // search_aranzmani = svi_aranzmani_neobrisani.ConvertAll(a => new Aranzman(a.Id, a.Naziv, a.TipAranzmana, a.TipPrevoza, a.TipLokacije, a.DatumPocetkaPutovanja, a.DatumZavrsetkaPutovanja, a.MestoNalazenja, a.VremeNalazenja, a.MaxBrojPutnika, a.Opis, a.Program, a.Poster, a.Smestaj, a.Obrisan));

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
            
            aranzmani_prosle = aranzmani_prosle.OrderBy(m => m.DatumPocetkaPutovanja).ToList();
            search_aranzmani = aranzmani_prosle;


            if (donjagranicaP == "" && gornjagranicaP == "" && gornjagranicaZ == "" && donjagranicaZ == "" && tipPrevoza == "" && tipAranzmana == "" && naziv == "")
            {
                ViewBag.Message = "Morate uneti barem jedan parametar pretrage!";

                //svi_aranzmani_neobrisani = svi_aranzmani_neobrisani.OrderBy(c => c.DatumPocetkaPutovanja).ToList();
                ViewBag.aranzmani = aranzmani_prosle;
                HttpRuntime.Cache["aranzman_kes"] = aranzmani_prosle;
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

    }
}