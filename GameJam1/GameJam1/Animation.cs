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
    }
}
