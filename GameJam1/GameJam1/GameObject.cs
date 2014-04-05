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
        protected Vector2[,] spriteMap;
        protected int spriteHeight;
        proteccted int spriteWidth;

        public GameObject(Texture2D texture, String name)
        {
            init(texture, name, 0, 0, texture.Height, texture.Width);
        }

        public GameObject(Texture2D texture, String name, float posx, float posy)
        {
            init(texture, name, posx, posy, texture.Height, texture.Width);
        }

        public GameObject(Texture2D texture, String name, Vector2 pos)
        {
            init(texture, name, pos.X, pos.Y, texture.Height, texture.Width);
        }

        public GameObject(Texture2D texture, String name, Vector2 pos, int spriteHeight, int spriteWidth)
        {
            init(texture, name, pos.X, pos.Y, spriteHeight, spriteWidth);
        }

        public GameObject(Texture2D texture, String name, Vector2 pos, int spriteHeight, int spriteWidth, int spriteRows, int spriteColumns)
        {
            init(texture, name, pos.X, pos.Y, spriteHeight, spriteWidth, spriteRows, spriteColumns);
        }

        private void init(Texture2D texture, String name, float posx, float posy, int spriteHeight, int spriteWidth, int spriteRows = 4, int spriteColumns = 3)
        {
            this.name = name;
            this.texture = texture;
            this.pos = new Vector2(posx, posy);
            this.spriteHeight = spriteHeight;
            this.spriteWidth = spriteWidth;

            //create the spritemap
            this.spriteMap = new Vector2[spriteRows, spriteColumns];
            
            int height = 0;
            for (int r = 0; r < spriteRows; ++r)
            {
                int width = 0;
                for (int c = 0; c < spriteColumns; ++c)
                {
                    spriteMap[r, c] = new Vector2(width, height);
                    width += spriteWidth;
                }
                height += spriteHeight;

            }
        }


        public virtual void Update(KeyboardState prevState, KeyboardState currentState)
        {
        }

        public virtual void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, Color.White);
        }
        public virtual void Draw(SpriteBatch spritebatch, Vector2 frame)
        {
            spritebatch.Draw(texture, pos, new Rectangle((int) frame.X, (int) frame.Y, spriteWidth, spriteHeight), Color.White);
        }
    }
}
