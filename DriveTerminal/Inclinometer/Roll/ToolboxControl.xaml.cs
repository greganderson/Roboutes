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

namespace Roll {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("Roll", true)]
    public partial class ToolboxControl : UserControl {

		/// <summary>
		/// Sets the angle of roll rotation in degrees.
		/// </summary>
        public double Roll {
            set {
                rotate.Angle = value;
                rollImage.RenderTransform = rotate;
            }
        }

        private RotateTransform rotate;

        public ToolboxControl() {
            InitializeComponent();

            rotate = new RotateTransform();
        }
    }
}
