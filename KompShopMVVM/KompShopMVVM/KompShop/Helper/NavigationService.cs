using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace KompShopMVVM.KompShop.Helper
{
    class NavigationService:INavigationService
    {
        //obicna navigacija bez parametra
        public void Navigate(Type sourcePage)
        {
            var frame = (Frame) Window.Current.Content;
            frame.Navigate(sourcePage);
        }
        //navigiranje na page ali da se proslijedi parametar stranici
        public void Navigate(Type sourcePage, object parameter)
        {
            var frame = (Frame) Window.Current.Content;
            frame.Navigate(sourcePage, parameter);
        }
        //poziv da se vrati na predhodnu stranicu
        public void GoBack()
        {
            var frame = (Frame) Window.Current.Content;
            frame.GoBack();
        }
    }
}
