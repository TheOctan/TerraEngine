using GameEngine.GUI;
using GameEngine.Resource;
using GameEngine.States.StateMachine;
using SFML.Graphics;
using SFML.System;
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
        Button button2;

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

            bar2 = new ScrollBar("Sound");
            bar2.Texture = ResourceHolder.Textures.Get("Widget/scrollBar");
            bar2.WidgetEvent += Bar2_WidgetEvent;

            locker1 = new Lock("Full screen mode");
            locker1.Texture = ResourceHolder.Textures.Get("Widget/lock");
            locker1.WidgetEvent += Locker1_WidgetEvent;

            button1 = new Button("Save", Widget.WidgetSize.Small);
            button1.Texture = ResourceHolder.Textures.Get("Widget/button");
            button1.WidgetEvent += Button1_WidgetEvent;

            button2 = new Button("Cansel", Widget.WidgetSize.Small);
            button2.Texture = ResourceHolder.Textures.Get("Widget/button");
            button2.WidgetEvent += Button2_WidgetEvent;

            menu = new StackMenu();
            menu.Title = "Settings";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(bar1);
            menu.AddWidget(bar2);
            menu.AddWidget(locker1);
            menu.AddWidget(button1);
            menu.AddWidget(button2, true);

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

        private void Window_Resized(object sender, SFML.Window.SizeEventArgs e)
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
        }

        private void Button1_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            //Game.Machine.ChangeState(new MainMenuState(Game));
            button1.WidgetEvent -= Button1_WidgetEvent;
            Game.Machine.PopState();
        }

        private void Button2_WidgetEvent(object sender, Event.WidgetEventArgs e)
        {
            //Game.Machine.ChangeState(new MainMenuState(Game));
            button2.WidgetEvent -= Button2_WidgetEvent;
            Game.Machine.PopState();
        }
    }
}
