﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using CustomNetworking;

namespace CC
{
    /// <summary>
    /// THIS CLASS IS HERE STRICTLY FOR TESTING PURPOSES.  IT IS NOT USED IN THE MAIN PART OF THE PROGRAM.  
    /// IT IS USED TO TEST THE BOGGLE SERVER AND NOTHING MORE.
    /// </summary>

    #region DO NOT GRADE - NOT PART OF THE ASSIGNMENT
    public class ChatClientModel
    {

        private bool connected = false;
        // The socket used to communicate with the server.  If no connection has been
        // made yet, this is null.
        public StringSocket socket;

        // Register for this event to be modified when a line of text arrives.
        public event Action<String> IncomingLineEvent;

        /// <summary>
        /// Creates an ALREADY CONNECTED client model.
        /// </summary>
        public ChatClientModel(Socket s) {
            
                socket = new StringSocket(s, UTF8Encoding.Default);
                socket.BeginReceive(LineReceived, null);
                connected = true;
            
        }

        /// <summary>
        /// Creates a not yet connected client model.
        /// </summary>
        public ChatClientModel()
        {
            socket = null;
        }

        /// <summary>
        /// Connect to the server at the given hostname and port and with the give name.
        /// </summary>
        public void Connect(string hostname, int port)
        {
            Console.WriteLine("ChatClientModel Connect called");
            if (!connected) {
                if (socket == null)
                {
                    TcpClient client = new TcpClient(hostname, port);
                    socket = new StringSocket(client.Client, UTF8Encoding.Default);
                    socket.BeginReceive(LineReceived, null);
                }
            }
            else {
                throw new Exception("Already connected!");
            }
        }

        /// <summary>
        /// Send a line of text to the server.
        /// </summary>
        /// <param name="line"></param>
        public void SendMessage(String line)
        {
            try
            {
                if (socket != null)
                {
                    socket.BeginSend(line + "\n", (e, p) => { if (e != null) /*throw e*/; }, null); //do NOT throw the exception here, theres nothing you can do
                }
            }
            catch
            {

            }
        }

        /// <summary>
        /// Deal with an arriving line of text.
        /// </summary>
        private void LineReceived(String s, Exception e, object p)
        {
            Console.WriteLine("Line received in ChatClientModel: " + s);
            if (s == null) {
                Console.WriteLine("DISCONNECTION (no data) received in ChatClientModel");
                s = "DISCONNECTION!";
                socket.close();
                return;
            }
            if (IncomingLineEvent != null)
            {
                IncomingLineEvent(s);
            }
            socket.BeginReceive(LineReceived, null);
        }       
    }
}
    #endregion
