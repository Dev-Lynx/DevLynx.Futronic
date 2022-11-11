using Accessibility;
using DevLynx.Futronic.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace DevLynx.Futronic.WPF
{
    public class FutronicProvider : INotifyPropertyChanged
    {
        private WriteableBitmap _bitmap;
        private DispatcherTimer _timer;
        private FutronicDeviceManager _manager;
        private FingerprintDevice _device;

        private Dispatcher Dispatcher => Application.Current.Dispatcher;

        public WriteableBitmap Bitmap => _bitmap;
        public FingerprintDevice CurrentDevice => _device;

        private double _fps = 10;
        public virtual double FPS 
        {
            get => _fps;
            set
            {
                if (_fps > 60)
                    value = 60;
                else if (_fps <= 0)
                    value = 10;

                if (value == _fps)
                    return;

                _fps = value;

                if (_timer != null)
                    _timer.Interval = TimeSpan.FromSeconds(1 / FPS);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public FutronicProvider()
        {
            _manager = FutronicDeviceManager.Instance;

            _manager.DeviceReady += OnDeviceReady;
            _manager.DeviceDisconnected += OnDeviceDisconneted;

            if (_manager.Devices.Count > 0)
                OnDeviceReady(this, _manager.Devices.Values.First());
            else _manager.Start();
        }

        private void OnDeviceReady(object sender, FingerprintDevice e)
        {
            if (_device != null)
                return;

            _device = e;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDevice)));
            _device.StartCapture();
            _device.FingerPresent += OnFingerprintPresent;
            _device.FingerAbsent += OnFingerprintAbsent;

            Application.Current.Dispatcher.Invoke(() =>
            {
                _bitmap = new WriteableBitmap(_device.FrameWidth, _device.FrameHeight,
                    96, 96, PixelFormats.Bgr32, null);

                FutronicImageExtensions.ClearImage(_bitmap);
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Bitmap)));
            });
        }

        private void OnDeviceDisconneted(object sender, FingerprintDevice e)
        {
            if (_device == e)
            {
                _device.FingerPresent -= OnFingerprintPresent;
                _device.FingerAbsent -= OnFingerprintAbsent;
                _device = null;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentDevice)));

                // Get the next device
                if (_manager.Devices.Count > 0)
                {
                    var pair = _manager.Devices.FirstOrDefault();
                    OnDeviceReady(this, pair.Value);
                }
            }
        }

        private void OnFingerprintPresent(object sender, EventArgs e)
        {
            BeginCapture();
        }

        private void OnFingerprintAbsent(object sender, EventArgs e)
        {
            StopCapture();
        }

        void BeginCapture()
        {
            if (_timer == null)
            {
                _timer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
                _timer.Interval = TimeSpan.FromSeconds(1 / FPS);
                _timer.Tick += OnDispatcherTimerTick;
            }

            _timer.Start();
        }

        void StopCapture()
        {
            Dispatcher.Invoke(() =>
            {
                FutronicImageExtensions.ClearImage(_bitmap);
                _timer.Stop();
            });
        }

        private void OnDispatcherTimerTick(object sender, EventArgs e)
        {
            if (_device == null) return;
            _device.GetImage(_bitmap);
        }
    }
}
