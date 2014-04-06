using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameLibrary;

namespace GameJam1
{
    class TrailSpot
    {
        public Vector2 position;
        public int life;
        public float rotation;

        public TrailSpot(Vector2 pos, int startLife=100)
        {
            position = pos;
            life = startLife;
            rotation = 0f;
        }
    }

    class Player : Character
    {
        const float DEFAULT_SPEED = 10f;
        const float RAMPAGE_SPEED = 20f;
        const int RAMPAGE_TICKS = 5 * 60;
        const int TRAIL_SIZE = 50;

        private int rampageRemaining;
        private float speed;
        private float scale;
        private TrailSpot[] trail;
        private int trailStart;

        public Player(Texture2D tex, Vector2 pos)
            : base(tex, pos)
        {
            rampageRemaining = 0;
            speed = DEFAULT_SPEED;
            SetAnimationFPS(speed * 2f);
            scale = 1f;
            trail = new TrailSpot[TRAIL_SIZE];
            trailStart = 0;

            Random r = new Random();
            for (int i = 0; i < TRAIL_SIZE; ++i)
            {
                trail[i] = new TrailSpot(new Vector2(0,0), 0);
            }
        }

        public void Update(KeyboardState currentState)
        {
            if(rampageRemaining == 1)
                EndRampage();

            if(rampageRemaining > 0)
                --rampageRemaining;

            foreach (TrailSpot spot in trail)
            {
                spot.life--;
            }
            
            Vector2? directionVec = CalculateMovementDirection(currentState);
            if (directionVec.HasValue)
            {
                Vector2 displacement = speed * directionVec.Value;
                pos += displacement;
                UpdateAnimation(displacement);

                if (IsRampaging())
                {
                    Random r = new Random();
                    if (r.Next(2) == 1)
                        AddTrailPosition(pos);
                }
            }
            else
            {
                UpdateAnimation(null);
            }

            base.Update();
        }

        private void AddTrailPosition(Vector2 pos)
        {
            trailStart = (trailStart + 1) % TRAIL_SIZE;
            Vector2 offset = GetCurrentSize();
            offset.X /= 2f;
            trail[trailStart].life = 100;
            trail[trailStart].position = pos + offset;
        }

        public override void Draw(SpriteBatch spritebatch, float frankieboy = 1f)
        {
            Texture2D texture = Game1.Instance.GetTexture("trail");;

            for (int i = 0; i < TRAIL_SIZE; ++i)
            {
                int idx = (trailStart + 1 + i) % TRAIL_SIZE;
                if (trail[idx].life > 0)
                {
                    float trailScale = (float)trail[idx].life / 100f;
                    spritebatch.Draw(texture,
                        trail[idx].position,
                        null,
                        Color.White,
                        trail[idx].rotation,
                        new Vector2(texture.Width/2f, texture.Height),
                        trailScale * scale,
                        SpriteEffects.None,
                        0);
                }
            }

            base.Draw(spritebatch, scale);
        }

        private Vector2? CalculateMovementDirection(KeyboardState currentState)
        {
            if (!Game1.Instance.CanMovePlayer())
            {
                ChangeDirection(Direction.Down);
                return null;
            }

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

            Game1.Instance.playSound("rampage");
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

        public bool RampageFlash()
        {
            return IsRampaging() && ((rampageRemaining/10) % 2) == 0;
        }
    }
}
