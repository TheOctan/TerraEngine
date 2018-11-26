using GameEngine.GUI;
using GameEngine.States.StateMachine;
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
        private StackMenu menu;

        public MainMenuState(Game game) : base(game)
        {
            menu = new StackMenu();
            Game.Window.Resized += Window_Resized;
        }

        public override void HandleInput()
        {
        }

        public override void Init()
        {
            menu.Title = "Main menu";

            menu.AddWidget(new Lock("Full Screen Mode"));
            menu.AddWidget(new ScrollBar("Sound", 1, 10), true);
            menu.AddWidget(new Button("Button 1", Widget.WidgetSize.Small), true);

            menu.AddWidget(new TextBox("Text_0"));
            menu.AddWidget(new Button("Button 2", Widget.WidgetSize.Small), true);
            menu.AddWidget(new Button("Button 3"), true);

            menu.AddWidget(new ScrollBar("Music"));
            menu.AddWidget(new Button("Button 4"), true);
            menu.AddWidget(new Button("Button 5", Widget.WidgetSize.Small), true);

            menu.AddWidget(new Lock("Text", Widget.WidgetSize.Small));
            menu.AddWidget(new Button("Button 6", Widget.WidgetSize.Small), true);
            menu.AddWidget(new Button("Button 7", Widget.WidgetSize.Small), true);
            menu.AddWidget(new Lock("A", Widget.WidgetSize.Narrow), true);
            menu.AddWidget(new Button("Button 8", Widget.WidgetSize.Small), true);
            menu.AddWidget(new Button("A", Widget.WidgetSize.Narrow), true);

            menu.AddWidget(new Lock("B", Widget.WidgetSize.Narrow));
            menu.AddWidget(new Button("Button 9"), true);
            menu.AddWidget(new TextBox("Text_1"), true);
            menu.AddWidget(new Button("B", Widget.WidgetSize.Narrow), true);

            menu.Origin = new Vector2f(menu.Size.X / 2f, menu.Size.Y / 2f);
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
        }

        public override void Render(float alpha)
        {
            Game.Window.Draw(menu);
        }

        public override void Update(Time time)
        {
            menu.Update();
        }

        private void Window_Resized(object sender, SizeEventArgs e)
        {
            menu.Position = new Vector2f(Game.Window.Size.X / 2f, Game.Window.Size.Y / 2f);
        }
    }
}
