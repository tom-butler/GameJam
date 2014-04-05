using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using GameLibrary;
using Microsoft.Xna.Framework.Graphics;


namespace GameJam1
{
    class BoundingBox
    {
        public Vector2 p1;
        public Vector2 p2;
        public Vector2 p3;
        public Vector2 p4;
        public Vector2 origin;
        public float heading;
        public int width;
        public int height;
        public Color colour;

        public BoundingBox(Vector2 pos, int spriteWidth, int spriteHeight, float scale)
        {
            this.origin = pos;
            this.heading = (float)Util.DegToRad(270);
            this.width = (int) (spriteWidth * scale);
            this.height = (int ) (spriteHeight * scale);
            this.colour = Color.Red;
        }
        public void Update(Vector2 position, Vector2 size)
        {
            height = (int)size.Y;
            width = (int)size.X;

            p1 = position;
            p2 = position; p2.X += width;
            p3 = position + new Vector2(width, height);
            p4 = position + new Vector2(0, height);

            //p1 = new Vector2(
            //        (float)(position.X * Math.Cos(heading) - (height / 2) * Math.Sin(heading)),
            //        (float)(position.Y * Math.Cos(heading) + (width / 2) * Math.Sin(heading))
            //        );
            //p2 = new Vector2(
            //        (float)(position.X * Math.Cos(heading) - (height / 2) * Math.Sin(heading)),
            //        (float)(position.Y * Math.Cos(heading) - (width / 2) * Math.Sin(heading))
            //        );
            //p3 = new Vector2(
            //        (float)(position.X * Math.Cos(heading) + (height / 2) * Math.Sin(heading)),
            //        (float)(position.Y * Math.Cos(heading) - (width / 2) * Math.Sin(heading))
            //        );
            //p4 = new Vector2(
            //        (float)(position.X * Math.Cos(heading) + (height / 2) * Math.Sin(heading)),
            //        (float)(position.Y * Math.Cos(heading) + (width / 2) * Math.Sin(heading))
            //        );

        }
        /// <summary>
        /// Draws the bounding box
        /// </summary>
        /// <param name="spritebatch"></param>
        /// <param name="tex"></param>
        public void Draw(SpriteBatch spritebatch, Texture2D tex)
        {
            Util.DrawLine(spritebatch, p1, p2, tex, colour);
            Util.DrawLine(spritebatch, p2, p3, tex, colour);
            Util.DrawLine(spritebatch, p3, p4, tex, colour);
            Util.DrawLine(spritebatch, p4, p1, tex, colour);
        }
        public Rectangle GetRect()
        {
            return new Rectangle((int)p1.X, (int)p1.Y, (int)(p2.X - p1.X), (int)(p4.Y - p1.Y));
        }

        public bool collides(GameObject o)
        {
            return GetRect().Intersects(o.boundingBox.GetRect());

            //bool coll = false;
            //Vector2[] poly = { p1, p2, p3, p4 };
            //coll = Util.insidePoly(o.boundingBox.p1, poly, 4);
            //if (coll)
            //    return coll;
            //coll = Util.insidePoly(o.boundingBox.p2, poly, 4);
            //if (coll)
            //    return coll;
            //coll = Util.insidePoly(o.boundingBox.p3, poly, 4);
            //if (coll)
            //    return coll;
            //coll = Util.insidePoly(o.boundingBox.p4, poly, 4);
            //if (coll)
            //    return coll;
            //else
            //    return coll;
        }

    }
}
