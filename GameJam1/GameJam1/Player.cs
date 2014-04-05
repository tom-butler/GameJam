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
        const float VELOCITY = 10f;

        public Player(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
        }

        public void Update(KeyboardState currentState)
        {
            Vector2? directionVec = CalculateMovementDirection(currentState);
            if (directionVec.HasValue)
            {
                Vector2 displacement = VELOCITY * directionVec.Value;
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

        protected override float AnimationFPS()
        {
            return VELOCITY * 2f;
        }

        public override void TakeDamage(float damage)
        {
            //do nothing. can't die
        }
    }
}
