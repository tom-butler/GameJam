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
        int sightRadius = 500;
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
            } 
            else
            {
                UpdateAnimation(null);
            }
            this.boundingBox.Update(pos);
        
        }
    }
}
