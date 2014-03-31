using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArduinoLibrary;

namespace test2 {
    class Program {
        static void Main(string[] args) {
            ArduinoManager manager = ArduinoManager.Instance;
            manager.findArduinos();
            Arduino tester = manager.getArmArduino();
            Console.WriteLine("NAME: "+tester.name);
            Console.Read();
        }
    }
}
