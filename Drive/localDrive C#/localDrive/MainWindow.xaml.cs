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

namespace localDrive
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XboxController.XboxController xboxCont;
        string valFormat = "000";
        Arduino frontDriveDuino;
        Arduino backDriveDuino;
        ArduinoManager ArduMan;
        float oldRightY = 0;
        float oldLeftY = 0;
        float deadzone = 15; // set lower for HIGHER resolution

        public MainWindow()
        {
            InitializeComponent();
            ArduMan = ArduinoManager.Instance;
            ArduMan.findArduinos();
            frontDriveDuino = ArduMan.getDriveFrontArduino();
            backDriveDuino = ArduMan.getDriveBackArduino();
            frontDriveDuino.Data_Received += frontDriveDuino_Data_Received;
            backDriveDuino.Data_Received += backDriveDuino_Data_Received;
            xboxCont = new XboxController.XboxController();
            xboxCont.ThumbStickLeft += xboxCont_ThumbStickLeft;
            xboxCont.ThumbStickRight += xboxCont_ThumbStickRight;

            backDuinoInViz.setTitle("BACK COM IN");
            backDuinoOutViz.setTitle("BACK COM OUT");
            frontDuinoInViz.setTitle("FRONT COM IN");
            frontDuinoOutViz.setTitle("FRONT COM OUT");
        }

        void backDriveDuino_Data_Received(string receivedData)
        {
            backDuinoInViz.addText("REC: " + receivedData);
        }

        void frontDriveDuino_Data_Received(string receivedData)
        {
            frontDuinoInViz.addText("REC: " + receivedData);
        }

        void xboxCont_ThumbStickRight(object sender, EventArgs e)
        {
            string toSend;
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetThumbStickRight();
            float newRightY = vec.Item2.Map(-1, 1, -255, 255);
            float rightDif = Math.Abs(newRightY - oldRightY);
            if (rightDif >= deadzone)
            {
                toSend = "R" + newRightY.ToString(valFormat);
                frontDuinoOutViz.addText("\nSENDING: " + toSend + "\n");
                backDuinoOutViz.addText("\nSENDING: " + toSend + "\n");
                frontDriveDuino.write(toSend);
                backDriveDuino.write(toSend);
            }
        }

        private void xboxCont_ThumbStickLeft(object sender, EventArgs e)
        {
            string toSend;
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetThumbStickLeft();
            float newLeftY = vec.Item2.Map(-1, 1, -255, 255);
            float leftDif = Math.Abs(newLeftY - oldLeftY);
            if (leftDif >= deadzone)
            {
                toSend = "L" + newLeftY.ToString(valFormat);
                frontDuinoOutViz.addText("\nSENDING: " + toSend + "\n");
                backDuinoOutViz.addText("\nSENDING: " + toSend + "\n");
                frontDriveDuino.write(toSend);
                backDriveDuino.write(toSend);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Environment.Exit(0);
        }
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
