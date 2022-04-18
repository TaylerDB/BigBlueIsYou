using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Lava
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var lava = new Entity();

            lava.Add(new Components.Appearance(square, new Color(100, 100, 100), Color.Black));
            lava.Add(new Components.Position(x, y));
            lava.Add(new Components.Collision());

            return lava;
        }
    }
}
