﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.IsolatedStorage;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BigBlueIsYou
{
    public class HelpView : GameStateView
    {
        // Fonts
        private SpriteFont m_font;
        private SpriteFont m_fontHelp;
        private SpriteFont m_fontHelpSelect;

        private const string CONTROLS = "Controls:";

        // Strings that can be modified
        private string S_MOVEUP = "Up";
        private string S_MOVEDOWN = "Down";
        private string S_MOVELEFT = "Left";
        private string S_MOVERIGHT = "Right";

        private bool captureUserKey = false;
        private bool upKey = false;
        private bool downKey = false;
        private bool leftKey = false;
        private bool rightKey = false;


        private enum ControlsState
        {
            Up,
            Down,
            Left,
            Right
        }

        private ControlsState m_currentSelection = ControlsState.Up;
        private bool m_waitForKeyRelease = false;

        //
        // Safeguard against multiple save/load happening at the same time
        private bool saving = false;
        private bool loading = false;

        // For key presses
        KeyboardState oldState;

        List<Keys> keyList = new List<Keys>();
        List<Keys> keyListUp = new List<Keys>();

        private static Keys V_MOVEUP = Keys.Up;
        private static Keys V_MOVEDOWN = Keys.Down;
        private static Keys V_MOVELEFT = Keys.Left;
        private static Keys V_MOVERIGHT = Keys.Right;

        public Keys MoveUp
        {
            get { return V_MOVEUP; }
            set { V_MOVEUP = value; }
        }

        public Keys MoveDown
        {
            get { return V_MOVEDOWN; }
            set { V_MOVEDOWN = value; }
        }

        public Keys MoveLeft
        {
            get { return V_MOVELEFT; }
            set { V_MOVELEFT = value; }
        }

        public Keys MoveRight
        {
            get { return V_MOVERIGHT; }
            set { V_MOVERIGHT = value; }
        }

        public override void loadContent(ContentManager contentManager)
        {
            m_fontHelp = contentManager.Load<SpriteFont>("Fonts/menu");
            m_fontHelpSelect = contentManager.Load<SpriteFont>("Fonts/menu-select");

            oldState = Keyboard.GetState();

            // TODO: Figure out why loading doesn't work
            loadSomething();

        }

        //static KeyboardState currentKeyState;
        //static KeyboardState previousKeyState;

        public override GameStateEnum processInput(GameTime gameTime)
        {
            bool enterPressed = false;

            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                return GameStateEnum.MainMenu;
            }

            if (captureUserKey)
            {
                if (Keyboard.GetState().GetPressedKeys().Length > 0)
                {
                    Keys userKey = Keyboard.GetState().GetPressedKeys()[0];
                    captureUserKey = false;
                    // Do whatever is needed to assign that key to that control
                    //V_MOVEUP = userKey;

                    if (upKey && userKey != Keys.Enter)
                    {
                        V_MOVEUP = userKey;
                        // Update string
                        S_MOVEUP = userKey.ToString();
                        upKey = false;
                    }

                    if (downKey && userKey != Keys.Enter)
                    {
                        V_MOVEDOWN = userKey;
                        // Update string
                        S_MOVEDOWN = userKey.ToString();
                        downKey = false;
                    }

                    if (leftKey && userKey != Keys.Enter)
                    {
                        V_MOVELEFT = userKey;
                        // Update string
                        S_MOVELEFT = userKey.ToString();
                        leftKey = false;
                    }

                    if (rightKey && userKey != Keys.Enter)
                    {
                        V_MOVERIGHT = userKey;
                        // Update string
                        S_MOVERIGHT = userKey.ToString();
                        rightKey = false;
                    }

                    saveSomething();
                }
            }

            if (!m_waitForKeyRelease)
            {
                // Arrow keys to navigate the menu
                if (Keyboard.GetState().IsKeyDown(Keys.Down) && m_currentSelection != ControlsState.Right && !enterPressed)
                {
                    m_currentSelection = m_currentSelection + 1;
                    m_waitForKeyRelease = true;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.Up) && m_currentSelection != ControlsState.Up && !enterPressed)
                {
                    m_currentSelection = m_currentSelection - 1;
                    m_waitForKeyRelease = true;
                }

                // TODO: Add hitting enter logic
                KeyboardState state = Keyboard.GetState();
                System.Text.StringBuilder sb = new StringBuilder();

                // Add each key press to keyList
                foreach (var key in state.GetPressedKeys())
                {
                    keyList.Add(key);
                    sb.Append("Key: ").Append(key).Append(" pressed ");
                }

                if (sb.Length > 0)
                    Debug.WriteLine(sb.ToString());

                // Get keyboard state
                KeyboardState newState = Keyboard.GetState();

                // If enter is pressed, return the appropriate new state
                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == ControlsState.Up)
                {
                    captureUserKey = true;
                    upKey = true;
                    m_waitForKeyRelease = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == ControlsState.Down)
                {
                    captureUserKey = true;
                    downKey = true;
                    m_waitForKeyRelease = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == ControlsState.Left)
                {
                    captureUserKey = true;
                    leftKey = true;
                    m_waitForKeyRelease = true;
                }

                if (Keyboard.GetState().IsKeyDown(Keys.Enter) && m_currentSelection == ControlsState.Right)
                {
                    captureUserKey = true;
                    rightKey = true;
                    m_waitForKeyRelease = true;
                }

                // Update saved state
                oldState = newState;

            }

            else if (Keyboard.GetState().IsKeyUp(Keys.Down) && Keyboard.GetState().IsKeyUp(Keys.Up)
                    && Keyboard.GetState().IsKeyUp(Keys.Left) && Keyboard.GetState().IsKeyUp(Keys.Right))
            {
                m_waitForKeyRelease = false;
            }

            return GameStateEnum.Help;
        }

        /// <summary>
        /// Demonstrates how serialize an object to storage
        /// </summary>
        private void saveSomething()
        {
            lock (this)
            {
                if (!this.saving)
                {
                    this.saving = true;
                    //
                    // Create something to save
                    ControlSaveState myControlState = new ControlSaveState(V_MOVEUP, V_MOVEDOWN, V_MOVELEFT, V_MOVERIGHT);
                    finalizeSaveAsync(myControlState);
                }
            }
        }

        private async void finalizeSaveAsync(ControlSaveState controlState)
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        using (IsolatedStorageFileStream fs = storage.OpenFile("Controls.xml", FileMode.OpenOrCreate))
                        {
                            if (fs != null)
                            {
                                XmlSerializer mySerializer = new XmlSerializer(typeof(ControlSaveState));
                                mySerializer.Serialize(fs, controlState);
                                Debug.WriteLine("Saved!");
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code
                        Debug.WriteLine("Error occurred");
                    }
                }

                this.saving = false;
            });
        }

        /// <summary>
        /// Demonstrates how to deserialize an object from storage device
        /// </summary>
        private void loadSomething()
        {
            lock (this)
            {
                if (!this.loading)
                {
                    this.loading = true;
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                    finalizeLoadAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                }
            }
        }
        private ControlSaveState m_loadedState = null;

        private async Task finalizeLoadAsync()
        {
            await Task.Run(() =>
            {
                using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    try
                    {
                        if (storage.FileExists("Controls.xml"))
                        {
                            using (IsolatedStorageFileStream fs = storage.OpenFile("Controls.xml", FileMode.Open))
                            {
                                if (fs != null)
                                {
                                    XmlSerializer mySerializer = new XmlSerializer(typeof(ControlSaveState));
                                    m_loadedState = (ControlSaveState)mySerializer.Deserialize(fs);

                                    // Set keys
                                    V_MOVEUP = m_loadedState.Up;
                                    V_MOVEDOWN = m_loadedState.Down;
                                    V_MOVELEFT = m_loadedState.Left;
                                    V_MOVERIGHT = m_loadedState.Right;

                                    S_MOVEUP = m_loadedState.Up.ToString();
                                    S_MOVEDOWN = m_loadedState.Down.ToString();
                                    S_MOVELEFT = m_loadedState.Left.ToString();
                                    S_MOVERIGHT = m_loadedState.Right.ToString();
                                }
                            }
                        }
                    }
                    catch (IsolatedStorageException)
                    {
                        // Ideally show something to the user, but this is demo code :)
                    }
                }

                this.loading = false;
            });
        }

        public override void render(GameTime gameTime)
        {
            m_spriteBatch.Begin();

            // Controls text
            Vector2 stringSize = m_fontHelpSelect.MeasureString(CONTROLS);
            m_spriteBatch.DrawString(m_fontHelpSelect, CONTROLS,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, m_graphics.PreferredBackBufferHeight / 5 - stringSize.Y), Color.Blue);

            // Draw selectable items
            float bottom = drawHelpItem(
                m_currentSelection == ControlsState.Up ? m_fontHelpSelect : m_fontHelp,
                "Move up - " + S_MOVEUP,
                200,
                m_currentSelection == ControlsState.Up ? Color.Yellow : Color.Blue);
            bottom = drawHelpItem(m_currentSelection == ControlsState.Down ? m_fontHelpSelect : m_fontHelp, "Move down - " + S_MOVEDOWN, bottom, m_currentSelection == ControlsState.Down ? Color.Yellow : Color.Blue);
            bottom = drawHelpItem(m_currentSelection == ControlsState.Left ? m_fontHelpSelect : m_fontHelp, "Move left - " + S_MOVELEFT, bottom, m_currentSelection == ControlsState.Left ? Color.Yellow : Color.Blue);
            drawHelpItem(m_currentSelection == ControlsState.Right ? m_fontHelpSelect : m_fontHelp, "Move right - " + S_MOVERIGHT, bottom, m_currentSelection == ControlsState.Right ? Color.Yellow : Color.Blue);
            

            m_spriteBatch.End();
        }

        private float drawHelpItem(SpriteFont font, string text, float y, Color color)
        {
            Vector2 stringSize = font.MeasureString(text);
            m_spriteBatch.DrawString(
                font,
                text,
                new Vector2(m_graphics.PreferredBackBufferWidth / 2 - stringSize.X / 2, y),
                color);

            return y + stringSize.Y;
        }

        public override void update(GameTime gameTime)
        {
        }
    }
}
