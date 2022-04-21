using Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace BigBlueIsYou
{
    static class GameLayout
    {
        static Entity[,] gamePos;

        public static Entity[,] GamePos
        {
            get { return gamePos; }
            set { gamePos = value; }
        }

        public static void addToGamePos(int x, int y, Entity entity)
        {
            gamePos[x, y] = entity;
        }
    }
}
