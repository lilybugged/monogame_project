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
    /// The main player object.
    /// </summary>
    class Player
    {
        public static int playerx;
        public static int playery;
        private int currentAction;
        private int currentDirection;
        private int speed;
        //private AnimatedSprite currentSprite;

        /// <summary>
        /// Initializes a new player object.
        /// </summary>
        public Player(int x, int y)
        {
            playerx = x;
            playery = y;
            speed = 2;
            //currentSprite = Game1.charaLeft[0];
            currentAction = 0;
            currentDirection = 0; //left
        }
        public void Update()
        {
            MouseKeyboardInfo.keyState = Keyboard.GetState();
            if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.A))
            {
                playerx-=speed;
                //currentSprite = Game1.charaLeft[1];
                currentAction = 1;
                currentDirection = 0;
            }
            else if (MouseKeyboardInfo.keyState.IsKeyDown(Keys.D))
            {
                playerx+=speed;
                //currentSprite = Game1.charaRight[1];
                currentAction = 1;
                currentDirection = 1;
            }
            else if (currentDirection == 0)
            {
                //currentSprite = Game1.charaLeft[0];
                currentAction = 0;
            }
            else if (currentDirection == 1)
            {
                //currentSprite = Game1.charaRight[0];
                currentAction = 0;
            }
            (currentDirection == 0 ? Game1.charaLeft[currentAction] : Game1.charaRight[currentAction]).Update();
        }
        public void Draw()
        {
            (currentDirection==0? Game1.charaLeft[currentAction] :Game1.charaRight[currentAction]).Draw(Game1.spriteBatch, new Vector2(Game1.WINDOW_WIDTH/2, Game1.WINDOW_HEIGHT/2));
        }
    }
}
