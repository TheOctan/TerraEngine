using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using GameEngine.Core;

namespace GameEngine
{
    class Program
    {
        public static Random Rand { get; private set; }

        static void Main(string[] args)
        {
            Rand = new Random();

            Game game = new Game();
            game.Run();
        }
    }
}
