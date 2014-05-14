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

namespace rocOnboard
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        networkManager NetMan;

        public MainWindow()
        {
            InitializeComponent();
            driveIPLabel.Content = rocConstants.MCIP_DRIVE.ToString();
            armIPLabel.Content = rocConstants.MCIP_ARM;
            logisticsIPLabel.Content = rocConstants.MCIP_LOGISTICS;

            NetMan = networkManager.getInstance(incomingDriveLineManager);
            NetMan.DriveConnectionStatusChanged += NetMan_DriveConnectionStatusChanged;
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
        }
    }
}
