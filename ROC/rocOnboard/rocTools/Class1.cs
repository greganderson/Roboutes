using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using commSockClient;

namespace rocTools
{
    public class networkManager
    {
        public delegate void ConnectionChangedEventHandler(bool commSockIsConnected);
        public event ConnectionChangedEventHandler DriveConnectionStatusChanged;

        public delegate void incomingLineEventHandler(string incomingString);
        private incomingLineEventHandler incomingDrive;
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
}
