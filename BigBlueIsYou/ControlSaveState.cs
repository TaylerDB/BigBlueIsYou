using Microsoft.Xna.Framework.Input;

namespace BigBlueIsYou
{
    public class ControlSaveState
    {
        /// <summary>
        /// Have to have a default constructor for the XmlSerializer.Deserialize method
        /// </summary>
        public ControlSaveState() { }

        /// <summary>
        /// Overloaded constructor used to create an object for long term storage
        /// </summary>
        /// <param name="up"></param>
        /// <param name="down"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <param name="reset"></param>
        public ControlSaveState(Keys up, Keys down, Keys left, Keys right, Keys reset)
        {
            this.Up = up;
            this.Down = down;
            this.Left = left;
            this.Right = right;
            this.Reset = reset;

        }

        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Reset { get; set; }
    }
}
