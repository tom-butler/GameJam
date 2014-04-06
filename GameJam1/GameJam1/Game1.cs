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
    class Game1 : Microsoft.Xna.Framework.Game
    {
        //global
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<string, Texture2D> textureList;
        Dictionary<string, SoundEffect> sounds;
        Dictionary<string, GameObject> gameObjects;
        static int hits = 0;
        string state;
        static KeyboardState keystate;
        static bool isDebug;
        static SpriteFont debugFont;
        static SpriteFont guiFont;


        //constants
        const int WINDOW_HEIGHT = 640;
        const int WINDOW_WIDTH = 800;
        static Vector2 WINDOW_CENTRE = new Vector2(WINDOW_WIDTH / 2, WINDOW_HEIGHT / 2);

        private static Game1 instance;
        public static Game1 Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Game1();
                }
                return instance;
            }
        }

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
            //Files downloaded from:
            // - http://opengameart.org/content/lpc-flames

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the background texture
            textureList.Add("background", this.Content.Load<Texture2D>(@"images/sand"));
            textureList.Add("player", this.Content.Load<Texture2D>(@"images/player2"));
            textureList.Add("villager1", this.Content.Load<Texture2D>(@"images/villager1"));
            textureList.Add("villager2", this.Content.Load<Texture2D>(@"images/villager2"));
            textureList.Add("corpse", this.Content.Load<Texture2D>(@"images/burnt"));
            textureList.Add("flames", this.Content.Load<Texture2D>(@"images/flames"));
            textureList.Add("empty", new Texture2D(GraphicsDevice, 1, 1));
            textureList["empty"].SetData(new Color[] { Color.White });

            sounds = new Dictionary<string, SoundEffect>();
            sounds.Add("scream1", this.Content.Load<SoundEffect>(@"sounds/scream2"));
            sounds.Add("scream2", this.Content.Load<SoundEffect>(@"sounds/scream3"));

            //Load the font
            debugFont = Content.Load<SpriteFont>(@"fonts/debug");
            guiFont = Content.Load<SpriteFont>(@"fonts/gui");

            LevelGenerator generator = new LevelGenerator(textureList);
            foreach (GameObject obj in generator.generate())
            {
                gameObjects.Add(obj.name, obj);
            }

            gameObjects.Add("player", new Player(textureList["player"], WINDOW_CENTRE));
            gameObjects["player"].Ignite();
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

            // Allows the game to exit
            if (keystate.IsKeyDown(Keys.Escape))
                this.Exit();

            if (keystate.IsKeyDown(Keys.B) && !prevKeyState.IsKeyDown(Keys.B))
            {
                if (isDebug)
                    isDebug = false;
                else
                    isDebug = true;
            }

            foreach (var g in gameObjects)
            {

                if (g.Key.StartsWith("villager"))
                {
                    ((Villager)g.Value).Update(gameObjects["player"]);
                }
                else if (g.Key == "player")
                {
                    ((Player)g.Value).Update(keystate);
                }
                else
                {
                    g.Value.Update();
                }

            }

            foreach (var g in gameObjects)
            {
                if (g.Key != "player")
                {
                    if(gameObjects["player"].boundingBox.collides(g.Value) && !g.Value.isColliding)
                    {
                        g.Value.isColliding = true;
                        hits++;
                        g.Value.Ignite();
                    }
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            Vector2 playerPos = gameObjects["player"].pos;
            GraphicsDevice.Clear(Color.SandyBrown);
            Matrix transform = Matrix.CreateTranslation(-playerPos.X + WINDOW_CENTRE.X, -playerPos.Y + WINDOW_CENTRE.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, SamplerState.LinearWrap, null, null, null, transform);

            #region draw
            //draw the background
            Vector2 backgroundPos = Vector2.Transform(new Vector2(0, 0), Matrix.Invert(transform));
            spriteBatch.Draw(textureList["background"], backgroundPos, new Rectangle((int)playerPos.X, (int)playerPos.Y, WINDOW_WIDTH, WINDOW_HEIGHT), Color.White);

            //draw game objects
            foreach (var g in gameObjects)
            {
                g.Value.Draw(spriteBatch);
            }
            //draw bounding boxes
            if (isDebug)
            {
                foreach (var g in gameObjects)
                {
                        g.Value.DrawBB(spriteBatch, textureList["empty"]);
                        if (g.Value.isColliding == true)
                        {
                            spriteBatch.DrawString(debugFont, "collided", g.Value.pos, Color.Red);
                        }
                        spriteBatch.DrawString(debugFont, g.Value.pos.ToString(), g.Value.pos + new Vector2 ( 0, 10), Color.Red);
                }
            }
            //draw Gui
            spriteBatch.DrawString(guiFont, hits.ToString(), gameObjects["player"].pos + new Vector2(300, 270), Color.Red);
            

            #endregion

            spriteBatch.End();
            base.Draw(gameTime);
        }

        public Flames makeFlames()
        {
            return new Flames(textureList["flames"]);
        }

        public void playSound(string name)
        {
            sounds[name].Play();
        }
    }
}
