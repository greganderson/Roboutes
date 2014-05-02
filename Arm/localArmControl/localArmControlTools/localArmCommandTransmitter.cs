using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using XboxController;
using ArduinoLibrary;
using System.Threading;

namespace ArmControlTools
{
    public class localArmCommandTransmitter
    {
        private Arduino armArduino;

        public localArmCommandTransmitter(Arduino _armArduino)
        {
            armArduino = _armArduino;
            
        }
    }

    public class armInputManager
    {
        XboxController.XboxController xboxController;
        private static armInputManager actualInstance;

        double commandedTurntableAngle;
        double turnTableRate;
        object turnTableSync = 1;

        double commandedElbowAngle;
        double elbowRate;
        object elbowSync = 1;

        double commandedShoulderAngle;
        double shoulderRate;
        object shoulderSync = 1;

        Thread elbowUpdateThread;
        Thread shoulderUpdateThread;
        Thread turntableUpdateThread;

        public static armInputManager getInstance(XboxController.XboxController _xboxController)
        {
            if (actualInstance != null)
            {
                return actualInstance;
            }
            else
            {
                actualInstance = new armInputManager(_xboxController);
                return actualInstance;
            }
        }

        private armInputManager(XboxController.XboxController _xboxController)
        {
            xboxController = _xboxController;
            xboxController.ThumbStickRight += xboxController_ThumbStickRight;
            xboxController.TriggerLeft += xboxController_TriggerLeft;
            xboxController.TriggerRight += xboxController_TriggerRight;
            turntableUpdateThread = new Thread(new ThreadStart(turnTableUpdate));
            turntableUpdateThread.Start();
            elbowUpdateThread = new Thread(new ThreadStart(elbowUpdate));
            elbowUpdateThread.Start();
            shoulderUpdateThread = new Thread(new ThreadStart(shoulderUpdate));
            shoulderUpdateThread.Start();
        }

        void shoulderUpdate()
        {
            while (true)
            {
                lock (shoulderSync)
                {
                    commandedShoulderAngle += shoulderRate;
                    Thread.Sleep(20);
                }
            }
        }

        void elbowUpdate()
        {
            while (true)
            {
                lock (elbowSync)
                {
                    commandedElbowAngle -= elbowRate;   //TODO: This is currently inverted, make it += instead to un-invert it
                    Thread.Sleep(20);
                }
            }
        }

        void turnTableUpdate()
        {
            while (true)
            {
                lock (turnTableSync)
                {
                    commandedTurntableAngle += turnTableRate;
                    commandedTurntableAngle = commandedTurntableAngle.Constrain(0, 90);
                    Thread.Sleep(20);
                }
            }
        }

        void xboxController_ThumbStickRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            Tuple<float, float> vec = args.GetRightThumbStick();
            double X = vec.Item1.Map(-1, 1, -2, 2);
            lock (turnTableSync)
            {
                turnTableRate = X;
            }

            double Y = vec.Item2.Map(-1, 1, -2, 2);
            lock (elbowSync)
            {
                elbowRate = Y;
            }
        }

        void xboxController_TriggerLeft(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            float val = args.GetLeftTrigger();
            val = val / 2;  //keep it slow
            lock (shoulderSync)
            {
                shoulderRate = -val;    //left trigger is down
            }
        }

        void xboxController_TriggerRight(object sender, EventArgs e)
        {
            XboxEventArgs args = (XboxEventArgs)e;
            float val = args.GetRightTrigger();
            val = val / 2;  //keep it slow
            lock (shoulderSync)
            {
                shoulderRate = val;    //right trigger is up
            }
        }
    }

    public static class ExtensionMethods
    {
        public static float Map(this float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Map(this double value, double fromSource, double toSource, double fromTarget, double toTarget)
        {
            return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
        }

        public static double Constrain(this double value, double min, double max)
        {
            if (value > max)
            {
                return max;
            }
            else if (value < min)
            {
                return min;
            }
            else
            {
                return value;
            }
        }
    }
}
