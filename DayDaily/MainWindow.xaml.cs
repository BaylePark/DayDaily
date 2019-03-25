using System;
using System.Drawing;
using System.IO;
using System.Windows;
using DayDaily.ViewModel;

namespace DayDaily
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Initializes a new instance of the MainWindow class.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            Closing += (s, e) => ViewModelLocator.Cleanup();
        }

        private double GetDPI()
        {
            double dpi = 1.0;
            using (Graphics g = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpi = g.DpiX / 96.0; 
            }
            return dpi;
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            if (e.Property == MinWidthProperty)
            {
                SetValue(MinWidthProperty, (double)e.NewValue / GetDPI());
            }
            else if (e.Property == MinHeightProperty)
            {
                SetValue(MinHeightProperty, (double)e.NewValue / GetDPI());
            }
        }
    }
}