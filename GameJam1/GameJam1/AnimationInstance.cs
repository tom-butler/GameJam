using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace GameJam1
{
    class AnimationInstance
    {
        private int currentFrame = 0;
        private float secsElapsed = 0f;
        private bool backwards = false;
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

        private void GoNextFrame()
        {
            if (backwards)
            {
                if(currentFrame <= 0){
                    backwards = false;
                    currentFrame = 1;
                } 
                else
                {
                    currentFrame -= 1;
                }
            } else {
                if (currentFrame >= Animation.Frames.Length - 1)
                {
                    currentFrame = Animation.Frames.Length - 2;
                    backwards = true;
                }
                else
                {
                    currentFrame += 1;
                }
            }
        }
    }
}
