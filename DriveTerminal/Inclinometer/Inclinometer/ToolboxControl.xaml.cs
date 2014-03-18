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

namespace Inclinometer {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Inclinometer", true)]
    public partial class ToolboxControl : UserControl {

        private bool rollLight;
		/// <summary>
		/// Returns true if the roll warning light is on, false otherwise.  Sets
        /// the light to on or off.
		/// </summary>
        public bool RollWarningLight {
            get {
                return rollLight;
            }
            set {
                if (value) {
					rollIndicatorOff.Visibility = System.Windows.Visibility.Hidden;
					rollIndicatorOn.Visibility = System.Windows.Visibility.Visible;
                    rollLight = true;
                }
                else {
					rollIndicatorOff.Visibility = System.Windows.Visibility.Visible;
					rollIndicatorOn.Visibility = System.Windows.Visibility.Hidden;
                    rollLight = false;
                }
            }
        }

        private bool pitchLight;
		/// <summary>
		/// Returns true if the pitch warning light is on, false otherwise.  Sets
        /// the light to on or off.
		/// </summary>
        public bool PitchWarningLight {
            get {
                return pitchLight;
            }
            set {
                if (value) {
					pitchIndicatorOff.Visibility = System.Windows.Visibility.Hidden;
					pitchIndicatorOn.Visibility = System.Windows.Visibility.Visible;
                    pitchLight = true;
                }
                else {
					pitchIndicatorOff.Visibility = System.Windows.Visibility.Visible;
					pitchIndicatorOn.Visibility = System.Windows.Visibility.Hidden;
                    pitchLight = false;
                }
            }
        }

        public ToolboxControl() {
            InitializeComponent();

            pitchIndicatorOff.Visibility = System.Windows.Visibility.Visible;
            pitchIndicatorOn.Visibility = System.Windows.Visibility.Hidden;
            rollIndicatorOff.Visibility = System.Windows.Visibility.Visible;
            rollIndicatorOn.Visibility = System.Windows.Visibility.Hidden;

            rollLight = false;
            pitchLight = false;
        }
    }
}
