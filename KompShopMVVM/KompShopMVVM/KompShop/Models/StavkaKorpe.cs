using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class StavkaKorpe
    {
        public int Kolicina { get; set; }
        public Komponenta Komponenta { get; set; }
        public int Cijena { get { return Kolicina * Komponenta.Cijena; } }
    }
}
