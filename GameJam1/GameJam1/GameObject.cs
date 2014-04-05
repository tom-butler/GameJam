using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using GameLibrary;


namespace GameJam1
{
    class GameObject
    {
        protected Texture2D texture;
        public BoundingBox boundingBox;
        public Vector2 pos;
        public String name;
        public bool isColliding = false;
        public Flames flames;

        public GameObject(Texture2D texture)
            : this(texture, new Vector2(0, 0), "anonymous")
        {
        }

        public GameObject(Texture2D texture, Vector2 pos) :
            this(texture, pos, "anonymous")
        {
        }

        public GameObject(Texture2D texture, Vector2 pos, String name)
        {
            this.name = name;
            this.texture = texture;
            this.pos = pos;
            boundingBox = new BoundingBox( pos, 1, 1, 1.0f);
        }

        public virtual void Update()
        {
            if (flames != null)
            {
                flames.pos = pos + new Vector2(-5, -40);
                flames.Update();
            }
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            //spritebatch.Draw(texture, pos, Color.White);
            if (flames != null)
                flames.Draw(spritebatch);
        }

        public void DrawBB(SpriteBatch spritebatch, Texture2D tex)
        {
            boundingBox.Draw(spritebatch, tex);
        }

        public void Ignite()
        {
            if (flames != null)
                return;

            flames = Game1.Instance.makeFlames();
        }
    }
}
