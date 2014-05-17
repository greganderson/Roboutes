using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using commSockClient;
using ArduinoLibrary;

namespace rocTools
{
    public class networkManager
    {
        public delegate void ConnectionChangedEventHandler(bool commSockIsConnected);
        public event ConnectionChangedEventHandler DriveConnectionStatusChanged;

        public delegate void incomingLineEventHandler(string incomingString);
        public event incomingLineEventHandler incomingDrive;
       // private incomingLineEventHandler incomingArm;
       // private incomingLineEventHandler incomingLogistics;

        private static networkManager NM;

        public static networkManager getInstance(incomingLineEventHandler incomingDriveLineHandler)
        {
            if (NM != null)
            {
                return NM;
            }
            else
            {
                NM = new networkManager(incomingDriveLineHandler);
                return NM;
            }
        }

        private commSockSender DRIVECOM;
       // private commSockSender ARMCOM;
       // private commSockSender LOGISTICSCOM;

        private networkManager(incomingLineEventHandler incomingDriveLineHandler)
        {
            incomingDrive = incomingDriveLineHandler;
            DRIVECOM = new commSockSender("DRIVECOM");
            DRIVECOM.incomingLineEvent += DRIVECOM_incomingLineEvent;
            DRIVECOM.connectionStatusChanged += DRIVECOM_connectionStatusChanged;
            DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE);
        }

        void DRIVECOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (DriveConnectionStatusChanged != null)
            {
                DriveConnectionStatusChanged(commSockIsConnected);
            }
        }

        void DRIVECOM_incomingLineEvent(string obj)
        {
            if (incomingDrive != null)
            {
                incomingDrive(obj);
            }
        }

        public void disconnect(rocConstants.COMID ID){
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    DRIVECOM.disconnect();
                    break;
            }
        }

        /// <summary>
        /// Only needs to be called after calling disconnect. Sockets try to connect as soon as the networkManager is online.
        /// </summary>
        /// <param name="ID"></param>
        public void connect(rocConstants.COMID ID)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE,rocConstants.MCPORT_DRIVE);
                    break;
            }
        }

        public void write(rocConstants.COMID ID, string Message)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    DRIVECOM.sendMessage(Message);
                    break;
            }
        }

    }

    public static class rocConstants
    {
        public static readonly IPAddress MCIP_DRIVE = IPAddress.Parse("155.99.165.85");
        public static readonly string MCIP_ARM = "XXX.XXX.XXX.XXX";
        public static readonly string MCIP_LOGISTICS = "XXX.XXX.XXX.XXX";

        public static readonly int MCPORT_DRIVE = 35000;

        public enum COMID
        {
            DRIVECOM = 0,
            ARMCOM = 1,
            LOGISTICSCOM = 2
        };
    }

    public class driveManager
    {
        private Arduino frontDriveDuino;
        private Arduino backDriveDuino;

        private volatile int leftSpeed = 0;
        private volatile int rightSpeed = 0;

        private networkManager netMan;

        private static driveManager instance;
        public static driveManager getInstance(Arduino backArduino, Arduino frontArduino, networkManager _netMan)
        {
            if (instance == null)
            {
                instance = new driveManager(backArduino, frontArduino, _netMan);
            }
            return instance;
        }

        private driveManager(Arduino backArduino, Arduino frontArduino, networkManager _netMan)
        {
            frontDriveDuino = frontArduino;
            backDriveDuino = backArduino;

            frontDriveDuino.Data_Received += frontDriveDuino_Data_Received;
            backDriveDuino.Data_Received+=backDriveDuino_Data_Received;

            netMan = _netMan;
            netMan.incomingDrive += netMan_incomingDrive;
        }

        void netMan_incomingDrive(string incomingString)
        {
            if (incomingString.StartsWith("R:"))
            {
                incomingString = incomingString.Replace("R:", "");
                int newRightSpeed;
                if (int.TryParse(incomingString,out newRightSpeed))
                {
                    updateRightSpeed(newRightSpeed);
                }
            }
            else if (incomingString.StartsWith("L:"))
            {
                incomingString = incomingString.Replace("L:", "");
                int newLeftSpeed;
                if (int.TryParse(incomingString, out newLeftSpeed))
                {
                    updateLeftSpeed(newLeftSpeed);
                }
            }
        }

        public void updateLeftSpeed(int speed)
        {
            frontDriveDuino.write("L:" + speed);
            backDriveDuino.write("L:" + speed);
        }

        public void updateRightSpeed(int speed)
        {
            frontDriveDuino.write("R:" + speed);
            backDriveDuino.write("R:" + speed);
        }


        private void frontDriveDuino_Data_Received(string receivedData)
        {
            //throw new NotImplementedException();
        }

        private void backDriveDuino_Data_Received(string receivedData)
        {
            //throw new NotImplementedException();
        }
    }
}
