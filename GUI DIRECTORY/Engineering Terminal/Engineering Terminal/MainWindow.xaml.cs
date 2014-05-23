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

using commSockServer;
using engineeringTerminalTools;

namespace Engineering_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        commSockReceiver comSock;
        engineeringNetworkManager engNetMan;
        public MainWindow()
        {
            InitializeComponent();

            comSock = new commSockReceiver(40000);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            engNetMan = new engineeringNetworkManager(comSock, videoManager);   //Must happen after the form is loaded or else the videoManager will be a null pointer exception...
        }

        void videoManager_intendedCameraStatusChanged(videoManager.ToolboxControl.FeedID videoFeedID, bool feedState)
        {
            throw new NotImplementedException();
        }

        void videoManager_resetRequest(videoManager.ToolboxControl.FeedID videoFeedID)
        {
            throw new NotImplementedException();
        }

        void comSock_connectionLost()
        {
            Dispatcher.Invoke(() => connectionIndicator.connected = false);
        }

        void comSock_newConnection(bool obj)
        {
            Dispatcher.Invoke(() => connectionIndicator.connected = true);
        }

        void comSock_IncomingLine(string obj)
        {
            Dispatcher.Invoke(() => internetInComViz.addText(obj+"\r"));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
