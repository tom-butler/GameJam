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
        enum State
        {
            Title,
            Running,
            Lose,
            Win
        }

        //global
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Dictionary<string, Texture2D> textureList;
        Dictionary<string, SoundEffect> sounds;
        Dictionary<string, GameObject> gameObjects;
        Song music;
        float timeRemaining;
        int hits;
        State state;
        static KeyboardState keystate;
        static bool isDebug;
        static SpriteFont debugFont;
        static SpriteFont guiFont;
        static Vector2 playerPos;
        static float points = 0;


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
            state = State.Title;

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
            textureList.Add("title_screen", this.Content.Load<Texture2D>(@"images/title_screen"));
            textureList.Add("win_screen", this.Content.Load<Texture2D>(@"images/win_screen"));
            textureList.Add("lose_screen", this.Content.Load<Texture2D>(@"images/lose_screen"));
            textureList.Add("bar", this.Content.Load<Texture2D>(@"images/bar"));
            textureList.Add("empty", new Texture2D(GraphicsDevice, 1, 1));
            textureList["empty"].SetData(new Color[] { Color.White });

            sounds = new Dictionary<string, SoundEffect>();
            sounds.Add("scream1", this.Content.Load<SoundEffect>(@"sounds/scream2"));
            sounds.Add("scream2", this.Content.Load<SoundEffect>(@"sounds/scream3"));

            music = this.Content.Load<Song>(@"sounds/DST-ClubFight");

            //Load the font
            debugFont = Content.Load<SpriteFont>(@"fonts/debug");
            guiFont = Content.Load<SpriteFont>(@"fonts/metal");
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
            if (state == State.Running)
                UpdateGameScreen();
            else
                RunGameOnSpacePressed();
            

            base.Update(gameTime);
        }

        private void RunGameOnSpacePressed()
        {
            MediaPlayer.Stop();

            if (Keyboard.GetState().IsKeyDown(Keys.Space) || Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                points = 0;
                timeRemaining = 30f;
                state = State.Running;
                MediaPlayer.Play(music);
                hits = 0;

                gameObjects.Clear();
                LevelGenerator generator = new LevelGenerator(textureList);
                foreach (GameObject obj in generator.generate())
                {
                    gameObjects.Add(obj.name, obj);
                    hits++;
                }
                gameObjects.Add("player", new Player(textureList["player"], WINDOW_CENTRE));
            }
        }

        private void UpdateGameScreen()
        {
            KeyboardState prevKeyState = keystate;
            keystate = Keyboard.GetState();
            Player player = (Player)gameObjects["player"];


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
                    if (player.boundingBox.collides(g.Value) && !g.Value.isColliding)
                    {
                        g.Value.isColliding = true;

                        if (!player.IsRampaging())
                        {
                            points += 8;
                            if (points > 100)
                            {
                                player.Rampage();
                                points = 0;
                            }
                        }

                        hits--;
                        if (hits == 0)
                            state = State.Win;
                        g.Value.Ignite();
                    }
                }
            }

            timeRemaining -= 1f / 60f;
            if (timeRemaining <= 0f)
            {
                state = State.Lose;
                MediaPlayer.Stop();
            }
            playerPos = gameObjects["player"].pos;
            if (points >= 0)
                points -= 0.1f;
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.SandyBrown);

            if (state == State.Running)
            {
                DrawGameScreen();
            }
            else
            {
                Texture2D screen = null;
                switch (state)
                {
                    case State.Title:
                        screen = textureList["title_screen"];
                        break;
                    case State.Lose:
                        screen = textureList["lose_screen"];
                        break;
                    case State.Win:
                        screen = textureList["win_screen"];
                        break;
                    default:
                        throw new Exception("Unhandled screen");
                }

                spriteBatch.Begin();
                spriteBatch.Draw(screen, new Vector2(0, 0), Color.White);
                spriteBatch.End();
            }
            
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

        private void DrawGameScreen()
        {
            Player player = (Player)gameObjects["player"];
            Vector2 playerPos = player.pos;
            Vector2 playerSize = player.GetCurrentSize();
            Vector2 cameraCenter = new Vector2(playerPos.X + playerSize.X / 2f, playerPos.Y + playerSize.Y/2f);
           
            Matrix transform = Matrix.CreateTranslation(WINDOW_CENTRE.X - cameraCenter.X, WINDOW_CENTRE.Y - cameraCenter.Y, 0);
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
                    spriteBatch.DrawString(debugFont, g.Value.pos.ToString(), g.Value.pos + new Vector2(0, 10), Color.Red);
                }
            }
            //draw Gui
            int lineHeight = 100;
            Util.DrawLine(spriteBatch,
                new Vector2(cameraCenter.X - WINDOW_WIDTH / 2f - 5, cameraCenter.Y + WINDOW_HEIGHT / 2f - lineHeight / 2f),
                new Vector2(cameraCenter.X - WINDOW_WIDTH / 2f + 110, cameraCenter.Y + WINDOW_HEIGHT / 2f - lineHeight / 2f),
                textureList["empty"], Color.Black, lineHeight);
            Util.DrawLine(spriteBatch,
                new Vector2(cameraCenter.X + WINDOW_WIDTH / 2f - 185, cameraCenter.Y + WINDOW_HEIGHT / 2f - lineHeight / 2f),
                new Vector2(cameraCenter.X + WINDOW_WIDTH / 2f + 5, cameraCenter.Y + WINDOW_HEIGHT / 2f - lineHeight / 2f),
                textureList["empty"], Color.Black, lineHeight);

            //draw rectangle
            float barWidth = points;
            if (player.IsRampaging())
                barWidth = player.RampagePercentLeft();

            if (barWidth > 0)
                Util.DrawLine(spriteBatch, new Vector2(cameraCenter.X - 198, cameraCenter.Y + 293), new Vector2(cameraCenter.X + (barWidth * 4 - 200), cameraCenter.Y + 293), textureList["empty"], Color.Red, 15);
            spriteBatch.Draw(textureList["bar"], new Vector2(cameraCenter.X - 200, cameraCenter.Y + 90), null, Color.White, 0, new Vector2(0, 0), 2.1f, SpriteEffects.None, 0);

           

            spriteBatch.DrawString(guiFont, "Villagers: " + hits.ToString(), cameraCenter + new Vector2(230, 280), Color.Red);
            spriteBatch.DrawString(guiFont, "Time: " + ((int)timeRemaining).ToString() + " secs", cameraCenter + new Vector2(-390, 280), Color.Red);

            DrawVillagerArrows(spriteBatch, cameraCenter);

            #endregion

            spriteBatch.End();
        }

        private void DrawVillagerArrows(SpriteBatch spriteBatch, Vector2 center)
        {
            foreach (var objPair in gameObjects)
            {
                if (objPair.Value.IsAlive() && !objPair.Value.IsOnFire() && objPair.Key != "player")
                {
                    Vector2 direction = Vector2.Normalize(objPair.Value.pos - playerPos);
                    Vector2 start = 220f * direction + center;
                    Vector2 end = 5f * direction + start;
                    Util.DrawLine(spriteBatch, start, end, textureList["empty"], new Color(128, 90, 90), 3);
                }
            }
        }
    }
}
