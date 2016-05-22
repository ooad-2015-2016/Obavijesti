using MobileProjekat.OoadMobile.CallISMS.Helper;
using MobileProjekat.OoadMobile.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.ApplicationModel.Contacts;

namespace MobileProjekat.OoadMobile.CallISMS.ViewModels
{
    class CommunicationViewModel : INotifyPropertyChanged
    {
        //Za tekstbox static referenca
        private static string prefixMetodKomunikacije = "Metod Komunikacije:";
        //kontakt odabrani za komunikaciju
        Contact trenutniKontakt;
        //trenutni behaviour strategy patterna
        int trenutnaKomunikacija = 0;
        //lista svih ponasanja koje klasa ima i samo se rotiraju po potrebi
        List<ICommunicateBehaviour> communicateBehaviours;
        //command patern binding standardno
        public ICommand PromijeniKontakta { get; set; }
        public ICommand PromijeniKomunikaciju { get; set; }
        public ICommand Komuniciraj { get; set; }
        //ime za binding i metodkomunikacije da se prikazu u UI
        private string imeKontakta;
        public string ImeKontakta { get { return imeKontakta; } set { imeKontakta = value; OnNotifyPropertyChanged("ImeKontakta"); } }
        private string metodKomunikacije;
        public string MetodKomunikacije { get { return metodKomunikacije; } set { metodKomunikacije = value; OnNotifyPropertyChanged("MetodKomunikacije"); } }

        public CommunicationViewModel()
        {
            //postavljanje komandi
            PromijeniKontakta = new RelayCommand<object>(promijeniKontakta, (object parametar) => true);
            PromijeniKomunikaciju = new RelayCommand<object>(sljedeciNacinKomunikacije, (object parametar) => true);
            Komuniciraj = new RelayCommand<object>(komuniciraj, (object parametar) => true);
            //priprema svih ponasanja da se rotiraju
            communicateBehaviours = new List<ICommunicateBehaviour>();
            communicateBehaviours.Add(new EmailCommunicateBehaviour());
            communicateBehaviours.Add(new CallCommunicateBehaviour());
            communicateBehaviours.Add(new SmsCommunicateBehaviour());
            //prikaz prvog defaultnog u UI
            MetodKomunikacije = prefixMetodKomunikacije + communicateBehaviours[trenutnaKomunikacija].dajMetodKomunikacije();
            //Odmah traziti od korisnika jednog kontakta da navede
            pickContact();
        }

        public void promijeniKontakta(object parameter)
        {
            pickContact();
        }

        public void sljedeciNacinKomunikacije(object parameter)
        {
            //rotiraju se behaviours pomocu dugmeta kad dodje do zadnjeg ide od prvog
            trenutnaKomunikacija = (trenutnaKomunikacija+1>=communicateBehaviours.Count)?0:trenutnaKomunikacija+1;
            MetodKomunikacije = prefixMetodKomunikacije + communicateBehaviours[trenutnaKomunikacija].dajMetodKomunikacije();
        }

        public async void pickContact()
        {
            //kako zatraziti kontakta, moze i vise kontakata tako sto se PickContactsAsync pozove u mnozini
            var contactPicker = new ContactPicker();
            //sta nas sve interesuje od odabranog kontakta
            contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
            trenutniKontakt = await contactPicker.PickContactAsync();
           // ImeKontakta = "Trenutni Kontakt:" + trenutniKontakt.LastName + " " + trenutniKontakt.FirstName;
        }

        public void komuniciraj(object parameter)
        {
            //pozovi ponasanje/strategiju
            communicateBehaviours[trenutnaKomunikacija].Communicate(trenutniKontakt);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
