using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

            //if (File.Exists("levels-all.bbiy"))
            //var line = TitleContainer.OpenStream("levels-all.bbiy");

            //Debug.WriteLine(line);
            var file = new FileStream("Content/Levels/levels-all.bbiy", FileMode.Open, FileAccess.Read);

            //StreamReader streamReader = new StreamReader(file);
            
            // Create a byte array 
            // to read from the 
            // text file
            byte[] readArr = new byte[file.Length];
            int count;

            // Using the Read method 
            // read until end of file
            while ((count = file.Read(readArr, 0, readArr.Length)) > 0)
            {
                Debug.WriteLine(Encoding.UTF8.GetString(readArr, 0, count));
            }

            // Close the FileStream ObjectS
            file.Close();
            //Debug.ReadKey();

            //using (var fs = TitleContainer.OpenStream(@"C:\Users\tayler\source\repos\BigBlueIsYou\BigBlueIsYou\Content\Levels\levels-all.bbiy"))
            //{
            //    byte[] b = new byte[10];
            //    UTF8Encoding temp = new UTF8Encoding(true);
            //    while (fs.Read(b, 0, b.Length) > 0)
            //    {
            //        Debug.WriteLine(temp.GetString(b));
            //    }
            //}


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
