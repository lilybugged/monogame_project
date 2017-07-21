﻿using System;
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
        private int jumpTimer;
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
            //currentSprite = Game1.charaLeft[0];
            currentAction = 0;
            currentDirection = 0; //left
        }
        public void Update()
        {
            if (jumpTimer > 0) jumpTimer--;
            MouseKeyboardInfo.keyState = Keyboard.GetState();
            if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.W) && jumpTimer==0)
            {
                speedy = -13;
                jumpTimer = 35;
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
            speedy += accelerationy;
            //below handles x collisions
            if (speedx < 0 && (!(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16]]) ||
                !(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16]]))) speedx = 0;
            if (speedx > 0 && (!(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 32) / 16 , (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 32) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 16) / 16]])||
                !(Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 32) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16] == -1 || !Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 32) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2) / 16]]))) speedx = 0;
            playerx += speedx;
            for (int i = 0; i < Math.Abs(speedy); i++)
            {
                playery += speedy / Math.Abs(speedy);
                SnapOnCollision();
            }

        }
        public void Draw()
        {
            (currentDirection==0? Game1.charaLeft[currentAction] :Game1.charaRight[currentAction]).Draw(Game1.spriteBatch, new Vector2(Game1.WINDOW_WIDTH/2, Game1.WINDOW_HEIGHT/2));
        }
        private void SnapOnCollision() // handles y collisions
        {
            if (speedy>0 && (Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2 + 16) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16]!=-1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH/2 + 16)/16, (Player.playery + Game1.WINDOW_HEIGHT/ 2 + 32) /16]]) ||
                    (Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[(Player.playerx + Game1.WINDOW_WIDTH / 2) / 16, (Player.playery + Game1.WINDOW_HEIGHT / 2 + 32) / 16]]))
            {
                //playerx = playerx / 16 * 16;
                playery = playery / 16 * 16;
                speedy = 0;
            }
        }
    }
}
