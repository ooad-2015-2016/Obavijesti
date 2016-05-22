using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;
using Windows.Foundation.Metadata;

namespace MobileProjekat.OoadMobile.CallISMS.Helper
{
    public class CallCommunicateBehaviour : ICommunicateBehaviour
    {
        public void Communicate(Contact kontakt)
        {
            string telBroj = kontakt.Phones.FirstOrDefault<ContactPhone>().Number;
            if (Windows.Foundation.Metadata.ApiInformation.IsTypePresent("Windows.ApplicationModel.Calls.PhoneCallManager"))
            {
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(telBroj, kontakt.Name);
            }
        }

        public string dajMetodKomunikacije()
        {
            return "Call";
        }
    }
}
