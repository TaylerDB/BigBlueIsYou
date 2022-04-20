using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class Hedge
    {
        private const int MOVE_INTERVAL = 600; // milliseconds
        public static Entity create(Texture2D square, int x, int y)
        {
            var hedge = new Entity();

            hedge.Add(new Components.Appearance(square, Color.Green, Color.Black));
            hedge.Add(new Components.Position(x, y));

            hedge.Add(new Components.Stopable());


            return hedge;
        }
    }
}