using GameEngine.Core;
using GameEngine.Event;
using GameEngine.GUI;
using GameEngine.Resource;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;

namespace GameEngine.States
{
    public class PauseState : StateBase
    {
        private RectangleShape background;

        private StackMenu menu;
        private Button backButton;
        private Button restartButton;
        private Button menuButton;

        public PauseState(Game game) : base(game)
        {}

        public override void Init()
        {
            background = new RectangleShape();
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
            background.Texture = ResourceHolder.Textures.Get("Background/orig");

            backButton = new Button("Back");
            backButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            backButton.PressedEvent += OnBackButtonPressed;

            restartButton = new Button("Restart");
            restartButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            restartButton.PressedEvent += OnRestartButtonPressed;

            menuButton = new Button("To menu");
            menuButton.Texture = ResourceHolder.Textures.Get("Widget/button");
            menuButton.PressedEvent += OnMenuButtonPressed;

            menu = new StackMenu();
            menu.Title = "Pause";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(backButton);
            menu.AddWidget(restartButton);
            menu.AddWidget(menuButton);

            menu.Subscribe();

            menu.Origin = menu.Size / 2f;
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
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

        private void OnBackButtonPressed(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.stateMachine.PopState();
        }

        private void OnRestartButtonPressed(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.stateMachine.PopState();
            Game.stateMachine.ChangeState(new StatePlaying(game));
        }

        private void OnMenuButtonPressed(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.stateMachine.PopState();
            Game.stateMachine.ChangeState(new MainMenuState(game));
        }
    }
}
