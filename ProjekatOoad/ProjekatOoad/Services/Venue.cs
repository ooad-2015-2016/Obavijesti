using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatOoad.Services
{
    public class Venue
    {
        public string fourSqaureId { get; set; }//trebati ce za sihronizaciju kasnije
        public string Naziv { get; set; }//naziv restorana
        public string Opis { get; set; }//tekst o restoranu
        public string Slika { get; set; }//slika restorana
        public string Telefon { get; set; }//broj telefona
        public double GeoSirina { get; set; }//geografska sirina i duzina gdje se nalazi restoran
        public double GeoDuzina { get; set; }
        public double Rating { get; set; }//Za like
        public string Adresa { get; set; }//Adresa restorana
        public bool Detailed { get; set; } = false;//posto poziv List ne popuni sva polja ovako
        //se biljezi da li je Venue povucen iz liste pa treba dopuniti podatke
        //Moze se gledati i da li je Rating null ali sta ako je null i na servisu?, problem
    }
}
