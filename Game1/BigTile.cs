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
        public bool solid;
        private int state = 0;
        public int[][] inventory; // first index is itemid(0)/quantity(1), second is position in the inventory
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

        }
        public void Update()
        {
        }
        public void Trigger()
        {
            switch (tileType)
            {
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
            switch (tileType)
            {
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
        public static int FindTileId(int x, int y)
        {
            for (int i = 0; i < Game1.bigTiles.Count; i++)
            {
                if (x >= Game1.bigTiles[i].tilex && x < Game1.bigTiles[i].tilex + Game1.bigTiles[i].width*16 &&
                    y >= Game1.bigTiles[i].tiley - Game1.bigTiles[i].height * 16 + 16&& y < Game1.bigTiles[i].tiley + 16)
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
