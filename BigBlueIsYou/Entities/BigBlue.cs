using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class BigBlue
    {
        public static Entity create(Texture2D BigBlue, int x, int y)
        {
            var bigBlue = new Entity();
            // rename to sprite - modify appearance to include time and number of sprites
            bigBlue.Add(new Components.Appearance(BigBlue, Color.White, Color.White));
            bigBlue.Add(new Components.Position(x, y));
            bigBlue.Add(new Components.Collision());

            
            bigBlue.Add(new Components.Movable());



            return bigBlue;
        }
    }
}