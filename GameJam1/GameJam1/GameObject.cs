using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace GameJam1
{
    class GameObject
    {
        public String name;
        public Vector2 pos;
        protected Texture2D texture;

        public GameObject(Texture2D texture, String name)
        {
            init(texture, name, 0, 0);
        }

        public GameObject(Texture2D texture, String name, float posx, float posy)
        {
            init(texture, name, posx, posy);
        }

        public GameObject(Texture2D texture, String name, Vector2 pos)
        {
            init(texture, name, pos.X, pos.Y);
        }

        private void init(Texture2D texture, String name, float posx, float posy)
        {
            this.name = name;
            this.texture = texture;
            this.pos = new Vector2(posx, posy);
        }

        public virtual void Update(KeyboardState prevState, KeyboardState currentState)
        {
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.White);
        }
    }
}
