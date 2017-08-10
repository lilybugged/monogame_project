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
    public class MapInfo
    {
        public int[,] mapTiles;
        public int[,] mapBackTiles;
        public MapInfo(int length, int height)
        {
            mapTiles = new int[length,height];
            mapBackTiles = new int[length, height];
            //mapTiles stores item ids, which can be used to get the block ids to draw

            for (int i = 0; i < length; i++)
            {
                for (int a = 0; a < height; a++)
                {
                    mapTiles[i, a] = -1;
                    mapBackTiles[i, a] = -1;
                }
            }
            for (int i = 0; i < length; i++)
            {
                for (int a = 120; a < height; a++)
                {
                    mapTiles[i, a] = 4;
                }
            }
            //mapTiles[0, 90] = 4;
        }
        public void DrawMap()
        {
            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int a = 0; a < mapTiles.GetLength(1); a++)
                {
                    if (mapBackTiles[i, a] != -1)
                    {
                        if (!Game1.itemInfo.ITEM_AUTOTILE[mapBackTiles[i, a]])
                            Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[mapBackTiles[i, a]], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        else ItemInfo.DrawAutoTile(mapBackTiles[i, a], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                    }
                    if (mapTiles[i, a] != -1)
                    {
                        if (!Game1.itemInfo.ITEM_BIGTILE[mapTiles[i, a]])
                        {
                            if (!Game1.itemInfo.ITEM_AUTOTILE[mapTiles[i, a]])
                                Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[mapTiles[i, a]], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            else ItemInfo.DrawAutoTile(mapTiles[i, a], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                    }
                }
            }
        }
    }
}
