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
    public class BigTile
    {
        public int[] intVars;
        public int tileType;
        public int tilex;
        public int tiley;
        public int width;
        public int height;
        public int timer = -1;
        public int recipeInProgressIndex = -1;
        public int recipeOutputIndex = -1;
        //private int lastTick = 0;
        private int[] endpoint;
        public bool solid;
        public int power = -1;
        public int energy = 0;
        public int energyCapacity = 0;
        public int state = 0;
        public int[][] inventory; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int[][] output; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int[][] schematic; // first index is itemid(0)/quantity(1), second is position in the inventory
        public int tileState = 0; // dependent on what the tile is
        public int fluidId = 0; // for tanks and stuff containing fluids
        public double fluidPercent = 0; // the percentage of this item's capacity that is filled by liquid
        int tileRank = 0; //0-2 - small, medium, large
        Color[] tileRankColors = new Color[] {Color.White, new Color(128,128,128,255), new Color(229,137,104, 255),
        new Color(199,199,199, 255),new Color(244,220,151, 255),new Color(188,216,237, 255)};

        public BigTile(int type, int x, int y, int rank, int[][] inv)
        {

            tilex = x;
            tiley = y;
            inventory = inv;
            tileRank = rank;
            tileType = type;

            width = Game1.itemInfo.ITEM_BIGTILE_WIDTH[tileType];
            height = Game1.itemInfo.ITEM_BIGTILE_HEIGHT[tileType];
            solid = Game1.itemInfo.ITEM_SOLID[tileType];
            for (int i = x / 16 * 16; i < x + width * 16; i++)
            {
                for (int a = y / 16 * 16 - height * 16 + 16; a < y / 16 * 16 + 16; a++)
                {
                    Game1.currentMap.mapTiles[i / 16, a / 16] = tileType;
                }
            }
            int[,] map;
            Vector2 position;
            switch (tileType)
            {
                case 63:
                    intVars = new int[3];
                    break;
                case 59:
                    energyCapacity = 1000;
                    timer = 0;
                    break;
                case 58:
                    energyCapacity = 1000;
                    timer = 0;
                    break;
                case 52:
                    map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
                    position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    endpoint = new int[] { -1, -1 };
                    endpoint = FindEndPoint(tilex / 16, tiley / 16, 3, 51);
                    Debug.WriteLine("" + endpoint[0] + "," + endpoint[1]);
                    break;
                case 51:
                    PipeUpdate(51);
                    UpdateNearbyPipes(51); //can change this to UpdateAllPipes, but worried about performance
                    break;
                case 41:
                    fluidId = -1;
                    foreach (BigTile tile in Game1.bigTiles)
                    {
                        if (tile.tileType == 41) tile.timer = 200;
                    }
                    timer = 200;
                    break;
                case 31:
                    map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
                    position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    endpoint = new int[] { -1, -1 };
                    endpoint = FindEndPoint(tilex / 16, tiley / 16, 3, 30);
                    Debug.WriteLine("" + endpoint[0] + "," + endpoint[1]);
                    break;
                case 39:
                    energyCapacity = 500;
                    schematic = new int[2][];
                    schematic[0] = new int[] { -1 };
                    schematic[1] = new int[] { -1 };

                    inventory = new int[2][];
                    inventory[0] = new int[6];
                    inventory[1] = new int[6];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            inventory[i][a] = -1;
                        }
                    }

                    output = new int[2][];
                    output[0] = new int[6];
                    output[1] = new int[6];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < 6; a++)
                        {
                            output[i][a] = -1;
                        }
                    }
                    break;
                case 29:
                    energyCapacity = 1000;
                    inventory = new int[2][];
                    inventory[0] = new int[8];
                    inventory[1] = new int[8];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < 8; a++)
                        {
                            inventory[i][a] = -1;
                        }
                    }

                    output = new int[2][];
                    output[0] = new int[8];
                    output[1] = new int[8];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < 8; a++)
                        {
                            output[i][a] = -1;
                        }
                    }
                    break;
                case 30:
                    PipeUpdate(30);
                    UpdateNearbyPipes(30); //can change this to UpdateAllPipes, but worried about performance
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    inventory = new int[2][];
                    inventory[0] = new int[(tileType - 7) * 7];
                    inventory[1] = new int[(tileType - 7) * 7];
                    for (int i = 0; i < 2; i++)
                    {
                        for (int a = 0; a < (tileType - 7) * 7; a++)
                        {
                            inventory[i][a] = -1;
                        }
                    }
                    output = inventory;
                    break;
            }

        }
        public void Update()
        {
            int[,] map2;
            Vector2 position2;
            if (timer > 0) timer--;
            switch (tileType)
            {
                case 63:
                    if (power!=1)
                    {
                        state = 0;
                        intVars[0] = 0;
                    }
                    else if (state == 0)
                    {
                        //look for grass/dirt until there isn't any, and count
                        //left
                        for (int i=1; Game1.currentMap.mapTiles[this.tilex / 16 - i, this.tiley / 16] == 49; i++)
                        {
                            intVars[0]++;
                        }
                        Debug.WriteLine("length left: " + intVars[0]);
                        //right
                        for (int i = 1; Game1.currentMap.mapTiles[this.tilex / 16 + i, this.tiley / 16] == 49; i++)
                        {
                            intVars[1]++;
                        }
                        Debug.WriteLine("length right: " + intVars[1]);
                        if (intVars[0]>2 && intVars[1] > 2)
                        {
                            state = 2;
                        }
                        else state = 1;
                    }
                    if (state == 2 && timer < 1)
                    {

                        timer = 60;
                    }
                    break;
                case 61:
                    if (timer == -1) timer = 60;
                    if (timer == 0 && state < 2 && Game1.currentMap.mapTiles[this.tilex / 16, this.tiley / 16 + 1] == 62)
                    {
                        timer = 120;
                        state++;
                    }
                    break;
                case 59:
                    if (timer == 0)
                    {
                        energy += 100;
                        if (energy >= 100)
                        {
                            string[] outputs1 = new string[] { (tilex / 16) + "," + (tiley / 16) };
                            outputs1 = PushEnergy(tilex / 16 + 1, tiley / 16, 57, 0, 0, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16 - 1, tiley / 16, 57, 0, 1, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16, tiley / 16 + 1, 57, 0, 2, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16, tiley / 16 - 1, 57, 0, 3, new string[0], outputs1);

                            int emptySources = outputs1.Length - 1;

                            for (int i = 1; i < outputs1.Length; i++)
                            {
                                if (Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy > Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energyCapacity - 100) emptySources--;
                            }

                            if (emptySources > 0)
                            {
                                energy -= 100;

                                for (int i = 1; i < outputs1.Length; i++)
                                {
                                    if (Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy <= Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energyCapacity - (100 / (emptySources)))
                                    {
                                        Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy += (100 / (emptySources));
                                    }
                                }
                            }
                            timer = 10;
                        }
                    }
                    break;
                case 58:
                    if (timer == 0){
                        if (energy >= 100) {
                            string[] outputs1 = new string[] { (tilex / 16) + "," + (tiley / 16) };
                            outputs1 = PushEnergy(tilex / 16 + 1, tiley / 16, 57, 0, 0, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16 - 1, tiley / 16, 57, 0, 1, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16, tiley / 16 + 1, 57, 0, 2, new string[0], outputs1);
                            outputs1 = PushEnergy(tilex / 16, tiley / 16 - 1, 57, 0, 3, new string[0], outputs1);

                            int emptySources = outputs1.Length - 1;

                            for (int i = 1; i < outputs1.Length; i++)
                            {
                                if (Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy > Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energyCapacity - 100) emptySources--;
                            }

                            if (emptySources > 0)
                            {
                                energy -= 100;

                                for (int i = 1; i < outputs1.Length; i++)
                                {
                                    if (Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy <= Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energyCapacity - (100 / (emptySources)))
                                    {
                                        Game1.bigTiles[BigTile.FindTileId(Int32.Parse(outputs1[i].Split(',')[0]) * 16, Int32.Parse(outputs1[i].Split(',')[1]) * 16)].energy += (100 / (emptySources));
                                    }
                                }
                            }
                            timer = 10;
                        }
                    }
                    break;
                case 41:
                    if (timer > 0) TankUpdate();
                    break;
                case 52:
                    map2 = Game1.currentMap.mapTiles;
                    position2 = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    switch (state)
                    {
                        case 1:
                            if (map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1]]
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 - 1, 1, tileType - 1);
                                if (BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) + 16) != -1)
                                {
                                    PushAFluid(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) + 16)], endpoint);
                                }
                            }
                            break;
                        case 2:
                            if (map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1]]
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 + 1, 2, tileType - 1);
                                if (BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) - 16) != -1)
                                {
                                    PushAFluid(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) - 16)], endpoint);
                                }

                            }
                            break;
                        case 3:
                            if (map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16]]
                                && map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16 + 1, tiley / 16, 3, tileType - 1);
                                if (BigTile.FindTileId((int)(position2.X + Player.playerx) - 16, (int)(position2.Y + Player.playery)) != -1)
                                {
                                    PushAFluid(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) - 16, (int)(position2.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                        case 4:
                            if (map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16]]
                                && map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16 - 1, tiley / 16, 4, tileType - 1);
                                if (BigTile.FindTileId((int)(position2.X + Player.playerx) + 16, (int)(position2.Y + Player.playery)) != -1)
                                {
                                    PushAFluid(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) + 16, (int)(position2.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                    }

                    break;
                case 31:
                    map2 = Game1.currentMap.mapTiles;
                    position2 = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
                    switch (state)
                    {
                        case 1:
                            if (map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1]]
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 + 1] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 - 1, 1, tileType - 1);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) + 16) != -1 && Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) + 16)].output!=null)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) + 16)], endpoint);
                                }
                            }
                            break;
                        case 2:
                            if (map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1]]
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16, (int)(position2.Y + Player.playery) / 16 - 1] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16, tiley / 16 + 1, 2, tileType - 1);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) - 16) != -1 && Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) - 16)].output != null)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx), (int)(position2.Y + Player.playery) - 16)], endpoint);
                                }

                            }
                            break;
                        case 3:
                            if (map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16]]
                                && map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16 - 1, (int)(position2.Y + Player.playery) / 16] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16 + 1, tiley / 16, 3, tileType - 1);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position2.X + Player.playerx) - 16, (int)(position2.Y + Player.playery)) != -1 && Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) - 16, (int)(position2.Y + Player.playery))].output != null)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) - 16, (int)(position2.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                        case 4:
                            if (map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != -1
                                && Game1.itemInfo.ITEM_ENDPOINT[map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16]]
                                && map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != tileType + 1
                                && map2[(int)(position2.X + Player.playerx) / 16 + 1, (int)(position2.Y + Player.playery) / 16] != tileType)
                            {
                                endpoint = FindEndPoint(tilex / 16 - 1, tiley / 16, 4, tileType - 1);
                                if (Game1.globalTick > 14 && BigTile.FindTileId((int)(position2.X + Player.playerx) + 16, (int)(position2.Y + Player.playery)) != -1 && Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) + 16, (int)(position2.Y + Player.playery))].output != null)
                                {
                                    PushAnItem(Game1.bigTiles[BigTile.FindTileId((int)(position2.X + Player.playerx) + 16, (int)(position2.Y + Player.playery))], endpoint);
                                }
                            }
                            break;
                    }

                    break;
                case 39:
                    if (energy>=100 && power > 0 && schematic[0][0] >= ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT && timer == -1)
                    {
                        //if (Array.IndexOf(inventory[0], Recipes.recipeInputIds[i]) != -1 && (Array.IndexOf(inventory[0], Recipes.recipeOutputIds[i]) != -1 || Array.IndexOf(inventory[0], -1) == -1))
                        for (int a = 0; a < Recipes.recipeInputIds[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))].Length; a++)
                        {
                            if (!(CountItem(Recipes.recipeInputIds[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))][a]) >= Recipes.recipeInputQuants[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))][a])) break;
                            if (a == Recipes.recipeInputIds[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))].Length - 1)
                            {
                                timer = Recipes.recipeProcessingTime[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))];
                                recipeInProgressIndex = (schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT));
                                //inventory[1][a] -= Recipes.recipeInputQuants[(schematic[0][0] - (ItemInfo.ITEM_COUNT - ItemInfo.SCHEMATICS_COUNT))][0];
                                state = 1;

                                if (inventory[1][a] < 1)
                                {
                                    inventory[0][a] = -1;
                                    inventory[1][a] = -1;
                                }
                                //we'll check for open output slots when the timer goes off if need be
                                //items can move around during processing time is why
                                
                            }
                        }
                    }
                    if (timer == 0 && recipeInProgressIndex != -1)
                    {
                        for (int i = 0; i < Recipes.recipeOutputIds[recipeInProgressIndex].Length; i++)
                        {
                            if (recipeOutputIndex == -1)
                            {
                                for (int b = 0; b < output[0].Length; b++)
                                {
                                    if (output[0][b] == Recipes.recipeOutputIds[recipeInProgressIndex][i]
                                        && output[1][b] + Recipes.recipeOutputQuants[recipeInProgressIndex][i] <= Game1.ITEM_STACK_SIZE && Game1.itemInfo.ITEM_STACKABLE[output[0][b]])
                                    {
                                        recipeOutputIndex = b;
                                        break;
                                    }
                                }
                                if (recipeOutputIndex == -1)
                                {
                                    recipeOutputIndex = Array.IndexOf(output[0], -1);
                                }
                            }
                            if (recipeOutputIndex != -1)
                            {
                                output[0][recipeOutputIndex] = Recipes.recipeOutputIds[recipeInProgressIndex][i];
                                output[1][recipeOutputIndex] = (output[1][recipeOutputIndex] < 1) ? Recipes.recipeOutputQuants[recipeInProgressIndex][i] :
                                    output[1][recipeOutputIndex] + Recipes.recipeOutputQuants[recipeInProgressIndex][i];
                                energy -= 100;
                                if (i == Recipes.recipeOutputIds[recipeInProgressIndex].Length - 1)
                                {
                                    for (int a = 0; a < Recipes.recipeInputIds[recipeInProgressIndex].Length; a++)
                                    {
                                        RemoveItemAmount(Recipes.recipeInputIds[recipeInProgressIndex][a], Recipes.recipeInputQuants[recipeInProgressIndex][a]);
                                    }
                                    timer = -1;
                                    recipeInProgressIndex = -1;
                                    recipeOutputIndex = -1;
                                    state = 0;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case 29:

                    if (power > 0 && energy >= 500)
                    {
                        if (timer == -1)
                        {
                            //if (Array.IndexOf(inventory[0], Recipes.recipeInputIds[i]) != -1 && (Array.IndexOf(inventory[0], Recipes.recipeOutputIds[i]) != -1 || Array.IndexOf(inventory[0], -1) == -1))
                            for (int a = 0; a < inventory[0].Length; a++)
                            {
                                for (int i = 0; i < Recipes.RECIPES; i++)
                                {
                                    if (Array.IndexOf(output[0], Recipes.recipeOutputIds[i]) == -1 && Array.IndexOf(output[0], -1) == -1)
                                    {
                                        break;
                                    }
                                    if (Recipes.recipeInputIds[i][0] == inventory[0][a] && Recipes.recipeInputQuants[i][0] <= inventory[1][a])
                                    {
                                        timer = Recipes.recipeProcessingTime[i];
                                        recipeInProgressIndex = i;
                                        inventory[1][a] -= Recipes.recipeInputQuants[i][0];
                                        state = 1;
                                        energy -= 500;
                                        if (inventory[1][a] < 1)
                                        {
                                            inventory[0][a] = -1;
                                            inventory[1][a] = -1;
                                        }
                                        goto Outerloop;

                                        //we'll check for an open output slot when the timer goes off if need be
                                        //items can move around during processing time is why
                                    }
                                }
                            }
                        }
                        Outerloop: Debug.Write("");

                    }
                    if (timer == 0)
                    {
                        if (recipeOutputIndex == -1)
                        {
                            for (int b = 0; b < output[0].Length; b++)
                            {
                                if (output[0][b] == Recipes.recipeOutputIds[recipeInProgressIndex][0]
                                    && output[1][b] + Recipes.recipeOutputQuants[recipeInProgressIndex][0] <= Game1.ITEM_STACK_SIZE)
                                {
                                    recipeOutputIndex = b;
                                    break;
                                }
                            }
                            if (recipeOutputIndex == -1)
                            {
                                recipeOutputIndex = Array.IndexOf(output[0], -1);
                            }
                        }
                        if (recipeOutputIndex != -1)
                        {
                            output[0][recipeOutputIndex] = Recipes.recipeOutputIds[recipeInProgressIndex][0];
                            output[1][recipeOutputIndex] = (output[1][recipeOutputIndex] < 1) ? Recipes.recipeOutputQuants[recipeInProgressIndex][0] :
                                output[1][recipeOutputIndex] + Recipes.recipeOutputQuants[recipeInProgressIndex][0];

                            timer = -1;
                            recipeInProgressIndex = -1;
                            recipeOutputIndex = -1;
                            state = 0;
                        }

                    }
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    if (Game1.openChest != -1 && Game1.bigTiles[Game1.openChest] == this && Player.RangeFromPoint(tilex, tiley)[0] >= Game1.PLAYER_RANGE_REQUIREMENT)
                    {
                        state = 0;
                        Game1.uiObjects[1] = null;
                        Game1.openChest = -1;
                        Debug.WriteLine("shut");
                    }
                    if (state == 1 && Game1.chestSprites[0].currentFrame == 3)
                    {
                        state = 2;
                    }
                    if (state == 3 && Game1.chestSprites[1].currentFrame == 3)
                    {
                        state = 0;
                    }
                    if (state == 2 && (Game1.openChest == -1 || Game1.bigTiles[Game1.openChest] != this))
                    {
                        state = 3;
                        Debug.WriteLine("closing");
                    }
                    break;
            }
        }
        public void Trigger()
        {
            switch (tileType)
            {
                case 57:
                    
                    break;
                case 47:
                    if (state == 0)
                    {
                        state = 1;
                        Power(tilex / 16 + 1, tiley / 16, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 0, 1, new string[0]);
                        Power(tilex / 16 - 1, tiley / 16, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 1, 1, new string[0]);
                        Power(tilex / 16, tiley / 16 + 1, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 2, 1, new string[0]);
                        Power(tilex / 16, tiley / 16 - 1, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 3, 1, new string[0]);
                    }
                    else if (state == 1)
                    {
                        state = 0;
                        Power(tilex / 16 + 1, tiley / 16, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 0, 0, new string[0]);
                        Power(tilex / 16 - 1, tiley / 16, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 1, 0, new string[0]);
                        Power(tilex / 16, tiley / 16 + 1, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 2, 0, new string[0]);
                        Power(tilex / 16, tiley / 16 - 1, Game1.currentMap.mapWires[tilex / 16, tiley / 16], 0, 3, 0, new string[0]);
                    }
                    break;
                case 41:
                    if (fluidPercent == 100) fluidId = 0;
                    else fluidId = 1;
                    fluidPercent = 100;
                    
                    foreach (BigTile tile in Game1.bigTiles)
                    {
                        if (tile.tileType == 41) tile.timer = 100;
                    }
                    break;
                case 39:
                    if (Game1.currentMap.mapTiles[tilex / 16, tiley / 16 - 1] == 39
                        && Game1.currentMap.mapTiles[tilex / 16 + 1, tiley / 16] == 39
                        && Game1.currentMap.mapTiles[tilex / 16 + 1, tiley / 16 - 1] == 39)
                    {
                        Game1.bigTiles[BigTile.FindTileId(tilex, tiley - 16)].Destroy();
                        Game1.bigTiles[BigTile.FindTileId(tilex + 16, tiley)].Destroy();
                        Game1.bigTiles[BigTile.FindTileId(tilex + 16, tiley - 16)].Destroy();
                        BigTile newTile = new BigTile(40, tilex, tiley, 0, new int[0][]);
                        Game1.bigTiles.Add(newTile);
                        Game1.bigTiles.Remove(this);
                    }
                    else
                    {
                        if (Game1.uiObjects[1] != null)
                        {
                            Game1.uiObjects[1] = null;
                            Game1.openChest = -1;
                        }
                        else
                        {
                            UI ui = new UI(Game1.uiPosX[1], Game1.uiPosY[1], 3, inventory[0], inventory[1], output[0], output[1], 5, 2);
                            ui.attachment = this;
                            Game1.uiObjects[1] = ui;
                            Game1.openChest = -1;
                        }
                    }
                    break;
                case 53:
                case 32:
                    state++;
                    if (state > 4) state = 1;
                    UpdateNearbyPipes(30);
                    break;
                case 52:
                case 31:
                    state++;
                    if (state > 4) state = 1;
                    switch (state)
                    {
                        case 1:
                            endpoint = FindEndPoint(tilex / 16, tiley / 16 - 1, state, 30);
                            break;
                        case 2:
                            endpoint = FindEndPoint(tilex / 16, tiley / 16 + 1, state, 30);
                            break;
                        case 3:
                            endpoint = FindEndPoint(tilex / 16 + 1, tiley / 16, state, 30);
                            break;
                        case 4:
                            endpoint = FindEndPoint(tilex / 16 - 1, tiley / 16, state, 30);
                            break;
                    }

                    Debug.WriteLine("" + endpoint[0] + ", " + endpoint[1]);
                    if (endpoint[0] != -1) Debug.WriteLine("" + Game1.currentMap.mapTiles[endpoint[0], endpoint[1]]);
                    UpdateNearbyPipes(30);
                    break;
                case 51:
                case 30:
                    state++;
                    if (state == 1 || state > 8) state = 3;
                    //UpdateNearbyPipes();
                    break;
                case 29:
                    if (Game1.uiObjects[1] != null)
                    {
                        Game1.uiObjects[1] = null;
                        Game1.openChest = -1;
                    }
                    else
                    {
                        UI ui = new UI(Game1.uiPosX[1], Game1.uiPosY[1], 2, inventory[0], inventory[1], output[0], output[1], 4, 4);
                        ui.attachment = this;
                        Game1.uiObjects[1] = ui;
                        Game1.openChest = -1;
                    }
                    break;
                case 27:
                    //door 
                    Debug.WriteLine("door trigger");
                    if (state == 0)
                    {
                        state = (Player.currentDirection == 0 ? 1 : 2);
                        solid = false;
                    }
                    else
                    {
                        state = 0;
                        solid = true;
                    }
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    Game1.uiObjects[1] = new UI(Game1.uiPosX[1], Game1.uiPosY[1], tileType - 7, this.inventory[0], this.inventory[1], null, null, 2, 7);
                    Game1.uiToggle = false;

                    if (state == 2 && Game1.openChest == BigTile.FindTileId(tilex, tiley))
                    {
                        Debug.WriteLine("closed");
                        Game1.uiObjects[1] = null;
                        if (UI.cursorItemOrigin == 2)
                        {
                            UI.cursorItemOrigin = -1;
                            UI.cursorItemIndex = -1;
                        }
                        state = 3;
                        Game1.openChest = -1;
                    }
                    else
                    {
                        state = 1;
                        Game1.openChest = BigTile.FindTileId(tilex, tiley);
                        Debug.WriteLine(Game1.openChest);
                    }
                    UI.cursorItem = -1;
                    UI.cursorQuantity = -1;
                    UI.cursorItemIndex = -1;
                    UI.cursorItemOrigin = -1;
                    break;
                default:
                    Debug.WriteLine("default trigger");
                    break;

            }
        }
        /// <summary>
        /// Returns the amount of this item in this inventory.
        /// </summary>
        /// <returns></returns>
        public int CountItem(int itemId)
        {
            int count = 0;

            for (int i = 0; i < inventory[0].Length; i++)
            {
                if (inventory[0][i] == itemId) count += inventory[1][i];
            }

            return count;
        }
        public void RemoveItemAmount(int itemId, int quantity)
        {
            int leftover = quantity;
            for (int i = 0; i < inventory[0].Length; i++)
            {
                if (inventory[0][i] == itemId)
                {
                    if (inventory[1][i] > leftover)
                    {
                        inventory[1][i] -= leftover;
                        leftover = 0;
                    }
                    else
                    {
                        leftover -= inventory[1][i];
                        inventory[1][i] = -1;
                        inventory[0][i] = -1;
                    }
                }
                if (leftover == 0) break;
            }
        }
        public void Draw()
        {
            Draw(tilex - Player.playerx, tiley - Player.playery);
        }
        public void Draw(int x, int y)
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
            switch (tileType)
            {
                case 63:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 242, new Vector2(x, y));
                    //draw left pillar
                    for (int i=0;intVars[2]>i&&i<10;i++)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 245, new Vector2(x - intVars[0]*16, y - 16*i));
                    }
                    //draw right pillar
                    //draw overhead pipe
                    Game1.spriteBatch.End();
                    break;
                case 61:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 233 + state, new Vector2(x, y));
                    Game1.spriteBatch.End();
                    break;
                case 60:
                    Game1.spriteBatch.Begin();
                    if (map[this.tilex / 16, this.tiley / 16] == 60)
                    {

                        int[,] surround = new int[3, 3];

                        surround[0, 0] = map[this.tilex / 16 - 1, this.tiley / 16 - 1];
                        surround[1, 0] = map[this.tilex / 16, this.tiley / 16 - 1];
                        surround[2, 0] = map[this.tilex / 16 + 1, this.tiley / 16 - 1];
                        surround[0, 1] = map[this.tilex / 16 - 1, this.tiley / 16];
                        surround[2, 1] = map[this.tilex / 16 + 1, this.tiley / 16];
                        surround[0, 2] = map[this.tilex / 16 - 1, this.tiley / 16 + 1];
                        surround[1, 2] = map[this.tilex / 16, this.tiley / 16 + 1];
                        surround[2, 2] = map[this.tilex / 16 + 1, this.tiley / 16 + 1];

                        if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 216, Color.White, new Vector2(x, y));
                        }
                        else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 9, Color.White, new Vector2(x, y));
                        }
                        else
                        {
                            //solos
                            if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 10, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 11, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 11, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 10, Color.White, new Vector2(x, y));
                            }
                            //corners
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 1, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 2, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 3, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 4, Color.White, new Vector2(x, y));
                            }
                            //3 ways
                            else if (surround[1, 0] != map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 5, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] != map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 6, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] != map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 7, Color.White, new Vector2(x, y));
                            }
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] != map[this.tilex / 16, this.tiley / 16] && surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 8, Color.White, new Vector2(x, y));
                            }
                            //sides
                            else if (surround[1, 0] == map[this.tilex / 16, this.tiley / 16] && surround[1, 2] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 11, Color.White, new Vector2(x, y));
                            }
                            else if (surround[0, 1] == map[this.tilex / 16, this.tiley / 16] && surround[2, 1] == map[this.tilex / 16, this.tiley / 16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 216 + 10, Color.White, new Vector2(x, y));
                            }
                        }
                        if (map[this.tilex / 16, this.tiley / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[this.tilex / 16, this.tiley / 16]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 225, new Vector2(x, y));
                        }
                        if (surround[0, 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[0, 1]]) Game1.tiles.DrawTile(Game1.spriteBatch, 228, new Vector2(x, y));
                        if (surround[2, 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[2, 1]]) Game1.tiles.DrawTile(Game1.spriteBatch, 230, new Vector2(x, y));
                        if (surround[1, 2] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[1, 2]]) Game1.tiles.DrawTile(Game1.spriteBatch, 229, new Vector2(x, y));
                        if (surround[1, 0] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[1, 0]]) Game1.tiles.DrawTile(Game1.spriteBatch, 231, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 59:
                    Game1.spriteBatch.Begin();
                    if (map[this.tilex / 16, this.tiley / 16 - 2] == 59)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 215, new Vector2(x, y - 16));
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 213, new Vector2(x, y - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 214, new Vector2(x, y));
                    
                    Game1.tiles.DrawTile(Game1.spriteBatch, 209 + Game1.globalTick/4, new Vector2(x, y - 16));
                    Game1.spriteBatch.End();
                    break;
                case 58:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 207, new Vector2(x, y));
                    Game1.spriteBatch.End();
                    DrawFullAutoTile(136);
                    Game1.spriteBatch.Begin();
                    //Game1.spriteBatch.DrawString(Game1.font, ""+energy, new Vector2(x + 8, y), Color.White);
                    if (energy > 666)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 208, Color.LightGreen, new Vector2(x, y));
                    }
                    else if (energy > 333)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 208, Color.Yellow, new Vector2(x, y));
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 208, Color.Red, new Vector2(x, y));
                    }

                    Game1.spriteBatch.End();
                    break;
                case 57:
                    Game1.spriteBatch.Begin();
                    if (map[this.tilex/16, this.tiley/16] == 57)
                    {

                        int[,] surround = new int[3, 3];

                        surround[0, 0] = map[this.tilex/16 - 1, this.tiley/16 - 1];
                        surround[1, 0] = map[this.tilex/16, this.tiley/16 - 1];
                        surround[2, 0] = map[this.tilex/16 + 1, this.tiley/16 - 1];
                        surround[0, 1] = map[this.tilex/16 - 1, this.tiley/16];
                        surround[2, 1] = map[this.tilex/16 + 1, this.tiley/16];
                        surround[0, 2] = map[this.tilex/16 - 1, this.tiley/16 + 1];
                        surround[1, 2] = map[this.tilex/16, this.tiley/16 + 1];
                        surround[2, 2] = map[this.tilex/16 + 1, this.tiley/16 + 1];

                        if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 191, Color.White, new Vector2(x , y ));
                        }
                        else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 9, Color.White, new Vector2(x , y ));
                        }
                        else
                        {
                            //solos
                            if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 10, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 11, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 11, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 10, Color.White, new Vector2(x , y ));
                            }
                            //corners
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 1, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 2, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 3, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 4, Color.White, new Vector2(x , y ));
                            }
                            //3 ways
                            else if (surround[1, 0] != map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 5, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] != map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 6, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] != map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 7, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] != map[this.tilex/16, this.tiley/16] && surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 8, Color.White, new Vector2(x , y ));
                            }
                            //sides
                            else if (surround[1, 0] == map[this.tilex/16, this.tiley/16] && surround[1, 2] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 11, Color.White, new Vector2(x , y ));
                            }
                            else if (surround[0, 1] == map[this.tilex/16, this.tiley/16] && surround[2, 1] == map[this.tilex/16, this.tiley/16])
                            {
                                Game1.tiles.DrawTile(Game1.spriteBatch, 191 + 10, Color.White, new Vector2(x , y ));
                            }
                        }
                        if (map[this.tilex/16, this.tiley/16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[this.tilex/16, this.tiley/16]])
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 200, new Vector2(x , y ));
                        }
                        if (surround[0, 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[0, 1]]) Game1.tiles.DrawTile(Game1.spriteBatch, 203, new Vector2(x, y));
                        if (surround[2, 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[2, 1]]) Game1.tiles.DrawTile(Game1.spriteBatch, 205, new Vector2(x, y));
                        if (surround[1, 2] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[1, 2]]) Game1.tiles.DrawTile(Game1.spriteBatch, 204, new Vector2(x, y));
                        if (surround[1, 0] != -1 && Game1.itemInfo.ITEM_ENDPOINT[surround[1, 0]]) Game1.tiles.DrawTile(Game1.spriteBatch, 206, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 55:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 186, new Vector2(x, y - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 187, new Vector2(x + 16, y - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 189, new Vector2(x + 16, y));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 188, new Vector2(x, y));
                    Game1.spriteBatch.End();
                    break;
                case 54:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 174 + ((power > 0) ? 1 + Game1.globalTick / 8 : 0), new Vector2(x, y - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 177 + ((power > 0) ? 1 + Game1.globalTick / 8 : 0), new Vector2(x + 16, y - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 183 + ((power > 0) ? 1 + Game1.globalTick / 8 : 0), new Vector2(x + 16, y));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 180 + ((power > 0) ? 1 + Game1.globalTick / 8 : 0), new Vector2(x, y));
                    Game1.spriteBatch.End();
                    break;
                case 51:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 157 + state, position);
                    Game1.spriteBatch.End();
                    break;
                case 47:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 155 + state, position);
                    Game1.spriteBatch.End();
                    break;
                case 41:
                    Game1.spriteBatch.Begin();
                    //draw fluids first
                    for (int i = 0; i < 16; i++)
                    {
                        if (i < (int)(Math.Round(fluidPercent) / 100.0 * 16)) Game1.fluids.DrawTile(Game1.spriteBatch, fluidId, Color.White*0.9f, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery + 15 - i));
                    }

                    Game1.tiles.DrawTile(Game1.spriteBatch, 153, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                    Game1.spriteBatch.End();
                    DrawFullAutoTile(136);
                    break;
                case 40:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 134, new Vector2(this.tilex - Player.playerx + 16, this.tiley - Player.playery - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 132, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery - 16));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 133, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                    Game1.tiles.DrawTile(Game1.spriteBatch, 135, new Vector2(this.tilex - Player.playerx + 16, this.tiley - Player.playery));
                    Game1.spriteBatch.End();
                    break;
                case 39:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 131, position);
                    Game1.spriteBatch.DrawString(Game1.font, "" + energy, new Vector2(x + 8, y), Color.White);
                    Game1.spriteBatch.End();
                    break;
                case 53:
                    Game1.spriteBatch.Begin();
                    //Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 52 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 166, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 52 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 167, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 52 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 168, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 52 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 169, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 166, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 166 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 32:
                    Game1.spriteBatch.Begin();
                    //Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 119, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 120, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 121, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 122, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 119, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 119 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 52:
                    Game1.spriteBatch.Begin();
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 52 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 170, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 52 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 171, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 52 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 172, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 52 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 53)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 173, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 170, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 170 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 31:
                    Game1.spriteBatch.Begin();
                    if (state == 0)
                    {
                        if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 123, position); //under
                            state = 1;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1]]
                            && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 31 && map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 124, position); //above
                            state = 2;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 125, position); //left
                            state = 3;
                        }
                        else if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && Game1.itemInfo.ITEM_ENDPOINT[map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16]]
                            && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 31 && map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != 32)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 126, position); //right
                            state = 4;
                        }
                        else Game1.tiles.DrawTile(Game1.spriteBatch, 123, position); //default to under
                    }
                    else Game1.tiles.DrawTile(Game1.spriteBatch, 123 + state - 1, position);
                    Game1.spriteBatch.End();
                    break;
                case 30:
                    Game1.spriteBatch.Begin();
                    Game1.tiles.DrawTile(Game1.spriteBatch, 110 + state, position);
                    Game1.spriteBatch.End();
                    break;
                case 29:
                    Game1.spriteBatch.Begin();
                    //Game1.tiles.DrawTile(Game1.spriteBatch, Game1.itemInfo.ITEM_BLOCKID[itemId], position);
                    if (!(map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == tileType))
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 44, position); //nounder
                        if (state == 1)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 128, position); //lit
                        }
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 43, position); //else
                        if (state == 1)
                        {
                            Game1.tiles.DrawTile(Game1.spriteBatch, 127, position); //lit
                        }
                    }
                    Game1.spriteBatch.End();
                    break;
                case 28:
                    Game1.spriteBatch.Begin();
                    if (Game1.globalTick / 4 > 2)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 98 + 1, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 101 + 1, new Vector2(x + 16, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 107 + 1, new Vector2(x + 16, y));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 104 + 1, new Vector2(x, y));
                    }
                    else
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 98 + Game1.globalTick / 4, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 101 + Game1.globalTick / 4, new Vector2(x + 16, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 107 + Game1.globalTick / 4, new Vector2(x + 16, y));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 104 + Game1.globalTick / 4, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 27:
                    Game1.spriteBatch.Begin();
                    if (state == 0)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 92, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 93, new Vector2(x, y));
                    }
                    else if (state == 1)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 96, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 97, new Vector2(x, y));
                    }
                    else if (state == 2)
                    {
                        Game1.tiles.DrawTile(Game1.spriteBatch, 94, new Vector2(x, y - 16));
                        Game1.tiles.DrawTile(Game1.spriteBatch, 95, new Vector2(x, y));
                    }
                    Game1.spriteBatch.End();
                    break;
                case 13:
                case 12:
                case 11:
                case 10:
                case 9:
                case 8:
                    switch (state)
                    {
                        case 0:
                            Game1.spriteBatch.Begin();
                            Game1.tiles.DrawTile(Game1.spriteBatch, 35, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.tiles.DrawTile(Game1.spriteBatch, 37, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            Game1.spriteBatch.End();
                            break;
                        case 1:
                            Game1.chestSprites[0].Draw(Game1.spriteBatch, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.chestSprites[2].Draw(Game1.spriteBatch, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            break;
                        case 2:
                            Game1.spriteBatch.Begin();
                            Game1.tiles.DrawTile(Game1.spriteBatch, 36, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.tiles.DrawTile(Game1.spriteBatch, 38, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            Game1.spriteBatch.End();
                            break;
                        case 3:
                            Game1.chestSprites[1].Draw(Game1.spriteBatch, new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            if (tileType > 8) Game1.chestSprites[3].Draw(Game1.spriteBatch, Chest.chestRankColors[tileType - 8], new Vector2(tilex - Player.playerx, tiley - Player.playery));
                            break;
                    }
                    break;
            }
        }
        public void Destroy()
        {
            if (width == 1 && height == 1)
            {
                Game1.currentMap.mapTiles[tilex / 16, tiley / 16] = -1;
            }
            else
            {
                for (int i = tilex / 16 * 16; i < tilex + width * 16; i++)
                {
                    for (int a = tiley / 16 * 16 - height * 16 + 16; a < tiley / 16 * 16 + 16; a++)
                    {
                        Game1.currentMap.mapTiles[i / 16, a / 16] = -1;
                    }
                }
            }
            if (tileType == 30) UpdateNearbyPipes(30);
            else if (tileType == 51) UpdateNearbyPipes(51);
            Debug.WriteLine("destroy: " + Game1.bigTiles.IndexOf(this));
            Game1.bigTiles.Remove(this);
        }
        public static int FindTileId(int idx, int idy)
        {
            for (int i = 0; i < Game1.bigTiles.Count; i++)
            {
                //idx = idx - idx % 16;
                //idy = idy - idy % 16;
                if (idx >= Game1.bigTiles[i].tilex && idx < Game1.bigTiles[i].tilex + Game1.bigTiles[i].width * 16 &&
                    idy >= Game1.bigTiles[i].tiley - Game1.bigTiles[i].height * 16 + 16 && idy < Game1.bigTiles[i].tiley + 16)
                {
                    return i;
                }
            }
            return -1;
        }



        // MORE FUNCTIONS

        /// <summary>
        /// finds an item in a given inventory and "pushes" it to another given inventory
        /// MUST ONLY be called from a tileType 32 endpoint
        /// </summary>
        public static void PushAnItem(BigTile startInv, int[] endPoint)
        {
            BigTile end;
            //check if there are items in the starting inventory's output
            for (int i = 0; i < startInv.output[0].Length; i++)
            {
                if (startInv.output[0][i] != -1 && (endPoint[0] != -1 && BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16) != -1))
                {
                    // check if there's a spot in the endpoint's attached inventory, if there is one
                    switch (Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16)].state)
                    {
                        case 1:
                            if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 2:
                            if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 3:
                            if (BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                        case 4:
                            if (BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16) != -1)
                            {
                                end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16)];
                                for (int a = 0; a < end.inventory[0].Length; a++)
                                {
                                    if (end.inventory[0][a] == -1)
                                    {
                                        end.inventory[0][a] = startInv.output[0][i];
                                        end.inventory[1][a] = 1;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                    else if (end.inventory[0][a] == startInv.output[0][i] && end.inventory[1][a] + 1 <= Game1.ITEM_STACK_SIZE)
                                    {
                                        end.inventory[1][a]++;
                                        startInv.output[1][i]--;
                                        if (startInv.output[1][i] < 1) startInv.output[0][i] = -1;
                                        goto OuterLoop;
                                    }
                                }
                            }
                            break;
                    }
                }
            }
            OuterLoop:;
        }
        /// <summary>
        /// finds an pushes a fluid from one liquid inventory to another
        /// MUST ONLY be called from a tileType 53 endpoint
        /// </summary>
        public static void PushAFluid(BigTile startInv, int[] endPoint)
        {
            BigTile end;
            //check if there are items in the starting inventory's output
            if ((int)startInv.fluidPercent > 0 && (endPoint[0] != -1 && BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16) != -1))
            {
                // check if there's a spot in the endpoint's attached inventory, if there is one
                switch (Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16)].state)
                {
                    case 1:
                        if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16) !=-1 && Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16)].fluidPercent<100)
                        {
                            end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 + 16)];
                            if (startInv.fluidId != -1) end.fluidId = startInv.fluidId;
                            end.fluidPercent ++;
                            startInv.fluidPercent --;
                            Debug.WriteLine("fluidId " + end.fluidId);
                        }
                        else if (Game1.currentMap.mapFluids[endPoint[0], endPoint[1] + 1]<100 && Game1.currentMap.mapTiles[endPoint[0], endPoint[1] + 1]==-1&&(Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1]==-1 || Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1] == startInv.fluidId))
                        {
                            startInv.fluidPercent--;
                            Game1.currentMap.mapFluids[endPoint[0], endPoint[1] + 1]++;
                            if (startInv.fluidId!=-1) Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1] = startInv.fluidId;
                            //Debug.WriteLine("fluidId " + Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1]);
                        }
                        foreach (BigTile tile in Game1.bigTiles)
                        {
                            if (tile.tileType == 41) tile.timer = 200;
                        }
                        break;
                    case 2:
                        if (BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16) != -1 && Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16)].fluidPercent < 100)
                        {
                            end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16, endPoint[1] * 16 - 16)];
                            if (startInv.fluidId != -1) end.fluidId = startInv.fluidId;
                            end.fluidPercent++;
                            startInv.fluidPercent--;
                            Debug.WriteLine("fluidId " + end.fluidId);
                        }
                        else if (Game1.currentMap.mapFluids[endPoint[0], endPoint[1] - 1] < 100 && Game1.currentMap.mapTiles[endPoint[0], endPoint[1] - 1] == -1 && (Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] - 1] == -1 || Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] - 1] == startInv.fluidId))
                        {
                            startInv.fluidPercent--;
                            Game1.currentMap.mapFluids[endPoint[0], endPoint[1] - 1]++;
                            if (startInv.fluidId != -1) Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] - 1] = startInv.fluidId;
                            //Debug.WriteLine("fluidId " + Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1]);
                        }
                        foreach (BigTile tile in Game1.bigTiles)
                        {
                            if (tile.tileType == 41) tile.timer = 200;
                        }
                        break;
                    case 3:
                        if (BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16) != -1 && Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16)].fluidPercent < 100)
                        {
                            end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 - 16, endPoint[1] * 16)];
                            if (startInv.fluidId != -1) end.fluidId = startInv.fluidId;
                            end.fluidPercent++;
                            startInv.fluidPercent--;
                            Debug.WriteLine("fluidId " + end.fluidId);
                        }
                        else if (Game1.currentMap.mapFluids[endPoint[0] - 1, endPoint[1]] < 100 && Game1.currentMap.mapTiles[endPoint[0] - 1, endPoint[1]] == -1 && (Game1.currentMap.mapFluidIds[endPoint[0] - 1, endPoint[1]] == -1 || Game1.currentMap.mapFluidIds[endPoint[0] - 1, endPoint[1]] == startInv.fluidId))
                        {
                            startInv.fluidPercent--;
                            Game1.currentMap.mapFluids[endPoint[0] - 1, endPoint[1]]++;
                            if (startInv.fluidId != -1) Game1.currentMap.mapFluidIds[endPoint[0] - 1, endPoint[1]] = startInv.fluidId;
                            //Debug.WriteLine("fluidId " + Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1]);
                        }
                        foreach (BigTile tile in Game1.bigTiles)
                        {
                            if (tile.tileType == 41) tile.timer = 200;
                        }
                        break;
                    case 4:
                        if (BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16) != -1 && Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16)].fluidPercent < 100)
                        {
                            end = Game1.bigTiles[BigTile.FindTileId(endPoint[0] * 16 + 16, endPoint[1] * 16)];
                            if (startInv.fluidId != -1) end.fluidId = startInv.fluidId;
                            end.fluidPercent++;
                            startInv.fluidPercent--;
                            Debug.WriteLine("fluidId " + end.fluidId);
                        }
                        else if (Game1.currentMap.mapFluids[endPoint[0] + 1, endPoint[1]] < 100 && Game1.currentMap.mapTiles[endPoint[0] + 1, endPoint[1]] == -1 && (Game1.currentMap.mapFluidIds[endPoint[0] + 1, endPoint[1]] == -1 || Game1.currentMap.mapFluidIds[endPoint[0] + 1, endPoint[1]] == startInv.fluidId))
                        {
                            startInv.fluidPercent--;
                            Game1.currentMap.mapFluids[endPoint[0] + 1, endPoint[1]]++;
                            if (startInv.fluidId != -1) Game1.currentMap.mapFluidIds[endPoint[0] + 1, endPoint[1]] = startInv.fluidId;
                            //Debug.WriteLine("fluidId " + Game1.currentMap.mapFluidIds[endPoint[0], endPoint[1] + 1]);
                        }
                        foreach (BigTile tile in Game1.bigTiles)
                        {
                            if (tile.tileType == 41) tile.timer = 200;
                        }
                        break;
                }
                
            }
        }
        ///fully draws an autotile of this item - keep tileorder in the spritesheet in mind
        private void DrawFullAutoTile(int startTileId)
        {
            int[,] surround = new int[3, 3];
            int[,] map = Game1.currentMap.mapTiles;
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);

            surround[0, 0] = map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16 - 1];
            surround[1, 0] = map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1];
            surround[2, 0] = map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16 - 1];
            surround[0, 1] = map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16];
            surround[2, 1] = map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16];
            surround[0, 2] = map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16 + 1];
            surround[1, 2] = map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1];
            surround[2, 2] = map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16 + 1];

            Game1.spriteBatch.Begin();
            if (surround[1, 0] != tileType && surround[1, 2] != tileType && surround[0, 1] != tileType && surround[2, 1] != tileType)
            {
                Game1.tiles.DrawTile(Game1.spriteBatch, startTileId, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
            }
            else
            {
                if (surround[1, 0] != tileType && surround[1, 2] != tileType && surround[0, 1] == tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 5, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] == tileType && surround[1, 2] != tileType && surround[0, 1] != tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 6, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] != tileType && surround[1, 2] == tileType && surround[0, 1] != tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 7, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] != tileType && surround[1, 2] != tileType && surround[0, 1] != tileType && surround[2, 1] == tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 8, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }

                else if (surround[1, 0] == tileType && surround[1, 2] != tileType && surround[0, 1] != tileType && surround[2, 1] == tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 1, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] == tileType && surround[1, 2] != tileType && surround[0, 1] == tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 2, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] != tileType && surround[1, 2] == tileType && surround[0, 1] == tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 3, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] != tileType && surround[1, 2] == tileType && surround[0, 1] != tileType && surround[2, 1] == tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 4, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }

                else if (surround[1, 0] == tileType && surround[1, 2] == tileType && surround[2, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 12, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                    if (surround[0, 1] != tileType) Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 11, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[1, 0] == tileType && surround[1, 2] == tileType && surround[0, 1] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 11, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }

                else if (surround[0, 1] == tileType && surround[2, 1] == tileType && surround[1, 2] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 10, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                    if (surround[1, 0] != tileType) Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 9, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                else if (surround[0, 1] == tileType && surround[2, 1] == tileType && surround[1, 0] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 9, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }

                if (surround[0, 1] == tileType && surround[1, 2] == tileType && surround[0, 2] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 13, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                if (surround[0, 1] == tileType && surround[1, 0] == tileType && surround[0, 0] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 14, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                if (surround[2, 1] == tileType && surround[1, 2] == tileType && surround[2, 2] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 15, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
                if (surround[2, 1] == tileType && surround[1, 0] == tileType && surround[2, 0] != tileType)
                {
                    Game1.tiles.DrawTile(Game1.spriteBatch, startTileId + 16, new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery));
                }
            }
            Game1.spriteBatch.End();
        }
        ///finds an endpoint pipe (a puller) from the given starting pipe (a pusher) and returns an array [x,y]
        public static int[] FindEndPoint(int x, int y, int direction, int pipeTile)
        {
            if (Game1.currentMap.mapTiles[x, y] == pipeTile + 2)
            {
                if (BigTile.FindTileId(x * 16, y * 16) != -1)
                {
                    switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                    {
                        case 1:
                            if (direction == 2) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 2:
                            if (direction == 1) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 3:
                            if (direction == 4) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        case 4:
                            if (direction == 3) return new int[] { x, y };
                            else return new int[] { -1, -1 };
                            break;
                        default:
                            return new int[] { -1, -1 };
                    }
                }
                else return new int[] { -1, -1 };
            }
            else if (Game1.currentMap.mapTiles[x, y] == pipeTile + 1)
            {
                return new int[] { -1, -1 };
            }
            else if (Game1.currentMap.mapTiles[x, y] == pipeTile)
            {
                switch (direction)
                {
                    case 1:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 4:
                                return FindEndPoint(x, y - 1, 1, pipeTile);
                                break;
                            case 5:
                                return FindEndPoint(x + 1, y, 3, pipeTile);
                                break;
                            case 7:
                                return FindEndPoint(x - 1, y, 4, pipeTile);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 2:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 4:
                                return FindEndPoint(x, y + 1, 2, pipeTile);
                                break;
                            case 6:
                                return FindEndPoint(x + 1, y, 3, pipeTile);
                                break;
                            case 8:
                                return FindEndPoint(x - 1, y, 4, pipeTile);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 3:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 3:
                                return FindEndPoint(x + 1, y, 3, pipeTile);
                                break;
                            case 7:
                                return FindEndPoint(x, y + 1, 2, pipeTile);
                                break;
                            case 8:
                                return FindEndPoint(x, y - 1, 1, pipeTile);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    case 4:
                        switch (Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].state)
                        {
                            case 3:
                                return FindEndPoint(x - 1, y, 4, pipeTile);
                                break;
                            case 5:
                                return FindEndPoint(x, y + 1, 2, pipeTile);
                                break;
                            case 6:
                                return FindEndPoint(x, y - 1, 1, pipeTile);
                                break;
                            default:
                                return new int[] { -1, -1 };
                        }
                        break;
                    default:
                        return new int[] { -1, -1 };
                }
            }
            else return new int[] { -1, -1 };
        }

        /// <summary>
        /// updates pipes above, below, left and right of this pipe,
        /// but not the pipe itself
        /// </summary>
        public void UpdateNearbyPipes(int pipeTile)
        {
            int[,] map = (Game1.itemInfo.ITEM_BACKTILE[tileType] ? Game1.currentMap.mapBackTiles : Game1.currentMap.mapTiles);
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);

            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile)
            {
                Debug.WriteLine("" + tilex + ',' + tiley);
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].PipeUpdate(pipeTile);
                Debug.WriteLine("checked upward");
            }
            if (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].PipeUpdate(pipeTile);
            }
            if (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].PipeUpdate(pipeTile);
            }
            if (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile)
            {
                Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].PipeUpdate(pipeTile);
            }
        }
        public static void UpdateAllPipes(int pipeTile)
        {
            foreach (BigTile tile in Game1.bigTiles)
            {
                if (tile.tileType == 30)
                {
                    tile.PipeUpdate(pipeTile);
                }
            }
        }
        /// <summary>
        /// updates this pipe
        /// </summary>
        private void PipeUpdate(int pipeTile)
        {
            int[,] map = Game1.currentMap.mapTiles;
            Vector2 position = new Vector2(this.tilex - Player.playerx, this.tiley - Player.playery);
            
            {
                if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile)
                || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3))
                && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                    || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                    || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //left AND right
                    state = 3;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1)))
                {
                    //under AND above
                    state = 4;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1))
                    && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //downright
                    state = 5;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1))
                    && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3)))
                {
                    //downleft
                    state = 7;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //upright
                    state = 6;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                        || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3)))
                {
                    //upleft
                    state = 8;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3
                    || map[(int)(position.X + Player.playerx) / 16 - 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) - 16, (int)(position.Y + Player.playery))].state == 3))
                    || (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] != -1 && (map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4
                        || map[(int)(position.X + Player.playerx) / 16 + 1, (int)(position.Y + Player.playery) / 16] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx) + 16, (int)(position.Y + Player.playery))].state == 4)))
                {
                    //left OR right
                    state = 3;
                }
                else if (((map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile)
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2
                    || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 - 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) - 16)].state == 2))
                    || (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] != -1 && (map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 1 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1
                        || map[(int)(position.X + Player.playerx) / 16, (int)(position.Y + Player.playery) / 16 + 1] == pipeTile + 2 && Game1.bigTiles[BigTile.FindTileId((int)(position.X + Player.playerx), (int)(position.Y + Player.playery) + 16)].state == 1)))
                {
                    //under OR above
                    state = 4;
                }
                else
                {
                    //none
                    state = 0;
                }
            }
        }
        ///Updates this tank
        public void TankUpdate()
        {
            if (fluidPercent == 0) fluidId = -1;
            //move fluid downward
            if (Game1.currentMap.mapTiles[tilex / 16, tiley / 16 + 1] == tileType && BigTile.FindTileId(tilex, tiley + 16) != -1)
            {
                if (Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidId == fluidId || Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidId==-1|| fluidId==-1)
                {
                    if (Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidPercent + fluidPercent > 100)
                    {
                        fluidPercent = fluidPercent - (100 - Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidPercent);
                        Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidPercent = 100;
                        if (fluidId!=-1) Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidId = fluidId;
                    }
                    else
                    {
                        Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidPercent += fluidPercent;
                        if (fluidId != -1) Game1.bigTiles[BigTile.FindTileId(tilex, tiley + 16)].fluidId = fluidId;
                        fluidPercent = 0;
                        fluidId = -1;
                    } 
                }
            }

            //average fluid horizontally
            double total = 0;
            if (BigTile.FindTileId(tilex - 16, tiley) != -1 && (Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidId==fluidId || fluidId==-1 || Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidId==-1))
            {
                total = Game1.bigTiles[BigTile.FindTileId(tilex, tiley)].fluidPercent + Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidPercent;
                Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidPercent = total / 2;
                if (fluidId!=-1) Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidId = fluidId;
                else if (Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidId != -1) fluidId = Game1.bigTiles[BigTile.FindTileId(tilex - 16, tiley)].fluidId;
                fluidPercent = total / 2;
            }
        }
        /// <summary>
        /// returns a list of coordinates for outputs so that they can be powered with a distributed amount of energy.
        /// </summary>
        /// <returns></returns>
        public string[] PushEnergy(int x, int y, int cableType, int count, int dir, string[] list, string[] outputs)
        {
            //Debug.WriteLine("made it");
            string[] newlist = new string[list.Length + 1];
            Array.Copy(list, newlist, list.Length);
            if (list.Contains(x + "," + y)) return outputs;
            newlist[list.Length] = x + "," + y;
            list = newlist;
            if (count > 50) return outputs;
            if ((Game1.currentMap.mapTiles[x, y] != cableType && (Game1.currentMap.mapTiles[x, y]==-1 || !Game1.itemInfo.ITEM_ENDPOINT[Game1.currentMap.mapTiles[x, y]])) || cableType == -1 || (Game1.currentMap.mapTiles[x, y] == 47))
            {
                //Debug.WriteLine("terminated");
                return outputs;
            }
            else
            {
                if (Game1.currentMap.mapTiles[x, y] != -1 && Game1.itemInfo.ITEM_ENDPOINT[Game1.currentMap.mapTiles[x, y]] && BigTile.FindTileId(x * 16, y * 16) != -1)
                {
                    //Debug.WriteLine("power!");
                    if (!outputs.Contains(x + "," + y))
                    {
                        string[] newlist2 = new string[outputs.Length + 1];
                        Array.Copy(outputs, newlist2, outputs.Length);
                        newlist2[outputs.Length] = x + "," + y;
                        outputs = newlist2;
                        //Debug.WriteLine(outputs[outputs.Length - 1]);
                    }
                    return outputs;
                }
                if (!list.Contains((x + 1) + "," + y))
                {
                    outputs = PushEnergy(x + 1, y, cableType, count + 1, 0, list, outputs);
                }
                if (!list.Contains((x - 1) + "," + y))
                {
                    outputs = PushEnergy(x - 1, y, cableType, count + 1, 1, list, outputs);
                }
                if (!list.Contains(x + "," + (y + 1)))
                {
                    outputs = PushEnergy(x, y + 1, cableType, count + 1, 2, list, outputs);
                }
                if (!list.Contains("," + (y + 1)))
                {
                    outputs = PushEnergy(x, y - 1, cableType, count + 1, 3, list, outputs);
                }
            }
            return outputs;
        }
        public void Power(int x, int y, int wireType, int count, int dir, int powerLevel, string[] list)
        {
            string[] newlist = new string[list.Length + 1];
            Array.Copy(list, newlist, list.Length);
            if (list.Contains(x + "," + y)) return;
            newlist[list.Length] = x + "," + y;
            list = newlist;
            if (count > 50) return;
            if (Game1.currentMap.mapWires[x, y] != wireType || wireType == -1 || (Game1.currentMap.mapTiles[x, y] == 47)) return;
            else
            {
                if (Game1.currentMap.mapTiles[x, y] != -1 && Game1.itemInfo.ITEM_ENDPOINT[Game1.currentMap.mapTiles[x, y]] && BigTile.FindTileId(x * 16, y * 16) != -1)
                {
                    Game1.bigTiles[BigTile.FindTileId(x * 16, y * 16)].power = powerLevel;

                }
                if (Game1.currentMap.mapWires[x + 1, y] == wireType && dir != 1 && !list.Contains((x + 1) + "," + y))
                {
                    Power(x + 1, y, wireType, count + 1, 0, powerLevel, list);
                }
                if (Game1.currentMap.mapWires[x - 1, y] == wireType && dir != 0 && !list.Contains((x - 1) + "," + y))
                {
                    Power(x - 1, y, wireType, count + 1, 1, powerLevel, list);
                }
                if (Game1.currentMap.mapWires[x, y + 1] == wireType && dir != 3 && !list.Contains(x + "," + (y + 1)))
                {
                    Power(x, y + 1, wireType, count + 1, 2, powerLevel, list);
                }
                if (Game1.currentMap.mapWires[x, y - 1] == wireType && dir != 2 && !list.Contains("," + (y + 1)))
                {
                    Power(x, y - 1, wireType, count + 1, 3, powerLevel, list);
                }
            }
            return;
        }
    }
}
