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

        public Server()
        {
            this.IsMouseVisible = true;

            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            config = new NetPeerConfiguration("Game1");
            config.Port = 14242;

            server = new NetServer(config);
            server.Start();

            config.EnableMessageType(NetIncomingMessageType.ConnectionApproval);
            config.EnableMessageType(NetIncomingMessageType.DiscoveryRequest);

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
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //use local message variable
            
            //standard receive loop - loops through all received messages, until none is left
            while ((msgIn = server.ReadMessage()) != null)
            {
                //create message type handling with a switch
                switch (msgIn.MessageType)
                {
                    case NetIncomingMessageType.Data:
                        //This type handles all data that have been send by you.
                        messages.Add("" + msgIn.ReadString());
                        break;
                    //All other types are for library related events (some examples)
                    case NetIncomingMessageType.DiscoveryRequest:
                        NetOutgoingMessage msg = server.CreateMessage();
                        //add a string as welcome text
                        msg.Write("Hellooooo Client");
                        //send a response
                        server.SendDiscoveryResponse(msg, msgIn.SenderEndPoint);
                        break;
                    case NetIncomingMessageType.ConnectionApproval:
                        msgIn.SenderConnection.Approve();
                        break;
                    default:
                        messages.Add("" + msgIn.ReadString());
                        break;
                }
                //Recycle the message to create less garbage
                Debug.WriteLine(msgIn.ReadString());
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
            for (int i = 0; i < messages.Count; i++)
            {
                if ((new Regex(@"\W|_")).Replace(messages[i], "").Length > 0)spriteBatch.DrawString(font, (new Regex(@"\W|_")).Replace(messages[i], ""), new Vector2(0,16*i), Color.White);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
