using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace Game1
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        Texture2D grass2;
        Texture2D bg;
        Texture2D grass3;

        public static KeyboardState keyState;
        public static AnimatedSprite[] charaLeft = new AnimatedSprite[12];
        public static AnimatedSprite[] charaRight = new AnimatedSprite[12];
        Player player;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            player = new Player(960, 1136);
            Content.RootDirectory = "Content";
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
            this.graphics.PreferredBackBufferWidth = GraphicsDevice.DisplayMode.Width;
            this.graphics.PreferredBackBufferHeight = GraphicsDevice.DisplayMode.Height;
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

            // TODO: use this.Content to load your game content here
            grass2 = Content.Load<Texture2D>("img/grass2");
            bg = Content.Load<Texture2D>("img/blep");
            grass3 = Content.Load<Texture2D>("img/grass3");

            charaLeft[0] = new AnimatedSprite(Content.Load<Texture2D>("img/spr_chara_left_0"), 1, 1);
            charaLeft[1] = new AnimatedSprite(Content.Load<Texture2D>("img/spr_chara_left_1"), 2, 2);
            charaRight[0] = new AnimatedSprite(Content.Load<Texture2D>("img/spr_chara_Right_0"), 1, 1);
            charaRight[1] = new AnimatedSprite(Content.Load<Texture2D>("img/spr_chara_Right_1"), 2, 2);

            
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
            player.Update();
            keyState = Keyboard.GetState();
            base.Update(gameTime);

            //keyinput
            

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            //spriteBatch.Draw(bg, new Rectangle(0, 0, 800, 480), Color.White);
            //spriteBatch.Draw(grass2, new Vector2(400, 240), Color.White);
            //spriteBatch.Draw(grass3, new Vector2(450, 240), Color.White);
            
            

            // TODO: Add your drawing code here
            base.Draw(gameTime);
            player.Draw();
        }
    }
}
