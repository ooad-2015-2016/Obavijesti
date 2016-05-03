using ProjekatOoad.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjekatOoad.Venues.Model
{
    public class Restoran:Venue
    {
       //Transformacija iz base klase u derived klasu
       public Restoran(Venue v)
       {
            this.Naziv = v.Naziv;
            this.fourSqaureId = v.fourSqaureId;
            this.Opis = v.Opis;
            this.Slika = v.Slika;
            this.Telefon = v.Telefon;
            this.GeoSirina = v.GeoSirina;
            this.GeoDuzina = v.GeoDuzina;
            this.Rating = v.Rating/2;
            this.Adresa = v.Adresa;
        }
    }
}
