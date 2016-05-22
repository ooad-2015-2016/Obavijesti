using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using Windows.Services.Maps;

namespace MobileProjekat.OoadMobile.Gps.ViewModels
{
    class GpsViewModel : INotifyPropertyChanged
    {
        private Geopoint trenutnaLokacija;
        public Geopoint TrenutnaLokacija { get { return trenutnaLokacija; } set { trenutnaLokacija = value; OnNotifyPropertyChanged("TrenutnaLokacija"); } }
        private string lokacija;
        public string Lokacija { get { return lokacija; } set { lokacija = value; OnNotifyPropertyChanged("Lokacija"); } }
        private string adresa;
        public string Adresa { get { return adresa; } set { adresa = value; OnNotifyPropertyChanged("Adresa"); } }
        private string fax;
        public string Fax { get { return fax; } set { fax = value; OnNotifyPropertyChanged("Fax"); } }
        public GpsViewModel()
        {
            dajLokaciju();
        }
        public async void dajLokaciju()
        {
            Geoposition pos = null;
            var accessStatus = await Geolocator.RequestAccessAsync();
            if (accessStatus == GeolocationAccessStatus.Allowed)
            {
                    // If DesiredAccuracy or DesiredAccuracyInMeters are not set (or value is 0), DesiredAccuracy.Default is used.
                    Geolocator geolocator = new Geolocator { DesiredAccuracyInMeters = 10 };
                    pos = await geolocator.GetGeopositionAsync();
            }
            TrenutnaLokacija = pos.Coordinate.Point;
            Lokacija = "Geolokacija Lat: " + TrenutnaLokacija.Position.Latitude + " Lng: " + TrenutnaLokacija.Position.Longitude;
            MapLocationFinderResult result =
            await MapLocationFinder.FindLocationsAtAsync(pos.Coordinate.Point);
            // If the query returns results, display the name of the town
            // contained in the address of the first result.
            if (result.Status == MapLocationFinderStatus.Success)
            {
                Adresa = "Adresa je " + result.Locations[0].Address.Street;
            }


        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
