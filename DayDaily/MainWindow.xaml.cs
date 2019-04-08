using DayDaily.ViewModel;
using System;
using System.Windows;
using System.Windows.Interop;

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
            Loaded += (s, e) =>
            {
                HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(this).Handle);
                source.AddHook(WndProc);
            };
        }

        public event EventHandler DisplayChanged;

        private static IntPtr WndProc(IntPtr hWnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_DISPLAYCHANGE = 0x007e;
            if (!(HwndSource.FromHwnd(hWnd).RootVisual is MainWindow window)) return IntPtr.Zero;
            switch (msg)
            {
                case WM_DISPLAYCHANGE:
                    window.DisplayChanged?.Invoke(window, new EventArgs());
                    break;
            }
            return IntPtr.Zero;
        }
    }
}