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
    class AccelHelper
    {
        public Double AccelX { get; set; }
        public Double AccelY { get; set; }
        public Double AccelZ { get; set; }
        private Accelerometer accelerometer;
        private readonly CoreDispatcher dispatcher;
        public Action Callback { get; set; }
        private async void ReadingChanged(object sender, AccelerometerReadingChangedEventArgs e)
        {
            await dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                AccelerometerReading reading = e.Reading;
                AccelX = reading.AccelerationX;
                AccelY = reading.AccelerationY;
                AccelZ = reading.AccelerationZ;
                Callback();
            });

        }

        public AccelHelper(Action call, CoreDispatcher disp)
        {
            Callback = call;
            this.dispatcher = disp;
            accelerometer = Accelerometer.GetDefault();

            if (accelerometer != null)
            {
                // Establish the report interval
                uint minReportInterval = accelerometer.MinimumReportInterval;
                uint reportInterval = minReportInterval > 16 ? minReportInterval : 16;
                accelerometer.ReportInterval = reportInterval;

                // Assign an event handler for the reading-changed event
                accelerometer.ReadingChanged += new TypedEventHandler<Accelerometer, AccelerometerReadingChangedEventArgs>(ReadingChanged);
            }
        }
    }
}
