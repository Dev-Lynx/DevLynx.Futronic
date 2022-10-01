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

        FutronicProvider FutronicProvider { get; set; }

        void Initialize()
        {
            FutronicProvider = new FutronicProvider();

            FutronicProvider.PropertyChanged += (s, e) =>
            {
                switch (e.PropertyName)
                {
                    case nameof(FutronicProvider.Bitmap):
                        _image.Source = FutronicProvider.Bitmap;
                        break;
                }
            };
        }
    }
}
