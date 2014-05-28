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

using System.IO;
using videoSocketTools;
using System.Net;
using System.Net.Sockets;


namespace videoViewerWindow {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class videoWindow : Window {
        private videoSocketReceiver VSR;
        private int port;
        private monitors targetMonitor;

        public videoWindow(int _port, monitors monitorToDisplayOn) {
            InitializeComponent();
            port = _port;
            VSR = new videoSocketReceiver(port, connectionCallback);
            targetMonitor = monitorToDisplayOn;
        }

        public void setIDLabel(string ID)
        {
            Dispatcher.Invoke(() => idLabel.Content = ID);
        }

        private void connectionCallback(bool connectionStatus) {
            if (connectionStatus) {
                VSR.frameReceived += VSR_frameReceived;
                VSR.connectionLost += VSR_connectionLost;
            }
            else {
                VSR.close();
                VSR = new videoSocketReceiver(port, connectionCallback);
            }
        }

        private void VSR_connectionLost() {
            Console.WriteLine("video connection Loss Detected on Server Side, autereconnect routine engaged");
            VSR.close();
            VSR = new videoSocketReceiver(port, connectionCallback);
        }

        private void VSR_frameReceived(byte[] newFrame) {
            if (newFrame.Length != null && newFrame.Length > 0) {
                displayFrame(newFrame);
            }
        }

        public void displayFrame(byte[] frame) {
            try {
                Dispatcher.Invoke((Action)(() =>
                {
                    ImageSource IS = ByteImageConverter.ByteToImage(frame);
                    imageBox.Source = IS; //TODO: stack overflow???
                    /*BitmapImage bi = IS as BitmapImage;
                    bi.StreamSource.Dispose();*/
                }));
                
            }
            catch {
                return; //do nothing...
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switch (targetMonitor)
            {
                case monitors.secondMonitor:
                    this.MaximizeToSecondaryMonitor();
                    break;
                case monitors.thirdMonitor:
                    this.MaximizeToThirdMonitor();
                    break;
            }
        }

        private void MaximizeToSecondaryMonitor() {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null) {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }
        }

        private void MaximizeToThirdMonitor()
        {
            System.Windows.Forms.Screen secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();
            System.Windows.Forms.Screen tertiaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => s != secondaryScreen).FirstOrDefault();

            if (tertiaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = tertiaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }

            else if (secondaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }
        }

        public enum monitors
        {
            secondMonitor,
            thirdMonitor
        }

    }

    public class ByteImageConverter {

        public static ImageSource ByteToImage(byte[] imageData) {
            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;
        }


       /* MemoryStream MS;

        public ByteImageConverter()
        {
            MS = new MemoryStream();
        }

       /* public ImageSource ByteToImage(byte[] imageData)
        {
            MS.Dispose();

            BitmapImage biImg = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            biImg.BeginInit();
            biImg.StreamSource = ms;
            biImg.EndInit();

            ImageSource imgSrc = biImg as ImageSource;

            return imgSrc;

        }*/

    }
}
