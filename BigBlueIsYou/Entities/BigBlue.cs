using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    public class BigBlue
    {
        private const int MOVE_INTERVAL = 600; // milliseconds
        public static Entity create(Texture2D BigBlue, int x, int y)
        {
            var bigBlue = new Entity();
            // rename to sprite - modify appearance to include time and number of sprites
            bigBlue.Add(new Components.Appearance(BigBlue, Color.White, Color.Black));
            bigBlue.Add(new Components.Position(x, y));
            bigBlue.Add(new Components.Collision());
            
            // add renderAnimated component/system

            bigBlue.Add(new Components.Movable(Components.Direction.Stopped, MOVE_INTERVAL));
            
            

            return bigBlue;
        }
    }
}