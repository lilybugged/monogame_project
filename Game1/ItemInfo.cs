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
    public class ItemInfo
    {
        //item info
        public const int ITEM_COUNT = 5;
        public bool[] ITEM_EQUIPPABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_PLACEABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_REQUIRE_SURFACE = new bool[ITEM_COUNT]; //if it's placeable, must it be placed on a surface?
        public bool[] ITEM_SOLID = new bool[ITEM_COUNT]; //if it's placeable, does it have a solid collision state?
        public int[] ITEM_BLOCKID = new int[ITEM_COUNT];
        public bool[] ITEM_STACKABLE = new bool[ITEM_COUNT]; 

        public ItemInfo()
        {
            ITEM_EQUIPPABLE[0] = false;
            ITEM_EQUIPPABLE[1] = false;
            ITEM_EQUIPPABLE[2] = false;
            ITEM_EQUIPPABLE[3] = false;
            ITEM_EQUIPPABLE[4] = false;

            ITEM_PLACEABLE[0] = false;
            ITEM_PLACEABLE[1] = false;
            ITEM_PLACEABLE[2] = true;
            ITEM_PLACEABLE[3] = false;
            ITEM_PLACEABLE[4] = true;

            ITEM_REQUIRE_SURFACE[0] = false;
            ITEM_REQUIRE_SURFACE[1] = false;
            ITEM_REQUIRE_SURFACE[2] = true;
            ITEM_REQUIRE_SURFACE[3] = false;
            ITEM_REQUIRE_SURFACE[4] = false;

            ITEM_SOLID[0] = false;
            ITEM_SOLID[1] = false;
            ITEM_SOLID[2] = false;
            ITEM_SOLID[3] = false;

            ITEM_BLOCKID[0] = -1;
            ITEM_BLOCKID[1] = -1;
            ITEM_BLOCKID[2] = 101;
            ITEM_BLOCKID[3] = -1;
            ITEM_BLOCKID[4] = 7;

            ITEM_STACKABLE[0] = true;
            ITEM_STACKABLE[1] = true;
            ITEM_STACKABLE[2] = true;
            ITEM_STACKABLE[3] = true;
            ITEM_STACKABLE[4] = true;
        }

    }
}
