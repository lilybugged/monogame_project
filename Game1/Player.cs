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
        public static int screenPosX;
        public static int screenPosY;
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
            speedy = 0;
            speedx = 0;
            accelerationy = 1;
            screenPosX = ((Game1.WINDOW_WIDTH / 2) - (Game1.WINDOW_WIDTH / 2) % 16);
            screenPosY = ((Game1.WINDOW_HEIGHT / 2) - (Game1.WINDOW_HEIGHT / 2) % 16);
            //currentSprite = Game1.charaLeft[0];
            currentAction = 0;
            currentDirection = 0; //left
        }
        public void Update()
        {
            //handles *most* collisions
            if (speedy > 0) playery += speedy;
            if (speedy < 15) speedy += accelerationy;
            if (speedx != 0 && WillCollide(playerx + screenPosX, (playery + screenPosY), 32, 28))
            {
                if (speedx > 0)
                {
                    playerx -= 2;
                }
                else playerx += 2;
                if (speedy < 5) playerx = playerx / 16 * 16;
                speedx = 0;
            }
            if (WillCollide((playerx + screenPosX), (playery + screenPosY), 32, 32))
            {
                //playerx = playerx / 16 * 16;
                
                if (speedy < 0) playery += 10;
                if (speedy < 0 && !WillCollide((playerx + screenPosX), (playery + screenPosY), 32, 32)) playery += 5;
                if (speedy > 0) canJump = true;
                playery = playery / 16 * 16;
                speedy = 0;
                
            }

            if (speedx != 0 && WillCollide(playerx + screenPosX, (playery + screenPosY), 32, 28)) canJump = false;
            if (speedy < 0) playery += speedy;

            //handles movement
            MouseKeyboardInfo.keyState = Keyboard.GetState();
            //Debug.WriteLine(WillCollide((playerx + screenPosX), (playery + screenPosY) - 31, 32, 32));
            if ((MouseKeyboardInfo.keyState.IsKeyDown(Keys.W) || MouseKeyboardInfo.keyState.IsKeyDown(Keys.Space)) && canJump && !WillCollide((playerx + screenPosX), (playery + screenPosY) - 15, 32, 32))
            {
                speedy = -13;
                canJump = false;
                // Game1.client.messageQueue.Add(""+Game1.CLIENT_ID+" playerMove:"+playerx+","+playery);
            }
            if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.A) && !WillCollide((playerx + screenPosX) - 2, (playery + screenPosY), 32, 32) && playerx > 0)
            {
                //if (Game1.currentMap.mapTiles[(Player.playerx + screenPosX - 16) / 16, (Player.playery + screenPosY + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + screenPosX - 16) / 16, (Player.playery + screenPosY + 16) / 16]])
                {
                    speedx = -2;
                    currentAction = 1;
                    currentDirection = 0;
                    
                }
                // Game1.client.messageQueue.Add("" + Game1.CLIENT_ID + " playerMove:" + playerx + "," + playery);
            }
            else if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.D) && !WillCollide((playerx + screenPosX) + 2, (playery + screenPosY), 32, 32) && playerx < Game1.currentMap.mapTiles.GetLength(0)*16 - 500)
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
            playerx += speedx;
            frame = (currentDirection==0?Game1.charaLeft:Game1.charaRight)[currentAction].currentFrame;
            (currentDirection == 0 ? Game1.charaLeft : Game1.charaRight)[currentAction].Update();
        }
        public void Draw()
        {
            (currentDirection==0? Game1.charaLeft[currentAction] :Game1.charaRight[currentAction]).Draw(Game1.spriteBatch, new Vector2(screenPosX, screenPosY));
            Game1.spriteBatch.Begin();
            for (int i = Game1.playerEquippedItems.Length-2; i > 0; i--)
            {
                if (Game1.playerEquippedItems[i]!=-1) {
                    Game1.equippables.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_EQUIPID[Game1.playerEquippedItems[i]] * 8 + (currentDirection == 0 ? 0 : 4) + ((currentAction==1)? frame: 1),
                        new Vector2(screenPosX, screenPosY));
                }
            }
            if (Game1.playerEquippedItems[17] != -1) Game1.equippables.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_EQUIPID[Game1.playerEquippedItems[17]] * 8 + (currentDirection == 0 ? 0 : 4) + ((currentAction == 1) ? frame : 0),
                        new Vector2(screenPosX, screenPosY));
            Game1.spriteBatch.End();
        }
        public bool WillCollide(int x, int y, int width, int height) //checks if, in a given rectangle, any solid collision will occur
        {
            for (int i = 0; i < width; i++)
            {
                for (int a = 0; a < height; a++)
                {
                    if (Game1.currentMap.mapTiles[(x + i) / 16, (y + a) / 16] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(x + i) / 16, (y + a) / 16]] && (BigTile.FindTileId((x + i), (y + a)) == -1 || Game1.bigTiles[BigTile.FindTileId((x + i), (y + a))].solid))
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        /// <summary>
        /// Takes a world point and returns the player's x-distance and y-distance from it.
        /// </summary>
        public static int[] RangeFromPoint(int pointx, int pointy)
        {
            return new int[] {Math.Abs(playerx + screenPosX - pointx), Math.Abs(playery + screenPosY - pointy)};
        }
    }
}
