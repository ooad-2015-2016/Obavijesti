using ProjekatOoad.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;

namespace ProjekatOoad.Venues.Model
{
    //Koristi Implementirani servis da povuce Restorane (tj venues i pretvori ih u restorane
    //ovdje se moze naknadno zamjeniti webservis koji se koristi bez da se dira FourSquare servis
    //ovdje se moze dodati naknadno i neke vrijednosti na restorane koji su mozda u lokalnoj bazi
    public class RestoranDataSource
    {
        FourSqareConnector con;
        private VenusService venService;
        public List<Restoran> Restorani
        {
            get; set;
        }

        public VenusService VenService
        {
            get
            {
                return venService;
            }

            set
            {
                venService = value;
            }
        }

        public async void dajRestoraneFourSquare(SearchOpcije so, Action callback)
        {
            Restorani = new List<Restoran>();
            //povuci opcije iz json file
            JsonObject servicesConfig = JsonValue.Parse(File.ReadAllText("ServicesConfig.json")).GetObject();
            //samo jednom ide registracija
            if (con == null)
            {
                con = new FourSqareConnector(servicesConfig.GetNamedObject("FourSquareApiService").GetNamedObject("options"));
                await con.authenticate();
            }
            VenService = new VenusService(con.OAuthToken);
            await VenService.getVenus(so.GeoSirina, so.GeoDuzina);
            foreach(Venue v in VenService.Venues)
            {
                Restorani.Add(new Restoran(v));
            }
            callback();
        }
    }
}
