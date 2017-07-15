using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lidgren.Network;

namespace Game1
{
    class NetworkClient
    {
        private NetServer server;

        public NetworkClient()
        {
            //When initialising, create a configuration object.
            NetPeerConfiguration config = new NetPeerConfiguration("Server");

            //Setting the port, where the NetPeer shall listen. 
            //This is optional, but for a server its good to know, where it is reachable
            config.Port = 50001;
            //Create the NetPeer object with the configurations.
            server = new NetServer(config);
            //Start
            server.Start();
        }
    }
}
