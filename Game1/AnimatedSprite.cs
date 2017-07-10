using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Game1
{
    public class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int tick = 0;

        public AnimatedSprite(Texture2D texture, int rows, int columns)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
        }

        public void Update()
        {
            if (tick > 4) tick = 0;
            currentFrame += (tick==4? 1:0);
            if (currentFrame == totalFrames)
                currentFrame = 0;
            tick++;
        }

        public void Draw(SpriteBatch sprBatch, Vector2 location)
        {
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