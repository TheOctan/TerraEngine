using GameEngine.Core;
using GameEngine.Event;
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
        private ScrollBar bar1;
        private ScrollBar bar2;
        private Lock locker;
        private TextBox textBox1;
        private TextBox textBox2;
        private Button button1;

        public SettingState(Game game) : base(game)
        {
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
            bar1.Value = game.config.Music;

            bar2 = new ScrollBar("Sound");
            bar2.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
            bar2.WidgetEvent += Bar2_WidgetEvent;
            bar2.Value = game.config.Value;

            locker = new Lock("Full screen mode");
            locker.Texture = ResourceHolder.Textures.Get("Widget/lock");
            locker.WidgetEvent += Locker1_WidgetEvent;
            locker.Value = game.config.FullScreen;

            textBox1 = new TextBox(game.config.NickName1);
            textBox1.Texture = ResourceHolder.Textures.Get("Widget/textBox");
            textBox1.WidgetEvent += TextBox1_WidgetEvent;

            textBox2 = new TextBox(game.config.NickName2);
            textBox2.Texture = ResourceHolder.Textures.Get("Widget/textBox");
            textBox2.WidgetEvent += TextBox2_WidgetEvent;

            button1 = new Button("Back");
            button1.Texture = ResourceHolder.Textures.Get("Widget/button");
            button1.WidgetEvent += Button1_WidgetEvent;

            menu = new StackMenu();
            menu.Title = "Settings";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(bar1);
            menu.AddWidget(bar2);
            menu.AddWidget(locker);
            menu.AddWidget(textBox1);
            menu.AddWidget(textBox2);
            menu.AddWidget(button1);

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

        private void Bar1_WidgetEvent(object sender, WidgetEventArgs e)
        {
            game.config.Music = e.Value;
        }

        private void Bar2_WidgetEvent(object sender, WidgetEventArgs e)
        {
            game.config.Value = e.Value;
        }

        private void Locker1_WidgetEvent(object sender, WidgetEventArgs e)
        {
            game.config.FullScreen = locker.Value;

            if(game.config.FullScreen)
            {
                Game.Window.Close();
                Game.Window = new RenderWindow(VideoMode.DesktopMode, Game.GameName, Styles.Fullscreen);
                Game.Window.SetFramerateLimit(60);

                game.Subscribe();
                menu.Subscribe();
            }
            else
            {
                Game.Window.Close();
                Game.Window = new RenderWindow(VideoMode.DesktopMode, Game.GameName, Styles.Close);
                Game.Window.SetFramerateLimit(60);

                game.Subscribe();
                menu.Subscribe();
            }
        }

        private void TextBox1_WidgetEvent(object sender, WidgetEventArgs e)
        {
            game.config.NickName1 = e.Message;
        }

        private void TextBox2_WidgetEvent(object sender, WidgetEventArgs e)
        {
            game.config.NickName2 = e.Message;
        }

        private void Button1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            menu.Unsubscribe();

			game.config.SaveConfiguration();
            Game.Machine.PopState();
        }
    }
}
