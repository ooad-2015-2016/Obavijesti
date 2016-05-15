using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.Data.Json;
using Windows.Devices.Enumeration;
using Windows.Devices.SerialCommunication;
using Windows.Storage.Streams;

namespace ExterniUredjajKlijentApp.Uposlenici.Helper
{
    public class Rfid
    {
        //Konfiguracioni fajl radi laksih naknadnih izmjena mogu se i ostale opcije po potrebi Baud rate etc.
        JsonObject rfidConfig;
        string port;
        //Serial device je uredjaj koji je na COM portu
        SerialDevice serialDevice;
        //ovaj cita
        DataReader dataReaderObject;
        //ovo se koristi kao odgovor da se prekine
        public CancellationTokenSource ReadCancellationTokenSource;
        //zadnji procitani string od 14 bajta
        public string CurrentReadString { get; set; }

        public Rfid()
        {
            //ucitavanje json konfiguracija radi laksih izmjena
            rfidConfig = JsonValue.Parse(File.ReadAllText("RfidConfig.json")).GetObject();
            //port bitan cesto ce se mijenjati ovisno o racunaru COM 1 - 4 najcesce a moze i vise
            port = rfidConfig.GetNamedString("port");
        }

        public async void InitializeReader(Action<string> callback)
        {
            //Selector nadje device
            var selector = SerialDevice.GetDeviceSelector(port); 
            //info na onsovu selektora
            var devices = await DeviceInformation.FindAllAsync(selector);
            //nadje se device info
            if (devices.Any()) 
            {
                var deviceInfo = devices.First();
                //najosjetljivija metoda, vraca null ako nesto nije uredu
                //to moze biti da nije capability naveden u manifestu, da device ne prepoznaje ili da je device zauzet od druge aplikacije
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
                serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                //ima sekundu izmedju dva citanja, ako se kartica brzo provuce dvaput biti ce 28 bita
                ReadCancellationTokenSource = new CancellationTokenSource();
                //pokretanje uredjaja
                Listen(callback);
            }
        }
        private async void Listen(Action<string> callback)
        {
            try
            {
                if (serialDevice != null)
                {
                    //cita iz streama
                    dataReaderObject = new DataReader(serialDevice.InputStream);
                    //beskonacni loop citanja
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token, callback);
                    }
                }
            }
            catch (Exception ex)
            {
                //Moze se u output printati exception ako zatreba u nekim situacijama
                //ALI OBAVEZNO KORISTITI DEBUGGER 
                Debug.Write(ex.Message);
            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken, Action<string> callback)
        {
            Task<UInt32> loadAsyncTask;
            uint ReadBufferLength = 1024;
            //Ako se zahtjeva cancel canceluj
            cancellationToken.ThrowIfCancellationRequested();
            //Ako nisu svi biti tu probaj nesto procitati
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;
            // citanje task
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);
            //pokretanja taska citanja
            UInt32 bytesRead = await loadAsyncTask;
            //ako se nesto procita
            if (bytesRead > 0)
            {
                //procitaj i stavi u string koji se dalje moze procitati posto je public property
                CurrentReadString = dataReaderObject.ReadString(bytesRead);
                //pozovi callback da se izmjeni polje koje prati rfid citanja
                callback(CurrentReadString);
            }
        }




    }
}
