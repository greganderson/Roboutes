using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Control_Status {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Control_Status", true)]
    public partial class ControlStatus : UserControl {

        public delegate void activateButtonClickedEventHandler(Button sender);
        public event activateButtonClickedEventHandler activateButtonClicked;

        /// <summary>
        /// Specifies an indicator light
        /// </summary>
        public enum Indication_Lights { Main_Controller_Connected, Mini_Controller_Connected, Arm_Connected, GUI_Drive, Controller_Drive, Gripper_Input };

        public ControlStatus() {
            InitializeComponent();

            controllerDriveButton.Click += ButtonClicked;
            gripperInputButton.Click += ButtonClicked;
            guiDriveButton.Click += ButtonClicked;
        }

        private void ButtonClicked(object sender, RoutedEventArgs e) {
            if (activateButtonClicked != null)
                activateButtonClicked((Button)sender);
        }

        /// <summary>
        /// changes the state on an indicator light. specify the light and pass true (for on) or false (for off)
        /// </summary>
        /// <param name="light"></param>
        /// <param name="ON"></param>
        public void toggleLights(Indication_Lights light, bool ON) {
            switch (light) {
                case Indication_Lights.Main_Controller_Connected:
                    if (ON) {
                        Main_Controller_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        Main_Controller_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;

                case Indication_Lights.Mini_Controller_Connected:
                    if (ON) {
                        Mini_Controller_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        Mini_Controller_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;

                case Indication_Lights.Arm_Connected:
                    if (ON) {
                        Arm_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        Arm_Connected_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;

                case Indication_Lights.GUI_Drive:
                    if (ON) {
                        GUI_Drive_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        GUI_Drive_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;

                case Indication_Lights.Controller_Drive:
                    if (ON) {
                        Controller_Drive_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        Controller_Drive_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;

                case Indication_Lights.Gripper_Input:
                    if (ON) {
                        Gripper_Input_Indicator.Fill = new SolidColorBrush(Color.FromRgb(0, 255, 0));
                    }
                    else {
                        Gripper_Input_Indicator.Fill = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                    }
                    break;
            }
        }


    }
}
