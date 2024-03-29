﻿using BigBlueIsYou.Views;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace BigBlueIsYou
{
    public class ECSBigBlueIsYou : Game
    {
        private GraphicsDeviceManager m_graphics;
        private IGameState m_currentState;
        private GameStateEnum m_nextStateEnum = GameStateEnum.MainMenu;
        private Dictionary<GameStateEnum, IGameState> m_states;

        public ECSBigBlueIsYou()
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // Set window size preferences
            m_graphics.IsFullScreen = false;
            m_graphics.PreferredBackBufferWidth = 1440;
            m_graphics.PreferredBackBufferHeight = 900;

            m_graphics.ApplyChanges();

            // Create all the game states here
            m_states = new Dictionary<GameStateEnum, IGameState>();
            m_states.Add(GameStateEnum.MainMenu, new MainMenuView());
            m_states.Add(GameStateEnum.Levels, new Views.LevelsView());
            m_states.Add(GameStateEnum.GamePlay, new GamePlayView());
            m_states.Add(GameStateEnum.Help, new HelpView());
            m_states.Add(GameStateEnum.About, new AboutView());

            // We are starting with the main menu
            m_currentState = m_states[GameStateEnum.MainMenu];

            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Give all game states a chance to load their content
            foreach (var item in m_states)
            {
                item.Value.initialize(this.GraphicsDevice, m_graphics);
                item.Value.loadContent(this.Content);
            }
        }

        protected override void Update(GameTime gameTime)
        {
            m_nextStateEnum = m_currentState.processInput(gameTime);

            // Special case for exiting the game
            if (m_nextStateEnum == GameStateEnum.Exit)
            {
                Exit();
            }

            m_currentState.update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            m_currentState.render(gameTime);

            if (m_currentState != m_states[m_nextStateEnum])
            {
                if (m_nextStateEnum == GameStateEnum.GamePlay)
                {
                    LevelsView viewLevels = (LevelsView)m_states[GameStateEnum.Levels];
                    GamePlayView viewGame = (GamePlayView)m_states[GameStateEnum.GamePlay];

                    viewGame.setLevel(viewLevels.CurrentSelection, viewLevels.LevelsString);
                }

                m_currentState = m_states[m_nextStateEnum];
                m_currentState.initializeSession();
            }

            base.Draw(gameTime);
        }
    }
}
