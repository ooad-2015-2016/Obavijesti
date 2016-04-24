using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class Kupac
    {
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Ulica { get; set; }
        public string Grad { get; set; }
        public string Drzava { get; set; }
        public int Broj { get; set; }
        public KreditnaKartica Kartica { get; set; }
    }
}
