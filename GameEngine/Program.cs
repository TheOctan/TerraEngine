using SFML.Graphics;
using SFML.Window;
using SFML.System;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    class Program
    {
        public static RenderWindow window;

        static void Main(string[] args)
        {
            window = new RenderWindow(new VideoMode(640, 480), "GameEngine");

            window.Closed += Window_Closed;
            window.Resized += Window_Resized;

            while (window.IsOpen)
            {
                window.DispatchEvents();



                window.Clear();

                window.Display();
            }

        }

        private static void Window_Resized(object sender, SizeEventArgs e)
        {
            window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        private static void Window_Closed(object sender, EventArgs e)
        {
            window.Close();
        }
    }
}
