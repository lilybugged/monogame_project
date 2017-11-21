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
                    endpoint = new int[] { -1, -1 };
                    endpoint = FindEndPoint(tilex / 16, tiley / 16, 3);
                    Debug.WriteLine(""+endpoint[0]+","+endpoint[1]);

                    break;
                case 29:
                    break;
            }

        }
        public void Update()
        {
            switch (tileType)
            {
                case 31:
                    int[,] map = Game1.currentMap.mapTiles;
                    Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    switch (state)
                    {
                        case 1:
                            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]!=-1&&Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16, 1);
                                if (BigTile.FindTileId(endpoint[0]*16,endpoint[1]*16)!= -1)
                                {
                                    Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state = 1;
                                }
                                else Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state = 0;
                            }
                            break;
                        case 2:

                            break;
                        case 3:

                            break;
                        case 4:

                            break;
                    }
                    break;
            }
        }
        public void Trigger()
        {
            switch (tileType)
            {
                case 31:
                    state++;
                    if (state > 4) state = 1;
                    endpoint = FindEndPoint(tilex / 16, tiley / 16, state);
                    Debug.WriteLine(""+endpoint[0]+", "+endpoint[1]);
                    if (endpoint[0] != -1) Debug.WriteLine(""+Game1.currentMap.mapTiles[endpoint[0],endpoint[1]]);
                    break;
                case 32:
                    state++;
                    if (state > 4) state = 1;
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
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 119, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 120, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 121, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])
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
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 123, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 124, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 125, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])
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

                    if ((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == tileType || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]) &&
                            (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == tileType || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 113, position); //left AND right
                        state = 3;
                    }
                    else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType) || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]) &&
                        (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == tileType) || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1) && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 114, position); //under AND above
                        state = 4;
                    }
                    else if (((map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == tileType) || (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])) &&
                        ((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType) || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 115, position); //downright
                        state = 5;
                    }
                    else if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == tileType) || (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])) &&
                        ((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType) || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 117, position); //downleft
                        state = 7;
                    }
                    else if (((map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == tileType) || (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])) &&
                        ((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == tileType) || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 116, position); //upright
                        state = 6;
                    }
                    else if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == tileType) || (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])) &&
                        ((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == tileType) || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 118, position); //upleft
                        state = 8;
                    }
                    else if ((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType) ||
                        (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == tileType))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 114, position); //under or above
                        state = 4;
                    }
                    else if ((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == tileType) ||
                            (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == tileType))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 113, position); //left or right
                        state = 3;
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 110, position); //none
                        state = 0;
                    }

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
            }
        }
        public void Destroy()
        {
            for (int i = tilex / 16 * 16; i < tilex + width * 16; i++)
            {
                for (int a = tiley / 16 * 16 - height * 16 + 16; a < tiley / 16 * 16 + 16; a++)
                {
                    Game1.currentMap.mapTiles[i / 16, a / 16] = -1;
                }
            }
            Game1.bigTiles.Remove(this);
        }
        public static int FindTileId(int idx, int idy)
        {
            for (int i = 0; i < Game1.bigTiles.Count; i++)
            {
                if (idx >= Game1.bigTiles[i].tilex && idx < Game1.bigTiles[i].tilex + Game1.bigTiles[i].width*16 &&
                    idy >= Game1.bigTiles[i].tiley - Game1.bigTiles[i].height * 16 + 16&& idy < Game1.bigTiles[i].tiley + 16)
                {
                    return i;
                }
            }
            return -1;
        }



        // MORE FUNCTIONS

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
                switch (direction)
                {
                    case 1:
                        if (Game1.currentMap.mapTiles[x, y - 1] == 30 || Game1.currentMap.mapTiles[x, y - 1] == 31 || Game1.currentMap.mapTiles[x, y - 1] == 32)
                        {
                            return FindEndPoint(x, y - 1, 1);
                        }
                        else return new int[] { -1, -1 };
                        break;
                    case 2:
                        if (Game1.currentMap.mapTiles[x, y + 1] == 30 || Game1.currentMap.mapTiles[x, y + 1] == 31 || Game1.currentMap.mapTiles[x, y + 1] == 32)
                        {
                            return FindEndPoint(x, y + 1, 2);
                        }
                        else return new int[] { -1, -1 };
                        break;
                    case 3:
                        if (Game1.currentMap.mapTiles[x + 1, y] == 30 || Game1.currentMap.mapTiles[x + 1, y] == 31 || Game1.currentMap.mapTiles[x + 1, y] == 32)
                        {
                            return FindEndPoint(x +1, y, 3);
                        }
                        else return new int[] { -1, -1 };
                        break;
                    case 4:
                        if (Game1.currentMap.mapTiles[x - 1, y] == 30 || Game1.currentMap.mapTiles[x - 1, y] == 31 || Game1.currentMap.mapTiles[x - 1, y] == 32)
                        {
                            return FindEndPoint(x - 1, y, 4);
                        }
                        else return new int[] { -1, -1 };
                        break;
                    default:
                        return new int[] { -1, -1 };
                }
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
    }
}
