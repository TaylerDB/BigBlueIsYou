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

        int row;
        int col;

        char[,] charTopArr;
        char[,] charBottomArr;



        private AnimatedSprite hedgeRenderer;

        //private

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
            var rockObject = content.Load<Texture2D>("Animations/rock");
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
            
            // TODO: Find why the getter is passing an empty string
            //string levelsString = levelsView.LevelsString;
            //int levelChoice = levelsView.CurrentSelection;

            // Get level string minus Level-#
            int thisLevel = levelChoice + 1;
            int from = levelsString.IndexOf("Level-" + thisLevel.ToString() + "\r\n") + ("Level-" + thisLevel.ToString() + "\r\n").Length;
            int nextLevel = thisLevel + 1;
            int to = levelsString.IndexOf("Level-" + nextLevel.ToString());
            string selectedLevelString = levelsString.Substring(from, to - from);
            //Debug.WriteLine("result1: " + selectedLevelString);

            // Get first level dimension
            int d1To = selectedLevelString.IndexOf(" ");
            string levelDimensionString1 = selectedLevelString.Substring(0, d1To - 0);
            Debug.WriteLine("level dimension 1: " + levelDimensionString1);

            // Get size of level 1 second dimension
            int d2From = selectedLevelString.IndexOf("x ") + "x ".Length;
            int d2To = selectedLevelString.IndexOf("\r\n");
            string levelDimensionString2 = selectedLevelString.Substring(d2From, d2To - d2From);
            Debug.WriteLine("level dimension 2: " + levelDimensionString2);

            // Get level by itself
            int levelStart = selectedLevelString.IndexOf("\r\n") + "\r\n".Length;
            int levelEnd = selectedLevelString.Length; //selectedLevelString.LastIndexOf(selectedLevelString);
            string actualLevel = selectedLevelString.Substring(levelStart, levelEnd - levelStart);
            //Debug.WriteLine("Actual Level:\n" + actualLevel);

            // Get row and col dimension of level
            row = Int32.Parse(levelDimensionString1);
            col = Int32.Parse(levelDimensionString2);

            GameLayout.GamePos = new Entity[row, col];

            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    var proposed = Grass.create(grassObject, r, c);
                    GameLayout.addToGamePos(r, c, proposed);
                }
            }

                

            charTopArr = new char[row, col];
            charBottomArr = new char[row, col];

            string topLevel = actualLevel.Substring(0, actualLevel.Length / 2);
            string bottomLevel = actualLevel.Substring(actualLevel.Length / 2, actualLevel.Length / 2);

            Debug.WriteLine("topLevel:\n" + topLevel);
            Debug.WriteLine("bottomLevel:\n" + bottomLevel);

            // Put topLevel in a 2d char array charTopArr
            int t = 0;
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (topLevel[t] == '\r' || topLevel[t] == '\n')
                    { 
                        t++;
                        c--;
                    }
                    else
                    {
                        charTopArr[r, c] = topLevel[t];
                        t++;
                    }
                }
            }

            // Put bottomLevel in a 2d char array charBottomArr
            int b = 0;
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (bottomLevel[b] == '\r' || bottomLevel[b] == '\n')
                    {
                        b++;
                        c--;
                    }
                    else
                    {
                        charBottomArr[r, c] = bottomLevel[b];
                        b++;
                    }
                }
            }


            m_sysRenderer = new Systems.Renderer(spriteBatch, texSquare, WINDOW_WIDTH, WINDOW_HEIGHT, GRID_SIZE);
            m_sysCollision = new Systems.Collision((entity) =>
            {
                // Remove the existing food pill
                m_removeThese.Add(entity);
                // Need another food pill
                m_addThese.Add(createFood(texSquare));
            });

            m_sysMovement = new Systems.Movement();
            m_sysKeyboardInput = new Systems.KeyboardInput();

            // Add objects to topLevel
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (charTopArr[c, r] == 'g')
                    {
                        initializeGrass(grassObject, r, c);
                    }
                    if (charTopArr[c, r] == 'h')
                    {
                        initializeHedge(hedgeObject, r, c);
                    }
                    if (charTopArr[c, r] == 'l')
                    {
                        initializeFloor(floorObject, r, c);
                    }
                }
            }
            

            // Add objects to bottomLevel
            for (int r = 0; r < row; r++)
            {
                for (int c = 0; c < col; c++)
                {
                    if (charBottomArr[c, r] == 'a')
                    {
                        initializeWater(waterObject, r, c);
                    }
                    if (charBottomArr[c, r] == 'b')
                    {
                        initializeBigBlue(bigBlueSquare, r, c);
                    }
                    if (charBottomArr[c, r] == 'f')
                    {
                        initializeFlag(flagObject, r, c);
                    }
                    if (charBottomArr[c, r] == 'r')
                    {
                        initializeRock(rockObject, r, c);
                    }
                    if (charBottomArr[c, r] == 'w')
                    {
                        initializeWall(wallObject, r, c);
                    }
                    if (charBottomArr[c, r] == 'W')
                    {
                        initializeText(wallWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'R')
                    {
                        initializeText(rockWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'F')
                    {
                        initializeText(flagWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'B')
                    {
                        initializeText(bigBlueWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'I')
                    {
                        initializeText(isWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'S')
                    {
                        initializeText(stopWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'P')
                    {
                        initializeText(pushWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'V')
                    {
                        initializeText(lavaWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'A')
                    {
                        initializeText(waterWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'Y')
                    {
                        initializeText(youWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'X')
                    {
                        initializeText(winWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'N')
                    {
                        initializeText(sinkWord, r, c);
                    }
                    if (charBottomArr[c, r] == 'K')
                    {
                        initializeText(killWord, r, c);
                    }
                }
            }

            //// Setup centipede animation
            //hedgeRenderer = new AnimatedSprite(
            //    content.Load<Texture2D>("Animations/hedge"),
            //    new int[] { 100, 100, 100}
            //    );

            AddEntity(createFood(texSquare));
        }

        public void Update(GameTime gameTime)
        {
            m_sysKeyboardInput.Update(gameTime);
            m_sysMovement.Update(gameTime);
            m_sysCollision.Update(gameTime);

            foreach (var entity in m_removeThese)
            {
                RemoveEntity(entity);
            }
            m_removeThese.Clear();

            foreach (var entity in m_addThese)
            {
                AddEntity(entity);
            }
            m_addThese.Clear();
        }

        public void Draw(GameTime gameTime)
        {
            m_sysRenderer.Update(gameTime);
            //hedgeRenderer.draw(spriteBatch, Hedge);
        }

        private void AddEntity(Entity entity)
        {
            m_sysKeyboardInput.Add(entity);
            m_sysMovement.Add(entity);
            m_sysCollision.Add(entity);
            m_sysRenderer.Add(entity);
        }

        private void RemoveEntity(Entity entity)
        {
            m_sysKeyboardInput.Remove(entity.Id);
            m_sysMovement.Remove(entity.Id);
            m_sysCollision.Remove(entity.Id);
            m_sysRenderer.Remove(entity.Id);
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



        private Entity createFood(Texture2D square)
        {
            MyRandom rnd = new MyRandom();
            bool done = false;

            while (!done)
            {
                int x = (int)rnd.nextRange(1, GRID_SIZE - 1);
                int y = (int)rnd.nextRange(1, GRID_SIZE - 1);
                var proposed = Food.create(square, x, y);
                if (!m_sysCollision.collidesWithAny(proposed))
                {
                    return proposed;
                }
            }

            return null;
        }
    }
}
