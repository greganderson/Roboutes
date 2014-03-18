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

namespace LiveDriveData {
    /// <summary>
    /// Live drive data component.  Keeps track of wheel monitoring.
    /// </summary>
    [ProvideToolboxControl("LiveDriveData", true)]
    public partial class ToolboxControl : UserControl {

		/// <summary>
		/// Returns whether the stuck indicator light is on or off.  Setting true
        /// will turn the light on, false will turn it off.
		/// </summary>
		public bool Stuck {
			get {
                return stuckOn.Visibility == System.Windows.Visibility.Visible;
			}
			set {
                if (value) {
                    stuckOn.Visibility = System.Windows.Visibility.Visible;
                    stuckOff.Visibility = System.Windows.Visibility.Hidden;
                }
                else {
                    stuckOff.Visibility = System.Windows.Visibility.Visible;
                    stuckOn.Visibility = System.Windows.Visibility.Hidden;
                }
			}
		}

		/// <summary>
		/// Returns the PID Goal in m/s.  Set with an integer to change the value to the desired m/s.
		/// </summary>
        public int PIDGoal {
            get {
                return Int32.Parse(pidGoal.Content.ToString().Split(' ')[0]);	// Make sure to remove m/s
            }
            set {
                pidGoal.Content = value + " m/s";
            }
        }

		/// <summary>
		/// Returns the PID Actual in m/s.  Set with an integer to change the value to the desired m/s.
		/// </summary>
        public int PIDActual {
            get {
                return Int32.Parse(pidActual.Content.ToString().Split(' ')[0]);	// Make sure to remove m/s
            }
            set {
                pidActual.Content = value + " m/s";
            }
        }

        public WheelMonitor.ToolboxControl UL;
        public WheelMonitor.ToolboxControl UR;
        public WheelMonitor.ToolboxControl LL;
        public WheelMonitor.ToolboxControl LR;

        public ToolboxControl() {
            InitializeComponent();

            UL = ul;
            UR = ur;
            LL = ll;
            LR = lr;

            UL.Title = "UL";
            UR.Title = "UR";
            LL.Title = "LL";
            LR.Title = "LR";

            stuckOff.Visibility = System.Windows.Visibility.Visible;
            stuckOn.Visibility = System.Windows.Visibility.Hidden;
        }

    }
}
