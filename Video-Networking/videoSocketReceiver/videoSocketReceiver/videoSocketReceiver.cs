using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Collections;
using System.Threading;

namespace videoSocketTools
{
    public class videoSocketReceiver
    {
        private NetworkStream NetStream;
        bool parsing = false;
        private int currentFrameSize = -1;
        private byte[] currentFrameBuffer;

        public delegate void frameReceivedHandler(byte[] newFrame);
        public event frameReceivedHandler frameReceived;

        Thread worker;

        public videoSocketReceiver(TcpClient connectedClient)
        {
            NetStream = connectedClient.GetStream();
            worker = new Thread(new ThreadStart(beginReceive));
        }

        public void start(){
            worker.Start();
        }

        private void beginReceive()
        {
            currentFrameSize = -1;
            while (!parsing)
            {
                byte[] tempBuffer = new byte[5] { 0,0,0,0,0 }; //start with zero value.

                NetStream.Read(tempBuffer, 0, tempBuffer.Length);
                string sizeTagcandidate = Encoding.UTF8.GetString(tempBuffer);
                if (sizeTagcandidate == "SIZE:") //look for the "SIZE:" tag
                {
                    string size = "";
                    while (true)
                    {
                        byte[] tempSizeBuffer = new byte[1];
                        NetStream.Read(tempSizeBuffer, 0, 1);
                        string received = Encoding.UTF8.GetString(tempSizeBuffer);
                        if (received != "Z") //"Z" is the delimiter that specifies the end of the size parameter
                        {
                            size += received;
                        }
                        else
                        {
                            break;
                        }
                    }
                    int parsedSize;
                    if (!int.TryParse(size, out parsedSize))
                    {
                        Console.WriteLine("Unable to parse supposed size parameter, this might break stuff, but HOPEFULLY another beginReceive will be called to fix stuff...");
                        break;
                    }
                    NetStream.Flush();
                    parsing = true;
                    currentFrameSize = parsedSize;
                    break;
                }
            }

            if (parsing)
            {
                currentFrameBuffer = new byte[currentFrameSize];
                List<byte> tempBuffer = new List<byte>();
                while (tempBuffer.Count < currentFrameSize)
                {
                    int readSize = NetStream.Read(currentFrameBuffer, 0, currentFrameSize - tempBuffer.Count); //by reading currentFrameSize-tempBuffer.Count you only read up to the possible number of bytes remaining in the image. This keeps you from reading into the next image
                    tempBuffer.AddRange(currentFrameBuffer.Take(readSize).ToArray());
                }
                
                if (frameReceived != null)
                {
                    frameReceived(tempBuffer.ToArray());
                }
                parsing = false;
                currentFrameSize = -1;
            }
            beginReceive(); //TODO: Commented out to just reaceive a frame at a time
        }
    }

