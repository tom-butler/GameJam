using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam1
{
    class Character : GameObject
    {
        protected enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private Dictionary<Direction, AnimationInstance> animations;
        private Direction direction;

        public Character(Texture2D texture, Vector2 pos)
            : base(texture, pos, "DaPlaya")
        {
            animations = new Dictionary<Direction, AnimationInstance>();
            animations[Direction.Up] = MakeAnimationFromRowOfTexture(texture, 0);
            animations[Direction.Right] = MakeAnimationFromRowOfTexture(texture, 1);
            animations[Direction.Down] = MakeAnimationFromRowOfTexture(texture, 2);
            animations[Direction.Left] = MakeAnimationFromRowOfTexture(texture, 3);

            ChangeDirection(Direction.Down);
        }

        public override void Draw(SpriteBatch spritebatch, float scale = 1f)
        {
            animations[direction].Draw(spritebatch, pos, scale);
            base.Draw(spritebatch);
        }
        

        public override void Update()
        {
            this.boundingBox.Update(pos, GetCurrentSize());

            base.Update();
        }

        protected void UpdateAnimation(Vector2? displacement)
        {
            animations[direction].Update(1f / 60f);

            if (displacement.HasValue)
            {
                SetDirectionFromDisplacement(displacement.Value);
                animations[direction].Start();
            }
            else
            {
                animations[direction].Reset(1);
                animations[direction].Stop();
            }
        }

        private AnimationInstance MakeAnimationFromRowOfTexture(Texture2D texture, int row)
        {
            const int FRAMES_PER_ROW = 3;

            int width = texture.Width / FRAMES_PER_ROW;
            int height = texture.Height / 4;
            Rectangle[] frames = new Rectangle[FRAMES_PER_ROW + 1];
            for (int col = 0; col < FRAMES_PER_ROW; ++col)
            {
                frames[col] = new Rectangle(col*width, height*row, width, height);
                frames[col].Inflate(-1, -1);
            }
            frames[FRAMES_PER_ROW] = frames[1]; //repeating pattern
            

            return new AnimationInstance(new Animation(texture, frames), 4f);
        }

        protected void ChangeDirection(Direction dir)
        {
            if (direction != dir)
            {
                direction = dir;
                animations[direction].Reset();
            }
        }

        protected void SetDirectionFromDisplacement(Vector2 displacement)
        {
            if (Math.Abs(displacement.X) > Math.Abs(displacement.Y))
            {
                if (displacement.X >= 0)
                {
                    ChangeDirection(Direction.Right);
                }
                else
                {
                    ChangeDirection(Direction.Left);
                }
            }
            else
            {
                if (displacement.Y >= 0)
                {
                    ChangeDirection(Direction.Down);
                }
                else
                {
                    ChangeDirection(Direction.Up);
                }
            }
        }

        public virtual Vector2 GetCurrentSize()
        {
            Rectangle r = animations[direction].GetCurrentFrame();
            return new Vector2(r.Width, r.Height);
        }

        protected void SetAnimationFPS(float fps)
        {
            foreach(var elem in animations){
                elem.Value.SecsPerFrame = 1.0f / fps;
            }
        }
    }
}
