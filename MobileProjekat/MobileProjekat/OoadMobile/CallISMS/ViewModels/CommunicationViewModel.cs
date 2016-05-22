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
        private static string prefixMetodKomunikacije = "Metod Komunikacije:";
        Contact trenutniKontakt;
        int trenutnaKomunikacija = 0;
        List<ICommunicateBehaviour> communicateBehaviours;
        public ICommand PromijeniKontakta { get; set; }
        public ICommand PromijeniKomunikaciju { get; set; }
        public ICommand Komuniciraj { get; set; }
        private string imeKontakta;
        public string ImeKontakta { get { return imeKontakta; } set { imeKontakta = value; OnNotifyPropertyChanged("ImeKontakta"); } }
        private string metodKomunikacije;
        public string MetodKomunikacije { get { return metodKomunikacije; } set { metodKomunikacije = value; OnNotifyPropertyChanged("MetodKomunikacije"); } }

        public CommunicationViewModel()
        {
            PromijeniKontakta = new RelayCommand<object>(promijeniKontakta, (object parametar) => true);
            PromijeniKomunikaciju = new RelayCommand<object>(sljedeciNacinKomunikacije, (object parametar) => true);
            Komuniciraj = new RelayCommand<object>(komuniciraj, (object parametar) => true);
            communicateBehaviours = new List<ICommunicateBehaviour>();
            communicateBehaviours.Add(new EmailCommunicateBehaviour());
            communicateBehaviours.Add(new CallCommunicateBehaviour());
            communicateBehaviours.Add(new SmsCommunicateBehaviour());
            MetodKomunikacije = prefixMetodKomunikacije + communicateBehaviours[trenutnaKomunikacija].dajMetodKomunikacije();
            pickContact();
        }

        public void promijeniKontakta(object parameter)
        {
            pickContact();
        }

        public void sljedeciNacinKomunikacije(object parameter)
        {
            trenutnaKomunikacija = (trenutnaKomunikacija+1>=communicateBehaviours.Count)?0:trenutnaKomunikacija+1;
            MetodKomunikacije = prefixMetodKomunikacije + communicateBehaviours[trenutnaKomunikacija].dajMetodKomunikacije();
        }

        public async void pickContact()
        {
            var contactPicker = new ContactPicker();
            contactPicker.SelectionMode = Windows.ApplicationModel.Contacts.ContactSelectionMode.Fields;
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.PhoneNumber);
            contactPicker.DesiredFieldsWithContactFieldType.Add(ContactFieldType.Email);
            trenutniKontakt = await contactPicker.PickContactAsync();
           // ImeKontakta = "Trenutni Kontakt:" + trenutniKontakt.LastName + " " + trenutniKontakt.FirstName;
        }

        public void komuniciraj(object parameter)
        {
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
