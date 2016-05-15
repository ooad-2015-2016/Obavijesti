using System;
using System.Collections.Generic;
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
        JsonObject rfidConfig;
        string port;
        public static string uposleniciName = "UposlenikService";
        public static string dogadjajiName = "DogadjajService";

        SerialDevice serialDevice;
        DataReader dataReaderObject;
        public CancellationTokenSource ReadCancellationTokenSource;
        public string CurrentReadString { get; set; }

        public Rfid()
        {
            rfidConfig = JsonValue.Parse(File.ReadAllText("RfidConfig.json")).GetObject();
            port = rfidConfig.GetNamedString("port");
        }

        public async void InitializeReader(Action<string> callback)
        {
            var selector = SerialDevice.GetDeviceSelector(port); //Get the serial port on port '3'
            var devices = await DeviceInformation.FindAllAsync(selector);
            if (devices.Any()) //if the device is found
            {
                var deviceInfo = devices.First();
                serialDevice = await SerialDevice.FromIdAsync(deviceInfo.Id);
                //Set up serial device according to device specifications:
                //This might differ from device to device
                serialDevice.BaudRate = 9600;
                serialDevice.DataBits = 8;
                serialDevice.Parity = SerialParity.None;
                serialDevice.StopBits = SerialStopBitCount.One;
                serialDevice.ReadTimeout = TimeSpan.FromMilliseconds(1000);
                ReadCancellationTokenSource = new CancellationTokenSource();
                Listen(callback);
            }
        }
        private async void Listen(Action<string> callback)
        {
            try
            {
                if (serialDevice != null)
                {
                    dataReaderObject = new DataReader(serialDevice.InputStream);

                    // keep reading the serial input
                    while (true)
                    {
                        await ReadAsync(ReadCancellationTokenSource.Token, callback);
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        private async Task ReadAsync(CancellationToken cancellationToken, Action<string> callback)
        {
            Task<UInt32> loadAsyncTask;

            uint ReadBufferLength = 1024;

            // If task cancellation was requested, comply
            cancellationToken.ThrowIfCancellationRequested();

            // Set InputStreamOptions to complete the asynchronous read operation when one or more bytes is available
            dataReaderObject.InputStreamOptions = InputStreamOptions.Partial;

            // Create a task object to wait for data on the serialPort.InputStream
            loadAsyncTask = dataReaderObject.LoadAsync(ReadBufferLength).AsTask(cancellationToken);

            // Launch the task and wait
            UInt32 bytesRead = await loadAsyncTask;
            if (bytesRead > 0)
            {
                CurrentReadString = dataReaderObject.ReadString(bytesRead);
                callback(CurrentReadString);
            }
        }




    }
}
