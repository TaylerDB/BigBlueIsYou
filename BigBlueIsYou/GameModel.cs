using BigBlueIsYou.Views;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Components;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;

namespace BigBlueIsYou
{
    class GameModel
    {
        private const int GRID_SIZE = 20;   // Size of game

        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;

        private List<Entity> m_removeThese = new List<Entity>();
        private List<Entity> m_addThese = new List<Entity>();

        private Systems.Renderer m_sysRenderer;
        private Systems.Collision m_sysCollision;
        private Systems.Movement m_sysMovement;
        private Systems.KeyboardInput m_sysKeyboardInput;
        private Systems.Rules m_rules;

        int row;
        int col;

        char[,] charTopArr;
        char[,] charBottomArr;

        Texture2D rockObject;

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch, int levelChoice, string levelsString)
        {
            var texSquare = content.Load<Texture2D>("Images/square");
            var bigBlueSquare = content.Load<Texture2D>("Images/BigBlue");
            var flagObject = content.Load<Texture2D>("Animations/flag");
            var floorObject = content.Load<Texture2D>("Animations/floor");
            var grassObject = content.Load<Texture2D>("Animations/grass");
            var hedgeObject = content.Load<Texture2D>("Animations/hedge");
            var lavaObject = content.Load<Texture2D>("Animations/lava");
            rockObject = content.Load<Texture2D>("Animations/rock");
            var wallObject = content.Load<Texture2D>("Animations/wall");
            var waterObject = content.Load<Texture2D>("Animations/water");

            var bigBlueWord = content.Load<Texture2D>("Animations/word-baba");
            var flagWord = content.Load<Texture2D>("Animations/word-flag");
            var isWord = content.Load<Texture2D>("Animations/word-is");
            var killWord = content.Load<Texture2D>("Animations/word-kill");
            var lavaWord = content.Load<Texture2D>("Animations/word-lava");
            var pushWord = content.Load<Texture2D>("Animations/word-push");
            var rockWord = content.Load<Texture2D>("Animations/word-rock");
            var sinkWord = content.Load<Texture2D>("Animations/word-sink");
            var stopWord = content.Load<Texture2D>("Animations/word-stop");
            var wallWord = content.Load<Texture2D>("Animations/word-wall");
            var winWord = content.Load<Texture2D>("Animations/word-win");
            var waterWord = content.Load<Texture2D>("Animations/word-water");
            var youWord = content.Load<Texture2D>("Animations/word-you");

            string[] lines = levelsString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

            string levelDimensionStringX;
            string levelDimensionStringY;
            string[] actualLevel; // = new string[,];
            string[] actualLevel2;

            string topLevel = "";
            string bottomLevel = "";

            int count = 0;
            int it = 0;
            for (int i = 0; i < lines.Length; i++)
            {
                //Debug.WriteLine("lines size " + lines.Length);
                string selectedLevelString = lines[i];  // or safe: lines.ElementAtOrDefault(3)  

                selectedLevelString = lines[i + 1];
                // Get first level dimension
                int d1To = selectedLevelString.IndexOf(" ");
                levelDimensionStringX = selectedLevelString.Substring(0, d1To - 0);
                //Debug.WriteLine("level dimension 1: " + levelDimensionString1);

                // Get size of level 1 second dimension
                int d2From = selectedLevelString.IndexOf("x ") + "x ".Length;
                int d2To = selectedLevelString.Length; //IndexOf("\r\n ");
                levelDimensionStringY = selectedLevelString.Substring(d2From, d2To - d2From);
                //Debug.WriteLine("level dimension 2: " + levelDimensionString2);

                col = Int32.Parse(levelDimensionStringY);
                row = Int32.Parse(levelDimensionStringX);

                if (count == levelChoice)
                {
                    it = i;
                    break;
                }
                else
                {
                    count++;
                }

                i += (col * 2) + 1;
            }

            actualLevel = new string[row * 2];
            actualLevel2 = new string[row];

            for (int j = 0; j < row * 2; j++)
            {
                actualLevel[j] = lines[it + 2];
                it++;
            }

            for (int k = 0; k < row; k++)
            {
                if ((it + 2) < lines.Length)
                {
                    actualLevel2[k] = lines[it + 2];
                    it++;
                }
            }

            for (int j = 0; j < row * 2; j++)
            {
                Debug.WriteLine(actualLevel[j]);
                
            }

            GameLayout.GamePos = new Entity[row, col];

            // Put topLevel in a 2d char array charTopArr
            char[][] charTopArr = actualLevel.Select(item => item.ToArray()).ToArray();

            m_sysRenderer = new Systems.Renderer(spriteBatch, texSquare, WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);
            m_sysCollision = new Systems.Collision((entity) =>
            {
                // Remove the existing food pill
                m_removeThese.Add(entity);
            });

            m_sysMovement = new Systems.Movement();
            m_sysKeyboardInput = new Systems.KeyboardInput();
            m_rules = new Systems.Rules();

            m_sysMovement.LoadContent(content);

            // Add objects to level
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < (col * 2); c++)
                {
                    if (charTopArr[c][r] == 'B')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(bigBlueWord, r, x);
                        }
                        else
                            initializeText(bigBlueWord, r, c);
                    }

                    if (charTopArr[c][r] == 'g')
                    {
                        if (c > col)
                        {
                            int x = c - row;

                            initializeGrass(grassObject, r, x);
                        }
                        else
                            initializeGrass(grassObject, r, c);
                    }

                    if (charTopArr[c][r] == 'h')
                    {
                        if (c > col)
                        {
                            int x = c - row;

                            initializeHedge(hedgeObject, r, x);
                        }
                        else
                            initializeHedge(hedgeObject, r, c);
                    }

                    if (charTopArr[c][r] == 'l')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeFloor(floorObject, r, x);
                        }
                        else
                            initializeFloor(floorObject, r, c);
                    }
                    
                    if (charTopArr[c][r] == 'a')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeWater(waterObject, r, x);
                        }
                        else
                            initializeWater(waterObject, r, c);
                    }

                    if (charTopArr[c][r] == 'b')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeBigBlue(bigBlueSquare, r, x);
                        }
                        else
                            initializeBigBlue(bigBlueSquare, r, c);
                    }

                    if (charTopArr[c][r] == 'f')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeFlag(flagObject, r, x);
                        }
                        else
                            initializeFlag(flagObject, r, c);
                    }

                    if (charTopArr[c][r] == 'r')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeRock(rockObject, r, x);
                        }
                        else
                            initializeRock(rockObject, r, c);
                    }

                    if (charTopArr[c][r] == 'v')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeLava(lavaObject, r, x);
                        }
                        else
                            initializeLava(lavaObject, r, c);
                    }

                    if (charTopArr[c][r] == 'w')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeWall(wallObject, r, x);
                        }
                        else
                            initializeWall(wallObject, r, c);
                    }
                    if (charTopArr[c][r] == 'W')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(wallWord, r, x);
                        }
                        else
                            initializeText(wallWord, r, c);
                    }
                    if (charTopArr[c][r] == 'R')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(rockWord, r, x);
                        }
                        else
                            initializeText(rockWord, r, c);
                    }

                    if (charTopArr[c][r] == 'F')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(flagWord, r, x);
                        }
                        else
                            initializeText(flagWord, r, c);
                    }
                    
                    if (charTopArr[c][r] == 'I')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(isWord, r, x);
                        }
                        else
                            initializeText(isWord, r, c);
                    }

                    if (charTopArr[c][r] == 'S')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(stopWord, r, x);
                        }
                        else
                            initializeText(stopWord, r, c);
                    }
                    if (charTopArr[c][r] == 'P')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(pushWord, r, x);
                        }
                        else
                            initializeText(pushWord, r, c);
                    }

                    if (charTopArr[c][r] == 'V')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(lavaWord, r, x);
                        }
                        else
                            initializeText(lavaWord, r, c);
                    }

                    if (charTopArr[c][r] == 'A')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(waterWord, r, x);
                        }
                        else
                            initializeText(waterWord, r, c);
                    }

                    if (charTopArr[c][r] == 'Y')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(youWord, r, x);
                        }
                        else
                            initializeText(youWord, r, c);
                    }

                    if (charTopArr[c][r] == 'X')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(winWord, r, x);
                        }
                        else
                            initializeText(winWord, r, c);
                    }

                    if (charTopArr[c][r] == 'N')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(sinkWord, r, x);
                        }
                        else
                            initializeText(sinkWord, r, c);
                    }

                    if (charTopArr[c][r] == 'K')
                    {
                        if (c > col)
                        {
                            int x = c - col;

                            initializeText(killWord, r, x);
                        }
                        else
                            initializeText(killWord, r, c);
                    }
                }
            }
        }

        public void Update(GameTime gameTime)
        {
            m_sysKeyboardInput.Update(gameTime);
            m_sysMovement.Update(gameTime);
            m_sysCollision.Update(gameTime);
            m_rules.Update(gameTime);


            foreach (var entity in m_removeThese)
            {
                RemoveEntity(entity);
            }
            m_removeThese.Clear();

            foreach (var entity in m_addThese)
            {
                AddEntity(entity);
                //rules(gameTime, entity);
            }
            m_addThese.Clear();

        }

        public void Draw(GameTime gameTime)
        {
            m_sysRenderer.Update(gameTime);
        }

        private void AddEntity(Entity entity)
        {
            m_sysKeyboardInput.Add(entity);
            m_sysMovement.Add(entity);
            m_sysCollision.Add(entity);
            m_sysRenderer.Add(entity);
            m_rules.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysMovement.Remove(entity.Id);
            m_sysCollision.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
            m_rules.Remove(entity.Id);
        }

        private void initializeBigBlue(Texture2D square, int x, int y)
        {
            var proposed = BigBlue.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeFlag(Texture2D square, int x, int y)
        {
            var proposed = Flag.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeFloor(Texture2D square, int x, int y)
        {
            var proposed = Floor.create(square, x, y);
            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeGrass(Texture2D square, int x, int y)
        {
            var proposed = Grass.create(square, x, y);
  
            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeHedge(Texture2D square, int x, int y)
        {
            var proposed = Hedge.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }

        }

        private void initializeLava(Texture2D square, int x, int y)
        {
            var proposed = Lava.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeRock(Texture2D square, int x, int y)
        {
            //bool push = false;

            var proposed = Rock.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeText(Texture2D square, int x, int y)
        {
            var proposed = Text.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeWall(Texture2D square, int x, int y)
        {
            var proposed = Wall.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

        private void initializeWater(Texture2D square, int x, int y)
        {
            var proposed = Water.create(square, x, y);

            GameLayout.addToGamePos(x, y, proposed);

            if (!m_sysCollision.collidesWithAny(proposed))
            {
                AddEntity(proposed);
            }
        }

    }
}
