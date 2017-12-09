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
        public Color[] WIRE_COLORS = new Color[]{ new Color(231, 63, 63), new Color(82, 101, 122), new Color(229, 203, 130), new Color(224, 255, 79) };
        public int[,] mapWires;
        public int[,] mapTiles;
        public int[,] mapBackTiles;
        public MapInfo(int length, int height)
        {
            mapWires = new int[length, height];
            mapTiles = new int[length,height];
            mapBackTiles = new int[length, height];
            //mapTiles stores item ids, which can be used to get the block ids to draw

            for (int i = 0; i < length; i++)
            {
                for (int a = 0; a < height; a++)
                {
                    mapWires[i, a] = -1;
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
                            Game1.tiles.DrawTile(Game1.spriteBatch, 71, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                        else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 9, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                        }
                        else
                        {
                            //solos
                            if (surround[1, 0] != mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 10, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 13, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 12, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 11, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //corners
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 1, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 2, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 3, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 4, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //3 ways
                            else if (surround[1, 0] != mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 5, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] != mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 6, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] != mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 7, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] != mapWires[i, a] && surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 8, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            //sides
                            else if (surround[1, 0] == mapWires[i, a] && surround[1, 2] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 13, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
                            }
                            else if (surround[0, 1] == mapWires[i, a] && surround[2, 1] == mapWires[i, a])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 71 + 11, WIRE_COLORS[mapWires[i, a]-1], new Vector2(i * 16 - Player.playerx, a * 16 - Player.playery));
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
