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
    class BigTile
    {
        public int tilex;
        public int tiley;
        public int[][] inventory; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int tileState = 0; // dependent on what the tile is
        int tileRank = 0; //0-2 - small, medium, large
        Color[] tileRankColors = new Color[] {Color.White, new Color(128,128,128,255), new Color(229,137,104, 255),
        new Color(199,199,199, 255),new Color(244,220,151, 255),new Color(188,216,237, 255)};

        public BigTile(int x, int y, int rank, int[][] inv)
        {
            tilex = x;
            tiley = y;
            inventory = inv;
            tileRank = rank;
        }

        public override String ToString()
        {
            return "" + tilex + "," + tiley;
        }
    }
}
