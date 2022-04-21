using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Rock
    {
        private const int MOVE_INTERVAL = 600; // milliseconds

        public static Entity create(Texture2D square, int x, int y)
        {
            var rock = new Entity();

            rock.Add(new Components.Appearance(square, new Color(255, 165, 0), Color.Black));
            rock.Add(new Components.Position(x, y));
            rock.Add(new Components.Collision());

            rock.Add(new Components.Movable(Components.Direction.Stopped, MOVE_INTERVAL));

            return rock;
        }
    }
}
