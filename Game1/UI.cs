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

//NOTICE: inventory arrays passed into ui functions MUST be the size of the ui's inventory. ALL empty spaces should be filled with -1's.

namespace Game1
{
    /// <summary>
    /// The UI System.
    /// </summary>
    public class UI
    {
        //private AnimatedSprite currentSprite;
        private int uix, uiy;
        private int uiState;
        private Color menu_0;
        private Color menu_1;
        private Color menu_2;
        private Color menu_3;
        private int inventoryRows;
        private int[] inventoryItemIds;
        private int cursorItem;
        private int cursorItemIndex;
        /// <summary>
        /// Initializes the UI System.
        /// </summary>
        public UI(int newx, int newy, int rows, int[] itemIds, int type)
        {
            this.uix = newx; 
            this.uiy = newy;
            this.uiState = type;

            menu_0 = new Color(129, 114, 114, 255);
            menu_1 = new Color(141, 127, 127, 255);
            menu_2 = new Color(156, 143, 143, 255);
            menu_3 = new Color(178, 166, 166, 255);

            cursorItem = -1;
            cursorItemIndex = -1;
            inventoryRows = rows;
            inventoryItemIds = itemIds;
        }
        public void Update()
        {
            DragAndDrop(this.uix + 136 + 19, this.uiy + 17);
        }
        public void Draw()
        {
            Game1.spriteBatch.Begin();

            

            switch (uiState)
            {
                case 1:
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
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 9 + (46) * a, this.uiy + 220 + (48) * i, 42, 42), menu_1);
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
                    DrawItems(this.uix + 136 + 19 + 5, this.uiy + 17 + 5);
                    HoverSquares(this.uix + 136 + 19, this.uiy + 17);
                    break;
                case 2:
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix - 1, this.uiy - 1, 354, 396), Color.Black);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 352, 394), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 2, this.uiy + 2, 348, 390), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 4, this.uiy + 4, 344, 386), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 6, this.uiy + 6, 340, 382), menu_1);

                    for (int i = 0; i < inventoryRows; i++)
                    {
                        for (int a = 0; a < 7; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 7 + (44 + 5) * a, this.uiy + 7 + (48) * i, 44, 44), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + (44 + 5) * a, this.uiy + 8 + (48) * i, 42, 42), menu_2);
                        }
                    }
                    DrawItems(this.uix + 7 + 5, this.uiy + 7 + 5);
                    HoverSquares(this.uix + 7, this.uiy + 7);
                    break;
            }

            //if the cursor is dragging an item, draw it
            if (cursorItem != -1)
            {
                Game1.items_32.DrawTile(Game1.spriteBatch, cursorItem, new Vector2(Game1.mouseState.X, Game1.mouseState.Y));
            }

            Game1.spriteBatch.End();
        }

        private void DrawItems(int startx, int starty)
        {
            for (int i=0; i<inventoryItemIds.Length; i++) {
                Game1.items_32.DrawTile(Game1.spriteBatch, inventoryItemIds[i], new Vector2(startx + 49 * (i%7) + 1, starty + 48 * (i / 7) + 1));
            }
        }

        private void HoverSquares(int startx, int starty)
        {
            if (Game1.mouseState.X>=startx && Game1.mouseState.X<=startx+(7 * (49)) - 20 && Game1.mouseState.Y >= starty && Game1.mouseState.Y <= starty + (inventoryRows * (48)) - 25)
            {
                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(startx%49 + 49 * (Game1.mouseState.X/49) + 1, starty%48 + 48 * (Game1.mouseState.Y / 48) + 1, 42, 42), Color.White*0.25f);
            }
        }

        private void DragAndDrop(int startx, int starty)
        {
            if (cursorItem == -1 && ((Game1.mouseState.X - startx) / 49 + ((Game1.mouseState.Y - starty) / 48 * 7)) < inventoryItemIds.Length && Game1.mouseClicked && Game1.mouseState.X >= startx && Game1.mouseState.X <= startx + (7 * (49)) - 20 && Game1.mouseState.Y >= starty && Game1.mouseState.Y <= starty + (inventoryRows * (48)) - 25)
            {
                cursorItem = inventoryItemIds[(Game1.mouseState.X - startx) / 49 + ((Game1.mouseState.Y - starty) / 48 * 7)];
                inventoryItemIds[(Game1.mouseState.X - startx) / 49 + ((Game1.mouseState.Y - starty) / 48 * 7)] = -1;
                cursorItemIndex = (Game1.mouseState.X - startx) / 49 + ((Game1.mouseState.Y - starty) / 48 * 7);

                //TODO: add ability to drop items outside and inside bounds.
            }
        }
    }
}
