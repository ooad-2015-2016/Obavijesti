using ProjekatOoad.Helper;
using ProjekatOoad.Venues.Model;
using ProjekatOoad.Venues.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class RestoraniListDetailView : Page
    {
        //trenutno selektovani restoran
        Restoran r;
        public RestoraniListDetailView()
        {
            this.InitializeComponent();
            DataContext = new RestoraniViewModel();
            //ne zaboraviti kesirati kad se vraca nazad da nije novi list, ponovo bi se od registracije islo
            NavigationCacheMode = NavigationCacheMode.Enabled;
        }

        private void MasterListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            //kad se oznaci lista update detaila uraditi
            //ovo je krsenje mvvm, primjer kako se moze dobaviti kliknuti item
            //ovo se moglo uraditi i sa binding na selected item pa kad je selected pozvati tamo 
            ((RestoraniViewModel)DataContext).updateRestoran((Restoran)e.ClickedItem,setDataContext);
            r = (Restoran) e.ClickedItem;
            //ako je mali ekran navigiraj na detail page
            if (GrupaStanja.CurrentState == MaliEkran || GrupaStanja.CurrentState == MaliEkranSearchOpened)
            {
                // drill in je animacija kad se otvara page
                Frame.Navigate(typeof(RestoraniDetailView), e.ClickedItem, new DrillInNavigationTransitionInfo());
            }
        }
        //kad zavrsi update restorana postaviti data context detaila na updateovani restoran
        public void setDataContext()
        {
            DetailContentPresenter.Content = r;
        }
    }
}
