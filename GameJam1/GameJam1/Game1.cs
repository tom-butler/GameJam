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
        Dictionary<string, Texture2D> textureList;
        Dictionary<string, GameObject> gameObjects;
        string state;
        static KeyboardState keystate;

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
            gameObjects = new Dictionary<string, GameObject>();
            
            // initialise the texture list;
            textureList = new Dictionary<string, Texture2D>();

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
            textureList.Add("background", this.Content.Load<Texture2D>(@"images/sand"));
            textureList.Add("player", this.Content.Load<Texture2D>(@"images/player"));

            gameObjects.Add("player", new Player(textureList["player"], WINDOW_CENTRE));


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
            KeyboardState prevKeyState = keystate;
            keystate = Keyboard.GetState();

            foreach (var g in gameObjects)
            {
                g.Value.Update(prevKeyState, keystate);
            }

            // Allows the game to exit
            if (keystate.IsKeyDown(Keys.Escape))
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
            var p = gameObjects["player"].pos;
            spriteBatch.Draw(textureList["background"], new Vector2(0, 0), new Rectangle((int)p.X, (int)p.Y, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);

            //draw game objects
            foreach (var g in gameObjects)
            {
                if (g.Key == "player")
                    ((Player)g.Value).Draw(spriteBatch,new Vector2(1,1), WINDOW_CENTRE);
                else
                    g.Value.Draw(spriteBatch);
            }
            

            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
