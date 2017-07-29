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
    public class MouseKeyboardInfo
    {
        public static bool mouseClickedLeft = false;
        public static bool mouseReleasedLeft = false;
        private static bool canClickLeft = true;
        private static bool canReleaseLeft = false;

        public static bool mouseClickedRight = false;
        public static bool mouseReleasedRight = false;
        private static bool canClickRight = true;
        private static bool canReleaseRight = false;

        public static bool keyClickedI = false;
        public static bool keyReleasedI = false;
        private static bool canClickI = true;
        private static bool canReleaseI = false;

        public static MouseState mouseState;
        public static KeyboardState keyState;

        public static void Update()
        {
            mouseState = Mouse.GetState();
            keyState = Keyboard.GetState();

            //update mouse stats
            if (mouseState.LeftButton == ButtonState.Pressed && canClickLeft)
            {
                mouseClickedLeft = true;
                canClickLeft = false;
                canReleaseLeft = true;
            }
            else mouseClickedLeft = false;

            if (mouseState.LeftButton == ButtonState.Released && canReleaseLeft)
            {
                mouseReleasedLeft = true;
                canClickLeft = true;
                canReleaseLeft = false;
            }
            else mouseReleasedLeft = false;

            //mouse stats - right
            if (mouseState.RightButton == ButtonState.Pressed && canClickRight)
            {
                mouseClickedRight = true;
                canClickRight = false;
                canReleaseRight = true;
                
            }
            else mouseClickedRight = false;
            
            if (mouseState.RightButton == ButtonState.Released && canReleaseRight)
            {
                mouseReleasedRight = true;
                canClickRight = true;
                canReleaseRight = false;
            }
            else mouseReleasedRight = false;

            //TODO: finish this thing below
            //key stats - i
            if (keyState.i == ButtonState.Pressed && canClickRight)
            {
                mouseClickedRight = true;
                canClickRight = false;
                canReleaseRight = true;

            }
            else mouseClickedRight = false;

            if (mouseState.RightButton == ButtonState.Released && canReleaseRight)
            {
                mouseReleasedRight = true;
                canClickRight = true;
                canReleaseRight = false;
            }
            else mouseReleasedRight = false;
        }
    }
}
