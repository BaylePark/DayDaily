﻿using System;
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

namespace DayDaily.Views
{
    /// <summary>
    /// LoadingView.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoadingView : UserControl
    {
        public LoadingView()
        {
            InitializeComponent();
            Loaded += (s, e) =>
            {
            };
        }
        private void DisableAlert()
        {
            // Disable Alert
        }

        bool firstRun = true;

        private void Wb_DocumentCompleted(object sender, System.Windows.Forms.WebBrowserDocumentCompletedEventArgs e)
        {
            
        }
    }
}
