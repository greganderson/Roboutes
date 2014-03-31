using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;

namespace ArduinoLibrary {
    public class Arduino {

        private SerialPort ArduinoSerial;
        /// <summary>
        /// Reports data as a string once a newline ("\n") character is seen.
        /// </summary>
        /// <param name="receivedData"></param>
        public delegate void ArduinoDataReceivedEventHandler(List<string> receivedDataStrings);
        public event ArduinoDataReceivedEventHandler Data_Received;
        public string name;
        public string COMPORT;
        string currentString;

        public Arduino(SerialPort Arduino_Location, string _name) {
            currentString = "";

            if (!Arduino_Location.IsOpen) {
                throw new ArduinoImproperlyPassed("Arduino port must already be open (identified as an Arduino and whatnot) by the time it is passed into the Arduino Constructor");
            }
            COMPORT = Arduino_Location.PortName;
            ArduinoSerial = Arduino_Location;
            ArduinoSerial.DtrEnable = true;
            ArduinoSerial.DataReceived += ArduinoSerial_DataReceived;
            ArduinoSerial.ErrorReceived += ArduinoSerial_ErrorReceived;
            name = _name;
        }

        void ArduinoSerial_DataReceived(object sender, SerialDataReceivedEventArgs e) {
            List<string> receivedStringList = new List<string>();
            SerialPort temp = (SerialPort)sender;
            if (currentString.Length >= 10000) {
                currentString = null;
            }
            else {
                string[] tempString = (currentString + temp.ReadExisting()).Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
                foreach (string t in tempString) {
                    if (t == tempString.First()) {
                        receivedStringList.Add(t);
                        currentString = "";
                    }
                    else if (t != tempString.Last()) {
                        receivedStringList.Add(t);
                    }
                    else {
                        if (t.EndsWith("\r\n") || t.EndsWith("\n")) {
                            receivedStringList.Add(t);
                        }
                        else {
                            currentString += t;
                        }
                    }
                }
            }
            if (Data_Received != null) {
                Data_Received(receivedStringList);
            }
        }

        private void AppendToReceived(string toAppend) {

        }

        public void write(String data) {
            ArduinoSerial.WriteLine(data);
        }

        public void resetArduino() {
            Thread.Sleep(100);
            ArduinoSerial.DtrEnable = false;
            ArduinoSerial.DtrEnable = true;
            Thread.Sleep(250);
        }

        void ArduinoSerial_ErrorReceived(object sender, SerialErrorReceivedEventArgs e) {
            throw new ArduinoGeneralError((SerialPort)sender, e);
        }




    }

    public class ArduinoGeneralError : Exception {
        public SerialPort ArduinoSerialPortObject;
        public SerialErrorReceivedEventArgs eventArgs;
        public ArduinoGeneralError(SerialPort SP, SerialErrorReceivedEventArgs e) {
            ArduinoSerialPortObject = SP;
            eventArgs = e;
        }
    }

    public class ArduinoCOMCloseError : Exception {
        public ArduinoCOMCloseError(string ErrorMessage)
            : base(ErrorMessage) {
        }
    }

    public class ArduinoImproperlyPassed : Exception {
        public ArduinoImproperlyPassed(string ErrorMessage)
            : base(ErrorMessage) {
        }
    }
}
