using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game1
{
    public static class Recipes
    {
        public static int RECIPES = 4;
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

            //assembler
            Recipes.recipeInputIds[2] = new int[] { 15, 34 };
            Recipes.recipeInputQuants[2] = new int[] { 2, 1 };
            Recipes.recipeOutputIds[2] = new int[] { 16 };
            Recipes.recipeOutputQuants[2] = new int[] { 1 };
            Recipes.recipeTileType[2] = 39;
            Recipes.recipeProcessingTime[2] = 45;

            Recipes.recipeInputIds[3] = new int[] { 28, 34 };
            Recipes.recipeInputQuants[3] = new int[] { 1, 3 };
            Recipes.recipeOutputIds[3] = new int[] { 48 };
            Recipes.recipeOutputQuants[3] = new int[] { 1 };
            Recipes.recipeTileType[3] = 39;
            Recipes.recipeProcessingTime[3] = 200;
        }
        

    }
}
