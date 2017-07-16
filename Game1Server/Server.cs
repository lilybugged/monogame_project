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
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Server : Game
    {
        SpriteFont font;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        NetServer server;
        NetPeerConfiguration config;
        NetIncomingMessage msgIn;
        List<String> messages = new List<String>();
        List<ClientInformation> clients = new List<ClientInformation>();

        int[,] userInventories; 

        public Server()
        {
            this.IsMouseVisible = true;
            this.Window.Title = "Game1 Server";

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            config = new NetPeerConfiguration("Game1");
            config.Port = 7777;

            server = new NetServer(config);
            server.Start();

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);
            config.EnableMessageType(NetIncomingMessageType.Data);

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            this.graphics.PreferredBackBufferWidth = 640;
            this.graphics.PreferredBackBufferHeight = 480;
            this.graphics.IsFullScreen = false;
            this.graphics.ApplyChanges();
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("default_font");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// ALWAYS SEND THE CLIENT ID BEFORE STRINGS FOR CHECKING
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //use local message variable
            NetOutgoingMessage msg = server.CreateMessage();

            //msg.Write("activate id: " + clients.Count);

            //if (msgIn!=null && msgIn.SenderEndPoint != null && server.GetConnection(msgIn.SenderEndPoint) != null) server.SendMessage(msg, server.GetConnection(msgIn.SenderEndPoint), NetDeliveryMethod.ReliableOrdered);

            //update clients' information
            //msg.Write("syncMap: 15 15 1,1,1,1,1,1,1,1,1,1,1,1,1,1,1 99,3,54,6,2,1,1,1,1,1,1,1,1,1,1");

            //server.SendToAll(msg, NetDeliveryMethod.ReliableOrdered);

            //Game1.client.messageQueue.Add("" + Game1.CLIENT_ID + " playerMove:" + playerx + "," + playery);
            while ((msgIn = server.ReadMessage()) != null)
            {
                msg = server.CreateMessage();
                msg.Write("activate id: " + clients.Count);
                if (msgIn.SenderConnection!=null) server.SendMessage(msg, msgIn.SenderConnection, NetDeliveryMethod.ReliableOrdered);

                //create message type handling with a switch
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        //This type handles all data that have been send by you.
                        messages.Add("" + msgIn.ReadString());
                        String str;
                        str = msgIn.ReadString();
                        //Debug.WriteLine(msgIn.ReadString());
                        if (str.Contains("playerMove"))
                        {
                            clients[Int32.Parse(str.Split(' ')[0])].playerx = Int32.Parse(str.Substring(12+(""+ Int32.Parse(str.Split(' ')[0])).Length).Split(',')[0]);
                            clients[Int32.Parse(str.Split(' ')[0])].playery = Int32.Parse(str.Substring(12 + ("" + Int32.Parse(str.Split(' ')[0])).Length).Split(',')[1]);
                            Debug.WriteLine(""+clients[Int32.Parse(str.Split(' ')[0])].playerx+","+clients[Int32.Parse(str.Split(' ')[0])].playery);
                        }
                        //Debug.WriteLine("" + msgIn.ReadString());
                        break;
                    //All other types are for library related events (some examples)
                    case NetIncomingMessageType.DiscoveryRequest:
                        //add a string as welcome text
                        msg.Write("Hellooooo Client");
                        //send a response
                        server.SendDiscoveryResponse(msg, msgIn.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        clients.Add(new ClientInformation(server.GetConnection(msgIn.SenderEndPoint)));
                        msgIn.SenderConnection.Approve();
                        break;
                    default:
                        messages.Add("" + msgIn.ReadString());
                        //Debug.WriteLine("" + msgIn.ReadString());
                        break;
                }
                
                //Recycle the message to create less garbage
                //Debug.WriteLine(msgIn.ReadString());
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(53, 37, 67, 255));

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            for (int i = messages.Count - 1; i > 0; i--)
            {
                if ((new Regex(@"\W|_")).Replace(messages[i], "").Length > 0) spriteBatch.DrawString(font, (new Regex(@"\W|_")).Replace(messages[i], ""), new Vector2(0,480 - 16*(messages.Count - i)), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
