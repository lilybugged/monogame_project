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
    public class Chest
    {
        public int chestx;
        public int chesty;
        public int[][] inventory; // first index is itemid(0)/quantity(1), second is position in the inventory

        public Chest(int x, int y, int[][] inv)
        {
            chestx = x;
            chesty = y;
            inventory = inv;
        }

        public override String ToString()
        {
            return "" + chestx + "," + chesty;
        }

        public static int FindChestId(int xunit, int yunit)
        {
            for (int i = 0; i < Game1.chestInventories.Count; i++)
            {
                if (Game1.chestInventories[i].ToString().Equals(""+xunit+","+yunit))
                {
                    return i;
                }
            }
            return -1;
        }
    }
}
