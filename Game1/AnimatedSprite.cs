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
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int currentFrame;
        private int totalFrames;
        private int tick = 0;
        private int tickSpeed = 1; // don't make this any more than 1 or less than -1

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }
        //TODO: do something about the global tick issue lol
        public void Update()
        {
            if (tick > 4) tick = 0;
            currentFrame += (tick==4? 1:0);
            if (currentFrame >= totalFrames)
                currentFrame = 0;
            tick++;
        }

        public void Draw(SpriteBatch sprBatch, Vector2 location)
        {
            totalFrames = Rows * Columns;
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            sprBatch.End();
        }
        public void Draw(SpriteBatch sprBatch, Color color, Vector2 location)
        {
            totalFrames = Rows * Columns;
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);
            sprBatch.End();
        }
        public void Draw(SpriteBatch sprBatch, int startframe, int endframe, int speed, Vector2 location)
        {
            //this.tickSpeed = speed;
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = (int)(((float) startframe + (float)tickSpeed*(float)currentFrame) / (float)Columns);
            int column = (startframe + tickSpeed * currentFrame) % Columns;
            totalFrames = Math.Abs(endframe - startframe + 1);
            Debug.WriteLine(""+column+","+row);
            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            sprBatch.End();
        }
        /// <summary>
        /// Like the draw function, but for tilesheets. 
        /// "tileId" indicates the position in the sheet of the desired tile.
        /// </summary>
        public void DrawTile(SpriteBatch sprBatch, int tileId, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = tileId / Columns;
            int column = tileId % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            //sprBatch.End();
        }
        /// <summary>
        /// An overload which only serves to flip the tile. The parameter does nothing otherwise.
        /// </summary>
        public void DrawTile(SpriteBatch sprBatch, int tileId, Vector2 location, bool flip)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = tileId / Columns;
            int column = tileId % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White,0,new Vector2(0,0),SpriteEffects.FlipHorizontally,0);
            //sprBatch.End();
        }
        public void DrawTile(SpriteBatch sprBatch, int tileId, int width, int height, Vector2 location)
        {
            int row = tileId / Columns;
            int column = tileId % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White);
            //sprBatch.End();
        }
        public void DrawTile(SpriteBatch sprBatch, int tileId, Color color, Vector2 location)
        {
            int width = Texture.Width / Columns;
            int height = Texture.Height / Rows;
            int row = tileId / Columns;
            int column = tileId % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            //sprBatch.Begin();
            sprBatch.Draw(Texture, destinationRectangle, sourceRectangle, color);
            //sprBatch.End();
        }
    }
}