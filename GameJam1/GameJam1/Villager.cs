using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using GameLibrary;

namespace GameJam1
{
    class Villager : Character
    {
        int sightRadius = 600;
        float heading;
        const float VELOCITY = 2f;
        static Vector2 speed;

        public Villager(Texture2D texture, Vector2 position)
            : base(texture, position)
        {
           
        }

        /// <summary>
        /// Ai to have the villager run from player
        /// </summary>
        /// <param name="prevState"></param>
        /// <param name="currentState"></param>
        /// <param name="?"></param>
        public void Update(GameObject player)
        {
            if (IsDead())
                return;

            float distance = Util.PointDist(this.pos, player.pos);
            if (distance < sightRadius)
            {
                //make the villager run away
                float angle = Util.PointAngle(player.pos, this.pos);
                heading = angle;

                speed.X = (float)(VELOCITY * Math.Cos(Util.DegToRad(heading)));
                speed.Y = (float)(VELOCITY * Math.Sin(Util.DegToRad(heading)));

                this.pos = new Vector2(pos.X + speed.X, pos.Y + speed.Y);
                UpdateAnimation(speed);
                this.boundingBox.Update(pos, GetCurrentSize());
            } 
            else
            {
                UpdateAnimation(null);
            }

            base.Update();
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            if (IsDead())
            {
                //TODO: here
            }
            else
            {
                base.Draw(spritebatch);
            }
        }

        public override void Ignite()
        {
            Random r = new Random();
            Game1.Instance.playSound(r.Next() % 2 == 0 ? "scream1" : "scream2");
            base.Ignite();
        }
    }
}
