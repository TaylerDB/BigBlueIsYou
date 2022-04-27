using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Lava
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var lava = new Entity();

            lava.Add(new Components.Appearance(square, new Color(255, 165, 0), Color.Black));
            lava.Add(new Components.Position(x, y));
            lava.Add(new Components.Collision());

            lava.Add(new Components.Kill());

            return lava;
        }
    }
}
