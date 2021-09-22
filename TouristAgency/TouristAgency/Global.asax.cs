using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TouristAgency.Models;

namespace TouristAgency
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            List<Aranzman> aranzmani = Baza.ProcitajAranzmane("~/App_Data/aranzmani.txt");

            string putanja = Server.MapPath("~/Photos/");  
            string[] putanja_svake_slike = Directory.GetFiles(putanja);  
            List<UploadedFile> fajlovi = new List<UploadedFile>();

            foreach (string path in putanja_svake_slike)
            {
                fajlovi.Add(new UploadedFile(Path.GetFileName(path), path));
            }

          
            foreach (Aranzman m in aranzmani)
            {
                foreach (UploadedFile fajl in fajlovi)
                {
                    if (m.Poster.Name.Equals(fajl.Name))
                    {
                        m.Poster.Putanja = fajl.Putanja;
                    }
                }
            }

            HttpContext.Current.Application["aranzman"] = aranzmani;

            List<Smestaj> smestajs = Baza.ProcitajSmestaj("~/App_Data/smestaji.txt");
            HttpContext.Current.Application["smestaj"] = smestajs;

            List<SmestajnaJedinica> smestajnaJedinicas = Baza.ProcitajSmestajneJedinice("~/App_Data/smestajnejedinice.txt");
            HttpContext.Current.Application["smestajJ"] = smestajnaJedinicas;

            List<Korisnik> administratori = Baza.ProcitajAdmina("~/App_Data/admins.txt");
            HttpContext.Current.Application["admin"] = administratori;


            List<Korisnik> turisti = Baza.ProcitajTuristu("~/App_Data/turisti.txt");
            HttpContext.Current.Application["turista"] = turisti;

            List<Korisnik> menadzeri = Baza.ProcitajMenadzera("~/App_Data/menadzeri.txt");
            HttpContext.Current.Application["menadzer"] = menadzeri;

            List<Rezervacija> rezervacije = Baza.ProcitajRezervaciju("~/App_Data/rezervacije.txt");
            HttpContext.Current.Application["rezervacija"] = rezervacije;

            List<Komentar> komentari = Baza.ProcitajKomentar("~/App_Data/komentari.txt");
            HttpContext.Current.Application["komentar"] = komentari;
        }
    }
}
