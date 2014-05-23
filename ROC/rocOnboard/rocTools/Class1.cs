using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;
using commSockClient;
using ArduinoLibrary;
using AForge.Video;
using AForge.Video.DirectShow;
using videoSocketTools;

namespace rocTools
{
    public class networkManager
    {
        public delegate void ConnectionChangedEventHandler(bool commSockIsConnected);
        public event ConnectionChangedEventHandler DriveConnectionStatusChanged;
        public event ConnectionChangedEventHandler EngineeringConnectionStatusChanged;

        public delegate void incomingLineEventHandler(string incomingString);
        public event incomingLineEventHandler incomingDrive;
        public event incomingLineEventHandler incomingEngineering;
        // private incomingLineEventHandler incomingArm;
        // private incomingLineEventHandler incomingLogistics;

        private static networkManager NM;

        public static networkManager getInstance(incomingLineEventHandler incomingDriveLineHandler, incomingLineEventHandler incomingEngineeringLineHandler)
        {
            if (NM != null)
            {
                return NM;
            }
            else
            {
                NM = new networkManager(incomingDriveLineHandler, incomingEngineeringLineHandler);
                return NM;
            }
        }

        private commSockSender DRIVECOM;
        private commSockSender ENGCOM;
        // private commSockSender ARMCOM;
        // private commSockSender LOGISTICSCOM;

        private networkManager(incomingLineEventHandler incomingDriveLineHandler, incomingLineEventHandler incomingEngineeringLineHandler)
        {
            //Drive networking setup
            incomingDrive = incomingDriveLineHandler;
            DRIVECOM = new commSockSender("DRIVECOM");
            DRIVECOM.incomingLineEvent += DRIVECOM_incomingLineEvent;
            DRIVECOM.connectionStatusChanged += DRIVECOM_connectionStatusChanged;
            DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE);

            //Engineering networking setup
            incomingEngineering = incomingEngineeringLineHandler;
            ENGCOM = new commSockSender("ENGCOM");
            ENGCOM.incomingLineEvent += ENGCOM_incomingLineEvent;
            ENGCOM.connectionStatusChanged += ENGCOM_connectionStatusChanged;
            ENGCOM.beginConnect(rocConstants.MCIP_ENG, rocConstants.MCPORT_ENGINEERING);
        }

        void ENGCOM_connectionStatusChanged(bool commSockIsConnected)
        {
            if (EngineeringConnectionStatusChanged != null)
            {
                EngineeringConnectionStatusChanged(commSockIsConnected);
            }
        }

        void ENGCOM_incomingLineEvent(string obj)
        {
            if (incomingEngineering != null)
            {
                incomingEngineering(obj);
            }
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

        public void disconnect(rocConstants.COMID ID)
        {
            switch (ID)
            {
                case rocConstants.COMID.DRIVECOM:
                    DRIVECOM.disconnect();
                    break;
                case rocConstants.COMID.ENGCOM:
                    ENGCOM.disconnect();
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
                    DRIVECOM.beginConnect(rocConstants.MCIP_DRIVE, rocConstants.MCPORT_DRIVE);
                    break;
                case rocConstants.COMID.ENGCOM:
                    ENGCOM.beginConnect(rocConstants.MCIP_ENG, rocConstants.MCPORT_ENGINEERING);
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
                case rocConstants.COMID.ENGCOM:
                    Message = Message.Replace("\n", ""); //Do not allow \n to be transmitted... I think this is dealt with elsewhere, but this is safe...
                    ENGCOM.sendMessage(Message);
                    break;
            }
        }

    }

    public static class rocConstants
    {
        public static readonly IPAddress MCIP_DRIVE = IPAddress.Parse("155.99.167.9");
        public static readonly IPAddress MCIP_ENG = IPAddress.Parse("155.99.229.90");
        public static readonly string MCIP_ARM = "XXX.XXX.XXX.XXX";
        public static readonly string MCIP_LOGISTICS = "XXX.XXX.XXX.XXX";

        public static readonly int MCPORT_DRIVE = 35000;
        public static readonly int MCPORT_ENGINEERING = 40000;
        public static readonly int MCPORT_DRIVE_VIDEO_OCULUS = 45000;

        public enum COMID
        {
            DRIVECOM = 0,
            ARMCOM = 1,
            LOGISTICSCOM = 2,
            ENGCOM = 3
        };

        public static int[] defaultCameraAssignments = new int[] { 0, 1 };

        public enum CAMS
        {
            PT_left = 1,
            PT_right = 0,
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
            backDriveDuino.Data_Received += backDriveDuino_Data_Received;

