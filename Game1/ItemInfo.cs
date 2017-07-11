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
        public const int ITEM_COUNT = 4;
        public bool[] ITEM_EQUIPPABLE = new bool[ITEM_COUNT];
        public bool[] ITEM_PLACEABLE = new bool[ITEM_COUNT];
        public int[] ITEM_BLOCKID = new int[ITEM_COUNT];
        public bool[] ITEM_STACKABLE = new bool[ITEM_COUNT]; 

        public ItemInfo()
        {
            ITEM_EQUIPPABLE[0] = false;
            ITEM_EQUIPPABLE[1] = false;
            ITEM_EQUIPPABLE[2] = false;
            ITEM_EQUIPPABLE[3] = false;

            ITEM_PLACEABLE[0] = false;
            ITEM_PLACEABLE[1] = false;
            ITEM_PLACEABLE[2] = true;
            ITEM_PLACEABLE[3] = false;

            ITEM_BLOCKID[0] = -1;
            ITEM_BLOCKID[1] = -1;
            ITEM_BLOCKID[2] = 101;
            ITEM_BLOCKID[3] = -1;

            ITEM_STACKABLE[0] = false;
            ITEM_STACKABLE[1] = false;
            ITEM_STACKABLE[2] = false;
            ITEM_STACKABLE[3] = false;
        }

    }
}
