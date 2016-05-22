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
        AccelHelper accelHelper;
        private readonly CoreDispatcher dispatcher;
        public ObservableCollection<AccelOcitanje> HistorijatZValue { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
        private int xValue;
        public int XValue { get { return xValue; } set { xValue = value; OnNotifyPropertyChanged("XValue"); } }
        private int yValue;
        public int YValue { get { return yValue; } set { yValue = value; OnNotifyPropertyChanged("YValue"); } }
        private int zValue;
        public int ZValue { get { return zValue; } set { zValue = value; OnNotifyPropertyChanged("ZValue"); } }
        public ICommand PokreniOcitanje { get; set; }

        public int deMaloOhani = 0;

        public void ResetujSe()
        {
            XValue = (int)(accelHelper.AccelX * 10);
            ZValue = (int)(accelHelper.AccelZ * 1000);
            if (deMaloOhani > 100)
            {
                HistorijatZValue.Add(new AccelOcitanje() { Milisec = DateTime.Now.Millisecond, Vrijednost = ZValue });
                deMaloOhani = 0;
            }
            else
            {
                deMaloOhani++;
            }
            YValue = Math.Abs((int)(accelHelper.AccelY * 10000)); 
        }

        public void startSnimanje(object paramerer)
        {
            accelHelper = new AccelHelper(ResetujSe, dispatcher);
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                return;
            }
        }

        public AccelViewModel()
        {
            HistorijatZValue = new ObservableCollection<AccelOcitanje>();
            dispatcher = CoreWindow.GetForCurrentThread().Dispatcher;
            PokreniOcitanje = new RelayCommand<object>(startSnimanje, (object parametar) => true);
        }
    }
}
