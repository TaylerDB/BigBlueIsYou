using Entities;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigBlueIsYou
{
    static class GameLayout
    {
        static Entity[,] gamePos;

        public static bool pushRock = false;
        public static bool youRock = false;

        public static bool gameWin = false;

        public static int windowWidth = 0;
        public static int windowHeight = 0;

        public static ContentManager content;
        public static SpriteBatch spriteBatch;
        public static int levelChoice;
        public static string levelsString;

        public static GameTime gameTime;

        public static Entity[,] GamePos
        {
            get { return gamePos; }
            set { gamePos = value; }
        }

        public static bool PushRock
        {
            get { return pushRock; }
            set { pushRock = value; }
        }


        public static void addToGamePos(int x, int y, Entity entity)
        {
            gamePos[x, y] = entity;
        }
    }
}
