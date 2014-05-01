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
            Console.WriteLine("***XBOX CONTROLLER CONNECTED***");
        }

        void armDuino_Data_Received(string receivedData)
        {
            armComIn.addText(receivedData);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
