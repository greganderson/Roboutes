using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenHardwareMonitor.Hardware;
using OpenHardwareMonitor.Collections;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            Computer thisComp = new Computer();
            thisComp.CPUEnabled = true;
            thisComp.Open();

            string result = "";

            foreach (var hardwareItem in thisComp.Hardware)
            {
                if (hardwareItem.HardwareType == HardwareType.CPU)
                {
                    hardwareItem.Update();
                    foreach (IHardware subhardware in hardwareItem.SubHardware)
                    {
                        subhardware.Update();
                    }

                    foreach (var sensor in hardwareItem.Sensors)
                    {
                        if (sensor.SensorType == SensorType.Temperature)
                        {
                            result += String.Format("Sensor: " + sensor.Name + " TEMP: "+ sensor.Value);
                        }
                    }
                }
            }
            Console.WriteLine("RESULT: "+result);
            Console.WriteLine("done");
            Console.Read();
        }


    }
}
