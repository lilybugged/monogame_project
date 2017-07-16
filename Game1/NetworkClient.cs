using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;
using Lidgren.Network;
using System.Reflection.Emit;

namespace Game1
{
    /// <summary>
    /// Used for LAN connections to sync data.
    /// </summary>
    public class NetworkClient
    {
        private NetClient client;
        private NetPeerConfiguration config;
        private int tick;
        NetIncomingMessage msgIn;
        public List<string> messageQueue = new List<string>();

        public NetworkClient()
        {
            config = new NetPeerConfiguration("Game1");

            //don't set the port for clients.
            //Create the NetPeer object with the configurations.
            client = new NetClient(config);

            //Start
            client.Start();

            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
            config.EnableMessageType(NetIncomingMessageType.Data);
            client.DiscoverLocalPeers(14242);
            tick = 0;
        }

        /// <summary>
        /// Handles messages and syncing.
        /// </summary>
        public void Update()
        {
            tick++;
            if (tick > 100) tick = 0;
            NetOutgoingMessage msgOut = client.CreateMessage();
            //write your data
            //msgOut.Write("Some Text");
            //client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);

            //SEND USER DATA
            while (messageQueue.Count>0)
            {
                msgOut = client.CreateMessage();
                msgOut.Write(messageQueue[0]);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
                messageQueue.RemoveAt(0);

            }
            //SYNC USER DATA
            while ((msgIn = client.ReadMessage()) != null){
                //create message type handling with a switch
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        String str;
                        str = msgIn.ReadString();
                        //Debug.WriteLine(msgIn.ReadString());
                        if (str.Length > 13 && (str).Substring(0, 12) == ("activate id:") && Game1.CLIENT_ID!=-1)
                        {
                            if (str.Split(' ').Length > 0)
                                Game1.CLIENT_ID = Int32.Parse(str.Substring(13));
                            Debug.WriteLine("CLIENT_ID SET: " + Game1.CLIENT_ID);
                        }
                        break;
                    case NetIncomingMessageType.DiscoveryResponse:
                        Console.WriteLine("Server answered with: {0}", msgIn.ReadString());
                        client.Connect(msgIn.SenderEndPoint);
                        break;
                    //All other types are for library related events (some examples)
                    case NetIncomingMessageType.ConnectionApproval:
                        //...
                        break;

                }
                //Recycle the message to create less garbage
                client.Recycle(msgIn);
            }
        }

        public void Draw()
        {

        }

        public int[,] parseintArray2D(string str, int rows, int cols)
        {
            int[,] list = new int[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int a = 0; a < cols; a++)
                {
                    list[i, a] = Int32.Parse(str.Split(';')[i].Split(',')[a]);
                }
            }
            return list;
        }
        public int[] parseintArray1D(string str, int length)
        {
            int[] list = new int[length];

            for (int i = 0; i < length; i++)
            {
                list[i] = Int32.Parse(str.Split(',')[i]);
            }
            return list;
        }
        public string formatArray2D(int[,] array)
        {
            string str = "";

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int a = 0; a < array.GetLength(1); a++)
                {
                    str += array[i, a];
                    if (a != array.GetLength(1) - 1)
                        str += ",";
                }
                if (i != array.GetLength(0) - 1)
                    str += ";";
            }
            return str;
        }
    }
}
