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
        public const int ITEM_COUNT = 28;
        public int[] ITEM_RANK = new int[ITEM_COUNT]; // for tools mainly
        public bool[] ITEM_BIGTILE = new bool[ITEM_COUNT];
        public bool[] ITEM_EQUIPPABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_TOOL = new bool[ITEM_COUNT];
        public int[] ITEM_TOOL_RANGE = new int[ITEM_COUNT]; // range of tool - i.e. a square radius
        public int[] ITEM_UNIT_WIDTH = new int[ITEM_COUNT]; // e.g. 1,2,3
        public int[] ITEM_UNIT_HEIGHT = new int[ITEM_COUNT]; // e.g. 1,2,3
        public bool[] ITEM_PLACEABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_REQUIRE_SURFACE = new bool[ITEM_COUNT]; // if it's placeable, must it be placed on a surface?
        public bool[] ITEM_SOLID = new bool[ITEM_COUNT]; // if it's placeable, does it have a solid collision state?
        public int[] ITEM_BLOCKID = new int[ITEM_COUNT];
        public int[] ITEM_EQUIPID = new int[ITEM_COUNT];
        public int[] ITEM_EQUIP_SLOT = new int[ITEM_COUNT];
        public bool[] ITEM_STACKABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_AUTOTILE = new bool[ITEM_COUNT]; // should still work for things like beds
        public bool[] ITEM_BACKTILE = new bool[ITEM_COUNT]; // indicates whether the block should be on the layer behind
        public int[] ITEM_END_POSITION = new int[ITEM_COUNT]; // where the "mask" should end (for collisions or stopping CanBePlaced())
        public int[][] ITEM_YIELD_IDS = new int[ITEM_COUNT][]; // what items are dropped when the block is broken?
        public int[][] ITEM_YIELD_QUANTITIES = new int[ITEM_COUNT][];// how many of each?
        public UI[] ITEM_UI = new UI[ITEM_COUNT]; // ui associated with the item
        public Action[] ITEM_FUNCTION = new Action[ITEM_COUNT]; // for when you click an item with an empty cursor - think chests and other furniture
        public int[] ITEM_TOOL_TIER = new int[ITEM_COUNT]; // which tier of tool is required to break the item at minimum - "0" = wand
        public static int chestState = 0; // 0-4 values - closed, opening, open, closing
        public ItemInfo()
        {
            for (int i = 0; i < ITEM_COUNT; i++)
            {
                ITEM_UNIT_WIDTH[i] = 1;
                ITEM_UNIT_HEIGHT[i] = 1;
                ITEM_RANK[i] = -1;
                ITEM_STACKABLE[i] = true;
                ITEM_TOOL_TIER[i] = -1;
                ITEM_TOOL_RANGE[i] = -1;
                ITEM_EQUIPID[i] = -1;
                ITEM_EQUIP_SLOT[i] = -1;
            }

            ITEM_BIGTILE[27] = true;

            ITEM_EQUIPPABLE[22] = true;
            ITEM_EQUIPPABLE[23] = true;
            ITEM_EQUIPPABLE[24] = true;
            ITEM_EQUIPPABLE[25] = true;
            ITEM_EQUIPPABLE[26] = true;

            ITEM_EQUIPID[22] = 0;
            ITEM_EQUIPID[23] = 1;
            ITEM_EQUIPID[24] = 2;
            ITEM_EQUIPID[25] = 3;
            ITEM_EQUIPID[26] = 4;

            ITEM_EQUIP_SLOT[22] = 7;
            ITEM_EQUIP_SLOT[23] = 1;
            ITEM_EQUIP_SLOT[24] = 7;
            ITEM_EQUIP_SLOT[25] = 1;
            ITEM_EQUIP_SLOT[26] = 17;

            ITEM_RANK[14] = 0;
            ITEM_RANK[16] = 1;

            ITEM_TOOL_RANGE[14] = 5*16;
            ITEM_TOOL_RANGE[16] = 8*16;

            ITEM_BACKTILE[17] = true;
            ITEM_BACKTILE[18] = true;
            ITEM_BACKTILE[21] = true;

            ITEM_PLACEABLE[2] = true;
            ITEM_PLACEABLE[4] = true;
            ITEM_PLACEABLE[5] = true;
            ITEM_PLACEABLE[6] = true;
            ITEM_PLACEABLE[7] = true;
            ITEM_PLACEABLE[8] = true;
            ITEM_PLACEABLE[9] = true;
            ITEM_PLACEABLE[10] = true;
            ITEM_PLACEABLE[11] = true;
            ITEM_PLACEABLE[12] = true;
            ITEM_PLACEABLE[13] = true;
            ITEM_PLACEABLE[17] = true;
            ITEM_PLACEABLE[18] = true;
            ITEM_PLACEABLE[19] = true;
            ITEM_PLACEABLE[20] = true;
            ITEM_PLACEABLE[21] = true;
            ITEM_PLACEABLE[27] = true;

            ITEM_TOOL[14] = true;
            ITEM_TOOL[16] = true;

            ITEM_REQUIRE_SURFACE[2] = true;
            ITEM_REQUIRE_SURFACE[6] = true;
            ITEM_REQUIRE_SURFACE[7] = true;
            ITEM_REQUIRE_SURFACE[8] = true;
            ITEM_REQUIRE_SURFACE[9] = true;
            ITEM_REQUIRE_SURFACE[10] = true;
            ITEM_REQUIRE_SURFACE[11] = true;
            ITEM_REQUIRE_SURFACE[12] = true;
            ITEM_REQUIRE_SURFACE[13] = true;
            ITEM_REQUIRE_SURFACE[19] = true;
            ITEM_REQUIRE_SURFACE[27] = true;

            ITEM_SOLID[4] = true;
            ITEM_SOLID[5] = true;
            ITEM_SOLID[6] = true;
            ITEM_SOLID[20] = true;

            ITEM_TOOL_TIER[4] = 0;
            ITEM_TOOL_TIER[5] = 1;
            ITEM_TOOL_TIER[17] = 0;
            ITEM_TOOL_TIER[18] = 1;

            ITEM_BLOCKID[0] = -1;
            ITEM_BLOCKID[1] = -1;
            ITEM_BLOCKID[2] = 85;
            ITEM_BLOCKID[3] = -1;
            ITEM_BLOCKID[4] = 7;
            ITEM_BLOCKID[5] = 0;
            ITEM_BLOCKID[6] = 39;
            ITEM_BLOCKID[7] = 86;
            ITEM_BLOCKID[8] = 35;
            ITEM_BLOCKID[9] = 35;
            ITEM_BLOCKID[10] = 35;
            ITEM_BLOCKID[11] = 35;
            ITEM_BLOCKID[12] = 35;
            ITEM_BLOCKID[13] = 35;
            ITEM_BLOCKID[14] = -1;
            ITEM_BLOCKID[15] = -1;
            ITEM_BLOCKID[16] = -1;
            ITEM_BLOCKID[17] = 88;
            ITEM_BLOCKID[18] = 87;
            ITEM_BLOCKID[19] = 89;
            ITEM_BLOCKID[20] = 90;
            ITEM_BLOCKID[21] = 91;

            ITEM_STACKABLE[14] = false;
            ITEM_STACKABLE[16] = false;
            ITEM_STACKABLE[22] = false;
            ITEM_STACKABLE[23] = false;
            ITEM_STACKABLE[24] = false;
            ITEM_STACKABLE[25] = false;
            ITEM_STACKABLE[26] = false;

            ITEM_AUTOTILE[5] = true;
            ITEM_AUTOTILE[6] = true;
            ITEM_AUTOTILE[8] = true;
            ITEM_AUTOTILE[18] = true;
            ITEM_AUTOTILE[20] = true;
            ITEM_AUTOTILE[21] = true;
        }

        public static void DrawAutoTile(int itemId, Vector2 position)
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[itemId] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            switch (itemId)
            {
                case 20:
                case 21:
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y + 15, 16, 1), Color.Black); //nounder
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 16, 1), Color.Black); //noabove
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 16), Color.Black); //noleft
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 16), Color.Black); //noright
                    }
                    break;
                case 8:
                    break;
                case 6:
                    if (!(map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 39, position); //noleft, noright
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 40, position); //noleft
                    }
                    else if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == itemId))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 42, position); //noright
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 41, position); //leftandright
                    }
                    break;

                default:
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);

                    //draw edges for default block items
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y + 15, 16, 1), Color.Black); //nounder
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 16, 1), Color.Black); //noabove
                    }
                    if (!(map[(int)(position.X+Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 16), Color.Black); //noleft
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 16), Color.Black); //noright
                    }
                    break;
            }
        }

    }
}
