using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GameJam1
{
    class Player : GameObject
    {
        const float VELOCITY = 3f;

        public Player(Texture2D texture, Vector2 pos)
            : base(texture, "DaPlaya", pos, texture.Width /3, texture.Height /4)
        {
        }

        public void Draw(SpriteBatch spritebatch, Vector2? position = null)
        {
            if (position == null)
                position = pos;

            spritebatch.Draw(texture, position.Value, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);

        }
        public void Draw(SpriteBatch spritebatch, Vector2 frame, Vector2? position = null)
        {
            if (position == null)
                position = pos;

            spritebatch.Draw(texture, pos, new Rectangle((int)(spriteMap[(int)frame.X, (int)frame.Y].X),(int) (spriteMap[(int)frame.X, (int)frame.Y].Y), this.spriteWidth, this.spriteHeight), Color.White);

        }
        

        public override void Update(KeyboardState prevState, KeyboardState currentState)
        {
            if (currentState.IsKeyDown(Keys.Left))
                pos.X -= VELOCITY;
            else if(currentState.IsKeyDown(Keys.Right))
                pos.X += VELOCITY;

            if (currentState.IsKeyDown(Keys.Up))
                pos.Y -= VELOCITY;
            else if (currentState.IsKeyDown(Keys.Down))
                pos.Y += VELOCITY;
        }
    }
}
