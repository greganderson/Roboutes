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
using videoSocketTools;
using AForge.Video;
using AForge.Video.DirectShow;

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
        ptManager ptMan;
        cameraManager camMan;
        armManager armMan;

        Arduino backDrive;
        Arduino frontDrive;
        Arduino ptDuino;
        Arduino ArmDuino;
        Arduino HandDuino;

        managedDualVideoTransmitter panTiltTransmitter;

        public MainWindow()
        {
            InitializeComponent();
            driveIPLabel.Content = rocConstants.MCIP_DRIVE.ToString();
            armIPLabel.Content = rocConstants.MCIP_ARM.ToString();
            logisticsIPLabel.Content = rocConstants.MCIP_LOGISTICS;
            engineeringIPLabel.Content = rocConstants.MCIP_ENG.ToString();

            NetMan = networkManager.getInstance(incomingDriveLineManager, incomingEngLineManager, incomingArmLineManager);
            NetMan.DriveConnectionStatusChanged += NetMan_DriveConnectionStatusChanged;
            NetMan.EngineeringConnectionStatusChanged += NetMan_EngineeringConnectionStatusChanged;
            NetMan.ArmConnectionStatusChanged += NetMan_ArmConnectionStatusChanged;

            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();

            backDrive = ArduMan.getDriveBackArduino();
            frontDrive = ArduMan.getDriveFrontArduino();
            ptDuino = ArduMan.getPanTiltArduino();
            ArmDuino = ArduMan.getArmArduino();
            HandDuino = ArduMan.getHandArduino();

            backDrive.Data_Received += backDrive_Data_Received;
            frontDrive.Data_Received += frontDrive_Data_Received;
            ptDuino.Data_Received += ptDuino_Data_Received;
            ArmDuino.Data_Received += ArmDuino_Data_Received;

            driveMan = driveManager.getInstance(backDrive, frontDrive, NetMan);
            ptMan = ptManager.getInstance(ptDuino, NetMan);
            armMan = armManager.getInstance(ArmDuino, HandDuino, NetMan);

            camMan = cameraManager.getInstance();
            camMan.assignCameras();

            VideoCaptureDevice panTiltLeft;
            VideoCaptureDevice panTiltRight;
            if(camMan.getCamera(rocConstants.CAMS.PT_left, out panTiltLeft)  &&  camMan.getCamera(rocConstants.CAMS.PT_right, out panTiltRight)){ //if both the left and right cameras are found...
                panTiltTransmitter = new managedDualVideoTransmitter(panTiltLeft, panTiltRight, rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE_VIDEO_OCULUS);
                panTiltTransmitter.startTransmitting();
            }
        }

        void ArmDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => armCOMIN.addText(receivedData));
        }

        void NetMan_ArmConnectionStatusChanged(bool commSockIsConnected)
        {
            if (commSockIsConnected)
            {
                armConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                armConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        private void incomingArmLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming + "\n"));
            NetMan.write(rocConstants.COMID.ARMCOM, " armPING ");
        }

        void ptDuino_Data_Received(string receivedData)
        {
            Dispatcher.Invoke(() => panTiltCOMIN.addText(receivedData + "\r"));
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

        void NetMan_EngineeringConnectionStatusChanged(bool commSockIsConnected)
        {
            if (commSockIsConnected)
            {
                engineeringConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Green);
            }
            else
            {
                engineeringConnectedInd.setIndicatorState(toggleIndicator.indicatorState.Red);
            }
        }

        void incomingDriveLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming+"\n"));
            NetMan.write(rocConstants.COMID.DRIVECOM, " drivePING ");
        }

        void incomingEngLineManager(string incoming)
        {
            Dispatcher.Invoke(() => incomingInternet.addText(incoming + "\n"));
            NetMan.write(rocConstants.COMID.ENGCOM, " engPING ");
            if (incoming == "PT_TRANSMIT")
            {
                panTiltTransmitter.startTransmitting();
            }
            else if (incoming == "PT_STOP_TRANSMIT")
            {
                panTiltTransmitter.stop();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
