using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BigBlueIsYou
{

    public class AboutView : GameStateView
    {
        private SpriteFont m_font;
        private const string MESSAGE = "Tayler Baker wrote this amazing game!";

        private Texture2D m_bigBlueBackground;
        private Rectangle m_bigRectange;

        public override void loadContent(ContentManager contentManager)
        {
            m_font = contentManager.Load<SpriteFont>("Fonts/menu");

            m_bigBlueBackground = contentManager.Load<Texture2D>("Images/BigBlueBackGround");
            m_bigRectange = new Rectangle(0, 0, m_graphics.GraphicsDevice.Viewport.Width, m_graphics.GraphicsDevice.Viewport.Height);

        }

        public override GameStateEnum processInput(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            return GameStateEnum.About;
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            m_spriteBatch.Draw(m_bigBlueBackground, m_bigRectange, Color.White);

            Vector2 stringSize = m_font.MeasureString(MESSAGE);
            m_spriteBatch.DrawString(m_font, MESSAGE,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, 200), Color.Yellow);

            m_spriteBatch.End();
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
