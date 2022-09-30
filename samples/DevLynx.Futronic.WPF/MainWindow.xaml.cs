using DevLynx.Futronic.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace DevLynx.Futronic.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            Loaded += (s, e) => Initialize();
        }

        FutronicDeviceManager FutronicManager { get; set; }
        FingerprintDevice Device { get; set; }

        void Initialize()
        {
            FutronicManager = new FutronicDeviceManager();
            FutronicManager.DeviceReady += DeviceReady;

            FutronicManager.Start();
        }

        private void DeviceReady(object sender, FingerprintDevice e)
        {
            if (Device != null)
                Device.Dispose();

            Device = e;
            Device.StartCapture();

            Dispatcher.Invoke(() =>
            {
                WriteableBitmap bitmap = new WriteableBitmap(Device.FrameWidth, Device.FrameHeight,
                96, 96, PixelFormats.Bgr32, null);
                _image.Source = bitmap;

                DispatcherTimer timer = new DispatcherTimer(DispatcherPriority.Background, Dispatcher);
                timer.Interval = TimeSpan.FromMilliseconds(60);
                timer.Start();

                timer.Tick += (s, e) =>
                {
                    Device.GetImage(bitmap);
                };
            });

           
        }
    }
}
