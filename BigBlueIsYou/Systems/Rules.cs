using BigBlueIsYou;
using Components;
using Entities;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Diagnostics;


namespace Systems
{
    class Rules : System
    {
        Entity[,] gameState;

        bool pushRock;

        public Rules()
            : base(typeof(Components.Appearance))
        {
            
        }

        public override void Update(GameTime gameTime)
        {
            gameState = GameLayout.GamePos;

            for (int r = 0; r < 20; r++)
            {
                for (int c = 0; c < 20; c++)
                {
                    if (gameState[r, c] != null)
                    {
                        var a = gameState[r, c].GetComponent<Components.Appearance>();
                        if (a.image.Name == "Animations/word-is")
                        {
                            // Check left and right
                            if (gameState[r - 1, c] != null && gameState[r + 1, c] != null)
                            {
                                var aL = gameState[r - 1, c].GetComponent<Components.Appearance>();
                                var aR = gameState[r + 1, c].GetComponent<Components.Appearance>();

                                // Rock
                                if (aL.image.Name == "Animations/word-rock" && aR.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }
                                    
                                }

                                else if (aL.image.Name == "Animations/word-rock" && aR.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-rock" && aR.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-rock" && aR.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                // Wall
                                else if (aL.image.Name == "Animations/word-wall" && aR.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-wall" && aR.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-wall" && aR.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-wall" && aR.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                // BigBlue
                                else if (aL.image.Name == "Animations/word-baba" && aR.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        //GameLayout.pushRock = true;

                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Images/BigBlue")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                // Flag
                                else if (aL.image.Name == "Animations/word-flag" && aR.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-flag" && aR.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-flag" && aR.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aL.image.Name == "Animations/word-flag" && aR.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                else
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (name.image.Name != "Animations/word-baba" && name.image.Name != "Animations/word-flag" &&
                                            name.image.Name != "Animations/word-is" && name.image.Name != "Animations/word-kill" &&
                                            name.image.Name != "Animations/word-lava" && name.image.Name != "Animations/word-push" &&
                                            name.image.Name != "Animations/word-rock" && name.image.Name != "Animations/word-sink" &&
                                            name.image.Name != "Animations/word-stop" && name.image.Name != "Animations/word-wall" &&
                                            name.image.Name != "Animations/word-water" && name.image.Name != "Animations/word-win" &&
                                            name.image.Name != "Animations/word-you")
                                        {
                                            //if (entity.ContainsComponent<Components.Movable>())
                                            //{
                                            //    entity.Remove(new Components.Movable());
                                            //}
                                            if (entity.ContainsComponent<Components.Pushable>())
                                            {
                                                entity.Remove(new Components.Pushable());
                                            }
                                            if (entity.ContainsComponent<Components.Stoppable>())
                                            {
                                                entity.Remove(new Components.Stoppable());
                                            }
                                            if (entity.ContainsComponent<Components.Win>())
                                            {
                                                entity.Remove(new Components.Win());
                                            }
                                        }
                                    }
                                }

                            } // Left and Right

                            //// Remove components if "is" is alone
                            //if (gameState[r - 1, c] == null || gameState[r + 1, c] == null)
                            //{
                            //    foreach (var entity in m_entities.Values)
                            //    {
                            //        var name = entity.GetComponent<Components.Appearance>();

                            //        if (name.image.Name != "Animations/word-baba" || name.image.Name != "Animations/word-flag" ||
                            //            name.image.Name != "Animations/word-is" || name.image.Name != "Animations/word-kill" ||
                            //            name.image.Name != "Animations/word-lava" || name.image.Name != "Animations/word-push" ||
                            //            name.image.Name != "Animations/word-rock" || name.image.Name != "Animations/word-sink" ||
                            //            name.image.Name != "Animations/word-stop" || name.image.Name != "Animations/word-wall" ||
                            //            name.image.Name != "Animations/word-water" || name.image.Name != "Animations/word-win" ||
                            //            name.image.Name != "Animations/word-you")
                            //        {
                            //            if (entity.ContainsComponent<Components.Movable>())
                            //            {
                            //                entity.Remove(new Components.Movable());
                            //            }
                            //            if (entity.ContainsComponent<Components.Pushable>())
                            //            {
                            //                entity.Remove(new Components.Pushable());
                            //            }
                            //            if (entity.ContainsComponent<Components.Stoppable>())
                            //            {
                            //                entity.Remove(new Components.Stoppable());
                            //            }
                            //            if (entity.ContainsComponent<Components.Win>())
                            //            {
                            //                entity.Remove(new Components.Win());
                            //            }
                            //        }
                            //    }
                            //}



                            // Check up and down
                            if (gameState[r, c - 1] != null && gameState[r, c + 1] != null)
                            {
                                var aU = gameState[r, c - 1].GetComponent<Components.Appearance>();
                                var aD = gameState[r, c + 1].GetComponent<Components.Appearance>();

                                // Rock
                                if (aU.image.Name == "Animations/word-rock" && aD.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }

                                }

                                else if (aU.image.Name == "Animations/word-rock" && aD.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-rock" && aD.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-rock" && aD.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/rock")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                // Wall
                                else if (aU.image.Name == "Animations/word-wall" && aD.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-wall" && aD.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-wall" && aD.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-wall" && aD.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/wall")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                // BigBlue
                                else if (aU.image.Name == "Animations/word-baba" && aD.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        //GameLayout.pushRock = true;

                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Images/BigBlue")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                // Flag
                                else if (aU.image.Name == "Animations/word-flag" && aD.image.Name == "Animations/word-you")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Movable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Movable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-flag" && aD.image.Name == "Animations/word-push")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Pushable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Pushable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-flag" && aD.image.Name == "Animations/word-stop")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Stoppable>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Stoppable());
                                        }
                                    }
                                }

                                else if (aU.image.Name == "Animations/word-flag" && aD.image.Name == "Animations/word-win")
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (!entity.ContainsComponent<Components.Win>() && name.image.Name == "Animations/flag")
                                        {
                                            entity.Add(new Components.Win());
                                        }
                                    }
                                }

                                else
                                {
                                    foreach (var entity in m_entities.Values)
                                    {
                                        var name = entity.GetComponent<Components.Appearance>();

                                        if (name.image.Name != "Animations/word-baba" && name.image.Name != "Animations/word-flag" &&
                                            name.image.Name != "Animations/word-is" && name.image.Name != "Animations/word-kill" &&
                                            name.image.Name != "Animations/word-lava" && name.image.Name != "Animations/word-push" &&
                                            name.image.Name != "Animations/word-rock" && name.image.Name != "Animations/word-sink" &&
                                            name.image.Name != "Animations/word-stop" && name.image.Name != "Animations/word-wall" &&
                                            name.image.Name != "Animations/word-water" && name.image.Name != "Animations/word-win" &&
                                            name.image.Name != "Animations/word-you")
                                        {
                                            //if (entity.ContainsComponent<Components.Movable>())
                                            //{
                                            //    entity.Remove(new Components.Movable());
                                            //}
                                            if (entity.ContainsComponent<Components.Pushable>())
                                            {
                                                entity.Remove(new Components.Pushable());
                                            }
                                            if (entity.ContainsComponent<Components.Stoppable>())
                                            {
                                                entity.Remove(new Components.Stoppable());
                                            }
                                            if (entity.ContainsComponent<Components.Win>())
                                            {
                                                entity.Remove(new Components.Win());
                                            }
                                        }
                                    }
                                }

                            } // Up and Down


                        } // Word is
                    }
                } // Col
            } // Rows


        }


        private List<Entity> findAppearance(Dictionary<uint, Entity> entities)
        {
            var appearance = new List<Entity>();

            foreach (var entity in m_entities.Values)
            {
                if (entity.ContainsComponent<Components.Appearance>())
                {
                    appearance.Add(entity);
                }
            }

            return appearance;
        }
    }
}
