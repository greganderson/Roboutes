using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace ArduinoLibrary
{
    public class ArduinoManager
    {

        private static Dictionary<string,Arduino> ArduinoMap;
        private static ArduinoManager instance;

        public static ArduinoManager Instance {
            get {
                if (instance == null) {
                    instance = new ArduinoManager();
                    ArduinoMap = new Dictionary<string, Arduino>();
                }
                return instance;
            }
        }

        private ArduinoManager() {

        }

        public void findArduinos() {
            string[] COMPorts = SerialPort.GetPortNames();
            foreach (string potentialArduino in COMPorts) {
                identify(ref ArduinoMap, potentialArduino);
            }
        }

        private bool identify(ref Dictionary<string, Arduino> _ArduinoMap, string _potentialArduino) {
            if (!_ArduinoMap.ContainsKey(_potentialArduino)) {
                SerialPort temp;
                try {
                    temp = new SerialPort(_potentialArduino);
                    temp.Open();
                    temp.WriteLine("Marco");
                    Thread.Sleep(500);
                    string ID = temp.ReadExisting();
                    if (ID.Contains("POLO->ARM")) {
                        _ArduinoMap.Add("ARM", new Arduino(temp, "ARM"));
                        return true;
                    }
                }
                catch {
                    return false;
                }
            }
            return false;
        }

        public Arduino getArmArduino() {
            return ArduinoMap["ARM"];
        }
        
    }
}
