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
    /// <summary>
    /// The ItemInfo class should be instantiated exactly once in the game.
    /// It contains information for every item in the game.
    /// </summary>
    public class ItemInfo
    {
        //item info
        public const int ITEM_COUNT = 9;
        public bool[] ITEM_EQUIPPABLE = new bool[ITEM_COUNT];
        public int[] ITEM_UNIT_WIDTH = new int[ITEM_COUNT]; //e.g. 1,2,3
        public int[] ITEM_UNIT_HEIGHT = new int[ITEM_COUNT]; //e.g. 1,2,3
        public bool[] ITEM_PLACEABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_REQUIRE_SURFACE = new bool[ITEM_COUNT]; //if it's placeable, must it be placed on a surface?
        public bool[] ITEM_SOLID = new bool[ITEM_COUNT]; //if it's placeable, does it have a solid collision state?
        public int[] ITEM_BLOCKID = new int[ITEM_COUNT];
        public bool[] ITEM_STACKABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_AUTOTILE = new bool[ITEM_COUNT]; //should still work for things like beds
        public int[] ITEM_END_POSITION = new int[ITEM_COUNT]; //where the "mask" should end (for collisions or stopping CanBePlaced())
        public UI[] ITEM_UI = new UI[ITEM_COUNT]; // ui associated with the item
        public Action[] ITEM_FUNCTION = new Action[ITEM_COUNT]; // for when you click an item with an empty cursor - think chests and other furniture
        public int[] ITEM_TOOL_TIER = new int[ITEM_COUNT]; // which tier of tool is required to break the item at minimum - "0" = hands (empty cursor)
        public static int chestState = 0; //0-4 values - closed, opening, open, closing
        public ItemInfo()
        {
            for (int i = 0; i < ITEM_COUNT; i++)
            {
                ITEM_UNIT_WIDTH[i] = 1;
                ITEM_UNIT_HEIGHT[i] = 1;
            }

            ITEM_PLACEABLE[2] = true;
            ITEM_PLACEABLE[4] = true;
            ITEM_PLACEABLE[5] = true;
            ITEM_PLACEABLE[6] = true;
            ITEM_PLACEABLE[7] = true;
            ITEM_PLACEABLE[8] = true;

            ITEM_REQUIRE_SURFACE[2] = true;
            ITEM_REQUIRE_SURFACE[6] = true;
            ITEM_REQUIRE_SURFACE[7] = true;
            ITEM_REQUIRE_SURFACE[8] = true;

            ITEM_SOLID[4] = true;
            ITEM_SOLID[5] = true;
            ITEM_SOLID[6] = true;

            ITEM_BLOCKID[0] = -1;
            ITEM_BLOCKID[1] = -1;
            ITEM_BLOCKID[2] = 101;
            ITEM_BLOCKID[3] = -1;
            ITEM_BLOCKID[4] = 7;
            ITEM_BLOCKID[5] = 0;
            ITEM_BLOCKID[6] = 51;
            ITEM_BLOCKID[7] = 102;
            ITEM_BLOCKID[8] = 35;

            ITEM_STACKABLE[0] = true;
            ITEM_STACKABLE[1] = true;
            ITEM_STACKABLE[2] = true;
            ITEM_STACKABLE[3] = true;
            ITEM_STACKABLE[4] = true;
            ITEM_STACKABLE[5] = true;
            ITEM_STACKABLE[6] = true;
            ITEM_STACKABLE[7] = true;
            ITEM_STACKABLE[8] = true;

            ITEM_AUTOTILE[5] = true;
            ITEM_AUTOTILE[6] = true;


            ITEM_FUNCTION[8] = Function8;
        }

        private static void Function8()
        {
            chestState = 1;
        }
        public static void DrawAutoTile(int itemId, Vector2 position)
        {
            switch (itemId)
            {
                case 8:
                    Game1.tiles.DrawTile(Game1.spriteBatch, 35, position);
                    break;
                case 6:
                    if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 51, position); //noleft, noright
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 52, position); //noleft
                    }
                    else if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 54, position); //noright
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 53, position); //leftandright
                    }
                    break;

                default:
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);

                    //draw edges for default block items
                    if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y + 15, 16, 1), Color.Black); //nounder
                    }
                    if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 16, 1), Color.Black); //noabove
                    }
                    if (!(Game1.currentMap.mapTiles[(int)(position.X+Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 16), Color.Black); //noleft
                    }
                    if (!(Game1.currentMap.mapTiles[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 16), Color.Black); //noright
                    }
                    break;
            }
        }

    }
}
