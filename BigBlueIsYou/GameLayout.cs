using Entities;
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