            netMan = _netMan;
            netMan.incomingDrive += netMan_incomingDrive;
        }

        void netMan_incomingDrive(string incomingString)
        {
            if (incomingString.StartsWith("R:"))
            {
                incomingString = incomingString.Replace("R:", "");
                int newRightSpeed;
                if (int.TryParse(incomingString, out newRightSpeed))
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

    public class ptManager
    {
        private Arduino ptDuino;
        private networkManager netMan;

        private static ptManager instance;
        public static ptManager getInstance(Arduino panTiltArduino, networkManager _netMan)
        {
            if (instance == null)
            {
                instance = new ptManager(panTiltArduino, _netMan);
            }
            return instance;
        }

        private ptManager(Arduino panTiltArduino, networkManager _netMan)
        {
            ptDuino = panTiltArduino;
            netMan = _netMan;
            ptDuino.Data_Received += ptDuino_Data_Received;
            netMan.incomingDrive += netMan_incomingDrive;
        }

        void netMan_incomingDrive(string incomingString)
        {
            if (incomingString.StartsWith("Y:"))
            {
                incomingString = incomingString.Replace("Y:", "");
                int newYaw;
                if (int.TryParse(incomingString, out newYaw))
                {
                    updateYaw(newYaw);
                }
            }
            else if (incomingString.StartsWith("P:"))
            {
                incomingString = incomingString.Replace("P:", "");
                int newPitch;
                if (int.TryParse(incomingString, out newPitch))
                {
                    updatePitch(newPitch);
                }
            }
        }

        private void updatePitch(int newPitch)
        {
            ptDuino.write("P:" + newPitch);
        }

        private void updateYaw(int newYaw)
        {
            ptDuino.write("Y:" + newYaw);
        }

        void ptDuino_Data_Received(string receivedData)
        {
            //throw new NotImplementedException();
        }
    }

    public class cameraManager
    {
        private FilterInfoCollection videoDevices;
        private Dictionary<String, VideoCaptureDevice> cameraMap;


        private static cameraManager instance;
        public static cameraManager getInstance()
        {
            if (instance == null)
            {
                instance = new cameraManager();
            }
            return instance;
        }

        private cameraManager()
        {
            videoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            cameraMap = new Dictionary<string, VideoCaptureDevice>();
        }

        /// <summary>
        /// must be called before any cameras are retrieved!!! The assignmentSheet tells which cameras are which and corespond with the monikerString. Uses the default assignmentSheet if none is passed in.
        /// returns false if there were less cameras plugged in than are supposed to be assigned to or if something went wrong.
        /// assignmentSheet KEY: firstVal = PT_left , secondVal = PT_right
        /// </summary>
        public bool assignCameras(int[] assignmentSheet)
        {
            bool toReturn = true;
            try
            {
                if (assignmentSheet.Count() < videoDevices.Count)
                {
                    toReturn = false;
                }

                for (int i = 0; i < videoDevices.Count; i++)
                {
                    cameraMap.Add((((rocConstants.CAMS)i).ToString()), new VideoCaptureDevice(videoDevices[assignmentSheet[i]].MonikerString)); //really complicated way to just add the cameras in the order specified by the assignmentSheet 
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        /// <summary>
        /// must be called before any cameras are retrieved!!! The assignmentSheet tells which cameras are which and corespond with the monikerString. Uses the default assignmentSheet if none is passed in.
        /// returns false if there were less cameras plugged in than are supposed to be assigned to or if something went wrong.
        /// assignmentSheet KEY: firstVal = PT_left , secondVal = PT_right
        /// </summary>
        public bool assignCameras()
        {
            bool toReturn = true;
            try
            {
                int[] assignmentSheet = rocConstants.defaultCameraAssignments;
                if (assignmentSheet.Count() < videoDevices.Count)
                {
                    toReturn = false;
                }

                for (int i = 0; i < videoDevices.Count; i++)
                {
                    cameraMap.Add((((rocConstants.CAMS)i).ToString()), new VideoCaptureDevice(videoDevices[assignmentSheet[i]].MonikerString)); //really complicated way to just add the cameras in the order specified by the assignmentSheet 
                }
            }
            catch
            {
                toReturn = false;
            }
            return toReturn;
        }

        public bool getCamera(rocConstants.CAMS cameraID, out VideoCaptureDevice camera)
        {
            try
            {
                camera = cameraMap[cameraID.ToString()];
            }
            catch
            {
                camera = null;
                return false;
            }
            return true;
        }
    }
}
