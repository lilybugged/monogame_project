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
        private int inventoryRows;
        /// <summary>
        /// Initializes the UI System.
        /// </summary>
        public UI(int newx, int newy, int rows)
        {
            this.uix = newx; 
            this.uiy = newy;

            menu_0 = new Color(129, 114, 114, 255);
            menu_1 = new Color(141, 127, 127, 255);
            menu_2 = new Color(156, 143, 143, 255);
            menu_3 = new Color(178, 166, 166, 255);
            inventoryRows = rows;
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
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix-1, this.uiy-1, 514, 514), Color.Black);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 512, 512), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+2, this.uiy+2, 508, 508), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+4, this.uiy+4, 504, 504), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 6, this.uiy + 6, 500, 500), menu_2);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8, this.uiy + 8, 136, 172), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9, this.uiy + 9, 134, 170), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 11, this.uiy + 11, 130, 166), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 12, this.uiy + 12, 128, 164), menu_2);
                    
                    for (int i = 0; i < 6; i++)
                    {
                        for (int a = 0; a < 3; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + (46) * a, this.uiy + 219 + (48) * i, 44, 44), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9 + (46) * a, this.uiy + 220 + (48) * i, 42, 42), menu_2);
                        }
                    }
                    
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 10, this.uiy + 8, 357, 495), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 11, this.uiy + 9, 355, 493), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 14, this.uiy + 13, 349, 485), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 15, this.uiy + 14, 347, 483), menu_1);


                    for (int i = 0; i < inventoryRows; i++)
                    {
                        for (int a = 0; a < 7; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 19 + (44 + 5) * a, this.uiy + 17 + (48) * i, 44, 44), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 20 + (44 + 5) * a, this.uiy + 18 + (48) * i, 42, 42), menu_2);
                        }
                    }

                    Game1.spriteBatch.End();
                    break;
                case 2:
                    Game1.spriteBatch.Begin();
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 512, 512), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix+50, this.uiy, 512, 512), menu_0);
                    Game1.spriteBatch.End();
                    break;
            }
        }
    }
}
