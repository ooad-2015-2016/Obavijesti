using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace VanjskiUredjaji
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        //ovaj atribut koristite da pristupite serijskoj komunikaciji
        SerialDevice serialDevice;

        //ovaj objekt ce se koristiti da se citaju podaci sa serijskog porta
        DataReader dataReaderObject = null;

        private CancellationTokenSource ReadCancellationTokenSource;
        public MainPage()
        {
            this.InitializeComponent();
            InitializeSerial();
        }
        private async void InitializeSerial()
        {
            //Selector nadje device
            var selector = SerialDevice.GetDeviceSelector("COM4"); //podesiti tacno koji je port
            //info na onsovu selektora
            var devices = await DeviceInformation.FindAllAsync(selector);
            //nadje se device info

            if (devices.Any())
            {
                var deviceInfo = devices.First();
                //najosjetljivija metoda, vraca null ako nesto nije uredu
                //to moze biti da nije capability naveden u manifestu, da device ne prepoznaje ili da je
                //device zauzet od druge aplikacije
                //da je user blokirao ili sistem
                //moze biti null iako je nadjen device information
                serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);
                //Set up serial device according to device specifications:
                //This might differ from device to device
                //specifikacije com porta, nacin na koj iuredjaj salje bite
                serialDevice.BaudRate = 9600;
                serialDevice.DataBits = 8;
                serialDevice.Parity = SerialParity.None;
                serialDevice.StopBits = SerialStopBitCount.One;
                serialDevice.WriteTimeout = TimeSpan.FromMilliseconds(300);

                serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);

                // Create cancellation token object to close I/O operations when closing the device
                ReadCancellationTokenSource = new CancellationTokenSource();

                Listen();
            }
        }

        /// <summary>
        /// Event koji se poziva ako je kliknuto dugme "Upali" da bi se poslali podaci na serijski port da se upali dioda.
        /// Kod "1" je znak programu na arduinu da se upali dioda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task<UInt32> storeAsyncTask;

                var dataWriteObject = new DataWriter(serialDevice.OutputStream);
                dataWriteObject.WriteString("1");

                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();

                dataWriteObject.DetachStream();
                dataWriteObject = null;
            }
            catch (Exception ex)
            {
                //handlanje greske
            }
            
        }

        /// <summary>
        /// Event koji se poziva ako je kliknuto dugme "Ugasi" da bi se poslali podaci na serijski port da se upali dioda.
        /// Kod "0" je znak programu na arduinu da se ugasi dioda.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Task<UInt32> storeAsyncTask;

                var dataWriteObject = new DataWriter(serialDevice.OutputStream);
                dataWriteObject.WriteString("0"); //tipa saljemo "0" na serijski port

                storeAsyncTask = dataWriteObject.StoreAsync().AsTask();

                dataWriteObject.DetachStream();
                dataWriteObject = null;
            }
            catch (Exception ex)
            {
                //handlanje greske
            }
        }

        /// <summary>
        /// Metoda koja "osluskuje" da li ima nesto na serijskom portu i pokrece metodu za citanje tog sadrzaja ako ima.
        /// - Create a DataReader object
        /// - Create an async task to read from the SerialDevice InputStream
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Listen()
        {
            try
            {
                if (serialDevice != null)
                {
                    dataReaderObject = new DataReader(serialDevice.InputStream);

                    // ovo ce konstantno citati podatke i kada dodje do kraja onda poziva metodu da se iscita taj text
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token);
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.GetType().Name == "TaskCanceledException")
                {
                    if (serialDevice != null)
                    {
                        serialDevice.Dispose();
                    }
                    serialDevice = null;
                }
                else
                {
                    //ovdje ako zelite ispisati gresku
                   // status.Text = ex.Message;
                }
            }
            finally
            {
                // Cleanup once complete
                if (dataReaderObject != null)
                {
                    dataReaderObject.DetachStream();
                    dataReaderObject = null;
                }
            }
        }

        /// <summary>
        /// Metoda koja vrsi citanje nakon sto metoda Listen ustanovi da ima sadrzaja.
        /// ReadAsync: Task that waits on data and reads asynchronously from the serial device InputStream
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        private async Task ReadAsync(CancellationToken cancellationToken)
        {
            //ovdje je sada asinhrono citanje 
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask; //nakon ovoga je zavrseno citanje

            //ovdje radite sta vama vec treba: dataReaderObject.ReadString(bytesRead) je procitani text
            if (bytesRead > 0)
            {
                string procitanaVrijednost = dataReaderObject.ReadString(bytesRead); //ovim ispisujemo u listview
                listView.Items.Add(procitanaVrijednost); //dodajemo u listview podatke koji su dosli sa serijskog porta
            }
        }
    }
}
