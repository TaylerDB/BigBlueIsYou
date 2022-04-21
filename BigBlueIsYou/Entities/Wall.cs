using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall
    {
        private const int MOVE_INTERVAL = 600; // milliseconds

        public static Entity create(Texture2D square, int x, int y)
        {
            var wall = new Entity();

            wall.Add(new Components.Appearance(square, new Color(105, 105, 105), Color.Black));
            wall.Add(new Components.Position(x, y));
            wall.Add(new Components.Collision());

            //wall.Add(new Components.Stopable());

            //wall.Add(new Components.Movable(Components.Direction.Stopped, MOVE_INTERVAL));

            return wall;
        }
    }
}