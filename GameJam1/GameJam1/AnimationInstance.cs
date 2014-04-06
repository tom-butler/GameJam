using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam1
{
    class AnimationInstance
    {
        private int currentFrame = 0;
        private float secsElapsed = 0f;
        private bool running = true;

        public Animation Animation { get; set; }
        public float SecsPerFrame { get; set; }

        public AnimationInstance(Animation anim, float fps)
        {
            Animation = anim;
            SecsPerFrame = 1f / fps;
            Reset();
        }

        public void Start()
        {
            running = true;
        }

        public void Stop()
        {
            running = false;
        }

        public void Update(float secsElapsedArg)
        {
            if (!running)
                return;

            secsElapsed += secsElapsedArg;
            while (secsElapsed > SecsPerFrame)
            {
                secsElapsed -= SecsPerFrame;
                GoNextFrame();
            }
        }

        public Rectangle GetCurrentFrame()
        {
            return Animation.Frames[currentFrame];
        }

        public void Reset(int? frame = null)
        {
            currentFrame = frame.HasValue ? frame.Value : 0;
            secsElapsed = 0f;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position, float scale = 1f)
        {
            spriteBatch.Draw(Animation.Texture, position, GetCurrentFrame(), Color.White, 0f, new Vector2(0, 0), scale, SpriteEffects.None, 0);
        }

        private void GoNextFrame()
        {
            if (currentFrame >= Animation.Frames.Length - 1)
            {
                currentFrame = 0;
            } 
            else
            {
                currentFrame += 1;
            }
        }
    }
}
