using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
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
        private const int OBSTACLE_COUNT = 15;
        private readonly int WINDOW_WIDTH;
        private readonly int WINDOW_HEIGHT;

        private List<Entity> m_removeThese = new List<Entity>();
        private List<Entity> m_addThese = new List<Entity>();

        private Systems.Renderer m_sysRenderer;
        private Systems.Collision m_sysCollision;
        private Systems.Movement m_sysMovement;
        private Systems.KeyboardInput m_sysKeyboardInput;

        private string[] levelsArray;

        public GameModel(int width, int height)
        {
            this.WINDOW_WIDTH = width;
            this.WINDOW_HEIGHT = height;
        }

        public void Initialize(ContentManager content, SpriteBatch spriteBatch)
        {
            var texSquare = content.Load<Texture2D>("Images/square");
            var bigBlueSquare = content.Load<Texture2D>("Images/BigBlue");

            //StreamReader reader = File.OpenText(@"C:\Users\tayler\source\repos\BigBlueIsYou\BigBlueIsYou\Content\Levels");
            //string line[] = content.Load<XmlImporter>("Levels/levels-all");

            //while ((line = reader.ReadLine()) != null)
            //{
            //    System.Console.WriteLine(line);
            //}

            Debug.WriteLine("Attempting to find file");

            //Debug.WriteLine(line);
            var file = new FileStream("Content/Levels/levels-all.bbiy", FileMode.Open, FileAccess.Read);
            
            // Create a byte array 
            // to read from the 
            // text file
            byte[] readArr = new byte[file.Length];
            int count;

            // Using the Read method 
            // read until end of file - neccesary to keep
            while ((count = file.Read(readArr, 0, readArr.Length)) > 0)
            {
                //Debug.WriteLine(Encoding.UTF8.GetString(readArr, 0, count));
            }

            levelsArray = new string[readArr.Length];
            // Convert to string
            string levelsString = Encoding.UTF8.GetString(readArr, 0, readArr.Length);

            // Convert to array
            for (int i = 0; i < readArr.Length; i++)
            {
               levelsArray[i] = readArr[i].ToString();
            }


            //Debug.WriteLine("Levels: ", levelsSting);

            /***************************
             *  76 - L
             * 101 - e
             * 118 - v
             * 101 - e
             * 108 - l
             *  45 - '-'
             *  49 - 1
             *  13 - CR - new line
             *  10 - Line feed
             *  50 - 2
             *  48 - 0
             *  32 - Space 
             * 120 - x
             *  32 - Space
             *  50 - 2
             *  48 - 0
             *  13 - Carriage Return
             *  10 - Line feed
             * 104 - h
             * 104 - h
             *****************************/

            // Close the FileStream ObjectS
            file.Close();

            // Get level 1 string
            int lFrom1 = levelsString.IndexOf("Level-1\r\n") + "Level-1\r\n".Length;
            int lTo1 = levelsString.IndexOf("Level-2");
            string level1String = levelsString.Substring(lFrom1, lTo1 - lFrom1);
            //Debug.WriteLine("result1: " + level1String);

            // Get size of level 1 first dimension
            int n1To1 = level1String.IndexOf(" ");
            string level1Size1 = level1String.Substring(0, n1To1 - 0);
            //Debug.WriteLine("level1Size1: " + level1Size1);

            // Get size of level 1 second dimension
            int n2From1 = level1String.IndexOf("x ") + "x ".Length;
            int n2To1 = level1String.IndexOf("\r\n");
            string level1Size2 = level1String.Substring(n2From1, n2To1 - n2From1);
            //Debug.WriteLine("level1Size2: " + level1Size2);

            char[] charArr = level1String.ToCharArray();
            string result = "";
            for (int i = 0; i < charArr.Length; i++)
            {
                if (charArr[i] != 'h')
                {
                    result += charArr[i];
                }
                else
                    break;
            }

            // Get how many different levels are in file
            string ch = "Level-";
            int count1 = 0;
            int j = 0;
            while ((j = levelsString.IndexOf(ch, j)) != -1)
            {
                j += ch.Length;
                count1++;
            }
            
            Debug.WriteLine("Count: " + count1.ToString());

            //Debug.WriteLine("result: " + result);

            //string level1Display = level1String.Substring(0, result.Length - 0);
            //Debug.WriteLine("result: " + level1Display);

            // Get level 2 string
            int sFrom2 = levelsString.IndexOf("Level-2\r\n") + "Level-2\r\n".Length;
            int sTo2 = levelsString.IndexOf("Level-3");
            string level2String = levelsString.Substring(sFrom2, sTo2 - sFrom2);
            //Debug.WriteLine("result2: " + level2String);




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

            initializeBorder(texSquare);
            initializeObstacles(texSquare);
            initializeSnake(bigBlueSquare);
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

        private void initializeBorder(Texture2D square)
        {
            for (int position = 0; position < GRID_SIZE; position++)
            {
                var left = BorderBlock.create(square, 0, position);
                AddEntity(left);

                var right = BorderBlock.create(square, GRID_SIZE - 1, position);
                AddEntity(right);

                var top = BorderBlock.create(square, position, 0);
                AddEntity(top);

                var bottom = BorderBlock.create(square, position, GRID_SIZE - 1);
                AddEntity(bottom);
            }
        }

        private void initializeObstacles(Texture2D square)
        {
            MyRandom rnd = new MyRandom();
            int remaining = OBSTACLE_COUNT;

            while (remaining > 0)
            {
                int x = (int)rnd.nextRange(1, GRID_SIZE - 1);
                int y = (int)rnd.nextRange(1, GRID_SIZE - 1);
                var proposed = Obstacle.create(square, x, y);
                if (!m_sysCollision.collidesWithAny(proposed))
                {
                    AddEntity(proposed);
                    remaining--;
                }
            }
        }

        private void initializeSnake(Texture2D square)
        {
            MyRandom rnd = new MyRandom();
            bool done = false;

            while (!done)
            {
                int x = 1; //(int)rnd.nextRange(1, GRID_SIZE - 1);
                int y = 1; //(int)rnd.nextRange(1, GRID_SIZE - 1);
                var proposed = BigBlue.create(square, x, y);
                if (!m_sysCollision.collidesWithAny(proposed))
                {
                    AddEntity(proposed);
                    done = true;
                }
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
