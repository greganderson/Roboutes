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
using ArduinoLibrary;

namespace localArmControl
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxController;

        Arduino armDuino;
        Arduino handDuino;
        ArduinoManager ArduMan;

        public MainWindow()
        {
            InitializeComponent();
            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();

            armDuino = ArduMan.getArmArduino(); //TODO: Setup the hand arduino (handDuino)
            armDuino.Data_Received += armDuino_Data_Received;

            Console.SetOut(consoleViz.getStreamLink()); //Show console output in gui
            Console.WriteLine("***Arm Control Booted***");
            xboxController = new XboxController.XboxController();
            xboxControllerMonitor.xboxController = xboxController;
            armSideView.XboxController = xboxController;
            armTopView.XboxController = xboxController;
            Console.WriteLine("***XBOX CONTROLLER CONNECTED***");
        }

        void armDuino_Data_Received(string receivedData)
        {
            armComIn.addText(receivedData);
            armComInHandler(receivedData);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void armComInHandler(string receivedData)
        {
            if (receivedData.Contains("Shoulder Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":")+1);
                int parsedVal;
                if(int.TryParse(toParse,out parsedVal)){
                    armSideView.updateActualShoulder(parsedVal);
                }
            }
            else if (receivedData.Contains("Elbow Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":") + 1);
                int parsedVal;
                if (int.TryParse(toParse, out parsedVal))
                {
                    armSideView.updateActualElbow(parsedVal);
                }
            }
            else if (receivedData.Contains("Turn Table Position:"))
            {
                string toParse = receivedData.Substring(receivedData.LastIndexOf(":") + 1);
                int parsedVal;
                if (int.TryParse(toParse, out parsedVal))
                {
                    armTopView.updateActualArmAngle(parsedVal);
                }
            }
        }
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Map(this double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
