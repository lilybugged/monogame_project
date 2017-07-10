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
        public static MouseState mouseState;
        public static AnimatedSprite[] charaLeft = new AnimatedSprite[12];
        public static AnimatedSprite[] charaRight = new AnimatedSprite[12];
        public static AnimatedSprite items_32;
        public static Texture2D pixel;

        public static int[] userInventory;
        public static List<UI> uiObjects = new List<UI>();

        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 960;

        public static bool mouseClicked = false;
        public static bool mouseReleased = false;
        private bool canClick = true;
        private bool canRelease = false;

        Player player;
        UI ui;
        UI chest;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            player = new Player(1100, 450);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = true;
            userInventory = new int[] { 2, 1, 0, 2, 2, 2, 1, 0, 2, 1, 0 };

            ui = new UI(100,100,4, userInventory, 1);
            chest = new UI(600, 200, 8, new int[] { 0,2,2,1 }, 2);

            uiObjects.Add(ui);
            uiObjects.Add(chest);
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
            this.graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            this.graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
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

            items_32 = new AnimatedSprite(Content.Load<Texture2D>("img/icons_32"), 2, 2);
            pixel = Content.Load<Texture2D>("img/white_pixel2");


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
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
            mouseState = Mouse.GetState();
            base.Update(gameTime);

            //update all UIs
            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].Update();
            }

            //update mouse stats
            if (mouseState.LeftButton == ButtonState.Pressed && canClick)
            {
                mouseClicked = true;
                canClick = false;
                canRelease = true;
            }
            else mouseClicked = false;

            if (mouseState.LeftButton == ButtonState.Released && canRelease)
            {
                mouseReleased = true;
                canClick = true;
                canRelease = false;
            }
            else mouseReleased = false;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.LightSteelBlue);

            //spriteBatch.Draw(grass2, new Vector2(400, 240), Color.White);
            //spriteBatch.Draw(grass3, new Vector2(450, 240), Color.White);



            // TODO: Add your drawing code here
            
            base.Draw(gameTime);
            player.Draw();

            //draw all UIs
            for (int i=0;i<uiObjects.Count;i++) {
                uiObjects[i].Draw();
            }
            //draw cursor items for all UIs
            for (int i = 0; i < uiObjects.Count; i++)
            {
                uiObjects[i].DrawCursorItem();
            }
        }
    }
}
