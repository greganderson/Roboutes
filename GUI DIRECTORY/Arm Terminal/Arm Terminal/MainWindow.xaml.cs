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

using ArmControlTools;
using XboxController;
using commSockServer;

namespace Arm_Terminal
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxController;

        commSockReceiver comSock;

        armInputManager armInputMan;
        armCommandTransmitter armTransmit;

        public MainWindow()
        {
            InitializeComponent();
            inputOnlineInd.setIndicatorState(toggleIndicator.indicatorState.Red);

            xboxController = new XboxController.XboxController();

            comSock = new commSockReceiver(35002);
            comSock.IncomingLine += comSock_IncomingLine;
            comSock.newConnection += comSock_newConnection;
            comSock.connectionLost += comSock_connectionLost;
            comSock.beginAccept();

            armInputMan = armInputManager.getInstance(xboxController,comSock);
            armInputMan.InputUnlockedEvent += inputUnlocked;

            armTransmit = new armCommandTransmitter(armInputMan, comSock);

            xboxControlMonitor.xboxController = xboxController;
            armSideView.armInputManager = armInputMan;
            armTopView.armInputManager = armInputMan;
            wristVisualizer.armInput = armInputMan;
        }

        private void inputUnlocked()
        {
            Dispatcher.Invoke(()=>inputOnlineInd.setIndicatorState(toggleIndicator.indicatorState.Green));
        }

        void comSock_connectionLost()
        {
            Dispatcher.Invoke(() =>netStatusInd.connected = false);
        }

        void comSock_newConnection(bool obj)
        {
            Dispatcher.Invoke(() =>netStatusInd.connected = true);
        }

        void comSock_IncomingLine(string obj)
        {
            Dispatcher.Invoke(() => internetInFeed.addText(obj));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
