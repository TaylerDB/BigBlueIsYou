using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Systems
{

    class Renderer : System
    {
        private readonly int GRID_SIZE;
        private readonly int CELL_SIZE;
        private readonly int OFFSET_X;
        private readonly int OFFSET_Y;
        private readonly SpriteBatch m_spriteBatch;
        private readonly Texture2D m_texBackground;

        // Animation timer
        TimeSpan animationTimer = TimeSpan.FromMilliseconds(0);
        int count = 0;
        Rectangle size = new Rectangle();

        public Renderer(SpriteBatch spriteBatch, Texture2D texBackGround, int width, int height, int gridSize) :
            base(typeof(Components.Appearance), typeof(Components.Position))
        {
            GRID_SIZE = gridSize;
            CELL_SIZE = height / gridSize;
            OFFSET_X = (width - gridSize * CELL_SIZE) / 2;
            OFFSET_Y = (height - gridSize * CELL_SIZE) / 2;
            m_spriteBatch = spriteBatch;
            m_texBackground = texBackGround;
        }

        public override void Update(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // Update animation timer
            animationTimer += gameTime.ElapsedGameTime;

            //
            // Draw a blue background
            Rectangle background = new Rectangle(OFFSET_X, OFFSET_Y, GRID_SIZE * CELL_SIZE, GRID_SIZE * CELL_SIZE);
            m_spriteBatch.Draw(m_texBackground, background, Color.Black);

            foreach (var entity in m_entities.Values)
            {
                renderEntity(entity);
            }

            m_spriteBatch.End();
        }

        private void renderEntity(Entity entity)
        {
            var appearance = entity.GetComponent<Components.Appearance>();
            var position = entity.GetComponent<Components.Position>();
            Rectangle area = new Rectangle();

            // Animation speed
            if (animationTimer.TotalMilliseconds > 700)
            {
                if (count == 2)
                {
                    // Reset back to the begining 
                    size.X = 24;
                    count = 0;
                }

                else
                {
                    // Move to the next frame
                    size.X += 24;
                }

                // Reset animation timer
                animationTimer = TimeSpan.FromMilliseconds(0);

                // Increase to next animation frame
                count++;
            }

            int segment = 0;
            area.X = OFFSET_X + position.X * CELL_SIZE;
            area.Y = OFFSET_Y + position.Y * CELL_SIZE;
            area.Width = CELL_SIZE;
            area.Height = CELL_SIZE;

            size.Width = 24;
            size.Height = 24;

            if (appearance.image.Name == "Images/BigBlue")
            {
                m_spriteBatch.Draw(appearance.image, area, appearance.stroke);
            }

            m_spriteBatch.Draw(appearance.image, area, size, appearance.stroke);

            area.X = OFFSET_X + position.X * CELL_SIZE + 1;
            area.Y = OFFSET_Y + position.Y * CELL_SIZE + 1;
            area.Width = CELL_SIZE - 2;
            area.Height = CELL_SIZE - 2;
            float fraction = MathHelper.Min(segment / 30.0f, 1.0f);
            var color = new Color(
                (int)lerp(appearance.fill.R, 0, fraction),
                (int)lerp(appearance.fill.G, 0, fraction),
                (int)lerp(appearance.fill.B, 255, fraction));

            if (appearance.image.Name == "Images/BigBlue")
            {
                m_spriteBatch.Draw(appearance.image, area, appearance.stroke);
            }

            m_spriteBatch.Draw(appearance.image, area,size, color);

        }

        private float lerp(float a, float b, float f)
        {
            return a + f * (b - a);
        }
    }
}
