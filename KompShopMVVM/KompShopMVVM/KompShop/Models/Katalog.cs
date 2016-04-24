using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class Katalog
    {
        public List<Komponenta> Komponente { get; set; }
        public void povuciKomponenete()
        {
            Komponente = new List<Komponenta>();
            Komponente.Add(
                new Komponenta() {
                    Id = 1,
                    Naziv = "Kuciste",
                    Cijena = 100
                }
            );
            Komponente.Add(
               new Komponenta()
               {
                   Id = 2,
                   Naziv = "Maticna",
                   Cijena = 200
               }
            );
            Komponente.Add(
               new Komponenta()
               {
                   Id = 3,
                   Naziv = "Graficka",
                   Cijena = 325
               }
            );
            Komponente.Add(
               new Komponenta()
               {
                   Id = 4,
                   Naziv = "Procesor",
                   Cijena = 420
               }
            );
            Komponente.Add(
               new Komponenta()
               {
                  Id = 5,
                  Naziv = "Hard Disk",
                  Cijena = 120
               }
            );
        }
    }
}
