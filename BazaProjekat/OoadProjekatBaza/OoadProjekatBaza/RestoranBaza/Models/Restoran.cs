using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoadProjekatBaza.RestoranBaza.Models
{
    class Restoran
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RestoranId { get; set; }//primary key u bazi
        public string fourSqaureId { get; set; }//trebati ce za sihronizaciju kasnije
        public string Naziv { get; set; }//naziv restorana
        public string Opis { get; set; }//tekst o restoranu
        public byte[] Slika { get; set; }//slika restorana
        public string Telefon { get; set; }//broj telefona
        public float GeoSirina { get; set; }//geografska sirina i duzina gdje se nalazi restoran
        public float GeoDuzina { get; set; }
        public float Rating { get; set; }//Za like
    }
}
