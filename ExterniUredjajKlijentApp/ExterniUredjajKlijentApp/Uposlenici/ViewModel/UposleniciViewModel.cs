using ExterniUredjajKlijentApp.Uposlenici.Helper;
using ExterniUredjajKlijentApp.Uposlenici.Model;
using ExterniUredjajKlijentApp.Uposlenici.Model.Servis;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ExterniUredjajKlijentApp.Uposlenici.ViewModel
{
    class UposleniciViewModel : INotifyPropertyChanged
    {
        //uposlenik koji se priprema za kreiranje
        public Uposlenik CreateUposlenik { get; set; }
        //servis koji ce da komunicira sa drugom aplikacijom koja pruza web servis
        ExterniServis eksterniServis;
        //kamera uredjaj
        public CameraHelper Camera { get; set; }
        //rfid uredjaj
        Rfid rfid;
        //komand pattern
        public ICommand DodajUposlenika { get; set; }
        public ICommand Uslikaj { get; set; }
        //Negdje privremeno mora biti slika koja ce se prikazati kad se uslika
        private SoftwareBitmapSource slika;
        public SoftwareBitmapSource Slika { get { return slika; } set { slika = value; OnNotifyPropertyChanged("Slika"); } }
        //kontrola krsenje mvvm
        CaptureElement previewControl;
        public UposleniciViewModel(CaptureElement previewControl)
        {
            //incijalicacija uredjaja
            eksterniServis = new ExterniServis();
            CreateUposlenik = new Uposlenik();
            rfid = new Rfid();
            rfid.InitializeReader(RfidReadSomething);
            Camera = new CameraHelper(previewControl);
            Camera.InitializeCameraAsync();
            DodajUposlenika = new RelayCommand<object>(dodajUposlenika, (object parametar) => true);
            Uslikaj = new RelayCommand<object>(uslikaj, (object parametar) => true);
        }
        //komanda koja inicira slikanje
        public async void uslikaj(object parametar)
        {
            await Camera.TakePhotoAsync(SlikanjeGotovo);
        }
        //komanda za dodavanje uposlenika
        public void dodajUposlenika(object parametar)
        {
            eksterniServis.dodajKorisnika(CreateUposlenik);
            CreateUposlenik = new Uposlenik();
        }
        //callback na read rfid
        public void RfidReadSomething(string rfidKod)
        {
            CreateUposlenik.RfidKartica = rfidKod;
        }

        //callback funkcija kad se uslika 
        public void SlikanjeGotovo(SoftwareBitmapSource slikica)
        {
            Slika = slikica;
        }
        //proeprty changed observer
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
