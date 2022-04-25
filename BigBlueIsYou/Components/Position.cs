using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Components
{
    public class Position : Component
    {
        //public List<Point> segments = new List<Point>();
        public int X { get; set;}
        public int Y { get; set;}

        public Position(int x, int y)
        {
            //segments.Add(new Point(x, y));
            this.X = x;
            this.Y = y;
        }
    }
}
