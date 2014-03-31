using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace tempTest {
    class Program {
        static void Main(string[] args) {
            string[] COMPorts = SerialPort.GetPortNames();
            foreach (string s in COMPorts) {
                Console.WriteLine("Name: " + s);
                Console.WriteLine("Arduino?: "+ identify(s));
                Console.WriteLine("\n");
            }
            Console.Read();
        }

        public static bool identify(string COMPort_Name) {
            SerialPort temp = new SerialPort(COMPort_Name);
            temp.Open();
            temp.WriteLine("Marco");
            Thread.Sleep(500);
            string ID = temp.ReadExisting();
            if (ID.Contains("POLO->ARM")) {
                Console.WriteLine("IDENTIFIED ARM");
                return true;
            }
            return false;
        }
    }
}
