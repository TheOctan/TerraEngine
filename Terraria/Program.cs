using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GameEngine.Core;
using GameEngine.Core.impl;

namespace GameEngine
{
    class Program
    {
        public static Random Rand { get; private set; }

        static void Main(string[] args)
        {
            Rand = new Random();

			Game game = new Game(new ReestryConfigurator());

            game.Run();
        }
    }
}
