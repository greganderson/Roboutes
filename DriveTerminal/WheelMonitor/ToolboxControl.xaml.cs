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

namespace WheelMonitor {
    /// <summary>
    /// A single Wheel Monitor.  Has a title, speed, current, stall indicator, slip indicator,
    /// and an image that changes if there are any problems.
    /// </summary>
    [ProvideToolboxControl("WheelMonitor", true)]
    public partial class ToolboxControl : UserControl {

		/// <summary>
		/// Title.
		/// </summary>
        public string Title {
            get {
                return title.Content.ToString();
            }
            set {
                title.Content = value;
            }
        }

		// Represents the highlighting on the actual wheel image.  
        private string green = "#00ff00";
        private string red   = "#ff0000";
		/// <summary>
		/// Returns whether there are any problems with the wheel at the time.
		/// </summary>
        public bool WheelError {
            get {
                return isSlipping || isStalling;
            }
            private set {
                if (WheelError) {
					BrushConverter bc = new BrushConverter();
					Brush brush = (Brush)bc.ConvertFromString(red);
					spinning.Fill = brush;
                    spinning.Opacity = .5;
                }
                else {
					BrushConverter bc = new BrushConverter();
					Brush brush = (Brush)bc.ConvertFromString(green);
					spinning.Fill = brush;
                    spinning.Opacity = .4;
                }
            }
        }

		/// <summary>
		/// Returns the speed of the wheel.  Set with an integer value to the desired m/s.
		/// </summary>
        public int Speed {
            get {
                return Int32.Parse(speed.Content.ToString().Split(' ')[0]);	// Make sure to remove m/s
            }
            set {
                speed.Content = value + " m/s";
            }
        }

		/// <summary>
		/// Returns the current draw of the wheel.  Set with an integer value to the desired mA.
		/// </summary>
        public int Current {
            get {
                return Int32.Parse(current.Content.ToString().Split(' ')[0]);	// Make sure to remove mA
            }
            set {
                current.Content = value + " mA";
            }
        }

		// Keeps track of and changes whether the indicator light is on or not
        private bool isStalling;
		/// <summary>
		/// Returns whether the wheel is stalling.  Set to true if wheel is stalling, false otherwise.
		/// </summary>
        public bool Stall {
            get {
                return stallOn.Visibility == System.Windows.Visibility.Visible;
            }
            set {
				// Turn light on
                if (value) {
                    stallOff.Visibility = System.Windows.Visibility.Hidden;
                    stallOn.Visibility = System.Windows.Visibility.Visible;
					isStalling = true;
                }
				// Turn light off
                else {
                    stallOn.Visibility = System.Windows.Visibility.Hidden;
                    stallOff.Visibility = System.Windows.Visibility.Visible;
					isStalling = false;
                }
				WheelError = true;
            }
        }

		// Keeps track of and changes whether the indicator light is on or not
        private bool isSlipping;
		/// <summary>
		/// Returns whether the wheel is slipping.  Set to true if wheel is slipping, false otherwise.
		/// </summary>
        public bool Slip {
            get {
                return slipOn.Visibility == System.Windows.Visibility.Visible;
            }
            set {
				// Turn light on
                if (value) {
                    slipOff.Visibility = System.Windows.Visibility.Hidden;
                    slipOn.Visibility = System.Windows.Visibility.Visible;
                    isSlipping = true;
                }
				// Turn light off
                else {
                    slipOn.Visibility = System.Windows.Visibility.Hidden;
                    slipOff.Visibility = System.Windows.Visibility.Visible;
                    isSlipping = false;
                }
                WheelError = true;
            }
        }

        public ToolboxControl() {
            InitializeComponent();

			// Initialize indicator buttons to off
			stallOff.Visibility = System.Windows.Visibility.Visible;
			stallOn.Visibility = System.Windows.Visibility.Hidden;
			slipOff.Visibility = System.Windows.Visibility.Visible;
			slipOn.Visibility = System.Windows.Visibility.Hidden;

            isStalling = false;
            isSlipping = false;
        }
    }
}
