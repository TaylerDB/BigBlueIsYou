using BigBlueIsYou;
using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;


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

        HelpView helpView = new HelpView();
        GamePlayView gamePlayView = new GamePlayView();

        GameModel gameModel = new GameModel(GameLayout.windowWidth, GameLayout.windowHeight);
        

        bool canMove = true;
        Entity[,] gameState;

        int count = 0;

        private SoundEffect m_move;
        private SoundEffect m_horn;

        public Movement()
            : base(
                  typeof(Components.Movable),
                  typeof(Components.Position)                  
                  )
        {
            oldState = Keyboard.GetState();
        }

        public void LoadContent(ContentManager content)
        {
            m_move = content.Load<SoundEffect>("Music/blipSelect");
            m_horn = content.Load<SoundEffect>("Music/horn");
        }

        public override void Update(GameTime gameTime)
        {
            gameState = GameLayout.GamePos;
            
            processInput(gameTime);

        }

        private void processInput(GameTime gameTime)
        {
            // Get keyboard state
            KeyboardState newState = Keyboard.GetState();

            if (newState.IsKeyDown(helpView.MoveUp))
            {
                if (!oldState.IsKeyDown(helpView.MoveUp))
                {
                    m_move.Play(.3f, 1f, 0f);
                    foreach (var entity in m_entities.Values)
                    {
                        moveEntity(entity, Components.Direction.Up);
                    }
                }
            }
                
            if (newState.IsKeyDown(helpView.MoveDown))
            {
                if (!oldState.IsKeyDown(helpView.MoveDown))
                {
                    m_move.Play(.3f, 1f, 0f);
                    foreach (var entity in m_entities.Values)
                    {
                        moveEntity(entity, Components.Direction.Down);
                    }
                }
            }

            if (newState.IsKeyDown(helpView.MoveRight))
            {
                if (!oldState.IsKeyDown(helpView.MoveRight))
                {
                    m_move.Play(.3f, 1f, 0f);
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        moveEntity(entity, Components.Direction.Right);
                    }
                }
            }

            if (newState.IsKeyDown(helpView.MoveLeft))
            {
                if (!oldState.IsKeyDown(helpView.MoveLeft))
                {
                    m_move.Play(.3f, 1f, 0f);
                    
                    foreach (var entity in m_entities.Values)
                    {
                        var movable = entity.GetComponent<Components.Movable>();
                        moveEntity(entity, Components.Direction.Left);
                    }
                }
            }

            if (newState.IsKeyDown(helpView.Reset))
            {
                if (!oldState.IsKeyDown(helpView.Reset))
                {
                    Debug.WriteLine("r hit");
                    GameModel gameModel2 = new GameModel(gameModel);
                    gameModel2.Initialize(GameLayout.content, GameLayout.spriteBatch, GameLayout.levelChoice, GameLayout.levelsString);
                    //gameModel2.Update(GameLayout.gameTime);
                    //gameModel2.Draw(GameLayout.gameTime);

                }
            }

            // Update saved state
            oldState = newState;
        }

        private bool moveEntity(Entities.Entity entity, Components.Direction dir)
        {

            //var movable = entity.GetComponent<Components.Movable>();
            var position = entity.GetComponent<Components.Position>();
            var stopable = entity.GetComponent<Components.Appearance>();
            var front = position;

            switch (dir)
            {
                case Components.Direction.Up:
                    if (gameState[front.X, front.Y - 1] == null)
                    {
                        move(entity, 0, -1);
                        return true;
                    }

                    // Check for winning
                    if (gameState[front.X, front.Y - 1].ContainsComponent<Components.Win>())
                    {
                        m_horn.Play();
                        GameLayout.gameWin = true;
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
                        move(entity, 0, -1);
                    }

                    break;

                case Components.Direction.Down:
                    if (gameState[front.X, front.Y + 1] == null)
                    {
                        move(entity, 0, 1);
                        return true;
                    }

                    // Check for winning
                    if (gameState[front.X, front.Y + 1].ContainsComponent<Components.Win>())
                    {
                        m_horn.Play();
                        GameLayout.gameWin = true;
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
                        move(entity, 0, 1);
                    }

                    break;

                case Components.Direction.Left:
                    if (gameState[front.X - 1, front.Y] == null)
                    {
                        move(entity, -1, 0);
                        return true;
                    }

                    // Check for winning
                    if (gameState[front.X - 1, front.Y].ContainsComponent<Components.Win>())
                    {
                        m_horn.Play();
                        GameLayout.gameWin = true;
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
                        move(entity, -1, 0);
                    }

                    break;

                case Components.Direction.Right:
                    if (gameState[front.X + 1, front.Y] == null)
                    {
                        move(entity, 1, 0);
                        return true;
                    }

                    // Check for winning
                    if (gameState[front.X + 1, front.Y].ContainsComponent<Components.Win>())
                    {
                        m_horn.Play();
                        GameLayout.gameWin = true;
                        move(entity, 1, 0);
                        //entity.Remove(new Components.Movable());
                        return true;
                    }

                    if (gameState[front.X + 1, front.Y].ContainsComponent<Components.Kill>())
                    {
                        //entity.Remove(new Components.Position(front.X, front.Y));
                        //entity.Clear();
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

            //if (gameState[position.X, position.Y].GetComponent<Components.Position>() == null)
            //{
                gameState[position.X, position.Y] = null;
            //}
            //else if (gameState[position.X, position.Y].GetComponent<Components.Position>() != null)
            //{
            //    var temp = gameState[position.X, position.Y];
            //    gameState[position.X, position.Y] = null;
            //    gameState[position.X, position.Y] = temp;
            //}

            gameState[position.X + xIncrement, position.Y + yIncrement] = entity;

            position.X += xIncrement;
            position.Y += yIncrement;
        }
    }
}
