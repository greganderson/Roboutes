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

namespace ArmVideoComponent_Tester {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            vidComp.showLocalCam(0);
            //vidComp.setMJPEGVideoFeedSource("http://localhost:8080");
            //vidComp.StartVideo();
        }

        private void Window_Closed(object sender, EventArgs e) {
            Environment.Exit(0);
        }
    }
}
