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
        private MediaCapture mediaCapture;
        public MediaCapture MediaCapture { get { return mediaCapture; } set { mediaCapture = value; OnNotifyPropertyChanged("MediaCapture"); } }
        private bool _isInitialized;
        private bool _isPreviewing;
        private bool _isRecording;

        private bool _externalCamera;
        private bool _mirroringPreview;
        private readonly DisplayRequest _displayRequest = new DisplayRequest();
        public string internalStatus;
        public InMemoryRandomAccessStream Slika { get; set; }

        public SoftwareBitmapSource SlikaBitmap { get; set; }
        public CaptureElement PreviewControl { get; set; }
        public CameraHelper(CaptureElement previewControl)
        {
            PreviewControl = previewControl;
        }
        public async Task InitializeCameraAsync()
        {
            if (MediaCapture == null)
            {
                // Get available devices for capturing pictures
                var allVideoDevices = await DeviceInformation.FindAllAsync(DeviceClass.VideoCapture);
                // Get the desired camera by panel
                DeviceInformation cameraDevice =
                    allVideoDevices.FirstOrDefault(x => x.EnclosureLocation != null &&
                    x.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Back);
                // If there is no camera on the specified panel, get any camera
                cameraDevice = cameraDevice ?? allVideoDevices.FirstOrDefault();
                if (cameraDevice == null)
                {
                    internalStatus = "No camera device found.";
                    return;
                }
                // Create MediaCapture and its settings
                MediaCapture = new MediaCapture();
                // Register for a notification when video recording has reached the maximum time and when something goes wrong
                var mediaInitSettings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
                // Initialize MediaCapture
                try
                {
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

                // If initialization succeeded, start the preview
                if (_isInitialized)
                {
                    // Figure out where the camera is located
                    if (cameraDevice.EnclosureLocation == null || cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Unknown)
                    {
                        // No information on the location of the camera, assume it's an external camera, not integrated on the device
                        _externalCamera = true;
                    }
                    else
                    {
                        // Camera is fixed on the device
                        _externalCamera = false;

                        // Only mirror the preview if the camera is on the front panel
                        _mirroringPreview = (cameraDevice.EnclosureLocation.Panel == Windows.Devices.Enumeration.Panel.Front);
                    }

                    await StartPreviewAsync();

                    //UpdateCaptureControls();
                }
            }
        }

        private async Task StartPreviewAsync()
        {
            // Prevent the device from sleeping while the preview is running
            _displayRequest.RequestActive();

            // Set the preview source in the UI and mirror it if necessary
            PreviewControl.Source = MediaCapture;
            PreviewControl.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            // Start the preview
            try
            {
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
            Slika = new InMemoryRandomAccessStream();
            try
            {
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

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnNotifyPropertyChanged([CallerMemberName] string memberName = "")
        {
            //? je skracena verzija ako nije null
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(memberName));
        }
    }
}
