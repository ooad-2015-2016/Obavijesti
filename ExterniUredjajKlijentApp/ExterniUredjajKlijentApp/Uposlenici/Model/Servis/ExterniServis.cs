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
        JsonObject servicesConfig;
        string serviceHost;
        public static string uposleniciName = "UposlenikService";
        public static string dogadjajiName = "DogadjajService";


        public ExterniServis()
        {
            servicesConfig = JsonValue.Parse(File.ReadAllText("ServisConfig.json")).GetObject();
            serviceHost = servicesConfig.GetNamedString("serviceHost");
        }

        public async void dodajKorisnika(Uposlenik uposlenik)
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new HttpMediaTypeWithQualityHeaderValue("application/json"));
            string jsonContents = JsonConvert.SerializeObject(uposlenik);
            HttpResponseMessage response = await httpClient.PostAsync(new Uri(serviceHost+ uposleniciName), new HttpStringContent(jsonContents, Windows.Storage.Streams.UnicodeEncoding.Utf8, "application/json"));
            JsonValue value = JsonValue.Parse(response.Content.ToString());
        }

        public async void dajSveKorisnike()
        {
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(new Uri(serviceHost + uposleniciName));
            JsonValue value = JsonValue.Parse(response);
        }
    }
}
