using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Hedge
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var border = new Entity();

            border.Add(new Components.Appearance(square, Color.Green, Color.Black));
            border.Add(new Components.Position(x, y));
            border.Add(new Components.Collision());

            return border;
        }
    }
}