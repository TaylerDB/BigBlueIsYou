using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Entities
{
    class Text
    {
        static string name;

        public static string Name
        {
            get { return name; }
            set { name = value; }
        }

        public static Entity create(Texture2D square, int x, int y)
        {
            var text = new Entity();
            // White
            if (square.Name == "Animations/word-is" || square.Name == "Animations/word-baba")
            {
                name = square.Name;
                text.Add(new Components.Appearance(square, new Color(255, 255, 255), Color.Black));
            }

            // Grey
            if (square.Name == "Animations/word-wall")
            {
                text.Add(new Components.Appearance(square, new Color(105, 105, 105), Color.Black));
            }
            // Green
            if (square.Name == "Animations/word-stop")
            {
                text.Add(new Components.Appearance(square, new Color(0, 255, 0), Color.Black));
            }
            // Yellow
            if (square.Name == "Animations/word-flag" || square.Name == "Animations/word-win")
            {
                text.Add(new Components.Appearance(square, new Color(255, 255, 0), Color.Black));
            }

            // Orange
            if (square.Name == "Animations/word-rock" || square.Name == "Animations/word-push")
            {
                text.Add(new Components.Appearance(square, new Color(255, 165, 0), Color.Black));
            }

            // Magenta
            if (square.Name == "Animations/word-you")
            {
                text.Add(new Components.Appearance(square, new Color(255, 0, 255), Color.Black));
            }

            // Red
            if (square.Name == "Animations/word-lava" )
            {
                text.Add(new Components.Appearance(square, new Color(255, 0, 0), Color.Black));
            }

            if (square.Name == "Animations/word-kill")
            {
                text.Add(new Components.Appearance(square, new Color(255, 0, 0), Color.Black));
            }

            // Blue
            if (square.Name == "Animations/word-water" || square.Name == "Animations/word-sink")
            {
                text.Add(new Components.Appearance(square, new Color(0, 0, 255), Color.Black));
            }

            text.Add(new Components.Position(x, y));
            text.Add(new Components.Pushable());

            return text;
        }
    }
}
