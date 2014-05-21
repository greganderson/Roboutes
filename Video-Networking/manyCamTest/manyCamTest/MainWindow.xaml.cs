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

using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Controls;

namespace manyCamTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        VideoSourcePlayer cam1 = new VideoSourcePlayer();
        //VideoSourcePlayer cam2 = new VideoSourcePlayer();
       // VideoSourcePlayer cam3 = new VideoSourcePlayer();

        public MainWindow()
        {
            InitializeComponent();

            ////////////////////Video Config - Start//////////////////////
            FilterInfoCollection videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            //string toPrint = "";
            int camCount = 0;
            foreach (FilterInfo vidDevice in videoDevices)
            {
                //toPrint += vidDevice.MonikerString + "  \n  ";
                camCount++;
            }
            MessageBox.Show("Found: " + camCount + " cameras");

            VideoCaptureDevice cam1Source = new VideoCaptureDevice(videoDevices[0].MonikerString);
          //  VideoCaptureDevice cam2Source = new VideoCaptureDevice(videoDevices[1].MonikerString);
           // VideoCaptureDevice cam3Source = new VideoCaptureDevice(videoDevices[2].MonikerString);

            cam1.VideoSource = cam1Source;
           // cam2.VideoSource = cam2Source;
           // cam3.VideoSource = cam3Source;

            cam1Viewer.Child = cam1;
           // cam2Viewer.Child = cam2;
           // cam3Viewer.Child = cam3;

            cam1.Start();
           // cam2.Start();
           // cam3.Start();
        }
    }
}
