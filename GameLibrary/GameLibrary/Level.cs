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
//using Microsoft.Xna.Framework.Net;
//using Microsoft.Xna.Framework.Storage;


namespace GameLibrary
{
    /// <summary>
    /// Parent level class all levels inherit from this
    /// </summary>
    public abstract class UC_LevelParent
    {
        public GraphicsDevice gd;
        public SpriteBatch spriteBatch;
        
        public ContentManager Content;
        public UC_LevelManager levelManager;

        public KeyboardState keyState; // for convenience - not really needed
        public KeyboardState prevKeyState; // for convenience not really needed

        public float mouse_x = 0;
        public float mouse_y = 0; // for convenience not really needed
        public MouseState currentMouseState; // for convenience not really needed
        public MouseState previousMouseState; // for convenience not really needed

        public SpriteFont font1; // use if you want again not really needed

        public virtual void InitializeLevel(GraphicsDevice g, SpriteBatch s, ContentManager c, UC_LevelManager lm)
        {
            gd = g;
            spriteBatch = s;
            Content = c;
            levelManager = lm;
        }

        public virtual void LoadContent() { }
        public virtual void UnloadContent() { }
        public virtual void EnterLevel(int fromLevelNum) { } // runs on set and push
        public virtual void ExitLevel() { } // runs on set and pop
        public virtual void Update(GameTime gameTime) { }
        public abstract void Draw(GameTime gameTime);

        /// <summary>
        /// Utility routine to set up keyboard and mouse
        /// </summary>
        public void getKeyboardAndMouse()
        {

            prevKeyState = keyState;
            keyState = Keyboard.GetState();

            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            mouse_x = currentMouseState.X;
            mouse_y = currentMouseState.Y;
        }

    }

    /// <summary>
    /// To manage levels  
    /// </summary>
    public class UC_LevelManager
    {
        UC_LevelParent[] levels;
        UC_LevelParent cur; //current_Level;        
        UC_LevelParent prevLevel; //previous level;
        int curLevNum;

        int[] levelStack;
        int sp; // stack pointer

        public UC_LevelManager()
        {
            init(100);

        }

        private void init(int numLevelz)
        {
            levels = new UC_LevelParent[numLevelz];
            for (int i = 0; i < numLevelz; i++) levels[i] = null;
            levelStack = new int[30];
            sp = 0;
            setEmptyLevel();
        }

        public void AddLevel(int levNum, UC_LevelParent lev)
        {
            levels[levNum] = lev;
        }

        public UC_LevelParent getLevel(int levNum)
        {
            return levels[levNum];
        }

        public void setLevel(int levNum)
        {
            prevLevel = cur;
            levels[levNum].EnterLevel(curLevNum);
            cur = levels[levNum];
            prevLevel.ExitLevel();
            curLevNum = levNum;

            cur.prevKeyState = Keyboard.GetState();
            cur.keyState = Keyboard.GetState(); // fix legacy keystate issues
            cur.previousMouseState = Mouse.GetState();
            cur.currentMouseState = Mouse.GetState();
        }

        public void pushLevel(int levNum)
        {
            prevLevel = cur;
            levels[levNum].EnterLevel(curLevNum);
            levelStack[sp] = curLevNum;
            cur = levels[levNum];
            curLevNum = levNum;
            sp++;
        }

        public int popLevel()
        {
            sp--;
            prevLevel = cur;
            cur = levels[levelStack[sp]];
            curLevNum = levelStack[sp];
            prevLevel.ExitLevel();

            cur.prevKeyState = Keyboard.GetState();
            cur.keyState = Keyboard.GetState(); // fix legacy keystate issues
            cur.previousMouseState = Mouse.GetState();
            cur.currentMouseState = Mouse.GetState();
            return curLevNum;
        }

        public void setEmptyLevel()
        {
            cur = new EmptyLevel();
            curLevNum = -1; // hmm i guess empty level is now level -1
        }

        public UC_LevelParent getCurrentLevel()
        {
            return cur;
        }

        public int getCurrentLevelNum()
        {
            return curLevNum;
        }
    }

    /// <summary>
    /// A default 'empty' level to fix probelms of having nothing to Draw or Update
    /// </summary>
    class EmptyLevel : UC_LevelParent
    {
        public override void Draw(GameTime gameTime)
        {
        }
    }
}