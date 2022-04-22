using BigBlueIsYou;
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
        Entity[,] gameState;

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
            gameState = GameLayout.GamePos;
            processInput();
            //collision();
            foreach (var entity in m_entities.Values)
            {
                moveEntity(entity, gameTime);
            }
        }

        private void processInput()
        {
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
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        movable.facing = Components.Direction.Up;
                    }
                }
            }
                
            if (newState.IsKeyDown(Keys.Down))
            {
                if (!oldState.IsKeyDown(Keys.Down))
                {
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        movable.facing = Components.Direction.Down;
                    }
                }
            }

            if (newState.IsKeyDown(Keys.Right))
            {
                if (!oldState.IsKeyDown(Keys.Right))
                {
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        movable.facing = Components.Direction.Right;
                    }
                }
            }

            if (newState.IsKeyDown(Keys.Left))
            {
                if (!oldState.IsKeyDown(Keys.Left))
                {
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        movable.facing = Components.Direction.Left;
                    }
                }
            }                

            // Update saved state
            oldState = newState;
        }

        private void moveEntity(Entities.Entity entity, GameTime gameTime)
        {

            var movable = entity.GetComponent<Components.Movable>();
            var position = entity.GetComponent<Components.Position>();
            //var stopable = entity.GetComponent<Components.Stopable>();
            var front = position.segments[0];

            switch (movable.facing)
            {
                case Components.Direction.Up:
                    if (!gameState[front.X, front.Y - 1].ContainsComponent<Components.Stoppable>() && !gameState[front.X, front.Y - 1].ContainsComponent<Components.Pushable>())
                    {
                        // Add entity to new spot, clear previous spot
                        var temp = gameState[front.X, front.Y - 1];
                        gameState[front.X, front.Y - 1] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp;

                        move(entity, 0, -1);
                    }

                    // Check for Pushable
                    if (gameState[front.X, front.Y - 1].ContainsComponent<Components.Pushable>() && !gameState[front.X, front.Y - 2].ContainsComponent<Components.Stoppable>())
                    {
                        var push = gameState[front.X, front.Y - 1].GetComponent<Components.Position>();
                        var p = push.segments[0];

                        // Move pushable over 1 spaces
                        Point pFront = new Point(p.X, p.Y - 1);
                        push.segments.Insert(0, pFront);

                        // Remove tail
                        push.segments.RemoveAt(push.segments.Count - 1);

                        // Allows to be kept pushing
                        var temp = gameState[front.X, front.Y - 2];
                        gameState[front.X, front.Y - 2] = gameState[front.X, front.Y - 1];
                        gameState[front.X, front.Y - 1] = temp;

                        // Add entity to new spot, clear previous spot
                        var temp2 = gameState[front.X, front.Y - 1];
                        gameState[front.X, front.Y - 1] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp2;

                        move(entity, 0, -1);
                    }

                    break;

                case Components.Direction.Down:
                    if (!gameState[front.X, front.Y + 1].ContainsComponent<Components.Stoppable>() && !gameState[front.X, front.Y + 1].ContainsComponent<Components.Pushable>())
                    {
                        // Add entity to new spot, clear previous spot
                        var temp = gameState[front.X, front.Y + 1];
                        gameState[front.X, front.Y + 1] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp;

                        move(entity, 0, 1);
                    }

                    // Check for Pushable
                    if (gameState[front.X, front.Y + 1].ContainsComponent<Components.Pushable>() && !gameState[front.X, front.Y + 2].ContainsComponent<Components.Stoppable>())
                    {
                        var push = gameState[front.X, front.Y + 1].GetComponent<Components.Position>();
                        var p = push.segments[0];

                        // Move pushable over 1 spaces
                        Point pFront = new Point(p.X, p.Y + 1);
                        push.segments.Insert(0, pFront);

                        // Remove tail
                        push.segments.RemoveAt(push.segments.Count - 1);

                        // Allows to be kept pushing
                        var temp = gameState[front.X, front.Y + 2];
                        gameState[front.X, front.Y + 2] = gameState[front.X, front.Y + 1];
                        gameState[front.X, front.Y + 1] = temp;

                        // Add entity to new spot, clear previous spot
                        var temp2 = gameState[front.X, front.Y + 1];
                        gameState[front.X, front.Y + 1] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp2;

                        move(entity, 0, 1);
                    }

                    break;

                case Components.Direction.Left:
                    if (!gameState[front.X - 1, front.Y].ContainsComponent<Components.Stoppable>() && !gameState[front.X - 1, front.Y].ContainsComponent<Components.Pushable>())
                    {
                        // Add entity to new spot, clear previous spot
                        var temp = gameState[front.X - 1, front.Y];
                        gameState[front.X - 1, front.Y] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp;
                        
                        move(entity, -1, 0);
                    }

                    // Check for Pushable
                    if (gameState[front.X - 1, front.Y].ContainsComponent<Components.Pushable>() && !gameState[front.X - 2, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        var push = gameState[front.X - 1, front.Y].GetComponent<Components.Position>();
                        var p = push.segments[0];

                        // Move pushable over 1 spaces
                        Point pFront = new Point(p.X - 1, p.Y);
                        push.segments.Insert(0, pFront);

                        // Remove tail
                        push.segments.RemoveAt(push.segments.Count - 1);

                        // Allows to be kept pushing
                        var temp = gameState[front.X - 2, front.Y];
                        gameState[front.X - 2, front.Y] = gameState[front.X - 1, front.Y];
                        gameState[front.X - 1, front.Y] = temp;

                        // Add entity to new spot, clear previous spot
                        var temp2 = gameState[front.X - 1, front.Y];
                        gameState[front.X - 1, front.Y] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp2;

                        move(entity, -1, 0);

                    }

                    break;

                case Components.Direction.Right:
                    // Not stoppable of Pushable we can move
                    if (!gameState[front.X + 1, front.Y].ContainsComponent<Components.Stoppable>() && !gameState[front.X + 1, front.Y].ContainsComponent<Components.Pushable>())
                    {
                        // Add entity to new spot, clear previous spot
                        var temp = gameState[front.X + 1, front.Y];
                        gameState[front.X + 1, front.Y] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp;
                        move(entity, 1, 0);
                        
                    }

                    // Check for Pushable
                    if (gameState[front.X + 1, front.Y].ContainsComponent<Components.Pushable>() && !gameState[front.X + 2, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        var push = gameState[front.X + 1, front.Y].GetComponent<Components.Position>();
                        var p = push.segments[0];

                        // Move pushable over 1 spaces
                        Point pFront = new Point(p.X + 1, p.Y);
                        push.segments.Insert(0, pFront);

                        // Remove tail
                        push.segments.RemoveAt(push.segments.Count - 1);

                        // Allows to be kept pushing
                        var temp = gameState[front.X + 2, front.Y];
                        gameState[front.X + 2, front.Y] = gameState[front.X + 1, front.Y];
                        gameState[front.X + 1, front.Y] = temp;

                        // Add entity to new spot, clear previous spot
                        var temp2 = gameState[front.X + 1, front.Y];
                        gameState[front.X + 1, front.Y] = gameState[front.X, front.Y];
                        gameState[front.X, front.Y] = temp2;

                        move(entity, 1, 0);

                    }

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

            //foreach (Movement in entity)

            Point newFront = new Point(front.X + xIncrement, front.Y + yIncrement);
            position.segments.Insert(0, newFront);

            
        }
    }
}
