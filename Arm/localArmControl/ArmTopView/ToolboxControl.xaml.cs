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

namespace ArmTopView {
    /// <summary>
    /// Interaction logic for ToolboxControl.xaml
    /// </summary>
    [ProvideToolboxControl("ArmTopView", true)]
    public partial class ArmTop : UserControl {
        public double maxLength = 260; //starting standard value
        public double maxRotation = 90; //starting standard value
        public ArmTop() {
            InitializeComponent();
        }

        public void updateActualArmAngle(double angle) {
            if (angle >= 0 && angle <= maxRotation) { //changes goal arm shoulder rotation angle
                aRec.RenderTransform = new RotateTransform(180 + angle);
            }
        }

        public void updateGoalArmAngle(double angle){
            if (angle >= 0 && angle <= maxRotation) { //changes goal arm shoulder rotation angle
                gRec.RenderTransform = new RotateTransform(180+angle);
            }
        }

        public void updateActualArmLength(double lengthPercentage) {
            if (lengthPercentage >= 0 && lengthPercentage <= 100) { //changes goal arm length
                aRec.Width = maxLength * (lengthPercentage / 100);
            }
        }

        public void updateGoalArmLength(double lengthPercentage) {
            if (lengthPercentage >= 0 && lengthPercentage <= 100) { //changes goal arm length
                gRec.Width = maxLength * (lengthPercentage / 100);
            }
        }
    }
}
