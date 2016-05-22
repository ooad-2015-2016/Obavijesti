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
    class MainMenuViewModel
    {
        public ICommand Contact { get; set; }
        public ICommand Gps { get; set; }
        public ICommand Accel { get; set; }
        public INavigationService NavigationService { get; set; }

        public MainMenuViewModel()
        {
            NavigationService = new NavigationService();
            Contact = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(ComunicationView), new CommunicationViewModel()), (object parametar) => true);
            Gps = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(GpsView), new GpsViewModel()), (object parametar) => true);
            Accel = new RelayCommand<object>((object parametar) => NavigationService.Navigate(typeof(AccelView), new AccelViewModel()), (object parametar) => true);
        }
    }
}
