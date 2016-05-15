using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Graphics.Imaging;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace ExterniUredjajKlijentApp.Uposlenici.Helper
{
    public class CameraHelper:INotifyPropertyChanged
    {
        //meida capture, glavna variabla koja cuva api
        private MediaCapture mediaCapture;
        public MediaCapture MediaCapture { get { return mediaCapture; } set { mediaCapture = value; OnNotifyPropertyChanged("MediaCapture"); } }
        //samo za postavke kamere
        private bool _isInitialized;
        private bool _isPreviewing;
        private bool _isRecording;
        private bool _externalCamera;
        private bool _mirroringPreview;
        private readonly DisplayRequest _displayRequest = new DisplayRequest();
        public string internalStatus;
        //Ovdje se cuva zadnja uslikana slika Stream Verzija
        public InMemoryRandomAccessStream Slika { get; set; }
        //Ovdje se cuva zadnja uslikana slika Bitmap Verzija
        public SoftwareBitmapSource SlikaBitmap { get; set; }
        //Kontrola u view koja prikazuje trenutno stanje kamere, zaobilazak binding
        public CaptureElement PreviewControl { get; set; }

        public CameraHelper(CaptureElement previewControl)
        {
            PreviewControl = previewControl;
        }
        public async Task InitializeCameraAsync()
        {
            if (MediaCapture == null)
            {
                // daj sve devices, slicno kao i serial
                var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                // pokusace prvo pozadinsku kameru mobitela
                DeviceInformation cameraDevice =
                    allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null &&
                    x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
                // Ako je ne nadje prva kamera koja ima onda se uzima, tj spojena usb kamera
                cameraDevice = cameraDevice ?? allVideoDevices.FirstOrDefault();
                //negdje zabiljeziti statuse kad kamera ne radi ili varijabla ili exception, sta je vise pogodno
                if (cameraDevice == null)
                {
                    internalStatus = "No camera device found.";
                    return;
                }
                //Inicijalizacija api za mediacapture
                MediaCapture = new MediaCapture();
                var mediaInitSettings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
                try
                {
                    //incijalizirati kameru
                    await MediaCapture.InitializeAsync(mediaInitSettings);
                    _isInitialized = true;
                }
                catch (UnauthorizedAccessException)
                {
                    internalStatus = ("The app was denied access to the camera");
                }
                catch (Exception ex)
                {
                    internalStatus = ("Exception when initializing MediaCapture with "+ cameraDevice.Id+ ex.ToString());
                }

                // Ako se incijalizirala, prikazati preview u preview kontroli
                if (_isInitialized)
                {
                    // Kako baratati sa naopakim kamerama, da li prednju ili zadnju kameru na mobitelu treba flipati
                    if (cameraDevice.EnclosureLocation == null || cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                    {
                        _externalCamera = true;
                    }
                    else
                    {
                        _externalCamera = false;
                        _mirroringPreview = (cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
                    }
                    await StartPreviewAsync();
                }
            }
        }

        private async Task StartPreviewAsync()
        {
            // dok radi da ne zaspe kamera zbog neaktivnosti
            _displayRequest.RequestActive();
            // Kontrola u view krsenje mvvm da se postavi source na trenutni izlaz kamere
            PreviewControl.Source = MediaCapture;
            //flipanje slike po potrebi
            PreviewControl.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;
            try
            {
                //pokrenuti preview
                await MediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (Exception ex)
            {
                internalStatus = ("Exception when starting the preview:"+ ex.ToString());
            }

        }

        public async Task TakePhotoAsync(Action<SoftwareBitmapSource> callback)
        {
            //kada uslika postaviti svoj stream 
            Slika = new InMemoryRandomAccessStream();
            try
            {
                //konvertovati uslikano u Software bitmap da se moze prikazati u image kontroli
                await MediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateBmp(), Slika);
                BitmapDecoder decoder = await BitmapDecoder.CreateAsync(Slika);
                SoftwareBitmap softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                SoftwareBitmap softwareBitmapBGR8 = SoftwareBitmap.Convert(softwareBitmap,
        BitmapPixelFormat.Bgra8,
        BitmapAlphaMode.Premultiplied);
                 SlikaBitmap = new SoftwareBitmapSource();
                await SlikaBitmap.SetBitmapAsync(softwareBitmapBGR8);
                callback(SlikaBitmap);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception when taking a photo: {0}", ex.ToString());
            }
        }
        //observer pokusaj bindinga preview kontrole koja ne slusa
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
