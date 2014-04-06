using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GameJam1
{
    class Player : Character
    {
        const float DEFAULT_SPEED = 10f;
        const float RAMPAGE_SPEED = 20f;
        const int RAMPAGE_TICKS = 5 * 60;

        private int rampageRemaining;
        private float speed;
        private float scale;

        public Player(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
            rampageRemaining = 0;
            speed = DEFAULT_SPEED;
            SetAnimationFPS(speed * 2f);
            scale = 1f;
        }

        public void Update(KeyboardState currentState)
        {
            if(rampageRemaining == 1)
                EndRampage();

            if(rampageRemaining > 0)
                --rampageRemaining;

            
            Vector2? directionVec = CalculateMovementDirection(currentState);
            if (directionVec.HasValue)
            {
                Vector2 displacement = speed * directionVec.Value;
                pos.X += displacement.X;
                pos.Y += displacement.Y;
                UpdateAnimation(displacement);
            }
            else
            {
                UpdateAnimation(null);
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch, float frankieboy = 1f)
        {
            base.Draw(spritebatch, scale);
        }

        private Vector2? CalculateMovementDirection(KeyboardState currentState)
        {
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

        public override void TakeDamage(float damage)
        {
            //do nothing. can't die
        }

        public void Rampage()
        {
            rampageRemaining = RAMPAGE_TICKS;
            speed = RAMPAGE_SPEED;
            SetAnimationFPS(speed * 2f);
            scale = 2f;
        }

        public bool IsRampaging()
        {
            return rampageRemaining > 0;
        }

        public float RampagePercentLeft()
        {
            return 100f * ((float)rampageRemaining / (float)RAMPAGE_TICKS);
        }

        private void EndRampage()
        {
            speed = DEFAULT_SPEED;
            SetAnimationFPS(speed * 2f);
            scale = 1f;
        }

        public override Vector2 GetCurrentSize()
        {
            return scale * base.GetCurrentSize();
        }
    }
}
