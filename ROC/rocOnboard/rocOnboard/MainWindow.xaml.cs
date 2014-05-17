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

using rocTools;
using ArduinoLibrary;

namespace rocOnboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        networkManager NetMan;
        ArduinoManager ArduMan;
        driveManager driveMan;

        Arduino backDrive;
        Arduino frontDrive;

        public MainWindow()
        {
            InitializeComponent();
            driveIPLabel.Content = rocConstants.MCIP_DRIVE.ToString();
            armIPLabel.Content = rocConstants.MCIP_ARM;
            logisticsIPLabel.Content = rocConstants.MCIP_LOGISTICS;

            NetMan = networkManager.getInstance(incomingDriveLineManager);
            NetMan.DriveConnectionStatusChanged += NetMan_DriveConnectionStatusChanged;

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();

            backDrive = ArduMan.getDriveBackArduino();
            frontDrive = ArduMan.getDriveFrontArduino();
            backDrive.Data_Received += backDrive_Data_Received;
            frontDrive.Data_Received += frontDrive_Data_Received;

            driveMan = driveManager.getInstance(backDrive, frontDrive, NetMan);
        }

        void frontDrive_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => driveFrontCOMIN.addText(receivedData+"\r"));
        }

        void backDrive_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => driveBackCOMIN.addText(receivedData + "\r"));
        }

        void NetMan_DriveConnectionStatusChanged(bool commSockIsConnected)
        {
            if(commSockIsConnected){
                driveConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else{
                driveConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        void incomingDriveLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming+"\n"));
            NetMan.write(rocConstants.COMID.DRIVECOM, " PING ");
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
