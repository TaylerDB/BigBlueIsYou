using BigBlueIsYou;
using Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Rock
    {
        public static bool m_push;
        static Entity rock;
        public static Entity create(Texture2D square, int x, int y)
        {
            rock = new Entity();

            rock.Add(new Components.Appearance(square, new Color(255, 165, 0), Color.Black));
            rock.Add(new Components.Position(x, y));
            rock.Add(new Components.Collision());

            return rock;
        }

        public static void update()
        {
            if (rock != null)
            {
                var push = rock.ContainsComponent<Components.Pushable>();
                var you = rock.ContainsComponent<Components.Movable>();

                if (GameLayout.pushRock && !push)
                    rock.Add(new Components.Pushable());

                if (!GameLayout.pushRock && push)
                    rock.Remove(rock.GetComponent<Pushable>());

                if (GameLayout.youRock && !you)
                    rock.Add(new Components.Movable());

                if (!GameLayout.youRock && you)
                    rock.Remove(rock.GetComponent<Movable>());
            }
        }
    }
}
