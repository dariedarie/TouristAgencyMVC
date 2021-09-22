using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TouristAgency.Models
{
    public class UploadedFile
    {

        private string name;
        private string putanja;

        public string Name { get => name; set => name = value; }
        public string Putanja { get => putanja; set => putanja = value; }

        public UploadedFile(string name, string putanja)
        {
            Name = name;
            Putanja = putanja;
        }

        public UploadedFile()
        {
        }
    }
}