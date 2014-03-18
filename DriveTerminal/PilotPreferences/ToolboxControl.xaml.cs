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

namespace PilotPreferences {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("PilotPreferences", true)]
    public partial class ToolboxControl : UserControl {

		/// <summary>
		/// Returns the top speed percentage of the robot controls.
		/// </summary>
        public int TopSpeed {
            get {
                return Int32.Parse(speedLabel.Content.ToString().Substring(0, speedLabel.Content.ToString().Length-1));
            }
            private set {
                speedLabel.Content = value + "%";
            }
        }

		// Colors for the background
        private BrushConverter bc;
        private Brush greenBrush;
        private Brush redBrush;
        private Brush whiteBrush;
        private Brush blackBrush;
        private bool live;	// Keeps track of live value

		/// <summary>
		/// Returns whether the system is live.  Set to true to set system to live.
		/// </summary>
        public bool Live {
            get {
                return live;
            }
            set {
                if (value) {
                    liveLabel.Background = greenBrush;
                    liveLabel.Foreground = whiteBrush;
                    live = true;
                }
                else {
                    liveLabel.Background = redBrush;
                    liveLabel.Foreground = blackBrush;
                    live = false;
                }
            }
        }

        public ToolboxControl() {
            InitializeComponent();

            bc = new BrushConverter();
			greenBrush = (Brush)bc.ConvertFromString("#00ff00");
			redBrush = (Brush)bc.ConvertFromString("#ff0000");
			whiteBrush = (Brush)bc.ConvertFromString("#ffffff");
			blackBrush = (Brush)bc.ConvertFromString("#000000");

            live = false;

            speedSlider.Value = 100;
        }

        private void speedSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) {
            TopSpeed = (int)e.NewValue;
        }
    }
}
