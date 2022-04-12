using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace BigBlueIsYou.Views
{
    class LevelsView : GameStateView
    {
        private SpriteFont m_fontMenu;
        private SpriteFont m_fontMenuSelect;

        private const string MESSAGE = "Select a level you would like to play with 'S'";
        private bool m_waitForKeyRelease = false;

        List<string> levelsList = new List<string>();


        private int m_currentSelection;// = MenuState.NewGame;
        private string[] levelsArray;

        public string levelsString;
        //int levelChoice;
        int levelCount;

        public string LevelsString
        {
            get { return levelsString; }
            set { levelsString = value; }
        }

        public int CurrentSelection
        {
            get { return m_currentSelection; }
            //set { m_currentSelection = value; }
        }

        public int LevelCount
        {
            get { return levelCount; }
            //set { m_currentSelection = value; }
        }




        public override void loadContent(ContentManager contentManager)
        {
            m_fontMenu = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontMenuSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");


            Debug.WriteLine("Attempting to find file");

            // Open file
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
            levelsString = Encoding.UTF8.GetString(readArr, 0, readArr.Length);

            // Convert to array
            for (int i = 0; i < readArr.Length; i++)
            {
                levelsArray[i] = readArr[i].ToString();
            }

            // Close the FileStream ObjectS
            file.Close();

            // Get how many different levels are in file
            string levelNumberString = "Level-";

            int j = 0;

            //string loadLevel;
            while ((j = levelsString.IndexOf(levelNumberString, j)) != -1)
            {
                j += levelNumberString.Length;
                levelCount++;
            }

            Debug.WriteLine("Count: " + levelCount.ToString());

            // Add number of levels to levelsList
            for (int i = 1; i <= levelCount; i++)
            {
                string addLevel = "Level " + i.ToString();
                levelsList.Add(addLevel);
            }
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {

            // This is the technique I'm using to ensure one keypress makes one menu navigation move
            if (!m_waitForKeyRelease)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Escape))
                {
                    return GameStateEnum.MainMenu;
                }

                // Arrow keys to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && m_currentSelection != levelsList.Count - 1)
                {
                    m_currentSelection = m_currentSelection + 1;
                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && m_currentSelection != 0)
                {
                    m_currentSelection = m_currentSelection - 1;
                    m_waitForKeyRelease = true;
                }

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.S) /*&& m_currentSelection == MenuState.NewGame*/)
                {
                    Debug.WriteLine("m_currentSelection: " + m_currentSelection);
                    return GameStateEnum.GamePlay;
                }

            }
            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.Levels;
        }
        public override void update(GameTime gameTime)
        {
        }
        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            Vector2 stringSize = m_fontMenu.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_fontMenu, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, 200 - stringSize.Y), Color.Blue);

            int yPos = 300;

            foreach (var lev in levelsList)
            {
                float bottom = drawLevelItem(
                    levelsList.ElementAt(m_currentSelection) == lev ? m_fontMenuSelect : m_fontMenu,
                    lev,
                    yPos,
                    levelsList.ElementAt(m_currentSelection) == lev ? Color.Yellow : Color.Blue);
                
                yPos += 100;
            }

            m_spriteBatch.End();
        }

        private float drawLevelItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }
    }
}
