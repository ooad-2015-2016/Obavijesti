using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace MobileProjekat.OoadMobile.CallISMS.Helper
{
    //behaviour za strategy pattern
    public interface ICommunicateBehaviour
    {
        //glavno ponasanje koje obavi komunikaciju sa kontaktom
        void Communicate(Contact kontakt);
        //vraca koji je trenutni behaviour naziv njegov za UI
        string dajMetodKomunikacije();
    }
}
