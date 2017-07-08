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
    /// The UI System.
    /// </summary>
    class UI
    {
        //private AnimatedSprite currentSprite;
        private int uix, uiy;
        private Color menu_0;
        private Color menu_1;
        private Color menu_2;
        private Color menu_3;
        /// <summary>
        /// Initializes the UI System.
        /// </summary>
        public UI(int newx, int newy)
        {
            this.uix = newx; 
            this.uiy = newy;

            menu_0 = new Color(129, 114, 114, 255);
            menu_1 = new Color(141, 127, 127, 255);
            menu_2 = new Color(156, 143, 143, 255);
            menu_3 = new Color(178, 166, 166, 255);
        }
        public void Update()
        {
            
        }
        public void Draw(int uiState)
        {
            switch (uiState)
            {
                case 1:
                    Game1.spriteBatch.Begin();
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix-1, this.uiy-1, 128 * 4 + 2, 128 * 4 + 2), Color.Black);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 128 * 4, 128 * 4), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+2, this.uiy+2, 128 * 4 - 4, 128 * 4 - 4), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+4, this.uiy+4, 128 * 4 - 8, 128 * 4 - 8), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 6, this.uiy + 6, 128 * 4 - 12, 128 * 4 - 12), menu_2);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8, this.uiy + 8, 34 * 4, 43 * 4), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9, this.uiy + 9, 34 * 4 - 2, 43 * 4 - 2), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 11, this.uiy + 11, 34 * 4 - 6, 43 * 4 - 6), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 12, this.uiy + 13, 34 * 4 - 9, 43 * 4 - 9), menu_2);

                    //TODO: alignment looks janky rn vvv
                    for (int i = 0; i < 7; i++)
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8, this.uiy + 187+(i * (44 + 1)), 11 * 4, 11 * 4), menu_0);
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9, this.uiy + 188 + (i * (44 + 1)), 11 * 4 -2, 11 * 4 -2), menu_1);

                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + 2 + 44, this.uiy + 187 + (i * (44 + 1)), 11 * 4, 11 * 4), menu_0);
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9 + 2 + 44, this.uiy + 188 + (i * (44 + 1)), 11 * 4 - 2, 11 * 4 - 2), menu_1);

                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + 4 + 88, this.uiy + 187 + (i * (44 + 1)), 11 * 4, 11 * 4), menu_0);
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9 + 4 + 88, this.uiy + 188 + (i * (44 + 1)), 11 * 4 - 2, 11 * 4 - 2), menu_1);
                    }
                    
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 10, this.uiy + 8, 357, 495), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 11, this.uiy + 9, 355, 493), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 14, this.uiy + 13, 349, 485), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 15, this.uiy + 14, 347, 483), menu_1);


                    for (int i = 0; i < 10; i++)
                    {
                        for (int a = 0; a < 7; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 19 + (44 + 5) * a, this.uiy + 17 + (44 + 4) * i, 11 * 4, 11 * 4), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 20 + (44 + 5) * a, this.uiy + 18 + (44 + 4) * i, 11 * 4 - 2 , 11 * 4 - 2), menu_2);
                        }
                    }

                    Game1.spriteBatch.End();
                    break;
                case 2:
                    Game1.spriteBatch.Begin();
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 128 * 4, 128 * 4), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+50, this.uiy, 128 * 4, 128 * 4), menu_0);
                    Game1.spriteBatch.End();
                    break;
            }
        }
    }
}
