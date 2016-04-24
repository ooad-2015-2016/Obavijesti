using KompShopMVVM.KompShop.Helper;
using KompShopMVVM.KompShop.Models;
using Prism.Windows.Validation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace KompShopMVVM.KompShop.ViewModels
{
    //primjetiti da klasa realizira INotifyPropertyChanged interfejs
    class KarticaEditViewModel : INotifyPropertyChanged
    {
        public KorpaViewModel Parent { get; set; }
        public ICommand Potvrda { get; set; }
        //Po potrebi moze se napraviti i poseban getter koji ce skratiti putanju do kartice
        public KreditnaKartica Kartica
        {
            get { return Parent.Korpa.Kupac.Kartica; }
            set { Parent.Korpa.Kupac.Kartica = value; }
        }

        public KarticaEditViewModel(KorpaViewModel p)
        {
            this.Parent = p;
            Parent.Korpa.Kupac = new Kupac();
            Kartica = new KreditnaKartica();
            Kartica.DatumIsteka = DateTime.Now;
            Kartica.DatumIsteka = Kartica.DatumIsteka.AddYears(1);
            Potvrda = new RelayCommand<object>(potvrdi, mozeLiPotvrda);
            //dodavanje eventa koji ce se pozvati kad dodje do neispravne validacije
            Kartica.ErrorsChanged += Vm_ErrorsChanged;
        }


        public bool mozeLiPotvrda(object parametar)
        {
           return true;
        }

        public void potvrdi(object parametar)
        {
            Parent.NavigationService.GoBack();
        }

        private void Vm_ErrorsChanged(object sender, System.ComponentModel.DataErrorsChangedEventArgs e)
        {
            //event koji ce se pozvati kad dodje do neispravne validacije
            //daj sve greske i pretvori ih u listu stringova da se mogu ispisati
            Erori = new ObservableCollection<string>(Kartica.Errors.Errors.Values.SelectMany(x => x).ToList());
        }

        //mora biti observableCollection i pozvati OnNotifyPropertyChanged da bi se primjetila promjena liste da bi view skontao da se mora izmjeniti
        private ObservableCollection<string> erori;
        public ObservableCollection<string> Erori { get { return erori; } set { erori = value; OnNotifyPropertyChanged("Erori"); } }

        //implementacija INotifyPropertyChanged interfejsa koji ova klasa implementira
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
