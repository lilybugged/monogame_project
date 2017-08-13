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

        public static AnimatedSprite[] charaLeft = new AnimatedSprite[12];
        public static AnimatedSprite[] charaRight = new AnimatedSprite[12];
        public static AnimatedSprite[] chestSprites = new AnimatedSprite[4];
        public static AnimatedSprite equippables;
        public static AnimatedSprite equip_icons;
        public static AnimatedSprite items_32;
        public static AnimatedSprite tiles;
        public static AnimatedSprite tiles2;
        public static AnimatedSprite portrait_items;
        public static Texture2D pixel;
        public static Texture2D[] cursor = new Texture2D[4];
        public static int globalCursor = 0;
        public static SpriteFont font;

        public static int CLIENT_ID = -1;

        //public static bool gameIsActive = true;

        //public static AnimatedSprite[] chestSprites = new AnimatedSprite[4];

        public static ItemInfo itemInfo;
        
        public static MapInfo map0;
        public static MapInfo currentMap;

        public static List<Chest> chestInventories; //access using the chest id
        public static int openChest = -1;

        public static int[] userInventory;
        public static int[] userInventoryQuantities;
        public static int[] userCarry;
        public static int[] userCarryQuantities;
        public static int[] playerEquippedItems = new int[18];
        public static int carryRank = 3;
        public static UI carryUi;

        public static int globalTick = 0;

        public static bool uiToggle = true;
        public static UI[] uiObjects = new UI[4];
        public static int[] uiPosX = new int[4];
        public static int[] uiPosY = new int[4];
        public static List<BigTile> bigTiles; //access using the tile id

        public const int WINDOW_WIDTH = 1280;
        public const int WINDOW_HEIGHT = 960;
        public const int ITEM_STACK_SIZE = 99;
        public const int PLAYER_RANGE_REQUIREMENT = 64;

        public static NetworkClient client;


        Player player;
        UI ui;

        public Game1()
        {
            //client = new NetworkClient();
            map0 = new MapInfo(300, 150);
            currentMap = map0;
            itemInfo = new ItemInfo();
            graphics = new GraphicsDeviceManager(this);
            player = new Player(16, 16);
            Content.RootDirectory = "Content";
            this.IsMouseVisible = false;
            chestInventories = new List<Chest>();
            bigTiles = new List<BigTile>();
            userInventory = new int[] { 2, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 16, 14, 17, 18, 19, 20, 21, 22, 23, 24, 25, 26, 27, 28, -1, -1, -1, };
            userInventoryQuantities = new int[] { 999, 999, 999, 999, 999, 999, 21, 10, 12, 31, 1, 999, 1, 999, 999, 999, 999, 999, 1, 1, 1, 1, 1, 99, 99, -1, -1, -1 };
            ui = new UI(0,100,4, userInventory, userInventoryQuantities, 1, 7);

            userCarry = new int[] { -1, -1, -1, -1 };
            userCarryQuantities = new int[] { -1, -1, -1, -1 };
            carryUi = new UI(0, 0, 1, userCarry, userCarryQuantities, 3, carryRank+1);

            //so far, there can only be four uis up at once
            //each ui slot is its own type
            ui.id = uiObjects.Length;
            uiObjects[0] = ui;
            uiObjects[2] = carryUi;
            //chest.id = uiObjects.Length;
            //uiObjects[1] = chest;

            for (int i = 0; i < playerEquippedItems.Length; i++)
            {
                playerEquippedItems[i] = -1;
            }
            playerEquippedItems[1] = 23;
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

            items_32 = new AnimatedSprite(Content.Load<Texture2D>("img/icons_32"), 6, 5);
            equippables = new AnimatedSprite(Content.Load<Texture2D>("img/equippable_items"), 7, 6);

            equip_icons = new AnimatedSprite(Content.Load<Texture2D>("img/equip_slots"), 5, 4);
            tiles = new AnimatedSprite(Content.Load<Texture2D>("img/bg_tiles"), 11, 10);
            pixel = Content.Load<Texture2D>("img/white_pixel2");
            portrait_items = new AnimatedSprite(Content.Load<Texture2D>("img/portrait_items"), 2, 3);
            cursor[0] = Content.Load<Texture2D>("img/cursor");
            cursor[1] = Content.Load<Texture2D>("img/selectioncursor");
            cursor[2] = Content.Load<Texture2D>("img/breakcursor");
            cursor[3] = Content.Load<Texture2D>("img/breakactivatecursor");

            chestSprites[0] = new AnimatedSprite(Content.Load<Texture2D>("img/chests/chest_open_s"), 2, 2);
            chestSprites[1] = new AnimatedSprite(Content.Load<Texture2D>("img/chests/chest_close_s"), 2, 2);
            chestSprites[2] = new AnimatedSprite(Content.Load<Texture2D>("img/chests/chest_open_skin"), 2, 2);
            chestSprites[3] = new AnimatedSprite(Content.Load<Texture2D>("img/chests/chest_close_skin"), 2, 2);

            font = Content.Load<SpriteFont>("File");
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
            //Debug.WriteLine(MouseKeyboardInfo.IsMouseLeftClicked());
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || MouseKeyboardInfo.keyState.IsKeyDown(Keys.Escape))
                Exit();

            //SYNC INFORMATION
            //client.Update();

            //update all the things, only if the window is active
            if (this.IsActive)
            {
                for (int i = 0; i < chestInventories.Count; i++)
                {
                    chestInventories[i].Update();
                }
                MouseKeyboardInfo.Update();
                // TODO: Add your update logic here
                player.Update();
                tiles.Update();
                chestSprites[0].Update();
                chestSprites[1].Update();

                base.Update(gameTime);

                if (MouseKeyboardInfo.keyClickedI)
                {
                    uiToggle = !uiToggle;
                }
                //update all UIs
                for (int i = 0; i < uiObjects.Length; i++)
                {
                    if (uiObjects[i] != null && (i == 2 || !uiToggle)) uiObjects[i].Update();
                }
                globalTick++;
                if (globalTick > 15) globalTick = 0;
            }

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(230, 247, 255));

            //spriteBatch.Draw(grass2, new Vector2(400, 240), Color.White);
            //spriteBatch.Draw(grass3, new Vector2(450, 240), Color.White);

            Game1.spriteBatch.Begin();
            map0.DrawMap();
            Game1.spriteBatch.End();

            // TODO: Add your drawing code here
            for (int i = 0; i < chestInventories.Count; i++)
            {
                chestInventories[i].Draw();
            }
            for (int i = 0; i < bigTiles.Count; i++)
            {
                bigTiles[i].Draw();
            }

            base.Draw(gameTime);
            player.Draw();

            //draw all UIs
            
            for (int i = 0; i < uiObjects.Length; i++)
            {
                if (uiObjects[i] != null && (i==2 || !uiToggle)) uiObjects[i].Draw();
            }
            uiObjects[0].DrawCursorItem();
            
            Game1.spriteBatch.Begin();
            spriteBatch.Draw(cursor[globalCursor], new Vector2(MouseKeyboardInfo.mouseState.X,MouseKeyboardInfo.mouseState.Y), Color.White);
            Game1.spriteBatch.End();
        }
    }
}
