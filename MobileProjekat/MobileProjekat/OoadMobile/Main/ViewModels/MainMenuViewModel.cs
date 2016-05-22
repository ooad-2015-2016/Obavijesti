using MobileProjekat.OoadMobile.Accelerometar.ViewModels;
using MobileProjekat.OoadMobile.Accelerometar.Views;
using MobileProjekat.OoadMobile.CallISMS.ViewModels;
using MobileProjekat.OoadMobile.CallISMS.Views;
using MobileProjekat.OoadMobile.Gps.ViewModels;
using MobileProjekat.OoadMobile.Gps.Views;
using MobileProjekat.OoadMobile.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MobileProjekat.OoadMobile.Main.ViewModels
{
    //Obicni ViewModel za navigaciju na 3 views
    class MainMenuViewModel
    {
        public ICommand Contact { get; set; }
        public ICommand Gps { get; set; }
        public ICommand Accel { get; set; }
        public INavigationService NavigationService { get; set; }

        public MainMenuViewModel()
        {
            NavigationService = new NavigationService();
            //mogu se i obe metode u relaycommand staviti inline
            //plus je da se ne mora se pisati metoda, minus kad se radi debug tesko je debagirati tacno dio koji poziva metodu 
            Contact = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(ComunicationView)), (object parametar) => true);
            Gps = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(GpsView)), (object parametar) => true);
            Accel = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(AccelView)), (object parametar) => true);
        }
    }
}
