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
            : base(texture, "DaPlaya", pos)
        {
        }

        public override void Draw(SpriteBatch spritebatch)
        {
            spritebatch.Draw(texture, pos, null, Color.White, 0f, new Vector2(texture.Width / 2f, texture.Height / 2f), 1f, SpriteEffects.None, 0f);
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
