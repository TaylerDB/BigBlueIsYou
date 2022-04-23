using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Floor
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var floor = new Entity();

            floor.Add(new Components.Appearance(square, new Color(50, 50, 50), Color.Black));
            floor.Add(new Components.Position(x, y));
            floor.Add(new Components.Empty());
            return floor;
        }
    }
}
