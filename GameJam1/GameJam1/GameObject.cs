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

        public virtual void Update(KeyboardState prevState, KeyboardState currentState)
        {
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.White);
        }

        public void DrawBB(SpriteBatch spritebatch, Texture2D tex)
        {
            boundingBox.Draw(spritebatch, tex);
        }
    }
}
