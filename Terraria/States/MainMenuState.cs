using GameEngine.Core;
using GameEngine.GUI;
using GameEngine.Resource;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;

namespace GameEngine.States
{
    public class MainMenuState : StateBase
    {
        private RectangleShape background;
        private StackMenu menu;

        Button startButton;
        Button settingsButton;
        Button exitButton;

        public MainMenuState(Game game) : base(game)
        {}

        public override void Init()
        {
            background = new RectangleShape();
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
            background.Texture = ResourceHolder.Textures.Get("Background/orig");

            startButton = new Button("Start Game");
            startButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            startButton.PressedEvent += OnStaterButtonPressed;

            settingsButton = new Button("Settings");
            settingsButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            settingsButton.PressedEvent += OnSettingsButtonPressed;

            exitButton = new Button("Exit");
            exitButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            exitButton.PressedEvent += OnExitButtonPressed;

            menu = new StackMenu();
            menu.Title = "Main menu";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(startButton);
            menu.AddWidget(settingsButton);
            menu.AddWidget(exitButton);

            menu.Subscribe();

            menu.Origin = menu.Size / 2f;
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
        }

        private void OnStaterButtonPressed(object sender, Event.WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.stateMachine.ChangeState(new StatePlaying(game));
        }

        private void OnSettingsButtonPressed(object sender, Event.WidgetEventArgs e)
        {
            Game.stateMachine.PushState(new SettingState(game));
        }

        private void OnExitButtonPressed(object sender, Event.WidgetEventArgs e)
        {
            Game.stateMachine.Reset();
            //Game.Window.Close();
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(background);
            Game.Window.Draw(menu);
        }

        public override void Update(Time time)
        {
            menu.Update();
        }

        public override void Pause()
        {
            menu.Unsubscribe();
        }

        public override void Resume()
        {
            menu.Subscribe();
        }
    }
}
