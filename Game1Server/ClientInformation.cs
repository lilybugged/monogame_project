using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Text.RegularExpressions;
using System.Diagnostics;
using Lidgren.Network;

namespace Game1Server
{
    public class ClientInformation
    {
        public NetConnection connection;
        public int playerx = 0;
        public int playery = 0;

        public ClientInformation(NetConnection netConnection)
        {
            this.connection = netConnection;
        }
    }
}
