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
        enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        const float VELOCITY = 3f;

        private Dictionary<Direction, AnimationInstance> animations;
        private Direction direction;
        private bool movedThisUpdate = false;

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

        public void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, null, Color.White, 0f, new Vector2(0,0), 1f, SpriteEffects.None, 0f);
        }

        public void Draw(SpriteBatch spritebatch, Vector2 frame, Vector2? position = null)
        {
            if (position == null)
                position = pos;

            spritebatch.Draw(texture, pos, animations[direction].GetCurrentFrame(), Color.White, 0f, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);
            //spritebatch.Draw(texture, pos, new Rectangle((int)(spriteMap[(int)frame.X, (int)frame.Y].X),(int) (spriteMap[(int)frame.X, (int)frame.Y].Y), this.spriteWidth, this.spriteHeight), Color.White);

        }
        

        public override void Update(KeyboardState prevState, KeyboardState currentState)
        {
            Vector2? directionVec = CalculateMovementDirection(prevState, currentState);
            if (directionVec.HasValue)
            {
                Vector2 displacement = VELOCITY * directionVec.Value;
                pos.X += displacement.X;
                pos.Y += displacement.Y;
                UpdateAnimation(displacement);
                this.boundingBox.Update(pos);
            }
            else
            {
                UpdateAnimation(null);
            }
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

        private Vector2? CalculateMovementDirection(KeyboardState prevState, KeyboardState currentState)
        {
            if (name != "DaPlaya")
                return null;

            Vector2 directionVec = new Vector2(0, 0);
            if (currentState.IsKeyDown(Keys.Left))
            {
                directionVec.X -= 1f;
            }
            else if (currentState.IsKeyDown(Keys.Right))
            {
                directionVec.X += 1f;
            }

            if (currentState.IsKeyDown(Keys.Up))
            {
                directionVec.Y -= 1f;
            }
            else if (currentState.IsKeyDown(Keys.Down))
            {
                directionVec.Y += 1f;
            }

            if (directionVec.Length() > 0)
                return Vector2.Normalize(directionVec);
            else
                return null;
        }

        private AnimationInstance MakeAnimationFromRowOfTexture(Texture2D texture, int row)
        {
            const int FRAMES_PER_ROW = 3;

            int width = texture.Width / FRAMES_PER_ROW;
            int height = texture.Height / 4;
            Rectangle[] frames = new Rectangle[FRAMES_PER_ROW];
            for (int col = 0; col < FRAMES_PER_ROW; ++col)
            {
                frames[col] = new Rectangle(col*width, height*row, width, height);
                frames[col].Inflate(-1, -1);
            }

            return new AnimationInstance(new Animation(texture, frames), 4f);
        }

        private void ChangeDirection(Direction dir)
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
    }
}
