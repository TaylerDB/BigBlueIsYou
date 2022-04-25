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

        int count = 0;

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
            //var dir = Components.Direction;
//            Components.Direction = dir;
            processInput();
            //collision();
            //foreach (var entity in m_entities.Values)
            //{
            //    moveEntity(entity, gameTime, dir);
            //}
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
                        //movable.facing = Components.Direction.Up;

                        // TODO: move(entity, Components.Direction.Up);
                        moveEntity(entity, Components.Direction.Up);
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
                        //movable.facing = Components.Direction.Down;
                        moveEntity(entity, Components.Direction.Down);
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
                        //movable.facing = Components.Direction.Right;
                        moveEntity(entity, Components.Direction.Right);
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
                        //movable.facing = Components.Direction.Left;
                        moveEntity(entity, Components.Direction.Left);
                    }
                }
            }                

            // Update saved state
            oldState = newState;
        }

        private bool moveEntity(Entities.Entity entity, Components.Direction dir)
        {

            //var movable = entity.GetComponent<Components.Movable>();
            var position = entity.GetComponent<Components.Position>();
            //var stopable = entity.GetComponent<Components.Stopable>();
            var front = position;

            switch (dir)
            {
                case Components.Direction.Up:
                    if (gameState[front.X, front.Y - 1] == null)
                    {
                        move(entity, 0, -1);
                        return true;
                    }

                    if (gameState[front.X, front.Y - 1].ContainsComponent<Components.Stoppable>())
                    {
                        return false;
                    }

                    if (gameState[front.X, front.Y - 1].ContainsComponent<Components.Pushable>())
                    {
                        if (moveEntity(gameState[front.X, front.Y - 1], Components.Direction.Up))
                        {
                            move(entity, 0, -1);
                            return true;
                        }
                        else
                            return false;

                    }

                    if (gameState[front.X, front.Y - 1] != null || !gameState[front.X, front.Y - 1].ContainsComponent<Components.Stoppable>())
                    {
                        move(entity, 1, 0);
                    }

                    break;

                case Components.Direction.Down:
                    if (gameState[front.X, front.Y + 1] == null)
                    {
                        move(entity, 0, 1);
                        return true;
                    }

                    if (gameState[front.X, front.Y + 1].ContainsComponent<Components.Stoppable>())
                    {
                        return false;
                    }

                    if (gameState[front.X, front.Y + 1].ContainsComponent<Components.Pushable>())
                    {
                        if (moveEntity(gameState[front.X, front.Y + 1], Components.Direction.Down))
                        {
                            move(entity, 0, 1);
                            return true;
                        }
                        else
                            return false;

                    }

                    if (gameState[front.X, front.Y + 1] != null || !gameState[front.X, front.Y + 1].ContainsComponent<Components.Stoppable>())
                    {
                        move(entity, 1, 0);
                    }

                    break;

                case Components.Direction.Left:
                    if (gameState[front.X - 1, front.Y] == null)
                    {
                        move(entity, -1, 0);
                        return true;
                    }

                    if (gameState[front.X - 1, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        return false;
                    }

                    if (gameState[front.X - 1, front.Y].ContainsComponent<Components.Pushable>())
                    {
                        if (moveEntity(gameState[front.X - 1, front.Y], Components.Direction.Left))
                        {
                            move(entity, -1, 0);
                            return true;
                        }
                        else
                            return false;
                    }

                    if (gameState[front.X - 1, front.Y] != null || !gameState[front.X - 1, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        move(entity, 1, 0);
                    }

                    break;

                case Components.Direction.Right:
                    if (gameState[front.X + 1, front.Y] == null)
                    {
                        move(entity, 1, 0);
                        return true;
                    }
                    
                    if (gameState[front.X + 1, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        return false;
                    }

                    if (gameState[front.X + 1, front.Y].ContainsComponent<Components.Pushable>())
                    {
                        if (moveEntity(gameState[front.X + 1, front.Y], Components.Direction.Right))
                        {
                            move(entity, 1, 0);
                            return true;
                        }
                        else
                            return false;
                    }

                    if (gameState[front.X + 1, front.Y] != null || !gameState[front.X + 1, front.Y].ContainsComponent<Components.Stoppable>())
                    {
                        move(entity, 1, 0);
                    }

                    break;
            }

            return false;
        }

        private void move(Entities.Entity entity, int xIncrement, int yIncrement)
        {
            var position = entity.GetComponent<Components.Position>();

            gameState[position.X, position.Y] = null;
            gameState[position.X + xIncrement, position.Y + yIncrement] = entity;

            position.X += xIncrement;
            position.Y += yIncrement;
        }
    }
}
