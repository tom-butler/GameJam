using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using GameLibrary;

namespace GameJam1
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        //global
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        string state;
        SpriteList sprites;
        Texture2D background;

        //constants
        const int WINDOW_HEIGHT = 640;
        const int WINDOW_WIDTH = 800;
        static Vector2 WINDOW_CENTRE = new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            //initilise number of sprites
            sprites = new SpriteList(100);

            //set starting state should be menu (later)
            state = "playing";

            //set the window size
            graphics.IsFullScreen = false;
            graphics.PreferredBackBufferHeight = WINDOW_HEIGHT;
            graphics.PreferredBackBufferWidth = WINDOW_WIDTH;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the background texture
            background = this.Content.Load<Texture2D>(@"images/sand");
            //load the player
            //load textures here


        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null);

            #region draw
            //draw the background
            spriteBatch.Draw(background, new Vector2(0, 0), new Rectangle((int) WINDOW_WIDTH, (int) WINDOW_HEIGHT, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);

            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
