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
    class SettingState : StateBase
    {
        private RectangleShape background;
        private StackMenu menu;

        ScrollBar bar1;
        ScrollBar bar2;
        Lock locker1;
        Button button1;

        public SettingState(Game game) : base(game)
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

            bar1 = new ScrollBar("Music");
            bar1.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
            bar1.WidgetEvent += Bar1_WidgetEvent;
            bar1.Value = game.settings.Music;

            bar2 = new ScrollBar("Sound");
            bar2.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
            bar2.WidgetEvent += Bar2_WidgetEvent;
            bar2.Value = game.settings.Value;

            locker1 = new Lock("Full screen mode");
            locker1.Texture = ResourceHolder.Textures.Get("Widget/lock");
            locker1.WidgetEvent += Locker1_WidgetEvent;
            locker1.Value = game.settings.FullScreen;

            button1 = new Button("Back");
            button1.Texture = ResourceHolder.Textures.Get("Widget/button");
            button1.WidgetEvent += Button1_WidgetEvent;

            menu = new StackMenu();
            menu.Title = "Settings";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(bar1);
            menu.AddWidget(bar2);
            menu.AddWidget(locker1);
            menu.AddWidget(button1);

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

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            background.Size = new Vector2f(Game.Window.Size.X, Game.Window.Size.Y);
        }

        private void Bar1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {

        }

        private void Bar2_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {

        }

        private void Locker1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            game.settings.FullScreen = locker1.Value;

            if(game.settings.FullScreen)
            {
                //Game.Window = new RenderWindow(VideoMode.DesktopMode, Game.GameName, Styles.Default);
            }
            else
            {
                //Game.Window = new RenderWindow(VideoMode.DesktopMode, Game.GameName, Styles.Default);
            }
        }

        private void Button1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            //Game.Machine.ChangeState(new MainMenuState(Game));
            button1.WidgetEvent -= Button1_WidgetEvent;
            Game.Machine.PopState();
        }
    }
}
