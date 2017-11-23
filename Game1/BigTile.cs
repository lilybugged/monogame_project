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
    public class BigTile
    {
        public int tileType;
        public int tilex;
        public int tiley;
        public int width;
        public int height;
        public int timer = -1;
        public int recipeInProgressIndex = -1;
        public int recipeOutputIndex = -1;
        //private int lastTick = 0;
        private int[] endpoint;
        public bool solid;
        public int state = 0;
        public int[][] inventory; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int[][] output; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int tileState = 0; // dependent on what the tile is
        int tileRank = 0; //0-2 - small, medium, large
        Color[] tileRankColors = new Color[] {Color.White, new Color(128,128,128,255), new Color(229,137,104, 255),
        new Color(199,199,199, 255),new Color(244,220,151, 255),new Color(188,216,237, 255)};

        public BigTile(int type, int x, int y, int rank, int[][] inv)
        {

            tilex = x;
            tiley = y;
            inventory = inv;
            tileRank = rank;
            tileType = type;

            width = Game1.itemInfo.ITEM_BIGTILE_WIDTH[tileType];
            height = Game1.itemInfo.ITEM_BIGTILE_HEIGHT[tileType];
            solid = Game1.itemInfo.ITEM_SOLID[tileType];
            for (int i = x / 16 * 16; i < x + width * 16; i++)
            {
                for (int a = y / 16 * 16 - height * 16 + 16; a < y / 16 * 16 + 16; a++)
                {
                    Game1.currentMap.mapTiles[i / 16, a / 16] = tileType;
                }
            }
            
            switch (tileType)
            {
                case 31:
                    int[,] map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
                    Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    endpoint = new int[] { -1, -1 };
                    endpoint = FindEndPoint(tilex / 16, tiley / 16, 3);
                    Debug.WriteLine(""+endpoint[0]+","+endpoint[1]);
                    break;
                case 29:
                    inventory = new int[2][];
                    inventory[0] = new int[8];
                    inventory[1] = new int[8];
                    for (int i=0;i<2;i++)
                    {
                        for (int a = 0; a < 8; a++)
                        {
                            inventory[i][a] = -1;
                        }
                    }

                    output = new int[2][];
                    output[0] = new int[8];
                    output[1] = new int[8];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < 8; a++)
                        {
                            output[i][a] = -1;
                        }
                    }
                    break;
                case 30:
                    PipeUpdate();
                    UpdateNearbyPipes(); //can change this to UpdateAllPipes, but worried about performance
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    inventory = new int[2][];
                    inventory[0] = new int[(tileType-7)*7];
                    inventory[1] = new int[(tileType - 7) * 7];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < (tileType - 7) * 7; a++)
                        {
                            inventory[i][a] = -1;
                        }
                    }
                    output = inventory;
                    break;
            }

        }
        public void Update()
        {
            if (timer > 0) timer--;
            switch (tileType)
            {
                case 31:
                    int[,] map = Game1.currentMap.mapTiles;
                    Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    switch (state)
                    {
                        case 1:
                            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]!=-1
                                && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                                && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 32
                                && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 31)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 - 1, 1);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)!=-1)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)],endpoint);
                                }
                            }
                            break;
                        case 2:
                            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 
                                && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                                && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 32
                                && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 31)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 + 1, 2);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16) != -1)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)], endpoint);
                                }

                            }
                            break;
                        case 3:
                            if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 
                                && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                                && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 32
                                && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 31)
                            {
                                endpoint = FindEndPoint(tilex / 16 + 1, tiley / 16, 3);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery)) != -1)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                        case 4:
                            if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 
                                && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                                && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 32
                                && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 31)
                            {
                                endpoint = FindEndPoint(tilex / 16 - 1, tiley / 16, 4);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery)) != -1)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                    }
                    
                    break;
                case 29:
                    //WIP
                    //if (Game1.globalTick>14 && Array.IndexOf(inventory[0], 33) != -1 && inventory[1][Array.IndexOf(inventory[0], 33)]>0)
                    {
                        if (timer == -1)
                        {
                            //if (Array.IndexOf(inventory[0], Recipes.recipeInputIds[i]) != -1 && (Array.IndexOf(inventory[0], Recipes.recipeOutputIds[i]) != -1 || Array.IndexOf(inventory[0], -1) == -1))
                            for (int a = 0; a < inventory[0].Length; a++)
                            {
                                for (int i = 0; i < Recipes.RECIPES; i++)
                                {
                                    if (Array.IndexOf(output[0], Recipes.recipeOutputIds[i]) == -1 && Array.IndexOf(output[0], -1) == -1)
                                    {
                                        break;
                                    }
                                    if (Recipes.recipeInputIds[i][0] == inventory[0][a] && Recipes.recipeInputQuants[i][0] <= inventory[1][a])
                                    {
                                        timer = Recipes.recipeProcessingTime[i];
                                        recipeInProgressIndex = i;
                                        inventory[1][a] -= Recipes.recipeInputQuants[i][0];
                                        state = 1;

                                        if (inventory[1][a] < 1)
                                        {
                                            inventory[0][a] = -1;
                                            inventory[1][a] = -1;
                                        }
                                        goto Outerloop;
                                        
                                        //we'll check for an open output slot when the timer goes off if need be
                                        //items can move around during processing time is why
                                    }
                                }
                            }
                        }
                        Outerloop: Debug.Write("");
                        if (timer == 0)
                        {
                            if (recipeOutputIndex == -1)
                            {
                                for (int b = 0; b < output[0].Length; b++)
                                {
                                    if (output[0][b] == Recipes.recipeOutputIds[recipeInProgressIndex][0]
                                        && output[1][b] + Recipes.recipeOutputQuants[recipeInProgressIndex][0] <= Game1.ITEM_STACK_SIZE)
                                    {
                                        recipeOutputIndex = b;
                                        break;
                                    }
                                }
                                if (recipeOutputIndex == -1)
                                {
                                    recipeOutputIndex = Array.IndexOf(output[0],-1);
                                }
                            }
                            if (recipeOutputIndex != -1)
                            {
                                output[0][recipeOutputIndex] = Recipes.recipeOutputIds[recipeInProgressIndex][0];
                                output[1][recipeOutputIndex] = (output[1][recipeOutputIndex]<1)?Recipes.recipeOutputQuants[recipeInProgressIndex][0]:
                                    output[1][recipeOutputIndex]+ Recipes.recipeOutputQuants[recipeInProgressIndex][0];

                                timer = -1;
                                recipeInProgressIndex = -1;
                                recipeOutputIndex = -1;
                                state = 0;
                            }

                            /*inventory[1][Array.IndexOf(inventory[0], 33)] = (inventory[1][Array.IndexOf(inventory[0], 33)] > 1) ? (inventory[1][Array.IndexOf(inventory[0], 33)] - 1) : -1;
                            if (inventory[1][Array.IndexOf(inventory[0], 33)] < 1) inventory[0][Array.IndexOf(inventory[0], 33)] = -1;
                            output[0][0] = 34;

                            output[1][0] = (output[1][0] > 0) ? output[1][0] + 1 : 1;
                            Debug.WriteLine("" + output[0]);
                            Debug.WriteLine("" + output[1]);*/
                        }
                    }
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    if (Game1.openChest != -1 && Game1.bigTiles[Game1.openChest] == this && Player.RangeFromPoint(tilex, tiley)[0] >= Game1.PLAYER_RANGE_REQUIREMENT)
                    {
                        state = 0;
                        Game1.uiObjects[1] = null;
                        Game1.openChest = -1;
                        Debug.WriteLine("shut");
                    }
                    if (state == 1 && Game1.chestSprites[0].currentFrame == 3)
                    {
                        state = 2;
                    }
                    if (state == 3 && Game1.chestSprites[1].currentFrame == 3)
                    {
                        state = 0;
                    }
                    if (state == 2 && (Game1.openChest==-1 || Game1.bigTiles[Game1.openChest] != this))
                    {
                        state = 3;
                        Debug.WriteLine("closing");
                    }
                    break;
            }
        }
        public void Trigger()
        {
            switch (tileType)
            {
                case 32:
                    state++;
                    if (state > 4) state = 1;
                    UpdateNearbyPipes();
                    break;
                case 31:
                    state++;
                    if (state > 4) state = 1;
                    switch (state)
                    {
                        case 1:
                            endpoint = FindEndPoint(tilex / 16, tiley / 16 - 1, state);
                            break;
                        case 2:
                            endpoint = FindEndPoint(tilex / 16, tiley / 16 + 1, state);
                            break;
                        case 3:
                            endpoint = FindEndPoint(tilex / 16 + 1, tiley / 16, state);
                            break;
                        case 4:
                            endpoint = FindEndPoint(tilex / 16 - 1, tiley / 16, state);
                            break;
                    }
                    
                    Debug.WriteLine(""+endpoint[0]+", "+endpoint[1]);
                    if (endpoint[0] != -1) Debug.WriteLine(""+Game1.currentMap.mapTiles[endpoint[0],endpoint[1]]);
                    UpdateNearbyPipes();
                    break;
                case 30:
                    state++;
                    if (state == 1 || state > 8) state = 3;
                    //UpdateNearbyPipes();
                    break;
                case 29:
                    if (Game1.uiObjects[1] != null)
                    {
                        Game1.uiObjects[1] = null;
                        Game1.openChest = -1;
                    }
                    else
                    {
                        UI ui = new UI(Game1.uiPosX[1], Game1.uiPosY[1], 2, inventory[0], inventory[1], output[0], output[1], 4, 4);
                        ui.attachment = this;
                        Game1.uiObjects[1] = ui;
                        Game1.openChest = -1;
                    }
                    break;
                case 27:
                    //door 
                    Debug.WriteLine("door trigger");
                    if (state == 0)
                    {
                        state = (Player.currentDirection == 0 ? 1 : 2);
                        solid = false;
                    }
                    else
                    {
                        state = 0;
                        solid = true;
                    }
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    Game1.uiObjects[1] = new UI(Game1.uiPosX[1], Game1.uiPosY[1], tileType-7, this.inventory[0],this.inventory[1], null, null, 2, 7);
                    Game1.uiToggle = false;
                    
                    if (state == 2 && Game1.openChest == BigTile.FindTileId(tilex,tiley))
                    {
                        Debug.WriteLine("closed");
                        Game1.uiObjects[1] = null;
                        if (UI.cursorItemOrigin == 2)
                        {
                            UI.cursorItemOrigin = -1;
                            UI.cursorItemIndex = -1;
                        }
                        state = 3;
                        Game1.openChest = -1;
                    }
                    else
                    {
                        state = 1;
                        Game1.openChest = BigTile.FindTileId(tilex, tiley);
                        Debug.WriteLine(Game1.openChest);
                    }
                    UI.cursorItem = -1;
                    UI.cursorQuantity = -1;
                    UI.cursorItemIndex = -1;
                    UI.cursorItemOrigin = -1;
                    break;
                default:
                    Debug.WriteLine("default trigger");
                    break;
                
            }
        }
        public void Draw()
        {
            Draw(tilex - Player.playerx, tiley - Player.playery);
        }
        public void Draw(int x, int y)
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
            switch (tileType)
            {
                case 32:
                    Game1.spriteBatch.Begin();
                    //Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 119, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]!=31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]!=32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 120, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]!=31 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]!=32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 121, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]!=31 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]!=32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 122, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 119, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 119 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 31:
                    Game1.spriteBatch.Begin();
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 123, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 124, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 125, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 126, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 123, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 123 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 30:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 110+state, position);
                    Game1.spriteBatch.End();
                    break;
                case 29:
                    Game1.spriteBatch.Begin();
                    //Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 44, position); //nounder
                        if (state == 1)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 128, position); //lit
                        }
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 43, position); //else
                        if (state == 1)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 127, position); //lit
                        }
                    }
                    Game1.spriteBatch.End();
                    break;
                case 28:
                    Game1.spriteBatch.Begin();
                    if (Game1.globalTick / 4 > 2)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 98 + 1, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 101 + 1, new Vector2(x + 16, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 107 + 1, new Vector2(x + 16, y));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 104 + 1, new Vector2(x, y));
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 98 + Game1.globalTick / 4, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 101 + Game1.globalTick / 4, new Vector2(x + 16, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 107 + Game1.globalTick / 4, new Vector2(x + 16, y));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 104 + Game1.globalTick / 4, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 27:
                    Game1.spriteBatch.Begin();
                    if (state == 0)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 92, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 93, new Vector2(x, y));
                    }
                    else if (state == 1)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 96, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 97, new Vector2(x, y));
                    }
                    else if (state == 2)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 94, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 95, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    switch (state)
                    {
                        case 0:
                            Game1.spriteBatch.Begin();
                            Game1.tiles.DrawTile(Game1.spriteBatch, 35, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.tiles.DrawTile(Game1.spriteBatch, 37, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            Game1.spriteBatch.End();
                            break;
                        case 1:
                            Game1.chestSprites[0].Draw(Game1.spriteBatch, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.chestSprites[2].Draw(Game1.spriteBatch, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            break;
                        case 2:
                            Game1.spriteBatch.Begin();
                            Game1.tiles.DrawTile(Game1.spriteBatch, 36, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.tiles.DrawTile(Game1.spriteBatch, 38, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            Game1.spriteBatch.End();
                            break;
                        case 3:
                            Game1.chestSprites[1].Draw(Game1.spriteBatch, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.chestSprites[3].Draw(Game1.spriteBatch, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            break;
                    }
                    break;
            }
        }
        public void Destroy()
        {
            if (width==1 && height == 1)
            {
                Game1.currentMap.mapTiles[tilex / 16, tiley / 16] = -1;
            }
            else
            {
                for (int i = tilex / 16 * 16; i < tilex + width * 16; i++)
                {
                    for (int a = tiley / 16 * 16 - height * 16 + 16; a < tiley / 16 * 16 + 16; a++)
                    {
                        Game1.currentMap.mapTiles[i / 16, a / 16] = -1;
                    }
                }
            }
            if (tileType==30) UpdateNearbyPipes();
            Game1.bigTiles.Remove(this);
        }
        public static int FindTileId(int idx, int idy)
        {
            for (int i = 0; i < Game1.bigTiles.Count; i++)
            {
                //idx = idx - idx % 16;
                //idy = idy - idy % 16;
                if (idx >= Game1.bigTiles[i].tilex && idx < Game1.bigTiles[i].tilex + Game1.bigTiles[i].width*16 &&
                    idy >= Game1.bigTiles[i].tiley - Game1.bigTiles[i].height * 16 + 16&& idy < Game1.bigTiles[i].tiley + 16)
                {
                    return i;
                }
            }
            return -1;
        }



        // MORE FUNCTIONS
        
        /// <summary>
        /// finds an item in a given inventory and "pushes" it to another given inventory
        /// MUST ONLY be called from a tileType 32 endpoint
        /// </summary>
        public static void PushAnItem(BigTile startInv, int[] endPoint)
        {
            BigTile end;
            //check if there are items in the starting inventory's output
            for (int i = 0; i < startInv.output[0].Length; i++)
            {
                if (startInv.output[0][i] != -1 && (endPoint[0] != -1 && BigTile.FindTileId(endPoint[0]*16, endPoint[1]*16) != -1))
                {
                    // check if there's a spot in the endpoint's attached inventory, if there is one
                    switch(Game1.bigTiles[BigTile.FindTileId(endPoint[0]*16, endPoint[1]*16)].state)
                    {
                        case 1:
                            if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i]<1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a] ++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 3:
                            if (BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            OuterLoop:;
        }
        ///finds an endpoint pipe (a puller) from the given starting pipe (a pusher) and returns an array [x,y]
        public static int[] FindEndPoint(int x, int y, int direction)
        {
            if (Game1.currentMap.mapTiles[x, y] == 32)
            {
                //Debug.WriteLine(""+x+","+y);
                if (BigTile.FindTileId(x*16, y* 16) != -1)
                {
                    switch(Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                    {
                        case 1:
                            if (direction == 2) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 2:
                            if (direction == 1) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 3:
                            if (direction == 4) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 4:
                            if (direction == 3) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        default:
                            return new int[] { -1, -1 };
                    }
                }
                else return new int[] { -1, -1 };
            }
            else if (Game1.currentMap.mapTiles[x, y] == 31)
            {
                return new int[] { -1, -1 };
            }
            else if (Game1.currentMap.mapTiles[x, y] == 30)
            {
                switch (direction)
                {
                    case 1:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 4:
                                return FindEndPoint(x, y - 1, 1);
                                break;
                            case 5:
                                return FindEndPoint(x + 1, y, 3);
                                break;
                            case 7:
                                return FindEndPoint(x - 1, y, 4);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 2:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 4:
                                return FindEndPoint(x, y + 1, 2);
                                break;
                            case 6:
                                return FindEndPoint(x + 1, y, 3);
                                break;
                            case 8:
                                return FindEndPoint(x - 1, y, 4);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 3:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 3:
                                return FindEndPoint(x + 1, y, 3);
                                break;
                            case 7:
                                return FindEndPoint(x, y + 1, 2);
                                break;
                            case 8:
                                return FindEndPoint(x, y - 1, 1);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 4:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 3:
                                return FindEndPoint(x - 1, y, 4);
                                break;
                            case 5:
                                return FindEndPoint(x, y + 1, 2);
                                break;
                            case 6:
                                return FindEndPoint(x, y - 1, 1);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    default:
                        return new int[] { -1, -1 };
                }
            }
            else return new int[] { -1, -1 };
        }
        
        /// <summary>
        /// updates pipes above, below, left and right of this pipe,
        /// but not the pipe itself
        /// </summary>
        public void UpdateNearbyPipes()
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);

            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 30)
            {
                Debug.WriteLine(""+tilex+','+tiley);
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].PipeUpdate();
                Debug.WriteLine("checked upward");
            }
            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 30)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].PipeUpdate();
            }
            if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 30)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].PipeUpdate();
            }
            if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 30)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].PipeUpdate();
            }
        }
        public static void UpdateAllPipes()
        {
            foreach (BigTile tile in Game1.bigTiles)
            {
                if (tile.tileType == 30)
                {
                    tile.PipeUpdate();
                }
            }
        }
        /// <summary>
        /// updates this pipe
        /// </summary>
        private void PipeUpdate()
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[30] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);

            
            {
                if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 30)
                || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3))
                && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 30
                    || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                    || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //left AND right
                    state = 3;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 30
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1)))
                {
                    //under AND above
                    state = 4;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1))
                    && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 30
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //downright
                    state = 5;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1))
                    && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 30
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3)))
                {
                    //downleft
                    state = 7;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 30
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //upright
                    state = 6;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 30
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3)))
                {
                    //upleft
                    state = 8;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 30)
                    || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                    || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3))
                    || (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 30
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //left OR right
                    state = 3;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 30)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 30
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 31 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == 32 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1)))
                {
                    //under OR above
                    state = 4;
                }
                else
                {
                    //none
                    state = 0;
                }
            }
        }
    }
}
