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
        Texture2D corpse;
        int angle;
        int deathtick = 0;
        int step = 1;

        public Villager(Texture2D texture, Texture2D corpse, Vector2 position)
            : base(texture, position)
        {
            this.corpse = corpse;
            Random rand = new Random();
            this.angle = rand.Next(0, 360);
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

        public override void Draw(SpriteBatch spritebatch, float scale = 1f)
        {
            if (deathtick > (4 * 60))
                return;
            else if (IsDead())
            {
                if (deathtick % 30 == 0)
                    step = -step;

                if(step > 0)
                    spritebatch.Draw(corpse, pos, new Rectangle((texture.Width / 3), 0, texture.Width / 3, texture.Height / 4), Color.White, (float) Util.DegToRad((float) 0), new Vector2(0,0),1f, SpriteEffects.FlipHorizontally,0);
                if(step < 0)
                    spritebatch.Draw(corpse, pos, new Rectangle((texture.Width / 3), 0, texture.Width / 3, texture.Height / 4), Color.White, (float)Util.DegToRad((float)0), new Vector2(0, 0), 1f, SpriteEffects.None, 0);
                deathtick++;
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
