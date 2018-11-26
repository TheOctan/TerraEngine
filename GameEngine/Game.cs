﻿using GameEngine.States;
using GameEngine.States.StateMachine;
using GameEngine.Util;
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
        public static RenderWindow Window { get; private set; }
        public static StateMachine Machine { get; private set; }

        private FPSCounter counter;

        public Game()
        {
            Machine = new StateMachine();
            Window = new RenderWindow(new VideoMode(1300, 650), "Game Engine");
            Window.SetFramerateLimit(60);

            Window.Closed += Window_Closed;
            Window.Resized += Window_Resized;

            counter = new FPSCounter();

            Machine.PushState(new MainMenuState(this));
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

            while (Window.IsOpen && !Machine.Empty)
            {
                var state = Machine.GetCurrentState();

                // get times
                var time        = timer.ElapsedTime;
                var frameTime   = time - lastTime;
                if (frameTime > Time.FromSeconds(0.25f)) frameTime = Time.FromSeconds(0.25f);

                lastTime = time;
                accumulator += frameTime;

                // real time update
                state.HandleInput();
                counter.Update();

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
                Window.Draw(counter);
                Window.Display();

                Window.DispatchEvents();
                Machine.TryPop();
            }
        }
    }
}
