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

using XboxController;
using commSockServer;

namespace DriveTerminal {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        XboxController.XboxController xboxController;
        commSockReceiver comSock;

        public MainWindow() {
            InitializeComponent();
            xboxController = new XboxController.XboxController();
            pilotPreferences.xboxController = xboxController;
            comSock = new commSockReceiver(35000); //TODO: Make a little network management window, maybe it shows when the start button is pressed?
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection; 
            comSock.beginAccept();
        }

        void comSock_newConnection(bool obj) {
            Dispatcher.Invoke(() => connectionIndicator.connected = obj);
        }

        void comSock_IncomingLine(string obj) {
            Dispatcher.Invoke(() => commViz.addText("IN: " + obj));//TODO: Add an event that fires when a disconnect is detected my the comSockReceiver, currently it only shows connections
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
