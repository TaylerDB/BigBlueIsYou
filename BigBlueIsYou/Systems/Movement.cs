using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Systems
{
    /// <summary>
    /// This system is responsible for handling the movement of any
    /// entity with a movable & position components.
    /// </summary>
    class Movement : System
    {
        // For key presses
        KeyboardState oldState;

        bool canMove = true;

        public Movement()
            : base(
                  typeof(Components.Movable),
                  typeof(Components.Position)
                  )
        {
            oldState = Keyboard.GetState();
        }

        public override void Update(GameTime gameTime)
        {
            processInput();
            collision();
            foreach (var entity in m_entities.Values)
            {
                moveEntity(entity, gameTime);
            }
        }

        private void processInput()
        {
            foreach (var entity in m_entities.Values)
            {
                var movable = entity.GetComponent<Components.Movable>();
                //var input = entity.GetComponent<Components.KeyboardControlled>();


                bool canTurn = true;
                // Protect agains turning back onto itself
                // BUG: Note the Keys are hardcoded here and if they are changed to
                //      something else in the game model when the snake entity is created
                //      those keys won't be recognized here.

                // Get keyboard state
                KeyboardState newState = Keyboard.GetState();
                if (movable.CanMoveUp)
                {

                    if (newState.IsKeyDown(Keys.Up))
                    {
                        if (!oldState.IsKeyDown(Keys.Up))
                        {
                            movable.facing = Components.Direction.Up;
                        }
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

                if (canMove)
                {
                    if (newState.IsKeyDown(Keys.Left))
                    {
                        if (!oldState.IsKeyDown(Keys.Left))
                        {
                            movable.facing = Components.Direction.Left;
                        }
                    }
                }

                // Update saved state
                oldState = newState;
            }
        }

        private void collision()
        {
            //var movable = findMovable(m_entities);
            var stopable = findStopable(m_entities);

            foreach (var entity in m_entities.Values)
            {

                foreach (var entityStopable in stopable)
                {
                    if (collides(entity, entityStopable))
                    {
                        entityStopable.GetComponent<Components.Movable>().facing = Components.Direction.Stopped;
                        canMove = false;
                    }
                    else
                    {
                        canMove = true;
                    }
                }
            }
        }

        private List<Entity> findStopable(Dictionary<uint, Entity> entities)
        {
            var stopable = new List<Entity>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.Stopable>() && entity.ContainsComponent<Components.Position>())
                {
                    stopable.Add(entity);
                }
            }

            return stopable;
        }

        /// <summary>
        /// We know that only the snake is moving and that we only need
        /// to check its head for collision with other entities.  Therefore,
        /// don't need to look at all the segments in the position, with the
        /// exception of the movable itself...a movable can collide with itself.
        /// </summary>
        private bool collides(Entity a, Entity b)
        {
            var aPosition = a.GetComponent<Components.Position>();
            var bPosition = b.GetComponent<Components.Position>();

            //
            // A movable can collide with itself: Check segment against the rest
            if (a == b)
            {
                //
                // Have to skip the first segment, that's why using a counted for loop
                for (int segment = 1; segment < aPosition.segments.Count; segment++)
                {
                    if (aPosition.x == aPosition.segments[segment].X && aPosition.y == aPosition.segments[segment].Y)
                    {
                        return true;
                    }
                }

                return false;
            }

            return aPosition.x == bPosition.x && aPosition.y == bPosition.y;
        }

        private void moveEntity(Entities.Entity entity, GameTime gameTime)
        {
            var movable = entity.GetComponent<Components.Movable>();

            switch (movable.facing)
            {
                case Components.Direction.Up:
                    //if ()
                    move(entity, 0, -1);
                    break;
                case Components.Direction.Down:
                    move(entity, 0, 1);
                    break;
                case Components.Direction.Left:
                    if (movable.CanMoveUp)
                    {
                        move(entity, -1, 0);
                    }
                    break;
                case Components.Direction.Right:
                    move(entity, 1, 0);
                    break;
            }
            movable.facing = Components.Direction.Stopped;
        }

        private void move(Entities.Entity entity, int xIncrement, int yIncrement)
        {
            var movable = entity.GetComponent<Components.Movable>();
            var position = entity.GetComponent<Components.Position>();
            //var stopable = entity.ContainsComponent<Components.Stopable>();
            //
            // Remember current front position, so it can be added back in as the move
            var front = position.segments[0];

            //
            // Remove the tail, but only if there aren't new segments to add
            if (movable.segmentsToAdd == 0 && position.segments.Count > 0)
            {
                position.segments.RemoveAt(position.segments.Count - 1);
            }
            else
            {
                movable.segmentsToAdd--;
            }

            //
            // Update the front of the entity with the segment moving into the new spot
            //var movable = findMovable(m_entities);
            
            Point newFront = new Point(front.X + xIncrement, front.Y + yIncrement);
            //if (newFront != )
            //{

                position.segments.Insert(0, newFront);
            //}
            
        }
    }
}
