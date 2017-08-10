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
    /// The main player object.
    /// </summary>
    class Player
    {
        public static int playerx;
        public static int playery;
        private static int accelerationy;
        public static int currentAction;
        public static int currentDirection;
        private int speedy;
        private int speedx;
        private bool canJump;
        private static int frame = 0;
        //private AnimatedSprite currentSprite;

        /// <summary>
        /// Initializes a new player object.
        /// </summary>
        public Player(int x, int y)
        {
            playerx = x;
            playery = y;
            speedy = 3;
            speedx = 0;
            accelerationy = 1; 
            //currentSprite = Game1.charaLeft[0];
            currentAction = 0;
            currentDirection = 0; //left
        }
        public void Update()
        {
            //Debug.WriteLine(""+RangeFromPoint(playerx+MouseKeyboardInfo.mouseState.X, playery + MouseKeyboardInfo.mouseState.Y)[0]);
            MouseKeyboardInfo.keyState = Keyboard.GetState();
            if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.W) && canJump)
            {
                speedy = -13;
                canJump = false;
                // Game1.client.messageQueue.Add(""+Game1.CLIENT_ID+" playerMove:"+playerx+","+playery);
            }
            if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.A))
            {
                //if (Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 - 16) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 - 16) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16]])
                {
                    speedx = -2;
                    currentAction = 1;
                    currentDirection = 0;
                }
                // Game1.client.messageQueue.Add("" + Game1.CLIENT_ID + " playerMove:" + playerx + "," + playery);
            }
            else if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.D))
            {
                speedx = 2;
                //currentSprite = Game1.charaRight[1];
                currentAction = 1;
                currentDirection = 1;
                // Game1.client.messageQueue.Add("" + Game1.CLIENT_ID + " playerMove: " + playerx + "," + playery);
            }
            else
            {
                speedx = 0;
                if (currentDirection == 0)
                {
                    //currentSprite = Game1.charaLeft[0];
                    currentAction = 0;
                }
                else if (currentDirection == 1)
                {
                    //currentSprite = Game1.charaRight[0];
                    currentAction = 0;
                }
            }
            (currentDirection == 0 ? Game1.charaLeft[currentAction] : Game1.charaRight[currentAction]).Update();
            //speedy += accelerationy;
            //below handles x collisions
            SnapOnCollision();
            playerx += speedx;
            playery += speedy;
            /*if (speedx < 0 && (!(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16]]) ||
                !(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16]])))
            {
                //playerx -= speedx/Math.Abs(speedx);
                speedx = 0;
                playerx = playerx / 16 * 16;
                //playerx += 16;
            }
            else if (speedx > 0 && (!(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 31) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 31) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16]])||
                !(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 31) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 31) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16]])))
            {
                //playerx -= speedx / Math.Abs(speedx);
                speedx = 0;
                playerx = playerx / 16 * 16;
                playerx -= 16;
            }
            else if (speedx < 0 && ((BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16))].solid) ||
                (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2), (Player.playery + Game1.WINDOW_HEIGHT / 2)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2), (Player.playery + Game1.WINDOW_HEIGHT / 2))].solid)))
            {
                speedx = 0;
                playerx = playerx / 16 * 16;
                playerx += 16;
            }
            else if (speedx > 0 && ((BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 31), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 31), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16))].solid) ||
                (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 31), (Player.playery + Game1.WINDOW_HEIGHT / 2)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 31), (Player.playery + Game1.WINDOW_HEIGHT / 2))].solid)))
            {
                speedx = 0;
                playerx = playerx / 16 * 16;
                //playerx -= 16;
            }
            for (int i = 0; i < Math.Abs(speedy); i++)
            {
                if (speedy != 0) playery += speedy / Math.Abs(speedy);
                SnapOnCollision();
            }*/
            frame = (currentDirection==0?Game1.charaLeft:Game1.charaRight)[currentAction].currentFrame;
        }
        public void Draw()
        {
            (currentDirection==0? Game1.charaLeft[currentAction] :Game1.charaRight[currentAction]).Draw(Game1.spriteBatch, new Vector2(Game1.WINDOW_WIDTH/2, Game1.WINDOW_HEIGHT/2));
            Game1.spriteBatch.Begin();
            for (int i = Game1.playerEquippedItems.Length-2; i > 0; i--)
            {
                if (Game1.playerEquippedItems[i]!=-1) {
                    Game1.equippables.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_EQUIPID[Game1.playerEquippedItems[i]] * 8 + (currentDirection == 0 ? 0 : 4) + ((currentAction==1)? frame: 0),
                        new Vector2(Game1.WINDOW_WIDTH / 2, Game1.WINDOW_HEIGHT / 2));
                }
            }
            if (Game1.playerEquippedItems[17] != -1) Game1.equippables.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_EQUIPID[Game1.playerEquippedItems[17]] * 8 + (currentDirection == 0 ? 0 : 4) + ((currentAction == 1) ? frame : 0),
                        new Vector2(Game1.WINDOW_WIDTH / 2, Game1.WINDOW_HEIGHT / 2));
            Game1.spriteBatch.End();
        }
        private void SnapOnCollision() //the improved one
        {
            for (int i = 0; i < 32; i++) // left/right
            {
                for (int a = 0; a < 32; a++)
                {
                    if (((Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + i)/16, (Player.playerx + Game1.WINDOW_WIDTH / 2 + a)/16]!=-1) && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + i) / 16, (Player.playerx + Game1.WINDOW_WIDTH / 2 + a) / 16]]) ||
                        (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + i), (Player.playerx + Game1.WINDOW_WIDTH / 2 + a)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + i), (Player.playerx + Game1.WINDOW_WIDTH / 2 + a))].solid))
                    {
                        playery -= speedy;
                        playerx = playerx / 16 * 16;
                        playery = playery / 16 * 16;
                        speedy = 0;
                    }
                }
            }
        }
        /*private void SnapOnCollision() // handles y collisions
        {
            Debug.WriteLine(""+ (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 1), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32))));
            if (((Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 1) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16]!=-1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH/ 2 + 1) /16, (Player.playery + Game1.WINDOW_HEIGHT/ 2 + 32) /16]]) ||
                    (Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 17) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 17) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16]])) ||
                    ((BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 1), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 1), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32))].solid) ||
                (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 17), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 17), (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32))].solid)))
            {
                //playerx = playerx / 16 * 16;
                playery = playery / 16 * 16;
                speedy = 0;
                canJump = true;
            }
            if ((Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 1) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1) / 16] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 1) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1) / 16]]) ||
                    (Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 17) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1) / 16] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 17) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1) / 16]]) ||
                    ((BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 1), (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 1), (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1))].solid) ||
                (BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 17), (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1)) != -1 && Game1.bigTiles[BigTile.FindTileId((Player.playerx + Game1.WINDOW_WIDTH / 2 + 17), (Player.playery + Game1.WINDOW_HEIGHT / 2 - 1))].solid)))
            {
                //playery++;
                playery++;
                playery = playery / 16 * 16;
                speedy = 0;
            }
        }*/
        /// <summary>
        /// Takes a world point and returns the player's x-distance and y-distance from it.
        /// </summary>
        public static int[] RangeFromPoint(int pointx, int pointy)
        {
            return new int[] {Math.Abs(playerx + Game1.WINDOW_WIDTH/2 - pointx), Math.Abs(playery + Game1.WINDOW_HEIGHT / 2 - pointy)};
        }
    }
}
