using KompShopMVVM.KompShop.Helper;
using KompShopMVVM.KompShop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KompShopMVVM.KompShop.ViewModels
{
    class KomponentaDetailViewModel
    {
        //cuvanje glavnog viewmodela jer u njemu se sve sadrzi
        public KorpaViewModel Parent { get; set; }
        public Katalog Katalog { get; set; }
        public StavkaKorpe Stavka { get; set; }//za trenutni odabir u combobox
        public ICommand Dodaj { get; set; }
        public KomponentaDetailViewModel(KorpaViewModel parent)
        {
            this.Parent = parent;
            Katalog = new Katalog();
            Katalog.povuciKomponenete();
            Dodaj = new RelayCommand<object>(dodaj, mozeLiDodati);
            //odabrana stavka sa default komponentom, combobox ce da mjenja Stavka.Komponenta
            Stavka = new StavkaKorpe();
            Stavka.Komponenta = Katalog.Komponente[0];
            Stavka.Kolicina = 1;
        }

        public void dodaj(object parametar)
        {
            Parent.Korpa.Stavke.Add(Stavka);
            //go back koristi navigation service da se vrati nazad
            Parent.NavigationService.GoBack();
        }
        public bool mozeLiDodati(object parametar)
        {
            return true;
        }
    }
}
