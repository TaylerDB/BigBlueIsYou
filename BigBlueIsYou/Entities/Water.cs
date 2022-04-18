using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Water
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var water = new Entity();

            water.Add(new Components.Appearance(square, new Color(0, 0, 255), Color.Black));
            water.Add(new Components.Position(x, y));
            water.Add(new Components.Collision());

            return water;
        }
    }
}
