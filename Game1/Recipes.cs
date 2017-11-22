using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class Recipes
    {
        public static int RECIPES = 2;
        public static int[][] recipeInputIds = new int[RECIPES][];
        public static int[][] recipeInputQuants = new int[RECIPES][];

        public static int[][] recipeOutputIds = new int[RECIPES][];
        public static int[][] recipeOutputQuants = new int[RECIPES][];

        public static int[] recipeTileType = new int[RECIPES];
        public static int[] recipeProcessingTime = new int[RECIPES];

        static Recipes()
        {
            //furnace:
            Recipes.recipeInputIds[0] = new int[] { 33 };
            Recipes.recipeInputQuants[0] = new int[] { 2 };
            Recipes.recipeOutputIds[0] = new int[] { 34 };
            Recipes.recipeOutputQuants[0] = new int[] { 1 };
            Recipes.recipeTileType[0] = 29;
            Recipes.recipeProcessingTime[0] = 30;

            Recipes.recipeInputIds[1] = new int[] { 35 };
            Recipes.recipeInputQuants[1] = new int[] { 2 };
            Recipes.recipeOutputIds[1] = new int[] { 20 };
            Recipes.recipeOutputQuants[1] = new int[] { 1 };
            Recipes.recipeTileType[1] = 29;
            Recipes.recipeProcessingTime[1] = 15;

        }
        

    }
}
