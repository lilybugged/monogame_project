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
        public int chestState = 0; //0-3 - closed, opening, open, closing
        int chestRank = 0; //0-2 - small, medium, large
        Color[] chestRankColors = new Color[] {Color.White, new Color(128,128,128,255), new Color(229,137,104, 255),
        new Color(199,199,199, 255),new Color(244,220,151, 255),new Color(188,216,237, 255)};

        public Chest(int x, int y, int size, int[][] inv)
        {
            chestx = x;
            chesty = y;
            inventory = inv;
            chestRank = size;
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
        //TODO: use this to search for autotiled chest parts and update their states when this one is clicked.
        public void Update()
        {
            if (Game1.openChest != -1 && Game1.chestInventories[Game1.openChest] == this && Player.RangeFromPoint(chestx, chesty)[0] >= Game1.PLAYER_RANGE_REQUIREMENT)
            {
                chestState = 0;
                Game1.uiObjects[1] = null;
                Game1.openChest = -1;
            }
            if (chestState==1&&Game1.chestSprites[0].currentFrame==3)
            {
                chestState = 2;
            }
            if (chestState == 3 && Game1.chestSprites[1].currentFrame == 3)
            {
                chestState = 0;
            }
            if (chestState==2 && Game1.chestInventories[Game1.openChest]!=this)
            {
                chestState = 3;
            }
        }
        public void Draw()
        {
            switch (chestState)
            {
                case 0:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[8], new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    if (chestRank>0) Game1.tiles.DrawTile(Game1.spriteBatch, 37, chestRankColors[chestRank], new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    Game1.spriteBatch.End();
                    break;
                case 1:
                    Game1.chestSprites[0].Draw(Game1.spriteBatch, new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    if (chestRank > 0) Game1.chestSprites[2].Draw(Game1.spriteBatch, chestRankColors[chestRank], new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    break;
                case 2:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 36, new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    if (chestRank > 0) Game1.tiles.DrawTile(Game1.spriteBatch, 38, chestRankColors[chestRank], new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    Game1.spriteBatch.End();
                    break;
                case 3:
                    Game1.chestSprites[1].Draw(Game1.spriteBatch, new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    if (chestRank > 0) Game1.chestSprites[3].Draw(Game1.spriteBatch, chestRankColors[chestRank], new Vector2(chestx - Player.playerx, chesty - Player.playery));
                    break;
            }
        }
    }
}
