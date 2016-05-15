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
        public Uposlenik CreateUposlenik { get; set; }
        ExterniServis eksterniServis;
        public CameraHelper Camera { get; set; }
        Rfid rfid;
        public ICommand DodajUposlenika { get; set; }
        public ICommand Uslikaj { get; set; }
        private SoftwareBitmapSource slika;
        public SoftwareBitmapSource Slika { get { return slika; } set { slika = value; OnNotifyPropertyChanged("Slika"); } }
        CaptureElement previewControl;
        public UposleniciViewModel(CaptureElement previewControl)
        {
            eksterniServis = new ExterniServis();
            CreateUposlenik = new Uposlenik();
            rfid = new Rfid();
            rfid.InitializeReader(RfidReadSomething);
            Camera = new CameraHelper(previewControl);
            Camera.InitializeCameraAsync();
            DodajUposlenika = new RelayCommand<object>(dodajUposlenika, (object parametar) => true);
            Uslikaj = new RelayCommand<object>(uslikaj, (object parametar) => true);
        }

        public async void uslikaj(object parametar)
        {
            await Camera.TakePhotoAsync(SlikanjeGotovo);
        }

        public void dodajUposlenika(object parametar)
        {
            eksterniServis.dodajKorisnika(CreateUposlenik);
        }

        public void RfidReadSomething(string rfidKod)
        {
            CreateUposlenik.RfidKartica = rfidKod;
        }

        public void SlikanjeGotovo(SoftwareBitmapSource slikica)
        {
            Slika = slikica;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
