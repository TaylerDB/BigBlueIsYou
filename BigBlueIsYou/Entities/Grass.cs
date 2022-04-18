using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Grass
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var grass = new Entity();

            grass.Add(new Components.Appearance(square, new Color(0, 255, 0), Color.Black));
            grass.Add(new Components.Position(x, y));
            
            return grass;
        }
    }
}
