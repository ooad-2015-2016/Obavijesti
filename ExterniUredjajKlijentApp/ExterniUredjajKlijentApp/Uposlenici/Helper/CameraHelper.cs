using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage.Streams;
using Windows.System.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace ExterniUredjajKlijentApp.Uposlenici.Helper
{
    public class CameraHelper
    {
        private MediaCapture _mediaCapture;
        private bool _isInitialized;
        private bool _isPreviewing;
        private bool _isRecording;

        private bool _externalCamera;
        private bool _mirroringPreview;
        private readonly DisplayRequest _displayRequest = new DisplayRequest();
        public string internalStatus;
        public InMemoryRandomAccessStream Slika { get; set; }

        CaptureElement PreviewControl{get;set;}


        public CameraHelper(CaptureElement cameraElement)
        {
            PreviewControl = cameraElement;
        }
        public async Task InitializeCameraAsync()
        {
            if (_mediaCapture == null)
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
                _mediaCapture = new MediaCapture();
                // Register for a notification when video recording has reached the maximum time and when something goes wrong
                var mediaInitSettings = new MediaCaptureInitializationSettings { VideoDeviceId = cameraDevice.Id };
                // Initialize MediaCapture
                try
                {
                    await _mediaCapture.InitializeAsync(mediaInitSettings);
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
            PreviewControl.Source = _mediaCapture;
            PreviewControl.FlowDirection = _mirroringPreview ? FlowDirection.RightToLeft : FlowDirection.LeftToRight;

            // Start the preview
            try
            {
                await _mediaCapture.StartPreviewAsync();
                _isPreviewing = true;
            }
            catch (Exception ex)
            {
                internalStatus = ("Exception when starting the preview:"+ ex.ToString());
            }

        }

        public async Task TakePhotoAsync()
        {
            Slika = new InMemoryRandomAccessStream();
            try
            {
                await _mediaCapture.CapturePhotoToStreamAsync(ImageEncodingProperties.CreateBmp(), Slika);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception when taking a photo: {0}", ex.ToString());
            }

        }

    }
}
