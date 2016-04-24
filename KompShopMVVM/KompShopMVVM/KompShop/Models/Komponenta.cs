using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class Komponenta
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public int DostupnaKolicina { get; set; }
        public int Cijena { get; set; }
        public Komponenta() { }
    }
}
