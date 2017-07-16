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
            client.DiscoverLocalPeers(7777);
        }

        public void Update()
        {
            NetOutgoingMessage msgOut = client.CreateMessage();
            //write your data
            msgOut.Write("Some Text");
            msgOut.Write((short)54);
            client.SendMessage(msgOut, NetDeliveryMethod.ReliableOrdered);

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
