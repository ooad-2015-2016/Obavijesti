using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Contacts;

namespace MobileProjekat.OoadMobile.CallISMS.Helper
{
    public interface ICommunicateBehaviour
    {
        void Communicate(Contact kontakt);
        string dajMetodKomunikacije();
    }
}
