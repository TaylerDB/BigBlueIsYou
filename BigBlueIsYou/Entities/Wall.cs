using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Wall
    {

        public static Entity create(Texture2D square, int x, int y)
        {
            var wall = new Entity();

            wall.Add(new Components.Appearance(square, new Color(105, 105, 105), Color.Black));
            wall.Add(new Components.Position(x, y));
            wall.Add(new Components.Collision());

            //wall.Add(new Components.Stoppable());

            //wall.Add(new Components.Movable(Components.Direction.Stopped));

            wall.Add(new Components.Pushable());

            return wall;
        }
    }
}