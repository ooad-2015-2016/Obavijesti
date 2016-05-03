using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Data.Json;
using System.IO;

namespace ProjekatOoad.Services
{
    public class VenusService
    {
        //poziv za dobivanje slike preko url zahtjeva zeljeni width i height
        //ovo je samo da se vrijednost moze namjesiti na jednom mjestu po potrebi
        private static int defaultPictureWidth = 500;
        private static int defaultPictureHeight = 500;
        //Moze se proslijediti i citav FourSqareConnector ali za potrebe ovog servisa je dovoljan oAuthToken
        string oAuthToken;
        //Svi venues koji ce se povuci sa servera
        private List<Venue> venues;

        public VenusService(string oAuth)
        {
            oAuthToken = oAuth;
        }

        public List<Venue> Venues
        {
            get
            {
                return venues;
            }

            set
            {
                venues = value;
            }
        }

        //glavni poziv koji pozove Rest Upit da se dobiju svi venues oko lat,lng
        //i pretvore u listu venues
        public async Task getVenus(double latitude, double longitude)
        {
            Venues = new List<Venue>();
            //pozvati rest upit
            HttpClient httpClient = new HttpClient();
            string uriString = "https://api.foursquare.com/v2/venues/search?ll=" + latitude + "," + longitude + "&query=restaurant&oauth_token=" + oAuthToken + "&v=20160410";
            string response = await httpClient.GetStringAsync(new Uri(uriString));
            //response je string koji se konvertuje u JsonObject
            JsonObject value = JsonValue.Parse(response).GetObject();
            //Sad je potrebno parsirati taj objekat
            //Pratiti izgled response u dokumentaciji i polje po polje parsirati
            //osjetljiv dio koda posto servisi imaju obicaj da promjene nazive polja
            JsonObject responseVenus = value.GetNamedObject("response");
            JsonArray jsonVenus = responseVenus.GetNamedArray("venues");
            for (uint i = 0; i < jsonVenus.Count; i++)
            {
                JsonObject contact = jsonVenus.GetObjectAt(i).GetNamedObject("contact");
                JsonObject location = jsonVenus.GetObjectAt(i).GetNamedObject("location");
                Venue ven = new Venue();
                ven.fourSqaureId = jsonVenus.GetObjectAt(i).GetNamedString("id");
                IJsonValue jsonValue;
                //neke vrijednosti mozda ne postoje u tom slucaju ne mogu se ni postaviti
                if (jsonVenus.GetObjectAt(i).TryGetValue("name", out jsonValue))
                {
                    ven.Naziv = jsonValue.GetString();
                }
                if (jsonVenus.GetObjectAt(i).TryGetValue("phone", out jsonValue))
                {
                    ven.Telefon = jsonValue.GetString();
                }
                if (jsonVenus.GetObjectAt(i).TryGetValue("lat", out jsonValue))
                {
                    ven.GeoSirina = jsonValue.GetNumber();
                }
                if (jsonVenus.GetObjectAt(i).TryGetValue("lng", out jsonValue))
                {
                    ven.GeoDuzina = jsonValue.GetNumber();
                }
                if (jsonVenus.GetObjectAt(i).TryGetValue("address", out jsonValue))
                {
                    ven.Adresa = jsonValue.GetString();
                }
                Venues.Add(ven);
            }
        }

        //posto lista ne vrati sve podatke u venue, ostali podaci Slika, Opis Rating se moraju naknadno dodati
        public async Task updateVenueDetailed(Venue v,Action callback)
        {
            //ako je vec updated nemoj opet
            if (!v.Detailed)
            {
                Venues = new List<Venue>();
                //opet rest poziv
                HttpClient httpClient = new HttpClient();
                string uriString = "https://api.foursquare.com/v2/venues/" + v.fourSqaureId + "?oauth_token=" + oAuthToken + "&v=20160410";
                string response = await httpClient.GetStringAsync(new Uri(uriString));
                //pa parsiranje
                JsonObject value = JsonValue.Parse(response).GetObject();
                JsonObject responseVenue = value.GetNamedObject("response");
                JsonObject venue = responseVenue.GetNamedObject("venue");
                IJsonValue jsonValue;
                if (venue.TryGetValue("rating", out jsonValue))
                {
                    v.Rating = jsonValue.GetNumber()/2;
                }
                JsonObject tips = venue.GetNamedObject("tips");
                JsonArray tipGroups = tips.GetNamedArray("groups");
                if (tipGroups.Count > 0)
                {
                    jsonValue = tipGroups.Last();
                    JsonObject tip = jsonValue.GetObject();
                    JsonArray tipItems = tip.GetNamedArray("items");
                    if (tipItems.Count > 0)
                    {
                        v.Opis = tipItems.GetObjectAt(0).GetNamedString("text");
                    }
                }
                if (venue.TryGetValue("bestPhoto", out jsonValue))
                {
                    JsonObject bestPhoto = jsonValue.GetObject();
                    string prefix = bestPhoto.GetNamedString("prefix");
                    string suffix = bestPhoto.GetNamedString("suffix");
                    v.Slika = prefix + defaultPictureWidth + "x" + defaultPictureHeight + suffix;
                }
                else
                {
                    //else iskopati sliku iz liste svih slika
                }
                v.Detailed = true;
            }
            //posto je funkcija async i cesto ce imati i delay ovisno o konekciji
            //u callback se stavi funkcija (izmjeni gui npr) koja se poziva kada async zavrsi
            callback();
        }
    }
}
