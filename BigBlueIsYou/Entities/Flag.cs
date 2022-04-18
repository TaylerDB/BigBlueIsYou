using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Flag
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var flag = new Entity();

            flag.Add(new Components.Appearance(square, new Color(255, 255, 0), Color.Black));
            flag.Add(new Components.Position(x, y));
            flag.Add(new Components.Collision());

            return flag;
        }
    }
}
