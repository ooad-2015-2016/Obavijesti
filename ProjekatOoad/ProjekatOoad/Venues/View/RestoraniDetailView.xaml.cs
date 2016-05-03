using ProjekatOoad.Venues.Model;
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
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;

namespace ProjekatOoad.Venues.View
{
    public sealed partial class RestoraniDetailView : Page
    {
        //data context stranice, mogao se i viewmodel napraviti ali u pitanju je jedan restoran bez ikakvih komandi
        public Restoran RestoranDetail;
        public RestoraniDetailView()
        {
            this.InitializeComponent();

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            //prima parametar restoran koji mu je proslijedjen
            RestoranDetail = (Restoran) e.Parameter;
            DataContext = RestoranDetail;
            //prikazati back button
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested += DetailPage_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Visible;
        }

        //sakriti back button kad se vraca na list
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            SystemNavigationManager systemNavigationManager = SystemNavigationManager.GetForCurrentView();
            systemNavigationManager.BackRequested -= DetailPage_BackRequested;
            systemNavigationManager.AppViewBackButtonVisibility = AppViewBackButtonVisibility.Collapsed;
        }

        //realizacija eventa klika na dugme nazad
        private void DetailPage_BackRequested(object sender, BackRequestedEventArgs e)
        {
            e.Handled = true;
            OnBackRequested();
        }

        private void OnBackRequested()
        {
            //animacija pri navigaciji nazad
            DrillInNavigationTransitionInfo d = new DrillInNavigationTransitionInfo();
            Frame.GoBack(d);
        }

        void NavigateBackForWideState(bool useTransition)
        {
            //Ne kesirati, sta ce page kad ce 99% uvijek biti novi pri novoj navigaciji
            NavigationCacheMode = NavigationCacheMode.Disabled;
            //ako je kliknuto back onda animaciju a ako se povecao prozor ne treba animacija
            if (useTransition)
            {
                Frame.GoBack(new EntranceNavigationTransitionInfo());
            }
            else
            {
                Frame.GoBack(new SuppressNavigationTransitionInfo());
            }
        }

        //uslov da li treba otici u List mode ako se prozor prosiri
        private bool ShouldGoToWideState()
        {
            return Window.Current.Bounds.Width >= 720;
        }

        private void PageRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //ako je window velik dovoljno vrati se u List
            if (ShouldGoToWideState())
            {
                NavigateBackForWideState(useTransition: true);
            }
            //dodati event na promjenu prozora da kad predje 720 da se vrati u list
            Window.Current.SizeChanged += Window_SizeChanged;
        }

        private void PageRoot_Unloaded(object sender, RoutedEventArgs e)
        {
            Window.Current.SizeChanged -= Window_SizeChanged;
        }

        private void Window_SizeChanged(object sender, Windows.UI.Core.WindowSizeChangedEventArgs e)
        {
            if (ShouldGoToWideState())
            {
                //maknuti sebe iz sizeChanged eventa
                Window.Current.SizeChanged -= Window_SizeChanged;
                NavigateBackForWideState(useTransition: false);
            }
        }


    }
}
