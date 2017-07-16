using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Game1
{
    public class NetworkClient
    {
        private NetClient client;
        private NetPeerConfiguration config;
        NetIncomingMessage msgIn;

        public NetworkClient()
        {
            config = new NetPeerConfiguration("Game1");

            //don't set the port for clients.
            //Create the NetPeer object with the configurations.
            client = new NetClient(config);

            //Start
            client.Start();

            config.EnableMessageType(NetIncomingMessageType.DiscoveryResponse);
<<<<<<< HEAD
            config.EnableMessageType(NetIncomingMessageType.Data);

            client.DiscoverLocalPeers(14242);
            tick = 0;
=======
            client.DiscoverLocalPeers(7777);
>>>>>>> parent of 99170b8... Server/client playerposition upsync
        }

        public void Update()
        {
<<<<<<< HEAD
            //tick++;
            //if (tick > 100) tick = 0;
            NetOutgoingMessage msgOut = client.CreateMessage();
            //write your data
            //msgOut.Write("Some Text");
            //client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
            if (Game1.CLIENT_ID == -1)
            {
                
                //Debug.WriteLine("doin it");
            }
            //SEND USER DATA
            while (messageQueue.Count>0)
            {
                msgOut = client.CreateMessage();
                msgOut.Write(messageQueue[0]);
                client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);
                messageQueue.RemoveAt(0);

            }
            //SYNC USER DATA
=======
            NetOutgoingMessage msgOut = client.CreateMessage();
            //write your data
            msgOut.Write("Some Text");
            msgOut.Write((short)54);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);

>>>>>>> parent of 99170b8... Server/client playerposition upsync
            while ((msgIn = client.ReadMessage()) != null){
                //create message type handling with a switch
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        //This type handles all data that have been send by you.
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
    }
}
