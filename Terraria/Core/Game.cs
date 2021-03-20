using GameEngine.States;
using GameEngine.States.StateMachine;
using GameEngine.Util;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;

namespace GameEngine.Core
{
    public class Game
    {
        public static RenderWindow Window { get; set; }
        public static StateMachine stateMachine { get; private set; }

        public static readonly string GameName = "Terraria demo_0.0.5";
        public static bool VisibleCounter = false;

        public IConfigurator config;
        private FPSCounter counter;

        public Game(IConfigurator configurator)
        {
            config = configurator;
            config.LoadConfiguration();

            stateMachine = new StateMachine();

            Window = new RenderWindow(new VideoMode(1366, 768), GameName, config.FullScreen ? Styles.Fullscreen : Styles.Close);
            Window.SetFramerateLimit(60);

            InitEvents();

            counter = new FPSCounter()
            {
                Text = GameName
            };

            stateMachine.PushState(new MainMenuState(this));
        }

        public void Run()
        {
            const uint TPS = 60;
            Time dt = Time.FromSeconds(1.0f / TPS);

            Clock timer = new Clock();
            var lastTime = Time.Zero;
            var accumulator = Time.Zero;

            while (Window.IsOpen && !stateMachine.Empty)
            {
                var state = stateMachine.CurrentState;

                // get times
                var time = timer.ElapsedTime;
                var frameTime = time - lastTime;
                if (frameTime > Time.FromSeconds(0.25f))
                {
                    frameTime = Time.FromSeconds(0.25f);
                }

                lastTime = time;
                accumulator += frameTime;

                // real time update
                state.HandleInput();
                counter.Text = $"{GameName} \nCount states: { stateMachine.Count}";
                counter.Update();

                while (accumulator >= dt)
                {
                    // updating with fixed time
                    state.Update(dt);
                    accumulator -= dt;
                }

                float interpolation = (accumulator / dt).AsSeconds();

                // render
                Render(state, interpolation);

                Window.DispatchEvents();
                stateMachine.TryPop();
            }
        }

        public void InitEvents()
        {
            Window.Closed += OnWindowClosed;
            Window.KeyPressed += OnWindowKeyPressed;
        }
        private void Render(StateBase state, float interpolation)
        {
            Window.Clear(Color.Black);
            state.Render(interpolation);

            if (VisibleCounter)
            {
                Window.Draw(counter);
            }

            Window.Display();
        }
        private void OnWindowKeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.Tab)
            {
                VisibleCounter = VisibleCounter ? false : true;
            }
        }
        private void OnWindowClosed(object sender, EventArgs e)
        {
            Window.Close();
        }
    }
}
