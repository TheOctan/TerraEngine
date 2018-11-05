using GameEngine.States;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    public class Game
    {
        public Game()
        {
            Machine = new StateMachine();
            Window = new RenderWindow(new VideoMode(640, 480), "Game Engine");
            Window.SetFramerateLimit(60);

            Window.Closed += Window_Closed;
            Window.Resized += Window_Resized;

            Machine.PushState(new StatePlaying(this));
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            Window.SetView(new View(new FloatRect(0, 0, e.Width, e.Height)));
        }

        public void Run()
        {
            const uint  TPS = 60;
            Time dt = Time.FromSeconds(1.0f / TPS);
            //uint ticks = 0;

            Clock timer = new Clock();
            var lastTime    = Time.Zero;
            var accumulator = Time.Zero;

            while (Window.IsOpen)
            {
                var state = Machine.GetCurrentState();

                // get times
                var time = timer.ElapsedTime;
                var frameTime = time - lastTime;
                if (frameTime > Time.FromSeconds(0.25f)) frameTime = Time.FromSeconds(0.25f);

                // real time update
                state.HandleInput();

                while (accumulator >= dt)
                {
                    //ticks++;
                    // updating with fixed time
                    state.Update(dt);
                    accumulator -= dt;
                }

                var interpolation = (accumulator / dt).AsSeconds();
                //State state = currentState * interpolation + previousState * (1.0 - interpolation);


                // render
                Window.Clear(Color.Black);
                state.Render(interpolation);
                Window.Display();

                Window.DispatchEvents();
                Machine.TryPop();
            }
        }

        public static RenderWindow Window { get; set; }
        public StateMachine Machine { get; set; }
    }
}
