using KompShopMVVM.KompShop.Helper;
using KompShopMVVM.KompShop.Models;
using KompShopMVVM.KompShop.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KompShopMVVM.KompShop.ViewModels
{
    class KorpaViewModel 
    {
        //U view Biniding Korpa.Stavke prvo trazi Korpa propertz a zatim pristupa njegovom Stavke property
        public Korpa Korpa { get; set; }
        //Servis za navigaciju koji će preći na druge forme po potrebi
        public INavigationService NavigationService { get; set; }
        //Komande koje realiziraju Binding UnosKartice i DodavanjeKomponente
        public ICommand UnosKartice { get; set; }
        public ICommand DodavanjeKomponente { get; set; }

        public KorpaViewModel()
        {
            Korpa = new Korpa();
            NavigationService = new NavigationService();
            //prvi parametar funkcija koja se pozove na klik a druga funkcija koja testira da li se treba pozvati komanda
            DodavanjeKomponente = new RelayCommand<object>(dodavanjeKomponente, mozeLiSeDodatiKomponenta);
            UnosKartice = new RelayCommand<object>(unosKreditneKartice, mozeLiUnosKreditneKartice);
        }

        public bool mozeLiSeDodatiKomponenta(object parametar)
        {
            //ovdje se moze dodati uslov ako je potrebno da se komanda ne izvrsi
            return true;
        }

        public void dodavanjeKomponente(object parametar)
        {
            //prebacuje na sljedeci view i proslijedjuje viewmodel za taj view, koji ima ovaj view (this) kao Parent
            NavigationService.Navigate(typeof(KomponentaDetailView), new KomponentaDetailViewModel(this));
        }

        public void unosKreditneKartice(object parametar)
        {
            NavigationService.Navigate(typeof(KreditnaKarticaEditView), new KarticaEditViewModel(this));
        }

        public bool mozeLiUnosKreditneKartice(object parametar)
        {
            //ovdje se moze dodati uslov ako je potrebno da se komanda ne izvrsi
            return true;
        }
    }
}
