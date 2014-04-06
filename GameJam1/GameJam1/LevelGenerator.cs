using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace GameJam1
{
    class LevelGenerator
    {
        const float RADIUS = 1000f;
        public const int NPC_COUNT = 1;

        private Dictionary<string, Texture2D> textures;
        private Random random = new Random();
        private int counter = 1;

        public LevelGenerator(Dictionary<string, Texture2D> textures)
        {
            this.textures = textures;
        }

        public List<GameObject> generate()
        {
            List<GameObject> gameObjects = new List<GameObject>();
            for (int i = 0; i < NPC_COUNT; ++i)
            {
                gameObjects.Add(randomNPC());

            }
            return gameObjects;
        }

        private GameObject randomNPC()
        {
            string[] npcTextures = {"villager1", "villager2"};
            string textureName = npcTextures[random.Next(npcTextures.Length)];
            string objectName = "villager" + nextCounter();
            Vector2 position = new Vector2((float)random.Next((int)-RADIUS, (int)RADIUS), (float)random.Next((int)-RADIUS, (int)RADIUS));

            Character p =  new Villager(textures[textureName], textures["corpse"], position);
            p.name = objectName;
            return p;
        }

        private string nextCounter()
        {
            ++counter;
            return counter.ToString();
        }
    }
}
