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
    /// [Notice that the player inventory (type 1) will always be open - it's safe to use that for single-instance!]
    /// </summary>
    public class UI
    {
        //private AnimatedSprite currentSprite;
        private int uix, uiy; //pos of ui on screen
        private int posx, posy; //for DragUi
        private int uiDragging; //for DragUi
        private int uiState;
        private Color menu_0;
        private Color menu_1;
        private Color menu_2;
        private Color menu_3;
        private int inventoryRows;
        public int[] inventoryItemIds;
        public int[] inventoryItemQuantities;
        public int rowSize;
        private static int cursorItem;
        private static int cursorItemOrigin;
        private static int cursorItemIndex;
        private static int cursorQuantity;
        private String toString;
        public static int selectedCarry = 0;
        public int id;
        /// <summary>
        /// Initializes the UI System. [Only pass arrays as large as the chosen ui type's inventory.]
        /// </summary>
        public UI(int newx, int newy, int rows, int[] itemIds, int[] itemQuants, int type, int rowLength)
        {
            this.uix = newx; 
            this.uiy = newy;
            this.uiState = type;
            this.rowSize = rowLength;

            toString = ""+type;

            menu_0 = new Color(129, 114, 114, 255);
            menu_1 = new Color(141, 127, 127, 255);
            menu_2 = new Color(156, 143, 143, 255);
            menu_3 = new Color(178, 166, 166, 255);

            cursorItem = -1;
            cursorItemIndex = -1;
            inventoryRows = rows;
            inventoryItemIds = itemIds;
            cursorItemOrigin = -1;
            cursorQuantity = -1;
            inventoryItemQuantities = itemQuants;

            posx = 0;
            posy = 0;
            uiDragging = -1;
        }
        public void Update()
        {
            switch (uiState)
            {
                case 1:
                    DragAndDrop(this.uix + 136 + 19, this.uiy + 17);
                    break;
                case 2:
                    DragAndDrop(this.uix + 7, this.uiy + 7);
                    break;
                case 3:
                    DragAndDrop(this.uix + 7, this.uiy + 7);
                    break;
            }
            InteractItem();
            PlaceItem();
            DragUi();
            Game1.userInventory = this.inventoryItemIds;
            Game1.userInventoryQuantities = this.inventoryItemQuantities;
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

                    Game1.spriteBatch.Draw(Game1.portrait, new Vector2(this.uix + 12, this.uiy + 12), Color.White*0.5f);

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
                        for (int a = 0; a < rowSize; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 19 + (44 + 5) * a, this.uiy + 17 + (48) * i, 44, 44), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 136 + 20 + (44 + 5) * a, this.uiy + 18 + (48) * i, 42, 42), menu_2);
                        }
                    }
                    DrawItems(this.uix + 136 + 19 + 5, this.uiy + 17 + 5);
                    Hover(this.uix + 136 + 19, this.uiy + 17);
                    break;
                case 2:
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix - 1, this.uiy - 1, 354, 396), Color.Black);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix, this.uiy, 352, 394), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 2, this.uiy + 2, 348, 390), menu_3);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 4, this.uiy + 4, 344, 386), menu_0);
                    Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 6, this.uiy + 6, 340, 382), menu_1);

                    for (int i = 0; i < inventoryRows; i++)
                    {
                        for (int a = 0; a < rowSize; a++)
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 7 + (44 + 5) * a, this.uiy + 7 + (48) * i, 44, 44), menu_0);
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + (44 + 5) * a, this.uiy + 8 + (48) * i, 42, 42), menu_2);
                        }
                    }
                    DrawItems(this.uix + 7 + 5, this.uiy + 7 + 5);
                    Hover(this.uix + 7, this.uiy + 7);
                    break;

                case 3:
                    for (int i = 0; i < inventoryRows; i++)
                    {
                        for (int a = 0; a < rowSize; a++)
                        {
                            if (selectedCarry != a)
                            {
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 5 + (44 + 5) * a, this.uiy + 5 + (48) * i, 48, 48), menu_0);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 6 + (44 + 5) * a, this.uiy + 6 + (48) * i, 46, 46), menu_3);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 7 + (44 + 5) * a, this.uiy + 7 + (48) * i, 44, 44), menu_0);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 8 + (44 + 5) * a, this.uiy + 8 + (48) * i, 42, 42), menu_2);
                            }
                            else
                            {
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 2 + (44 + 5) * a, this.uiy + 2 + (48) * i, 54, 54), menu_0);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 3 + (44 + 5) * a, this.uiy + 3 + (48) * i, 52, 52), menu_3);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 4 + (44 + 5) * a, this.uiy + 4 + (48) * i, 50, 50), menu_0);
                                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(this.uix + 5 + (44 + 5) * a, this.uiy + 5 + (48) * i, 48, 48), menu_2);
                            }
                        }
                    }
                    DrawItems(this.uix + 7 + 5, this.uiy + 7 + 5);
                    Hover(this.uix + 7, this.uiy + 7);
                    break;
            }
            DrawCarry();
            Game1.spriteBatch.End();
        }
        public void DrawCarry()
        {
            if (uiState == 3 && inventoryItemIds[selectedCarry]!=-1 && Game1.itemInfo.ITEM_TOOL[inventoryItemIds[selectedCarry]])
            {
                switch (Player.currentAction)
                {
                    case 0:
                    case 1:
                        if (Player.currentDirection == 0)
                        {
                            Game1.items_32.DrawTile(Game1.spriteBatch, inventoryItemIds[selectedCarry], new Vector2(Game1.WINDOW_WIDTH / 2 - 15, Game1.WINDOW_HEIGHT / 2 - 1));
                        }
                        else
                        {
                            Game1.items_32.DrawTile(Game1.spriteBatch, inventoryItemIds[selectedCarry], new Vector2(Game1.WINDOW_WIDTH / 2, Game1.WINDOW_HEIGHT / 2 - 1),true);
                        }
                        break;
                }
                
            }
        }
        public void DragUi()
        {
            if (uiState !=3 && cursorItem == -1 && MouseKeyboardInfo.mouseClickedLeft && uiDragging==-1 && ((CountUis()==1 && WithinUi(this.uiState)) || (uiState == 2 && WithinUi(this.uiState)))) { 
                {
                    posx = MouseKeyboardInfo.mouseState.X - uix;
                    posy = MouseKeyboardInfo.mouseState.Y - uiy;
                    uiDragging = uiState;
                    if (Game1.uiObjects[1]!=null )Debug.WriteLine(""+uiState+","+(WithinUi(2)));
                }
            }
            if (cursorItem==-1 && MouseKeyboardInfo.mouseReleasedLeft) {
                posx = 0;
                posy = 0;
                uiDragging = -1;
            }
            if (cursorItem==-1 && uiDragging==uiState)
            {
                uix = MouseKeyboardInfo.mouseState.X - posx;
                uiy = MouseKeyboardInfo.mouseState.Y - posy;
                Game1.uiPosX[uiDragging-1] = MouseKeyboardInfo.mouseState.X - posx;
                Game1.uiPosY[uiDragging - 1] = MouseKeyboardInfo.mouseState.Y - posy;
            }
        }
        /// <summary>
        /// If an item can be placed from the cursor, place it.
        /// </summary>
        public void PlaceItem()
        {
            //Game1.uiObjects[1].uix - 1, Game1.uiObjects[1].uiy - 1, 354, 396
            if (uiState == 1 && MouseKeyboardInfo.mouseState.RightButton == ButtonState.Pressed && cursorItem!=-1 && Game1.itemInfo.ITEM_PLACEABLE[cursorItem] && CanBePlaced(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16) && !(MouseKeyboardInfo.mouseState.X >= this.uix - 1 && MouseKeyboardInfo.mouseState.X <= this.uix - 1 + 514 && MouseKeyboardInfo.mouseState.Y >= this.uiy - 1 && MouseKeyboardInfo.mouseState.Y <= this.uiy - 1 + 514) &&
                (Game1.uiObjects[1] == null || !(MouseKeyboardInfo.mouseState.X >= Game1.uiObjects[1].uix - 1 && MouseKeyboardInfo.mouseState.X <= Game1.uiObjects[1].uix - 1 + 354 && MouseKeyboardInfo.mouseState.Y >= Game1.uiObjects[1].uiy - 1 && MouseKeyboardInfo.mouseState.Y <= Game1.uiObjects[1].uiy - 1 + 396)))
            {
                Game1.currentMap.mapTiles[((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)), ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))] = cursorItem;
                // Game1.client.messageQueue.Add(""+Game1.CLIENT_ID+" placeItem:"+ ((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16))+","+((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))+" "+cursorItem);
                if (cursorItem>=8&&cursorItem<=13) //small chest of any rank
                {
                    int[][] inv = new int[2][];
                    for (int i = 0; i < 2; i++)
                    {
                        inv[i] = new int[7*(cursorItem-7)];
                        for (int a = 0; a < 7 * (cursorItem-7); a++)
                        {
                            inv[i][a] = -1;
                        }
                    }
                    Chest chest = new Chest(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16, cursorItem-8, inv);
                    Game1.chestInventories.Add(chest);
                }
                if (cursorQuantity > 1)
                {
                    cursorQuantity--;
                }
                else
                {
                    cursorItem = -1;
                    cursorQuantity = -1;
                    Game1.globalCursor = 0;
                }
                //TODO: update this method and CanBePlaced() to account for item width/height
            }
        }
        public void InteractItem()
        {
            if (uiState == 1 && MouseKeyboardInfo.mouseClickedRight && cursorItem == -1 && !(MouseKeyboardInfo.mouseState.X >= this.uix - 1 && MouseKeyboardInfo.mouseState.X <= this.uix - 1 + 514 && MouseKeyboardInfo.mouseState.Y >= this.uiy - 1 && MouseKeyboardInfo.mouseState.Y <= this.uiy - 1 + 514) &&
                (Game1.uiObjects[1] == null || !(MouseKeyboardInfo.mouseState.X >= Game1.uiObjects[1].uix - 1 && MouseKeyboardInfo.mouseState.X <= Game1.uiObjects[1].uix - 1 + 354 && MouseKeyboardInfo.mouseState.Y >= Game1.uiObjects[1].uiy - 1 && MouseKeyboardInfo.mouseState.Y <= Game1.uiObjects[1].uiy - 1 + 396)) && 
                Game1.currentMap.mapTiles[((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)), ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))]!=-1)
            {
                if (Game1.currentMap.mapTiles[((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)), ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))] >= 8 && Game1.currentMap.mapTiles[((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)), ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))] <= 13)
                {
                    Game1.uiObjects[1] = new UI(Game1.uiPosX[1], Game1.uiPosY[1], Game1.currentMap.mapTiles[((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)), ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))]-7, Game1.chestInventories[Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))*16)].inventory[0],
                        Game1.chestInventories[Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))*16)].inventory[1],2,7);


                    if (Game1.openChest == Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16))
                    {
                        Game1.uiObjects[1] = null;
                        Game1.chestInventories[Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16)].chestState = 3;
                        Game1.openChest = -1;
                    }
                    else
                    {
                        Game1.chestInventories[Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16)].chestState = 1;
                        Game1.openChest = Chest.FindChestId(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16)) * 16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16)) * 16);
                    }
                    cursorItem = -1;
                    cursorQuantity = -1;
                    cursorItemIndex = -1;
                    cursorItemOrigin = -1;

                }
            }
        }
        public int CountUis()
        {
            int count = 0;
            for (int i = 0; i < Game1.uiObjects.Length; i++)
            {
                if (Game1.uiObjects[i] != null && WithinUi(Game1.uiObjects[i].uiState))
                {
                    count++;
                }
            }
            return count;
        }

        public bool WithinUi(int uiStateVar)
        {
            switch (uiStateVar)
            {
                case 1:
                    return ((MouseKeyboardInfo.mouseState.X >= Game1.uiObjects[0].uix - 1 && MouseKeyboardInfo.mouseState.X <= Game1.uiObjects[0].uix - 1 + 514 && MouseKeyboardInfo.mouseState.Y >= Game1.uiObjects[0].uiy - 1 && MouseKeyboardInfo.mouseState.Y <= Game1.uiObjects[0].uiy - 1 + 514));
                case 2:
                    return (Game1.uiObjects[1] != null && (MouseKeyboardInfo.mouseState.X >= Game1.uiObjects[1].uix - 1 && MouseKeyboardInfo.mouseState.X <= Game1.uiObjects[1].uix - 1 + 354 && MouseKeyboardInfo.mouseState.Y >= Game1.uiObjects[1].uiy - 1 && MouseKeyboardInfo.mouseState.Y <= Game1.uiObjects[1].uiy - 1 + 396));
                case 3:
                    return ((MouseKeyboardInfo.mouseState.X >= Game1.carryUi.uix - 1 && MouseKeyboardInfo.mouseState.X <= Game1.carryUi.uix + (Game1.carryUi.rowSize * (49)) && MouseKeyboardInfo.mouseState.Y >= Game1.carryUi.uiy - 1 && MouseKeyboardInfo.mouseState.Y <= Game1.carryUi.uiy - 1 + 42));
                default:
                    return false;
            }
        }
        /// <summary>
        /// If the cursor is dragging an item, then draw it.
        /// </summary>
        public void DrawCursorItem()
        { 
            if (cursorItem != -1 )
            {
                if (!Game1.itemInfo.ITEM_PLACEABLE[cursorItem] || CountUis()>0)
                {
                    Game1.spriteBatch.Begin();
                    Game1.items_32.DrawTile(Game1.spriteBatch, cursorItem, new Vector2(MouseKeyboardInfo.mouseState.X, MouseKeyboardInfo.mouseState.Y));
                    Game1.spriteBatch.DrawString(Game1.font, "" + cursorQuantity, new Vector2(MouseKeyboardInfo.mouseState.X, MouseKeyboardInfo.mouseState.Y+24), Color.White);
                    Game1.spriteBatch.End();
                }
                else if (Game1.itemInfo.ITEM_PLACEABLE[cursorItem])
                {
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[cursorItem], (CanBePlaced(((Player.playerx / 16) + ((MouseKeyboardInfo.mouseState.X + (Player.playerx % 16)) / 16))*16, ((Player.playery / 16) + ((MouseKeyboardInfo.mouseState.Y + (Player.playery % 16)) / 16))*16) ? Color.White:Color.Red) *0.5f, new Vector2(MouseKeyboardInfo.mouseState.X, MouseKeyboardInfo.mouseState.Y));
                    Game1.spriteBatch.DrawString(Game1.font, "" + cursorQuantity, new Vector2(MouseKeyboardInfo.mouseState.X, MouseKeyboardInfo.mouseState.Y+ 16), Color.White);
                    Game1.spriteBatch.End();
                }
            }
        }
        /// <summary>
        /// Draw items in inventory slots. Draw int quantities as spritefonts.
        /// </summary>
        private void DrawItems(int startx, int starty)
        {
            for (int i=0; i<inventoryItemIds.Length; i++) {
                if (inventoryItemIds[i] > -1)
                {
                    Game1.items_32.DrawTile(Game1.spriteBatch, inventoryItemIds[i], new Vector2(startx + 49 * (i % 7) + 1, starty + 48 * (i / 7) + 1));
                    Game1.spriteBatch.DrawString(Game1.font, ""+inventoryItemQuantities[i], new Vector2(startx + 49 * (i % 7) + 1, starty + 48 * (i / 7) + 24), Color.White);
                }
            }
        }

        private void Hover(int startx, int starty)
        {
            if (MouseKeyboardInfo.mouseState.X>=startx && MouseKeyboardInfo.mouseState.X<=startx+(rowSize * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= starty && MouseKeyboardInfo.mouseState.Y <= starty + (inventoryRows * (48)) - 5)
            {
                Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(startx + 49 * ((MouseKeyboardInfo.mouseState.X - startx) / 49) + 1, starty + 48 * ((MouseKeyboardInfo.mouseState.Y-starty) / 48) + 1, 42, 42), Color.White*0.25f);
            }
        }
        private int FindFreeSlot()
        {
            for (int i = 0; i < inventoryItemIds.Length; i++)
            {
                if (inventoryItemIds[i] == -1 || (inventoryItemIds[i] != -1 && cursorItem == inventoryItemIds[i] && inventoryItemQuantities[i]+cursorQuantity<Game1.ITEM_STACK_SIZE)) return i;
            }
            return -1;
        }

        private void DragAndDrop(int startx, int starty)
        {
            int gottenIndex = (MouseKeyboardInfo.mouseState.X - startx) / 49 + ((MouseKeyboardInfo.mouseState.Y - starty) / 48) * rowSize;
            
            //pick up part of an item on right click
            if ((CountUis()==1 || uiState != 1 && uiState != 3) && gottenIndex > -1 && (gottenIndex) < inventoryItemIds.Length && inventoryItemIds[gottenIndex] != -1 && ((cursorItem == inventoryItemIds[gottenIndex] && cursorItem!=-1) || cursorItem==-1) && MouseKeyboardInfo.mouseClickedRight && MouseKeyboardInfo.mouseState.X >= startx && MouseKeyboardInfo.mouseState.X <= startx + (rowSize * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= starty && MouseKeyboardInfo.mouseState.Y <= starty + (inventoryRows * (48)) - 5)
            {
                cursorItem = inventoryItemIds[gottenIndex];
                cursorItemIndex = gottenIndex;
                cursorItemOrigin = uiState;

                if (cursorQuantity == -1) cursorQuantity = 1;
                else cursorQuantity++;
                if (inventoryItemQuantities[gottenIndex] == 1)
                {
                    inventoryItemQuantities[gottenIndex] = -1;
                    inventoryItemIds[gottenIndex] = -1;
                }
                else inventoryItemQuantities[gottenIndex] -= 1;

                Game1.globalCursor = 1;
                
            }
            //pick up an item
            else if ((CountUis() == 1 || uiState != 1 && uiState != 3) && cursorItem == -1 && gottenIndex>-1 && (gottenIndex) < inventoryItemIds.Length && inventoryItemIds[gottenIndex] != -1 && MouseKeyboardInfo.mouseClickedLeft && MouseKeyboardInfo.mouseState.X >= startx && MouseKeyboardInfo.mouseState.X <= startx + (rowSize * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= starty && MouseKeyboardInfo.mouseState.Y <= starty + (inventoryRows * (48)) - 5)
            {
                cursorItem = inventoryItemIds[gottenIndex];
                inventoryItemIds[gottenIndex] = -1;
                cursorItemIndex = gottenIndex;
                cursorItemOrigin = uiState;
                cursorQuantity = inventoryItemQuantities[gottenIndex];
                Game1.globalCursor = 1;
            }
            //drop off an item
            else if ((CountUis() == 1 || uiState != 1 && uiState != 3) && cursorItem != -1 && (gottenIndex) >-1 && (gottenIndex) < inventoryItemIds.Length && MouseKeyboardInfo.mouseClickedLeft && MouseKeyboardInfo.mouseState.X >= startx && MouseKeyboardInfo.mouseState.X <= startx + (rowSize * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= starty && MouseKeyboardInfo.mouseState.Y <= starty + (inventoryRows * (48)) - 5)
            {
                
                //item in cursor is the same as the one in the slot
                if (inventoryItemIds[gottenIndex]==cursorItem)
                {
                    if (Game1.itemInfo.ITEM_STACKABLE[inventoryItemIds[gottenIndex]] && cursorQuantity + inventoryItemQuantities[gottenIndex] > Game1.ITEM_STACK_SIZE)
                    {
                        cursorItemOrigin = (FindFreeSlot() != -1 ? uiState : -1);
                        cursorQuantity = inventoryItemQuantities[gottenIndex] + cursorQuantity - Game1.ITEM_STACK_SIZE;
                        inventoryItemQuantities[gottenIndex] = Game1.ITEM_STACK_SIZE;
                        cursorItemIndex = -1; //have to find the item a new slot if it has to "return"
                        Game1.globalCursor = 1;

                    }
                    else
                    {
                        inventoryItemQuantities[gottenIndex] += cursorQuantity;
                        cursorQuantity = -1;
                        cursorItem = -1;
                        cursorItemIndex = -1;
                        cursorItemOrigin = -1;
                        Game1.globalCursor = 0;
                    }
                }
                //slot is empty
                else if (inventoryItemIds[gottenIndex]==-1)
                {
                    inventoryItemIds[gottenIndex] = cursorItem;
                    inventoryItemQuantities[gottenIndex] = cursorQuantity;
                    cursorItem = -1;
                    cursorItemIndex = -1;
                    cursorItemOrigin = -1;
                    cursorQuantity = -1;
                    Game1.globalCursor = 0;
                }
                //item in cursor is different from the one in the slot
                else
                {
                    int temp = cursorItem;
                    cursorItem = inventoryItemIds[gottenIndex];
                    inventoryItemIds[gottenIndex] = temp;
                    
                    temp = cursorQuantity;
                    cursorQuantity = inventoryItemQuantities[gottenIndex];
                    inventoryItemQuantities[gottenIndex] = (temp);
                }
            }
            //return an item that is dropped out of bounds
            else if (MouseKeyboardInfo.mouseClickedLeft){
                if (uiState == 1 && !(MouseKeyboardInfo.mouseState.X >= (this.uix + 136 + 19) && MouseKeyboardInfo.mouseState.X <= (this.uix + 136 + 19) + (7 * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= (this.uiy + 17) && MouseKeyboardInfo.mouseState.Y <= (this.uiy + 17) + (inventoryRows * (48)) - 5) &&
                    (Game1.uiObjects[1] == null || !(MouseKeyboardInfo.mouseState.X >= (Game1.uiObjects[1].uix + 7) && MouseKeyboardInfo.mouseState.X <= (Game1.uiObjects[1].uix + 7) + (rowSize * (49)) - 20 && MouseKeyboardInfo.mouseState.Y >= (Game1.uiObjects[1].uiy + 7) && MouseKeyboardInfo.mouseState.Y <= (Game1.uiObjects[1].uiy + 7) + (inventoryRows * (48)) - 5)) &&
                    (Game1.uiObjects[2] == null || !WithinUi(3))) {
                    //if item has a previous destination to return to, return it
                    int slot;

                    if (cursorItemOrigin != -1 && cursorItemIndex != -1)
                    {
                        if (cursorItem == (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemIds[cursorItemIndex])
                        {
                            (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemQuantities[cursorItemIndex] += cursorQuantity;
                            cursorQuantity = -1;
                        }
                        else
                        {
                            (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemQuantities[cursorItemIndex] = cursorQuantity;
                            cursorQuantity = -1;
                        }

                        (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemIds[cursorItemIndex] = cursorItem;
                        Debug.WriteLine("o");

                    }
                    else if (cursorItemOrigin != -1 && (Game1.uiObjects[cursorItemOrigin - 1]).FindFreeSlot()!=-1) {
                        slot = (Game1.uiObjects[cursorItemOrigin - 1]).FindFreeSlot();

                        if (cursorItem == (Game1.uiObjects[cursorItemOrigin + 1]).inventoryItemIds[slot])
                        {
                            (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemQuantities[slot] += cursorQuantity;
                            cursorQuantity--;
                        }
                        else
                        {
                            (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemQuantities[slot] = cursorQuantity;
                            cursorQuantity = -1;
                        }

                        (Game1.uiObjects[cursorItemOrigin - 1]).inventoryItemIds[slot] = cursorItem;

                        Debug.WriteLine("o k");
                    }
                    else{
                        slot = this.FindFreeSlot();
                        if (slot!=-1) {
                            if (cursorItem == this.inventoryItemIds[slot])
                            {
                                this.inventoryItemQuantities[slot] += cursorQuantity;
                                cursorQuantity--;
                            }
                            else
                            {
                                this.inventoryItemQuantities[slot] = cursorQuantity;
                                cursorQuantity = -1;
                            }

                            this.inventoryItemIds[slot] = cursorItem;
                            
                            Debug.WriteLine("o k o");
                        }
                        else if (Game1.uiObjects[1].FindFreeSlot() != -1)
                        {
                            slot = Game1.uiObjects[1].FindFreeSlot();

                            if (cursorItem == Game1.uiObjects[1].inventoryItemIds[slot])
                            {
                                Game1.uiObjects[1].inventoryItemQuantities[slot] += cursorQuantity;
                                cursorQuantity--;
                            }
                            else
                            {
                                Game1.uiObjects[1].inventoryItemQuantities[slot] = cursorQuantity;
                                cursorQuantity = -1;
                            }

                            Game1.uiObjects[1].inventoryItemIds[slot] = cursorItem;
                            
                            Debug.WriteLine("o k o k");
                        }
                    }
                    cursorItem = -1;
                    cursorQuantity = -1;
                    cursorItemIndex = -1;
                    cursorItemOrigin = -1;
                    Game1.globalCursor = 0;
                }
            }
        }
        public bool CanBePlaced(int x, int y)
        {
            //figures out for a REQUIRE_SURFACE item whether the cursor is on top of a surface (solid) or whether the space is free for item placement
            //TODO: make it so you can't place items over the player
            if (MouseKeyboardInfo.mouseState.X>=0 && MouseKeyboardInfo.mouseState.Y >= 0 && MouseKeyboardInfo.mouseState.X < Game1.WINDOW_WIDTH && MouseKeyboardInfo.mouseState.Y < Game1.WINDOW_HEIGHT && Game1.itemInfo.ITEM_REQUIRE_SURFACE[cursorItem] && Game1.currentMap.mapTiles[x / 16, y / 16 + 1] != -1 && Game1.itemInfo.ITEM_SOLID[Game1.currentMap.mapTiles[x / 16, y / 16 + 1]] && Game1.currentMap.mapTiles[x / 16, y / 16] == -1) return true;
            else if (MouseKeyboardInfo.mouseState.X >= 0 && MouseKeyboardInfo.mouseState.Y >= 0 && MouseKeyboardInfo.mouseState.X < Game1.WINDOW_WIDTH && MouseKeyboardInfo.mouseState.Y < Game1.WINDOW_HEIGHT && !Game1.itemInfo.ITEM_REQUIRE_SURFACE[cursorItem] && Game1.currentMap.mapTiles[x / 16, y / 16] == -1) return true;
            return false;
        }
    }
}