    /*public class videoSocketReceiver
    {
        // Underlying socket.
        private Socket socket;

        // Array used to receive bytes from the underlying socket
        private byte[] receiveBytes;

        private int frameSize = -1;

        public delegate void frameReceivedEventHandler(byte[] image);
        public event frameReceivedEventHandler frameReceived;

        private List<byte[]> bigBuffer;

        /// ////////////////////////////////////
        private ArrayList arrayBuffer;
        private int imageSize = -1;
        /////////////////////////////////////


        public videoSocketReceiver(Socket connectedSocket) {
            socket = connectedSocket;
            receiveBytes = new byte[1024];
            bigBuffer = new List<byte[]>();
            arrayBuffer = new ArrayList();
        }

        public void BeginReceive() {
            //socket.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, BytesReceived, null);
            socket.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, BytesReceived2, null);
        }

        private void processImages() {
            int sizeDataLocation = -1;
            lock (bigBuffer) {
                if (bigBuffer.Count > 0) {//there is image data on the queue... We dont know if its complete
                    //1. find an image tag (size data) so we can start parsing...
                    if (frameSize == -1) {
                        int bigBufferIndex = 0;
                        for (bigBufferIndex = 0; bigBufferIndex < bigBuffer.Count; bigBufferIndex++) {
                            byte[] currentArray = bigBuffer[bigBufferIndex];
                            string currentArrayString = Encoding.UTF8.GetString(currentArray);
                            sizeDataLocation = (currentArrayString.LastIndexOf("SIZE:"));
                            if (sizeDataLocation > -1) {//sizeDataLocation == -1 if "SIZE:" was not found
                                string sizeNum = currentArrayString.Substring(sizeDataLocation + 5, 10);
                                if (!int.TryParse(sizeNum, out frameSize)) {
                                    Console.WriteLine("\nERROR: Failed to parse supposed frame size. Might have been fragmented. THIS BREAKS THE PROGRAM!!!");
                                    return; //maybe should call begin receive again?
                                }
                                break;
                            }
                        }
                        if (sizeDataLocation < 0) {//There is no size data in the buffer...
                            return;
                        }

                        //2. remove "junk" data in bigBuffer that comes before image data
                        for (int y = 0; y < bigBufferIndex; y++) {
                            bigBuffer.RemoveAt(0); //NOT at y. This always removes the first one, which is what we want...
                        }
                    }

                    //3. See if there is enough data in bigBuffer for an image.
                    int availableData = 0;
                    for (int t = 0; t < bigBuffer.Count; t++) {
                        availableData += bigBuffer[t].Count();
                        if (availableData >= frameSize) {
                            break;//There IS enough data in bigBuffer for the whole image
                        }
                    }
                    if (availableData < frameSize) {//There IS NOT enough data in bigBuffer for the whole image. wait for more.
                        return;
                    }
                    bigBuffer[0] = bigBuffer[0].Skip(sizeDataLocation + 11).ToArray(); //now this array JUST contains image data. Helps make parsing easier later on...

                    //4. get the image out
                    List<byte> imageData = new List<byte>();
                    while (frameSize > 0) {
                        for (int index = 0; index < bigBuffer.Count; index++) {
                            if (bigBuffer[index].Length > frameSize) {//Only some of the current byte[] of bigBuffer contains data. Only copy over data up to that point.
                                byte[] currentArray = bigBuffer[index];
                                imageData.AddRange(currentArray.Take(frameSize));
                                frameSize = 0;
                            }
                            else {//the entire current byte[] of bigBuffer contains image data. The end is not likely here.
                                imageData.AddRange(bigBuffer[index]);
                                frameSize -= bigBuffer[index].Count();
                            }
                        }
                    }

                    //5. got the image!
                    frameSize = -1;
                    byte[] imageBuffer = imageData.ToArray();
                    if (frameReceived != null) {
                        frameReceived(imageBuffer);
                    }
                }
            }
        }

        private void BytesReceived(IAsyncResult ar) {
            int count = socket.EndReceive(ar);
            lock (bigBuffer) {
                bigBuffer.Add(receiveBytes.Take(count).ToArray());
            }
            processImages();
            socket.BeginReceive(receiveBytes, 0, receiveBytes.Length, SocketFlags.None, BytesReceived, null);
            /*if (!parsing) {//look for size info
                string received = Encoding.UTF8.GetString(receiveBytes, 0, count);
                int sizeDataLocation = (received.LastIndexOf("SIZE:"));
                if (sizeDataLocation > -1) { //will == -1 if "SIZE:" was not found
                    string sizeNumber = received.Substring(sizeDataLocation + 1, 10);
                    if (!int.TryParse(sizeNumber, out bytesRemaining)) {
                        Console.WriteLine("\nERROR: Failed to parse supposed frame size. Might have been fragmented");
                        return; //maybe should call begin receive again?
                    }
                    parsing = true;
                    bigBuffer.Clear();//bigBuffer should only hold one frame at a time...
                    bigBuffer.Add(receiveBytes.Skip(sizeDataLocation + 11).Take(count-sizeDataLocation+11).ToArray());
                }
            }
            else {//if we are looking for data just add it to the bigData Buffer
                bytesRemaining -= count;
                if (bytesRemaining < 0) { //we finished getting the data we need AND ran over into other data.
                    bigBuffer.Add(receiveBytes.Take(bytesRemaining+count).ToArray()); //put the image data into the bigBuffer
                    bigBuffer.Add(receiveBytes.Skip(bytesRemaining + count).ToArray()); // add the fragment
                }
               // bigBuffer.Add(receiveBytes);
            }
        }

        private void BytesReceived2(IAsyncResult ar) {
            int count = socket.EndReceive(ar);
            lock (arrayBuffer) {
                arrayBuffer.AddRange(receiveBytes.Take(count).ToArray());
            }
            processImages2();
        }

        private void processImages2() {
            lock (arrayBuffer) {
                /*if(arrayBuffer.LastIndexOf(byte(48)){

                }
            }
        }
    }*/
}
