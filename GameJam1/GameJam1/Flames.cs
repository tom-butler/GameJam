using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace GameJam1
{
    class Flames : GameObject
    {
        private AnimationInstance animation;

        public Flames(Texture2D tex) : base(tex)
        {
            Animation a = Animation.BySplittingTexture(tex, 3, 4);
            animation = new AnimationInstance(a, 12);
        }

        public override void Update()
        {
            animation.Update(1f / 60f);
        }

        public override void Draw(SpriteBatch spritebatch, float scale = 1f)
        {
            spritebatch.Draw(texture, pos, animation.GetCurrentFrame(), Color.White, 0f, new Microsoft.Xna.Framework.Vector2(0, 0), scale, SpriteEffects.None, 0);
        }
    }
}
