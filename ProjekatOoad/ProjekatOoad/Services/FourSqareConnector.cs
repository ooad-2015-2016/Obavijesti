using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.Web;
using Windows.Web.Http;
using Windows.Data.Json;
using System.IO;

namespace ProjekatOoad.Services
{
    class FourSqareConnector
    {
        //OauthToken glavni potreban za Rest pozive, cuva autorizaciju da se moze obaviti poziv
        private string oAuthToken;
        //id i secret su potrebni da se dobije oauth
        private string clientId;
        private string clientSecret;
        //na koju stranicu da se ide nakon zavrsene autorizacije
        private string redirectUri;

        //konstruktor prima objekat sa postavkama
        public FourSqareConnector(JsonObject fourSquareSettings)
        {
            clientId = fourSquareSettings.GetNamedString("clientId");
            clientSecret = fourSquareSettings.GetNamedString("clientSecret");
            redirectUri = fourSquareSettings.GetNamedString("redirectUri");
        }

        public string OAuthToken
        {
            get
            {
                return oAuthToken;
            }

            set
            {
                oAuthToken = value;
            }
        }

        //Cilj metode je da pronadje i postavi OAuthToken
        public async Task authenticate()
        {
            try
            {
                //Sastavi url poziva
                String foursqareApiUrl = "https://foursquare.com/oauth2/authenticate?client_id="+clientId+ "&response_type=code&redirect_uri="+redirectUri;
                Uri StartUri = new Uri(foursqareApiUrl);
                //poziv authentifikacije, ovo ce otvoriti foursquare web stranicu koja trazi od korisnika da se loguje
                WebAuthenticationResult WebAuthenticationResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, StartUri, new Uri("https://www.google.ba/"));
                //Ako je sve ok izvuci oauthtoken iz response
                if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    await getOauthToken(WebAuthenticationResult.ResponseData.ToString());
                }
                //ako nije ok baciti exception
                else if (WebAuthenticationResult.ResponseStatus == WebAuthenticationStatus.ErrorHttp)
                {
                   throw new FourSquareAuthenticationException("HTTP Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseErrorDetail.ToString());
                }
                else
                {
                   throw new FourSquareAuthenticationException("Error returned by AuthenticateAsync() : " + WebAuthenticationResult.ResponseStatus.ToString());
                }
            }
            catch (Exception Error)
            {
                throw Error;
            }
        }

        private async Task getOauthToken(string webAuthResultResponseData)
        {
            //auth poziv dobije code parametar koji se zatim iskoristi da se dovbije oauth
            //izdvajanje code parametra iz dobivenog stringa
            string responseData = webAuthResultResponseData.Substring(webAuthResultResponseData.IndexOf("code"));
            String[] keyValPairs = responseData.Split('&');
            string code = null;
            for (int i = 0; i < keyValPairs.Length; i++)
            {
                String[] splits = keyValPairs[i].Split('=');
                if (splits[0].Equals("code"))
                {
                        code = splits[1]; 
                }
            }
            //Koristenje code za dobivanje acess tokena pozivom
            HttpClient httpClient = new HttpClient();
            string response = await httpClient.GetStringAsync(new Uri("https://foursquare.com/oauth2/access_token?client_id=" + clientId + "&client_secret="+ clientSecret + "&grant_type=authorization_code&redirect_uri="+redirectUri+"&code=" + code));
            JsonObject value = JsonValue.Parse(response).GetObject();
            OAuthToken = value.GetNamedString("access_token");
        }
    }
}
