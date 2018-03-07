﻿using System;
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
        public const int ITEM_COUNT = 78;
        public const int SCHEMATICS_COUNT = 4;
        public bool[] ITEM_ENDPOINT = new bool[ITEM_COUNT]; // if true, this item is an endpoint for pipes
        public String[] ITEM_NAME = new String[ITEM_COUNT];
        public String[] ITEM_DESC = new String[ITEM_COUNT];
        public int[] ITEM_RANK = new int[ITEM_COUNT]; // for tools mainly
        public bool[] ITEM_BIGTILE = new bool[ITEM_COUNT];
        public bool[] ITEM_TRIGGER = new bool[ITEM_COUNT]; // for switches etc
        public int[] ITEM_BIGTILE_WIDTH = new int[ITEM_COUNT];
        public int[] ITEM_BIGTILE_HEIGHT = new int[ITEM_COUNT];
        public bool[] ITEM_EQUIPPABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_TOOL = new bool[ITEM_COUNT];
        public int[] ITEM_TOOL_RANGE = new int[ITEM_COUNT]; // range of tool - i.e. a square radius
        public int[] ITEM_UNIT_WIDTH = new int[ITEM_COUNT]; // e.g. 1,2,3
        public int[] ITEM_UNIT_HEIGHT = new int[ITEM_COUNT]; // e.g. 1,2,3
        public bool[] ITEM_PLACEABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_REQUIRE_SURFACE = new bool[ITEM_COUNT]; // if it's placeable, must it be placed on a surface?
        public bool[] ITEM_REQUIRE_WALL = new bool[ITEM_COUNT];
        public bool[] ITEM_SOLID = new bool[ITEM_COUNT]; // if it's placeable, does it have a solid collision state?
        public int[] ITEM_ITEMID = new int[ITEM_COUNT];
        public int[] ITEM_BLOCKID = new int[ITEM_COUNT];
        public int[] ITEM_EQUIPID = new int[ITEM_COUNT];
        public int[] ITEM_EQUIP_SLOT = new int[ITEM_COUNT];
        public bool[] ITEM_STACKABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_AUTOTILE = new bool[ITEM_COUNT]; // should still work for things like beds
        public bool[] ITEM_BACKTILE = new bool[ITEM_COUNT]; // indicates whether the block should be on the layer behind
        public bool[] ITEM_YIELD = new bool[ITEM_COUNT]; // indicates whether the block will yield something other than one item of itself
        public int[][] ITEM_YIELD_IDS = new int[ITEM_COUNT][]; // what items are dropped when the block is broken?
        public int[][] ITEM_YIELD_QUANTITIES = new int[ITEM_COUNT][];// how many of each?
        public UI[] ITEM_UI = new UI[ITEM_COUNT]; // ui associated with the item
        public Action[] ITEM_FUNCTION = new Action[ITEM_COUNT]; // for when you click an item with an empty cursor - think chests and other furniture
        public int[] ITEM_TOOL_TIER = new int[ITEM_COUNT]; // which tier of tool is required to break the item at minimum - "0" = wand
        public static int chestState = 0; // 0-4 values - closed, opening, open, closing
        public ItemInfo()
        {
            for (int i = 0; i < ITEM_COUNT - SCHEMATICS_COUNT; i++)
            {
                ITEM_UNIT_WIDTH[i] = 1;
                ITEM_UNIT_HEIGHT[i] = 1;
                ITEM_RANK[i] = -1;
                ITEM_STACKABLE[i] = true;
                ITEM_TOOL_TIER[i] = -1;
                ITEM_TOOL_RANGE[i] = -1;
                ITEM_EQUIPID[i] = -1;
                ITEM_EQUIP_SLOT[i] = -1;
                ITEM_BIGTILE_WIDTH[i] = -1;
                ITEM_BIGTILE_HEIGHT[i] = -1;
                ITEM_ITEMID[i] = i;
            }
            for (int i = ITEM_COUNT - SCHEMATICS_COUNT * 2; i < ITEM_COUNT - SCHEMATICS_COUNT; i++)
            {
                //recipes
                ITEM_ITEMID[i] = 37;
                ITEM_STACKABLE[i] = true;
            }
            for (int i = ITEM_COUNT - SCHEMATICS_COUNT; i < ITEM_COUNT; i++)
            {
                //blueprints
                ITEM_ITEMID[i] = 38;
                ITEM_STACKABLE[i] = true;
            }

            ITEM_TRIGGER[47] = true;

            ITEM_YIELD[34] = true;
            ITEM_YIELD[40] = true;

            ITEM_YIELD_IDS[34] = new int[] { 33 };
            ITEM_YIELD_IDS[40] = new int[] { 39 };

            ITEM_YIELD_QUANTITIES[34] = new int[] { 2 };
            ITEM_YIELD_QUANTITIES[40] = new int[] { 4 };

            ITEM_ENDPOINT[8] = true;
            ITEM_ENDPOINT[9] = true;
            ITEM_ENDPOINT[10] = true;
            ITEM_ENDPOINT[11] = true;
            ITEM_ENDPOINT[12] = true;
            ITEM_ENDPOINT[13] = true;
            ITEM_ENDPOINT[29] = true;
            ITEM_ENDPOINT[31] = true;
            ITEM_ENDPOINT[32] = true;
            ITEM_ENDPOINT[41] = true;
            ITEM_ENDPOINT[47] = true;
            ITEM_ENDPOINT[39] = true;
            ITEM_ENDPOINT[54] = true;
            ITEM_ENDPOINT[58] = true;
            ITEM_ENDPOINT[59] = true;
            ITEM_ENDPOINT[63] = true;

            ITEM_BIGTILE[8] = true;
            ITEM_BIGTILE[9] = true;
            ITEM_BIGTILE[10] = true;
            ITEM_BIGTILE[11] = true;
            ITEM_BIGTILE[12] = true;
            ITEM_BIGTILE[13] = true;
            ITEM_BIGTILE[27] = true;
            ITEM_BIGTILE[28] = true;
            ITEM_BIGTILE[29] = true;
            ITEM_BIGTILE[30] = true;
            ITEM_BIGTILE[31] = true;
            ITEM_BIGTILE[32] = true;
            ITEM_BIGTILE[39] = true;
            ITEM_BIGTILE[40] = true;
            ITEM_BIGTILE[41] = true;
            ITEM_BIGTILE[47] = true;
            ITEM_BIGTILE[51] = true;
            ITEM_BIGTILE[52] = true;
            ITEM_BIGTILE[53] = true;
            ITEM_BIGTILE[54] = true;
            ITEM_BIGTILE[55] = true;
            ITEM_BIGTILE[57] = true;
            ITEM_BIGTILE[58] = true;
            ITEM_BIGTILE[59] = true;
            ITEM_BIGTILE[60] = true;
            ITEM_BIGTILE[61] = true;
            ITEM_BIGTILE[63] = true;

            ITEM_BIGTILE_WIDTH[8] = 1;
            ITEM_BIGTILE_WIDTH[9] = 1;
            ITEM_BIGTILE_WIDTH[10] = 1;
            ITEM_BIGTILE_WIDTH[11] = 1;
            ITEM_BIGTILE_WIDTH[12] = 1;
            ITEM_BIGTILE_WIDTH[13] = 1;
            ITEM_BIGTILE_WIDTH[27] = 1;
            ITEM_BIGTILE_WIDTH[28] = 2;
            ITEM_BIGTILE_WIDTH[29] = 1;
            ITEM_BIGTILE_WIDTH[30] = 1;
            ITEM_BIGTILE_WIDTH[31] = 1;
            ITEM_BIGTILE_WIDTH[32] = 1;
            ITEM_BIGTILE_WIDTH[39] = 1;
            ITEM_BIGTILE_WIDTH[40] = 2;
            ITEM_BIGTILE_WIDTH[41] = 1;
            ITEM_BIGTILE_WIDTH[47] = 1;
            ITEM_BIGTILE_WIDTH[51] = 1;
            ITEM_BIGTILE_WIDTH[52] = 1;
            ITEM_BIGTILE_WIDTH[53] = 1;
            ITEM_BIGTILE_WIDTH[54] = 2;
            ITEM_BIGTILE_WIDTH[55] = 2;
            ITEM_BIGTILE_WIDTH[57] = 1;
            ITEM_BIGTILE_WIDTH[58] = 1;
            ITEM_BIGTILE_WIDTH[59] = 1;
            ITEM_BIGTILE_WIDTH[60] = 1;
            ITEM_BIGTILE_WIDTH[61] = 1;
            ITEM_BIGTILE_WIDTH[63] = 1;

            ITEM_BIGTILE_HEIGHT[8] = 1;
            ITEM_BIGTILE_HEIGHT[9] = 1;
            ITEM_BIGTILE_HEIGHT[10] = 1;
            ITEM_BIGTILE_HEIGHT[11] = 1;
            ITEM_BIGTILE_HEIGHT[12] = 1;
            ITEM_BIGTILE_HEIGHT[13] = 1;
            ITEM_BIGTILE_HEIGHT[27] = 2;
            ITEM_BIGTILE_HEIGHT[28] = 2;
            ITEM_BIGTILE_HEIGHT[29] = 1;
            ITEM_BIGTILE_HEIGHT[30] = 1;
            ITEM_BIGTILE_HEIGHT[31] = 1;
            ITEM_BIGTILE_HEIGHT[32] = 1;
            ITEM_BIGTILE_HEIGHT[39] = 1;
            ITEM_BIGTILE_HEIGHT[40] = 2;
            ITEM_BIGTILE_HEIGHT[41] = 1;
            ITEM_BIGTILE_HEIGHT[47] = 1;
            ITEM_BIGTILE_HEIGHT[51] = 1;
            ITEM_BIGTILE_HEIGHT[52] = 1;
            ITEM_BIGTILE_HEIGHT[53] = 1;
            ITEM_BIGTILE_HEIGHT[54] = 2;
            ITEM_BIGTILE_HEIGHT[55] = 2;
            ITEM_BIGTILE_HEIGHT[57] = 1;
            ITEM_BIGTILE_HEIGHT[58] = 1;
            ITEM_BIGTILE_HEIGHT[59] = 2;
            ITEM_BIGTILE_HEIGHT[60] = 1;
            ITEM_BIGTILE_HEIGHT[61] = 1;
            ITEM_BIGTILE_HEIGHT[63] = 1;

            ITEM_EQUIPPABLE[19] = true;
            ITEM_EQUIPPABLE[22] = true;
            ITEM_EQUIPPABLE[23] = true;
            ITEM_EQUIPPABLE[24] = true;
            ITEM_EQUIPPABLE[25] = true;
            ITEM_EQUIPPABLE[26] = true;
            ITEM_EQUIPPABLE[42] = true;

            ITEM_EQUIPID[19] = 5;
            ITEM_EQUIPID[22] = 0;
            ITEM_EQUIPID[23] = 1;
            ITEM_EQUIPID[24] = 2;
            ITEM_EQUIPID[25] = 3;
            ITEM_EQUIPID[26] = 4;
            ITEM_EQUIPID[42] = 6;


            ITEM_EQUIP_SLOT[19] = 15;
            ITEM_EQUIP_SLOT[22] = 7;
            ITEM_EQUIP_SLOT[23] = 1;
            ITEM_EQUIP_SLOT[24] = 7;
            ITEM_EQUIP_SLOT[25] = 1;
            ITEM_EQUIP_SLOT[26] = 17;
            ITEM_EQUIP_SLOT[42] = 5;

            ITEM_RANK[14] = 0;
            ITEM_RANK[16] = 1;
            ITEM_RANK[29] = 0;
            ITEM_RANK[30] = 0;
            ITEM_RANK[51] = 0;
            ITEM_RANK[57] = 0;

            ITEM_TOOL_RANGE[14] = 5 * 16;
            ITEM_TOOL_RANGE[16] = 8 * 16;

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
            ITEM_PLACEABLE[28] = true;
            ITEM_PLACEABLE[29] = true;
            ITEM_PLACEABLE[30] = true;
            ITEM_PLACEABLE[31] = true;
            ITEM_PLACEABLE[32] = true;
            ITEM_PLACEABLE[33] = true;
            ITEM_PLACEABLE[34] = true;
            ITEM_PLACEABLE[35] = true;
            ITEM_PLACEABLE[36] = true;
            ITEM_PLACEABLE[39] = true;
            ITEM_PLACEABLE[40] = true;
            ITEM_PLACEABLE[41] = true;
            ITEM_PLACEABLE[47] = true;
            ITEM_PLACEABLE[49] = true;
            ITEM_PLACEABLE[50] = true;
            ITEM_PLACEABLE[51] = true;
            ITEM_PLACEABLE[52] = true;
            ITEM_PLACEABLE[53] = true;
            ITEM_PLACEABLE[54] = true;
            ITEM_PLACEABLE[55] = true;
            ITEM_PLACEABLE[56] = true;
            ITEM_PLACEABLE[57] = true;
            ITEM_PLACEABLE[58] = true;
            ITEM_PLACEABLE[59] = true;
            ITEM_PLACEABLE[60] = true;
            ITEM_PLACEABLE[61] = true;
            ITEM_PLACEABLE[62] = true;
            ITEM_PLACEABLE[63] = true;
            ITEM_PLACEABLE[65] = true;

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
            ITEM_REQUIRE_SURFACE[28] = true;
            ITEM_REQUIRE_SURFACE[36] = true;
            ITEM_REQUIRE_SURFACE[54] = true;
            ITEM_REQUIRE_SURFACE[61] = true;
            ITEM_REQUIRE_SURFACE[65] = true;

            ITEM_REQUIRE_WALL[47] = true;
            ITEM_REQUIRE_WALL[55] = true;
            ITEM_REQUIRE_WALL[56] = true;

            ITEM_SOLID[4] = true;
            ITEM_SOLID[5] = true;
            ITEM_SOLID[6] = true;
            ITEM_SOLID[20] = true;
            ITEM_SOLID[27] = true;
            ITEM_SOLID[33] = true;
            ITEM_SOLID[34] = true;
            ITEM_SOLID[35] = true;
            ITEM_SOLID[49] = true;
            ITEM_SOLID[50] = true;
            ITEM_SOLID[62] = true;
            ITEM_SOLID[63] = true;
            ITEM_SOLID[65] = true;

            ITEM_TOOL_TIER[4] = 0;
            ITEM_TOOL_TIER[5] = 1;
            ITEM_TOOL_TIER[6] = 1;
            ITEM_TOOL_TIER[17] = 0;
            ITEM_TOOL_TIER[18] = 1;
            ITEM_TOOL_TIER[28] = 1;
            ITEM_TOOL_TIER[29] = 1;
            ITEM_TOOL_TIER[30] = 1;
            ITEM_TOOL_TIER[31] = 1;
            ITEM_TOOL_TIER[32] = 1;
            ITEM_TOOL_TIER[33] = 0;
            ITEM_TOOL_TIER[34] = 1;
            ITEM_TOOL_TIER[35] = 0;
            ITEM_TOOL_TIER[36] = 0;
            ITEM_TOOL_TIER[39] = 0;
            ITEM_TOOL_TIER[40] = 0;
            ITEM_TOOL_TIER[41] = 1;
            ITEM_TOOL_TIER[49] = 0;
            ITEM_TOOL_TIER[50] = 0;
            ITEM_TOOL_TIER[51] = 0;
            ITEM_TOOL_TIER[52] = 0;
            ITEM_TOOL_TIER[53] = 0;
            ITEM_TOOL_TIER[54] = 1;
            ITEM_TOOL_TIER[57] = 1;
            ITEM_TOOL_TIER[62] = 1;

            //do NOT add a blockID for BigTile items
            ITEM_BLOCKID[0] = -1;
            ITEM_BLOCKID[1] = -1;
            ITEM_BLOCKID[2] = 85;
            ITEM_BLOCKID[3] = -1;
            ITEM_BLOCKID[4] = 7;
            ITEM_BLOCKID[5] = 0;
            ITEM_BLOCKID[6] = 39;
            ITEM_BLOCKID[7] = 86;
            ITEM_BLOCKID[14] = -1;
            ITEM_BLOCKID[15] = -1;
            ITEM_BLOCKID[16] = -1;
            ITEM_BLOCKID[17] = 88;
            ITEM_BLOCKID[18] = 87;
            ITEM_BLOCKID[19] = 89;
            ITEM_BLOCKID[20] = 90;
            ITEM_BLOCKID[21] = 91;
            ITEM_BLOCKID[29] = 44;
            ITEM_BLOCKID[33] = 5;
            ITEM_BLOCKID[34] = 4;
            ITEM_BLOCKID[35] = 129;
            ITEM_BLOCKID[36] = 130;
            ITEM_BLOCKID[39] = 131;
            ITEM_BLOCKID[49] = 1;
            ITEM_BLOCKID[50] = 2;
            ITEM_BLOCKID[56] = 190;
            ITEM_BLOCKID[62] = 3;
            ITEM_BLOCKID[65] = 243;

            ITEM_STACKABLE[14] = false;
            ITEM_STACKABLE[16] = false;
            ITEM_STACKABLE[19] = false;
            ITEM_STACKABLE[22] = false;
            ITEM_STACKABLE[23] = false;
            ITEM_STACKABLE[24] = false;
            ITEM_STACKABLE[25] = false;
            ITEM_STACKABLE[26] = false;
            ITEM_STACKABLE[42] = false;

            ITEM_AUTOTILE[4] = true;
            ITEM_AUTOTILE[5] = true;
            ITEM_AUTOTILE[6] = true;
            ITEM_AUTOTILE[17] = true;
            ITEM_AUTOTILE[18] = true;
            ITEM_AUTOTILE[20] = true;
            ITEM_AUTOTILE[21] = true;
            ITEM_AUTOTILE[33] = true;
            ITEM_AUTOTILE[34] = true;
            ITEM_AUTOTILE[35] = true;
            ITEM_AUTOTILE[49] = true;
            ITEM_AUTOTILE[50] = true;
            ITEM_AUTOTILE[62] = true;

            ITEM_NAME[0] = "Apple";
            ITEM_NAME[1] = "Banana";
            ITEM_NAME[2] = "Teacup";
            ITEM_NAME[3] = "Stick";
            ITEM_NAME[4] = "Wood";
            ITEM_NAME[5] = "White Block";
            ITEM_NAME[6] = "Wooden Table";
            ITEM_NAME[7] = "Mushroom";
            ITEM_NAME[8] = "Wooden Chest";
            ITEM_NAME[9] = "Stone Chest";
            ITEM_NAME[10] = "Bronze Chest";
            ITEM_NAME[11] = "Iron Chest";
            ITEM_NAME[12] = "Gold Chest";
            ITEM_NAME[13] = "Diamond Chest";
            ITEM_NAME[14] = "Novice's Wand";
            ITEM_NAME[15] = "Everwood Stick";
            ITEM_NAME[16] = "Everwood Wand";
            ITEM_NAME[17] = "Wood Panel";
            ITEM_NAME[18] = "White Block Panel";
            ITEM_NAME[19] = "OB's Fake Timberlands";
            ITEM_NAME[20] = "Glass";
            ITEM_NAME[21] = "Glass Panel";
            ITEM_NAME[22] = "Lavender Top";
            ITEM_NAME[23] = "Navy Hat";
            ITEM_NAME[24] = "Navy Top";
            ITEM_NAME[25] = "Lavender Hat";
            ITEM_NAME[26] = "Cat";
            ITEM_NAME[27] = "Door";
            ITEM_NAME[28] = "Heart Statue";
            ITEM_NAME[29] = "Furnace";
            ITEM_NAME[30] = "Pipe";
            ITEM_NAME[31] = "Push Pipe";
            ITEM_NAME[32] = "Pull Pipe";
            ITEM_NAME[33] = "Cobblestone";
            ITEM_NAME[34] = "Stone";
            ITEM_NAME[35] = "Sand";
            ITEM_NAME[36] = "Controller";
            ITEM_NAME[37] = "Blank Recipe";
            ITEM_NAME[38] = "Blank Blueprint";
            ITEM_NAME[39] = "Assembler";
            ITEM_NAME[40] = "Large Assembler";
            ITEM_NAME[41] = "Tank";
            ITEM_NAME[42] = "Wiring Goggles";
            ITEM_NAME[43] = "Red Wire";
            ITEM_NAME[44] = "Blue Wire";
            ITEM_NAME[45] = "Yellow Wire";
            ITEM_NAME[46] = "Green Wire";
            ITEM_NAME[47] = "Lever";
            ITEM_NAME[48] = "Wire Cutter";
            ITEM_NAME[49] = "Grass";
            ITEM_NAME[50] = "Dirt";
            ITEM_NAME[51] = "Fluid Pipe";
            ITEM_NAME[52] = "Fluid Push Pipe";
            ITEM_NAME[53] = "Fluid Pull Pipe";
            ITEM_NAME[54] = "Christmas Tree";
            ITEM_NAME[55] = "Large Wreath";
            ITEM_NAME[56] = "Small Wreath";
            ITEM_NAME[57] = "Small Cable";
            ITEM_NAME[58] = "Battery Box";
            ITEM_NAME[59] = "Windmill";
            ITEM_NAME[60] = "Large Cable";
            ITEM_NAME[61] = "Carrot Seed";
            ITEM_NAME[62] = "Earth";
            ITEM_NAME[63] = "Farm Controller";
            ITEM_NAME[64] = "Square Clock";
            ITEM_NAME[65] = "Scaffolding";
            ITEM_NAME[66] = "Socket";
            ITEM_NAME[67] = "AutoHopper";
            ITEM_NAME[68] = "AutoDropper";
            ITEM_NAME[69] = "Carrot";


            //recipes
            ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT * 2 + 0] = "Recipe: Furnace";
            ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT * 2 + 1] = "Recipe: Assembler";
            ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT * 2 + 2] = "Recipe: Everwood Wand";
            ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT * 2 + 3] = "Recipe: Wire Cutter";

            for (int i = 0; i < SCHEMATICS_COUNT; i++)
            {
                //blueprints
                ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT + i] = "Schematic: " + ITEM_NAME[ITEM_COUNT - SCHEMATICS_COUNT * 2 + i].Substring(8);
            }

        }

        public static void DrawAutoTile(int itemId, Vector2 position)
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[itemId] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            switch (itemId)
            {
                case 49:
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    //Debug.WriteLine(""+ ((int)(position.X + Player.playerx) / 16) +","+ ((int)(position.Y + Player.playery) / 16 + 1));
                    //draw edges for default block items
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y + 15, 16, 1), Color.Black); //nounder
                    }
                    if ((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != itemId) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        if (!(map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1)) {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 16), Color.Black); //noleft
                        }
                        else
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 7), Color.Black); //noleft
                        }
                    }
                    if ((map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != itemId) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1))
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 16), Color.Black); //noright
                        }
                        else
                        {
                            Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 8), Color.Black); //noright
                        }
                        
                    }
                    break;
                case 4:
                case 17:
                case 21:
                case 20:
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
                    //Debug.WriteLine(""+ ((int)(position.X + Player.playerx) / 16) +","+ ((int)(position.Y + Player.playery) / 16 + 1));
                    //draw edges for default block items
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1) || !(Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]== Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16]]) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y + 15, 16, 1), Color.Black); //nounder
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1) || !(Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]] == Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16]]) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 16, 1), Color.Black); //noabove
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1) || !(Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]] == Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16]]) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X, (int)position.Y, 1, 16), Color.Black); //noleft
                    }
                    if (!(map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1) || !(Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16 ]] == Game1.itemInfo.ITEM_SOLID[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16]]) || Game1.itemInfo.ITEM_REQUIRE_SURFACE[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]])
                    {
                        Game1.spriteBatch.Draw(Game1.pixel, new Rectangle((int)position.X + 15, (int)position.Y, 1, 16), Color.Black); //noright
                    }
                    break;
            }
        }

    }
}
