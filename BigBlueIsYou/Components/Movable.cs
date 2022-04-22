
namespace Components
{
    public enum Direction
    {
        Stopped,
        Up,
        Down,
        Left,
        Right
    }

    public class Movable : Component
    {
        public Direction facing;
        public uint segmentsToAdd = 0;

        public Movable(Direction facing)
        {
            this.facing = facing;
        }
    }
}
