using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Systems
{
    /// <summary>
    /// This system knows how to accept keyboard input and use that
    /// to move an entity, based on the entities 'KeyboardControlled'
    /// component settings.
    /// </summary>
    class KeyboardInput : System
    {
        // For key presses
        KeyboardState oldState;

        public KeyboardInput()
            : base(typeof(Components.KeyboardControlled))
        {
            oldState = Keyboard.GetState();
        }


        public override void Update(GameTime gameTime)
        {
            foreach (var entity in m_entities.Values)
            {
                var movable = entity.GetComponent<Components.Movable>();
                var input = entity.GetComponent<Components.KeyboardControlled>();

                bool canTurn = true;
                // Protect agains turning back onto itself
                // BUG: Note the Keys are hardcoded here and if they are changed to
                //      something else in the game model when the snake entity is created
                //      those keys won't be recognized here.

                // Get keyboard state
                KeyboardState newState = Keyboard.GetState();

                if (newState.IsKeyDown(Keys.Up))
                {
                    if (!oldState.IsKeyDown(Keys.Up))
                    {
                        movable.facing = Components.Direction.Up;
                    }
                }
                               
                if (newState.IsKeyDown(Keys.Down))
                {
                    if (!oldState.IsKeyDown(Keys.Down))
                    {
                        movable.facing = Components.Direction.Down;
                    }
                }

                if (newState.IsKeyDown(Keys.Right))
                {
                    if (!oldState.IsKeyDown(Keys.Right))
                    {
                        movable.facing = Components.Direction.Right;
                    }
                }

                if (newState.IsKeyDown(Keys.Left))
                {
                    if (!oldState.IsKeyDown(Keys.Left))
                    {
                        movable.facing = Components.Direction.Left;
                    }
                }
                
                // Update saved state
                oldState = newState;

            }
        }
    }
}
