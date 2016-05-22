using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Sensors;
using Windows.Foundation;
using Windows.UI.Core;

namespace MobileProjekat.OoadMobile.Accelerometar.Helper
{
    //klasa za rad sa accelerometrom
    class AccelHelper
    {
        //vrijednosti gdje ce se cuvati ocitanja od tri ose
        public Double AccelX { get; set; }
        public Double AccelY { get; set; }
        public Double AccelZ { get; set; }
        //objekat za komunikaciju sa accelerometrom
        private Accelerometer accelerometer;
        //posudjen thread od UI - threada
        private readonly CoreDispatcher dispatcher;
        //funkcija koja ce se pozvati kad se nesto ocita
        public Action Callback { get; set; }
        
        public AccelHelper(Action call, CoreDispatcher disp)
        {
            Callback = call;
            this.dispatcher = disp;
            //dobivanje accelerometra
            accelerometer = Accelerometer.GetDefault();
            if (accelerometer != null)
            {
                //koliko cesto da se dobiva vrijednost, ne pretjerivati zbog sporog iscrtavanja grafova
                uint minReportInterval = accelerometer.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                accelerometer.ReportInterval = reportInterval;
                //event sta da se pozove kad dodje do ocitanja
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            }
        }

        private async void ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
        {
            //iskoristi ui thread da se pozove ocitanje. Bez ovog dodje do pristupanja jednog threada drugom sto je fatalno za program
            //tj pozivanje bindinga vrijednosti iz threada koji radi ocitavanje uredjaja 
            //ako se naleti na slican problem sa hardverskim uredjajima dispatcher je rjesenje
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                //uzeti ocitanje i ugurati u vrijednosti i pozvati callback da se promijeni UI preko binding
                AccelerometerReading reading = e.Reading;
                AccelX = reading.AccelerationX;
                AccelY = reading.AccelerationY;
                AccelZ = reading.AccelerationZ;
                Callback();
            });
        }
    }
}
