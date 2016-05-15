using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Web.Http;
using Windows.Web.Http.Headers;

namespace ExterniUredjajKlijentApp.Uposlenici.Model.Servis
{
    class ExterniServis
    {
        //cuvaju se konfiguracije
        JsonObject servicesConfig;
        string serviceHost;//na kojoj adresi je servis
        //nazivi akcija specificirane servisnom aplikacijom
        public static string uposleniciName = "UposlenikService";
        public static string dogadjajiName = "DogadjajService";

        public ExterniServis()
        {
            //ucitavanje iz json radi laksih izmjena kasnije
            servicesConfig = JsonValue.Parse(File.ReadAllText("ServisConfig.json")).GetObject();
            serviceHost = servicesConfig.GetNamedString("serviceHost");
        }

        //primjer post zahtjeva prema servisu
        public async void dodajKorisnika(Uposlenik uposlenik)
        {
            HttpClient httpClient = new HttpClient();
            //mora se stavity content/type da je json inace ce aplikacija da odbija zahtjev
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            //json se salje u body post zahtjeva
            string jsonContents = JsonConvert.SerializeObject(uposlenik);
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(serviceHost+ uposleniciName), new HttpStringContent(jsonContents, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
            //dalje sa odgovorom se moze uraditi sta god zatreba
            JsonValue value = JsonValue.Parse(response.Content.ToString());
        }

        public async void dajSveKorisnike()
        {
            //primjer servis povlacenja svih korisnika, isti princip kao i FourSquare, koristi se isti Http api
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(new Uri(serviceHost + uposleniciName));
            JsonValue value = JsonValue.Parse(response);
        }
    }
}
