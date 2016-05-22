using MobileProjekat.OoadMobile.Accelerometar.Helper;
using MobileProjekat.OoadMobile.Accelerometar.Models;
using MobileProjekat.OoadMobile.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Core;

namespace MobileProjekat.OoadMobile.Accelerometar.ViewModels
{
    class AccelViewModel : INotifyPropertyChanged
    {
        //klasa za izdvojena sa ocitanjem accelerometrom
        AccelHelper accelHelper;
        //cuva thread ove klase (tj s obzirom da je kreirana od strane UI to je UI thread)
        private readonly CoreDispatcher dispatcher;
        //kolekcija svih Z ocitanja accelerometra za grafik
        public ObservableCollection<AccelOcitanje> HistorijatZValue { get; set; }
        //Ocitanja sa acelerometra konvertovana u vrijednosti pogodne za grafike OnNotify za refresh binding na silu
        private int xValue;
        public int XValue { get { return xValue; } set { xValue = value; OnNotifyPropertyChanged("XValue"); } }
        private int yValue;
        public int YValue { get { return yValue; } set { yValue = value; OnNotifyPropertyChanged("YValue"); } }
        private int zValue;
        public int ZValue { get { return zValue; } set { zValue = value; OnNotifyPropertyChanged("ZValue"); } }
        //posto se grafici dugo ucitavaju ima dugme za pocetak ocitanja kada je sve spremno inace bi se nacekalo
        public ICommand PokreniOcitanje { get; set; }
        //posto se line graf dugo iscrtava preskocicemo svakih 100 ocitanja da mobitel moze izdrati ocitavanja i iscrtavanja
        //sto slabiji mobitel to treba vise sacekati
        public int deMaloOhani = 0;
        public void ResetujSe()
        {
            //cirkularni graf Gauge
            XValue = (int)(accelHelper.AccelX * 10);
            ZValue = (int)(accelHelper.AccelZ * 1000);
            //Linijski graf sa historijatom
            if (deMaloOhani > 100)
            {
                HistorijatZValue.Add(new AccelOcitanje() { Milisec = DateTime.Now.Millisecond, Vrijednost = ZValue });
                deMaloOhani = 0;
            }
            else
            {
                deMaloOhani++;
            }
            //digitalna kontrola
            YValue = Math.Abs((int)(accelHelper.AccelY * 10000)); 
        }

        public AccelViewModel()
        {
            HistorijatZValue = new ObservableCollection<AccelOcitanje>();
            //uzmi trenutni thread UI
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            //binding komanda za pocetak ocitanja
            PokreniOcitanje = new RelayCommand<object>(startSnimanje, (object parametar) => true);
        }

        public void startSnimanje(object paramerer)
        {
            //pokrenuti pomocnu klasu za ocitavanja na button click
            accelHelper = new AccelHelper(ResetujSe, dispatcher);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
