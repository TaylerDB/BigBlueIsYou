using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Rock
    {
        public static Entity create(Texture2D square, int x, int y)
        {
            var rock = new Entity();

            rock.Add(new Components.Appearance(square, new Color(255, 165, 0), Color.Black));
            rock.Add(new Components.Position(x, y));
            rock.Add(new Components.Collision());

            //rock.Add(new Components.Movable(Components.Direction.Stopped));

            rock.Add(new Components.Pushable());

            return rock;
        }
    }
}
