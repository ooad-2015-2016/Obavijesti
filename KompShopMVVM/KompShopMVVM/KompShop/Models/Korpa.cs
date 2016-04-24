using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KompShopMVVM.KompShop.Models
{
    class Korpa
    {
        public ObservableCollection<StavkaKorpe> Stavke { get; set; }
        public Kupac Kupac { get; set; }
        public Korpa()
        {
            Stavke = new ObservableCollection<StavkaKorpe>();
        }
    }
}
