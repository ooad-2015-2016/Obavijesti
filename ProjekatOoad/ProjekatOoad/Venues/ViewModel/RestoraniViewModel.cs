using ProjekatOoad.Helper;
using ProjekatOoad.Venues.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ProjekatOoad.Venues.ViewModel
{
    public class RestoraniViewModel: INotifyPropertyChanged
    {
        //cuva lat i lng u search poljima
        public SearchOpcije SearchOpcije{ get; set; }
        //Command patern za Search dugme i otvaranje zatvaranje search panela kada je prozor manji od 720px
        public ICommand SearchCommand { get; set; }
        public ICommand CloseSearchCommand { get; set; }
        public ICommand OpenSearchCommand { get; set; }
        //Data source koji koristi web servis (ovdje se ne zna da je web servis, moze biti baza ili bilo sta viewmodel ne zna o tome)
        private RestoranDataSource restoraniDS;
        //obeservable collection observer patern koristi, potrebno da se lista refresha kada se doda restoran
        //upozorenje new ObservableCollection<Restoran> se ne pika kao promjena kolekcije pa se nece refreshati ni lista
        public ObservableCollection<Restoran> Restorani { get; set; }
        //biljezi se da li je otvoren serach panel
        private bool searchMode = false;
        //Ako se klikne na dugme i promjeni search mode, treba nekako informisati binding da je doslo do promjene
        //OnNotifyPropertyChanged je standardno na silu informisanje Binding da je doslo do promjene 
        public bool SearchMode { get { return searchMode; } set { searchMode = value; OnNotifyPropertyChanged("SearchMode");} }

        public RestoranDataSource RestoraniDS
        {
            get
            {
                return restoraniDS;
            }

            set
            {
                restoraniDS = value;
            }
        }

        //da se moze pozvati update detalja restorana
        public async void updateRestoran(Restoran r,Action callback)
        {
            await restoraniDS.VenService.updateVenueDetailed(r, callback);
        }


        public RestoraniViewModel()
        {
            //postavljanje komandi
            SearchCommand = new RelayCommand<object>(obaviSearch, mozelSearch);
            //lambda funkcije iz RPR da se ne prave metode za minorne stvari
            OpenSearchCommand = new RelayCommand<object>((object parametar) => SearchMode = true, (object parametar) => true);
            CloseSearchCommand = new RelayCommand<object>((object parametar) => SearchMode = false, (object parametar) => true);
            Restorani = new ObservableCollection<Restoran>();
            RestoraniDS = new RestoranDataSource();
            SearchOpcije = new SearchOpcije();
            //defaultne opcije da ima nekih restorana
            SearchOpcije.GeoSirina = 40.7;
            SearchOpcije.GeoDuzina = -74;
            //povuci restorane i kad se obavi upit, pozovi restorani loaded
            RestoraniDS.dajRestoraneFourSquare(SearchOpcije,RestoraniLoaded);
        }

        public bool mozelSearch(object parametar)
        {
            return SearchOpcije != null;
        }

        public void obaviSearch(object parametar)
        {
            //zatvori search panel
            SearchMode = false;
            //i povuci nove restorane
            RestoraniDS.dajRestoraneFourSquare(SearchOpcije, RestoraniLoaded);
        }

        //standarnda implementacija PropertyChanged za OnNotifyPropertyChanged, uvijek isto
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }

        //kad se ucitaju novi restorani obrisi listu i dodaj sve u listu
        //ne moze new jer nece registrovati listview da je doslo do promjene
        public void RestoraniLoaded()
        {
            Restorani.Clear();
            foreach (Restoran r in RestoraniDS.Restorani)
            {
                Restorani.Add(r);
            }
        }
    }
}
