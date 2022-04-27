using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace BigBlueIsYou
{
    public class GamePlayView : GameStateView
    {
        ContentManager m_content;
        private GameModel m_gameModel;

        private int m_levelSelection;
        private string m_levelString;

        public GamePlayView()
        {

        }

        public GamePlayView(GamePlayView gamePlayView)
        {
            GameModel m_gameModel;
        }

        public override void initializeSession()
        {
            m_gameModel = new GameModel(m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight);
            m_gameModel.Initialize(m_content, m_spriteBatch, m_levelSelection, m_levelString);
        }

        public void setLevel(int currentSelection, string levelsString)
        {
            m_levelSelection = currentSelection;
            m_levelString = levelsString;
        }

        public override void loadContent(ContentManager content)
        {
            m_content = content;
        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.GamePlay;
        }

        public override void render(GameTime gameTime)
        {
            m_gameModel.Draw(gameTime);
        }

        public override void update(GameTime gameTime)
        {
            m_gameModel.Update(gameTime);
        }
    }
}
