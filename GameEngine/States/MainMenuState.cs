using GameEngine.Core;
using GameEngine.GUI;
using GameEngine.Resource;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.States
{
    public class MainMenuState : StateBase
    {
        private RectangleShape background;
        private StackMenu menu;

        Button button1;
        Button button2;
        Button button3;

        public MainMenuState(Game game) : base(game)
        {
            Game.Window.Resized += Window_Resized;
        }

        public override void HandleInput()
        {
        }

        public override void Init()
        {
            background = new RectangleShape();
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
            background.Texture = ResourceHolder.Textures.Get("Background/orig");

            button1 = new Button("Start Game");
            button1.Texture = ResourceHolder.Textures.Get("Widget/button");
            button1.WidgetEvent += Button1_WidgetEvent;

            button2 = new Button("Settings");
            button2.Texture = ResourceHolder.Textures.Get("Widget/button");
            button2.WidgetEvent += Button2_WidgetEvent;

            button3 = new Button("Exit");
            button3.Texture = ResourceHolder.Textures.Get("Widget/button");
            button3.WidgetEvent += Button3_WidgetEvent;

            menu = new StackMenu();
            menu.Title = "Main menu";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(button1);
            menu.AddWidget(button2);
            menu.AddWidget(button3);

            menu.Subscribe();

            menu.Origin = menu.Size / 2f;
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
        }

        private void Button1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            Game.Window.Resized -= Window_Resized;
            menu.Unsubscribe();

            Game.Machine.ChangeState(new StatePlaying(game));
        }

        private void Button2_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            Game.Machine.PushState(new SettingState(game));
        }

        private void Button3_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            Game.Machine.Reset();
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

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
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
