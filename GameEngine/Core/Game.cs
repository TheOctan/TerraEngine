using GameEngine.States;
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

namespace GameEngine.Core
{
    public class Game
    {
        public static RenderWindow Window { get; set; }
        public static StateMachine Machine { get; private set; }

        public static readonly string GameName = "\nterraria demo_0.0.5";
        public static bool VisibleCounter = false;

        public Settings settings;
        private FPSCounter counter;

        public Game(bool fullScreen = true)
        {
            settings = new Settings()
            {
                Music = 100,
                Value = 100,
                FullScreen = fullScreen,
                NickName1 = "Player 0",
                NickName2 = "Player 1"
            };

            Machine = new StateMachine();

            Window = fullScreen ? 
                new RenderWindow(new VideoMode(1366, 768), GameName, Styles.Fullscreen) 
                : new RenderWindow(new VideoMode(1366, 768), GameName, Styles.Close);

            Window.SetFramerateLimit(60);

            Subscribe();

            counter = new FPSCounter();
            counter.Text = GameName;

            Machine.PushState(new MainMenuState(this));
        }

        public void Subscribe()
        {
            Window.Closed += Window_Closed;
            Window.KeyPressed += Window_KeyPressed;
        }

        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Tab)
            {
                VisibleCounter = VisibleCounter ? false : true;
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            Window.Close();
        }

        public void Run()
        {
            const uint TPS = 60;
            Time dt = Time.FromSeconds(1.0f / TPS);
            //uint ticks = 0;

            Clock timer = new Clock();
            var lastTime = Time.Zero;
            var accumulator = Time.Zero;

            while (Window.IsOpen && !Machine.Empty)
            {
                var state = Machine.CurrentState;

                // get times
                var time = timer.ElapsedTime;
                var frameTime = time - lastTime;
                if (frameTime > Time.FromSeconds(0.25f)) frameTime = Time.FromSeconds(0.25f);

                lastTime = time;
                accumulator += frameTime;

                // real time update
                state.HandleInput();
                counter.Text = GameName + "\ncount states: " + Machine.Count.ToString();
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
                if (VisibleCounter)
                    Window.Draw(counter);
                Window.Display();

                Window.DispatchEvents();
                Machine.TryPop();
            }
        }
    }
}
