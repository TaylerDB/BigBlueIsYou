﻿using System;

namespace BigBlueIsYou
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new ECSSnakeGame())
                game.Run();
        }
    }
}