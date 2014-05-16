using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XboxController;
using System.Threading;
using commSockServer;

namespace driveTools
{
    public class driveTransmitter {
        Timer sendTimer;
        private driveInputManager driveInput;
        private commSockReceiver CSR;

        int leftVal = 0;
        int rightVal = 0;
        int oldLeftVal = 0;
        int oldRightVal = 0;
        object sync = 1;

        public driveTransmitter(driveInputManager _driveInput, commSockReceiver _CSR) {
            CSR = _CSR;
            driveInput = _driveInput;
            driveInput.newLeftValue += driveInput_newLeftValue;
            driveInput.newRightValue += driveInput_newRightValue;
            sendTimer = new Timer(sendTimerCallback, null, 0, 100);
        }

        private void sendTimerCallback(object state) {
            lock (sync) {
                if (leftVal != oldLeftVal) {
                    oldLeftVal = leftVal;
                    CSR.write("L:" + leftVal);
                }

                if (rightVal != oldRightVal) {
                    oldRightVal = rightVal;
                    CSR.write("R:" + rightVal);
                }
            }
        }

        void driveInput_newRightValue(int obj) {
            lock (sync) {
                rightVal = obj;
            }
        }

        void driveInput_newLeftValue(int obj) {
            lock (sync) {
                leftVal = obj;
            }
        }
    }
    public class driveInputManager
    {
        XboxController.XboxController xboxController;

        public event Action<int> newLeftValue;
        public event Action<int> newRightValue;
        object sync = 1;
        private readonly string valFormat = "000";

        public driveInputManager(XboxController.XboxController _xboxController) {
            xboxController = _xboxController;
            xboxController.ThumbStickLeft += xboxController_ThumbStickLeft;
            xboxController.ThumbStickRight += xboxController_ThumbStickRight;
        }

        void xboxController_ThumbStickRight(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            float newRightY = vec.Item2.Map(-1, 1, 0, 100);
            if (newRightValue != null) {
                newRightValue((int)newRightY);
            }
        }

        void xboxController_ThumbStickLeft(object sender, EventArgs e) {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetLeftThumbStick();
            float newLeftY = vec.Item2.Map(-1, 1, 0, 100);
            if (newLeftValue != null) {
                newLeftValue((int)newLeftY);
            }
        }
    }

    public static class ExtensionMethods {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget) {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }
    }
}
