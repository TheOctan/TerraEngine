﻿using GameEngine.Core;
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
    public class PauseState : StateBase
    {
        private RectangleShape background;

        private StackMenu menu;
        private Button button1;
        private Button button2;
        private Button button3;


        public PauseState(Game game) : base(game)
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

            button1 = new Button("Back");
            button1.Texture = ResourceHolder.Textures.Get("Widget/button");
            button1.WidgetEvent += Button1_WidgetEvent;

            button2 = new Button("Restart");
            button2.Texture = ResourceHolder.Textures.Get("Widget/button");
            button2.WidgetEvent += Button2_WidgetEvent;

            button3 = new Button("To menu");
            button3.Texture = ResourceHolder.Textures.Get("Widget/button");
            button3.WidgetEvent += Button3_WidgetEvent;

            menu = new StackMenu();
            menu.Title = "Pause";
            menu.Texture = ResourceHolder.Textures.Get("Widget/demo_background");
            menu.FillColor = new Color(0, 0, 0, 150);

            menu.AddWidget(button1);
            menu.AddWidget(button2);
            menu.AddWidget(button3);

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

        private void Button1_WidgetEvent(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.Machine.PopState();
        }

        private void Button2_WidgetEvent(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.Machine.PopState();
            Game.Machine.ChangeState(new StatePlaying(game));
        }

        private void Button3_WidgetEvent(object sender, WidgetEventArgs e)
        {
            menu.Unsubscribe();

            Game.Machine.PopState();
            Game.Machine.ChangeState(new MainMenuState(game));
        }
    }
}
