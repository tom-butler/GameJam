using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam1
{
    class Animation
    {
        public Texture2D Texture { get; set; }
        public Rectangle[] Frames { get; set; }

        public Animation(Texture2D tex, Rectangle[] frames)
        {
            Texture = tex;
            Frames = frames;
        }

        public static Animation BySplittingTexture(Texture2D tex, int rows, int cols)
        {
            int width = tex.Width / cols;
            int height = tex.Height / rows;

            Rectangle[] frames = new Rectangle[rows * cols];
            for (int r = 0; r < rows; ++r)
            {
                for (int c = 0; c < cols; ++c)
                {
                    int i = r * cols + c;
                    frames[i] = new Rectangle(c * width, r * height, width, height);
                    frames[i].Inflate(-1, -1);
                }
            }
            return new Animation(tex, frames);
        }
    }
}
