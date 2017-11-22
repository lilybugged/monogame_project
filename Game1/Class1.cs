using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class Recipes
    {
        public static int[][] recipeInputIds = new int[1][];
        public static int[][] recipeInputQuants = new int[1][];

        public static int[][] recipeOutputIds = new int[1][];
        public static int[][] recipeOutputQuants = new int[1][];

        public static int[] recipeTileType = new int[1];

        static Recipes()
        {
            //furnace:
            Recipes.recipeInputIds[0] = new int[] { 33 };
            Recipes.recipeInputQuants[0] = new int[] { 2 };
            Recipes.recipeOutputIds[0] = new int[] { 34 };
            Recipes.recipeOutputQuants[0] = new int[] { 1 };
            Recipes.recipeTileType[0] = 29;

        }
        

    }
}
