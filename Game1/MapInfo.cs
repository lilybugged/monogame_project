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
        public Color[] WIRE_COLORS = new Color[] { new Color(231, 63, 63), new Color(82, 101, 122), new Color(229, 203, 130), new Color(224, 255, 79) };
        public int mapLength;
        public int mapHeight;
        public int[,] mapWires;
        public double[,] mapFluids;
        public int[,] mapFluidIds;
        public int[,] mapTiles;
        public int[,] mapBackTiles;
        public int updatingFluids = 200;
        public MapInfo(int length, int height)
        {
            mapWires = new int[length, height];
            mapTiles = new int[length, height];
            mapFluids = new double[length, height];
            mapFluidIds = new int[length, height];
            mapBackTiles = new int[length, height];
            //mapTiles stores item ids, which can be used to get the block ids to draw

            mapLength = length;
            mapHeight = height;

            for (int i = 0; i < length; i++)
            {
                for (int a = 0; a < height; a++)
                {
                    mapWires[i, a] = -1;
                    mapFluids[i, a] = 0;
                    mapFluidIds[i, a] = -1;
                    mapTiles[i, a] = -1;
                    mapBackTiles[i, a] = -1;
                }
            }
        }
        public void InitializeMap(int length, int height)
        {
            int[] mapHeights = new int[length];
            mapHeights[1] = 120;
            if (120 - mapHeights[1] < 30) mapHeights[1] = 30;
            if (120 - mapHeights[1] > height - 20) mapHeights[1] = height - 21;

            Random rng = new Random();
            int roll = 0;
            for (int i = 2; i < length - 2; i++)
            {
                roll = rng.Next(0, 6);
                switch (roll)
                {
                    case 0:
                        mapHeights[i] = mapHeights[i - 1];
                        break;
                    case 1:
                        mapHeights[i] = mapHeights[i - 1] - 1;
                        break;
                    case 2:
                        mapHeights[i] = mapHeights[i - 1];
                        break;
                    case 3:
                        mapHeights[i] = mapHeights[i - 1] + 1;
                        break;
                    case 4:
                        mapHeights[i] = mapHeights[i - 1];
                        break;
                    case 5:
                        mapHeights[i] = mapHeights[i - 1];
                        break;
                }
                if (120 - mapHeights[i - 1] < 30) mapHeights[i] = 30;
                if (120 - mapHeights[i - 1] > height - 20) mapHeights[i] = height - 21;
            }
            for (int i = 1; i < length - 1; i++)
            {
                bool first = true;
                for (int a = 120 - mapHeights[i]; a < height - 1; a++)
                {
                    if (first)
                    {
                        if (rng.Next(0, 5) == 0)
                        {
                            BigTile tile2 = new BigTile(8, i * 16, a * 16 - 16, 0, null);
                            for (int b = 0; b < rng.Next(0, 7); b++)
                            {
                                tile2.inventory[0][b] = rng.Next(0, Game1.itemInfo.ITEM_NAME.Length);
                                tile2.inventory[1][b] = rng.Next(0, 99);
                            }
                            Game1.bigTiles.Add(tile2);
                        }

                        mapTiles[i, a] = 49;
                        first = false;
                    }
                    else mapTiles[i, a] = 50;
                }
            }
        }
        public void DrawFluids()
        {
            Game1.spriteBatch.Begin();
            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int a = 0; a < mapTiles.GetLength(1); a++)
                {
                    //down
                    if (i < mapTiles.GetLength(0) - 1 && a < mapTiles.GetLength(1) - 1 && mapTiles[i, a] == -1 && mapTiles[i, a + 1] == -1 && mapFluids[i, a + 1] + mapFluids[i, a] > 100)
                    {
                        mapFluids[i, a] = mapFluids[i, a] - (100 - mapFluids[i, a + 1]);
                        mapFluids[i, a + 1] = 100;
                        mapFluidIds[i, a + 1] = mapFluidIds[i, a];
                        //Debug.WriteLine("aaa1");
                    }
                    else if (i < mapTiles.GetLength(0) - 1 && a < mapTiles.GetLength(1) - 1 && mapTiles[i, a] == -1 && mapTiles[i, a + 1] == -1 && mapFluids[i, a] > -1)
                    {
                        mapFluids[i, a + 1] += mapFluids[i, a];
                        mapFluids[i, a] = 0;
                        mapFluidIds[i, a + 1] = mapFluidIds[i, a];
                        //Debug.WriteLine("aaa2");
                    }
                    //horz
                    if (i > 1 && mapTiles[i, a] == -1 && mapTiles[i - 1, a] == -1 && (mapFluids[i - 1, a] > 1 || mapFluids[i, a] > 1))
                    {
                        double total = 0;
                        //Debug.WriteLine("aaa3");
                        //Debug.WriteLine("" + mapFluids[i, a] + "," + mapFluids[i - 1, a]);
                        total = mapFluids[i, a] + mapFluids[i - 1, a];
                        mapFluids[i - 1, a] = total / 2;
                        mapFluids[i, a] = total / 2;
                        if (mapFluidIds[i - 1, a] == -1)
                        {
                            mapFluidIds[i - 1, a] = mapFluidIds[i, a];
                        }
                        else mapFluidIds[i, a] = mapFluidIds[i - 1, a];
                    }

                    if (mapTiles[i, a] == -1 && mapFluids[i, a] > 0)
                    {
                        for (int b = 0; b < 16; b++)
                        {
                            if (b < (int)(Math.Round(mapFluids[i, a]) / 100.0 * 16)) Game1.fluids.DrawTile(Game1.spriteBatch, mapFluidIds[i, a], Color.White * 0.9f, new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery + 15 - b));
                            if (b == (int)(Math.Round(mapFluids[i, a]) / 100.0 * 16) && mapFluids[i, a - 1] < 1) Game1.fluids.DrawTile(Game1.spriteBatch, mapFluidIds[i, a], Color.Blue * 0.9f, new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery + 15 - b));
                        }
                    }
                }
            }
            Game1.spriteBatch.End();
        }
        public void DrawBackTileMap()
        {
            Game1.spriteBatch.Begin();
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
                }
            }
            Game1.spriteBatch.End();
        }
        public void DrawMap()
        {
            Game1.spriteBatch.Begin();
            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int a = 0; a < mapTiles.GetLength(1); a++)
                {
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
            Game1.spriteBatch.End();
            for (int i = 0; i < Game1.bigTiles.Count; i++)
            {
                Game1.bigTiles[i].Draw();
            }
            Game1.spriteBatch.Begin();
            if (Game1.playerEquippedItems[5] == 43)
            {
                //Game1.spriteBatch.Draw(Game1.pixel, new Rectangle(0, 0, Game1.WINDOW_WIDTH, Game1.WINDOW_HEIGHT), Color.LightGoldenrodYellow * 0.25f);
            }

            for (int i = 0; i < mapTiles.GetLength(0); i++)
            {
                for (int a = 0; a < mapTiles.GetLength(1); a++)
                {
                    
                    if (mapWires[i, a] != -1 && Game1.playerEquippedItems[5] == 42)
                    {

                        int[,] surround = new int[3, 3];

                        surround[0, 0] = mapWires[i - 1, a - 1];
                        surround[1, 0] = mapWires[i, a - 1];
                        surround[2, 0] = mapWires[i + 1, a - 1];
                        surround[0, 1] = mapWires[i - 1, a];
                        surround[2, 1] = mapWires[i + 1, a];
                        surround[0, 2] = mapWires[i - 1, a + 1];
                        surround[1, 2] = mapWires[i, a + 1];
                        surround[2, 2] = mapWires[i + 1, a + 1];

                        if (surround[1, 0] != mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 71, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                        else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 9, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                        else
                        {
                            //solos
                            if (surround[1, 0] != mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 10, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 13, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 12, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 11, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //corners
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 1, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 2, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 3, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 4, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //3 ways
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 5, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 6, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 7, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 8, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //sides
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 13, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 11, WIRE_COLORS[mapWires[i, a] - 1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                        }
                        if (mapTiles[i, a] != -1 && Game1.itemInfo.ITEM_ENDPOINT[mapTiles[i, a]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 154, new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                    }
                }
            }
            Game1.spriteBatch.End();
        }

    }
}
