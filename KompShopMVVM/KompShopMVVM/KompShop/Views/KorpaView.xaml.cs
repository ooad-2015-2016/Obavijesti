using KompShopMVVM.KompShop.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace KompShopMVVM.KompShop.Views
{
    public sealed partial class KorpaView : Page
    {
        public KorpaView()
        {
            this.InitializeComponent();
            //Svi Binding Koriste properties iz KorpaViewModel
            //Ovakav kontekst omogucava da se properties u KorpaViewModel koriste na nivou citavog page
            DataContext = new KorpaViewModel();
            //Kada se sa neke druge forme uradi GoBack bez ove linije opet bi se pozvao konstruktor KorpaView i izgubili bi se podaci u KorpaViewModel
            //ovim se za povratak nazad cuva forma da se ponovo iskoristi
            NavigationCacheMode = NavigationCacheMode.Required;
        }

        //Sluzi da kad se dodje na ovu formu, treba onemoguciti back dugme jer se nema gdje vratiti
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            var currentView = SystemNavigationManager.GetForCurrentView();
            currentView.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }
    }
}
