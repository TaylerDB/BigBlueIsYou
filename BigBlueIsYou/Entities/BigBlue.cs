﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

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
            bigBlue.Add(new Components.KeyboardControlled(
                new Dictionary<Keys, Components.Direction>
                {
                        { Keys.Up, Components.Direction.Up },
                        { Keys.Down, Components.Direction.Down },
                        { Keys.Left, Components.Direction.Left },
                        { Keys.Right, Components.Direction.Right }
                }));

            return bigBlue;
        }
    }
}