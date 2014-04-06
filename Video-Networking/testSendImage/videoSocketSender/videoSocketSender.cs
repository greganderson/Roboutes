﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace videoSocketTools
{
    public class videoSocketSender
    {
        private VideoCaptureDevice cameraSource;
        private IPAddress IPAddress;
        private int port;
        private TcpClient tcpClient;
        private Socket sendSocket;
        private volatile bool Transmitting = false;
        private long quality = 15; //15% is default quality
        private object qualitySync = 1;
        private volatile bool timeExpired = true; //when expired the frame can be sent (used to control FPS)
        private int FPS = 15;
        private object fpsSync = 1;
        Timer fpsTimer;

        public int transmissionFPS
        {
            get { return FPS; }
            set {
                if (value < 0)
                {
                    lock (fpsSync)
                    {
                        FPS = 1; //only go down to 1. Otherwise your dividing by zero...
                    }
                }
                else if (value > 1000)
                {
                    lock (fpsSync)
                    {
                        FPS = 1000; //only 1000 ms in a second...
                    }
                }
                else
                {
                    lock (fpsSync)
                    {
                        FPS = value;
                    }
                }
                lock (fpsSync)
                {
                    fpsTimer.Change(0, 1000 / FPS);
                }
            }
        }

        /// <summary>
        /// set the video quality to transmit, only accepts values from 0 to 100.
        /// </summary>
        public long transmissionQuality
        {
            get { return quality; } 
            set {
                    if (value>100)
                    {
                        lock (qualitySync)
                        {
                            quality = 100;
                        }
                    }
                    else if (value < 0)
                    {
                        lock (qualitySync)
                        {
                            quality = 0;
                        }
                    }
                    else
                    {
                        lock (qualitySync)
                        {
                            quality = value;
                        }
                    }
                }
        }

        public delegate void connectionLostEventHandler();
        public event connectionLostEventHandler connectionLost;


        public delegate void connectCallback(bool connectionStatus);

        public videoSocketSender(VideoCaptureDevice _cameraSource){
            lock (fpsSync)
            {
                fpsTimer = new Timer(timerCallback, null, 0, 1000 / FPS);
            }
            cameraSource = _cameraSource;
            cameraSource.NewFrame +=cameraSource_NewFrame;
            cameraSource.Start();
        }

        private void timerCallback(object state)
        {
            timeExpired = true;
        }

        public void beginConnect(IPAddress IP, int port, connectCallback callback){
            tcpClient = new TcpClient();
            tcpClient.BeginConnect(IP, port, connectedCallback, callback);
        }

        private void connectedCallback(IAsyncResult ar)
        {
            try
            {
                tcpClient.EndConnect(ar);
                if (tcpClient.Connected)
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    sendSocket = tcpClient.Client;
                    callback(true);
                }
                else
                {
                    connectCallback callback = (connectCallback)ar.AsyncState;
                    callback(false);
                }
            }
            catch
            {
                connectCallback callback = (connectCallback)ar.AsyncState;
                callback(false);
            }
        }

        public bool connect(IPAddress IP, int port)
        {
            tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("127.0.0.1"), port);
            sendSocket = tcpClient.Client;
            return true;
        }

        public bool beginTransmitting()
        {
            try
            {
                if (!tcpClient.Connected)
                {
                    return false;
                }
                Transmitting = true;
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void stop()
        {
            Transmitting = false;
        }

        private void cameraSource_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (Transmitting && timeExpired)
            {
                timeExpired = false;
                try
                {
                    byte[] jpegImage = Bitmap2JpegArray(eventArgs.Frame);
                    string headerBuilder = "SIZE:" + jpegImage.Length + "Z";
                    byte[] header = Encoding.ASCII.GetBytes(headerBuilder);
                    sendSocket.Send(header, header.Length, SocketFlags.None);
                    sendSocket.Send(jpegImage, jpegImage.Length, SocketFlags.None);
                }
                catch
                {
                    Transmitting = false;
                    if (connectionLost != null)
                    {
                        connectionLost();
                    }
                }
            }
        }

        private byte[] Bitmap2JpegArray(Bitmap Frame) //TODO:!!!! THIS IS WHERE QUALITY CAN BE DETERMINED!!!!
        {
            MemoryStream ms = new MemoryStream();
            long sendQuality;
            lock (qualitySync)
            {
                sendQuality = quality;
            }
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter qualityParameter = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, sendQuality);
            myEncoderParameters.Param[0] = qualityParameter;
            Frame.Save(ms, GetEncoder(ImageFormat.Jpeg), myEncoderParameters);
            byte[] toReturn = ms.ToArray();
            return toReturn;
        }

        private ImageCodecInfo GetEncoder(ImageFormat format)
        {

            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();

            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

    }
}