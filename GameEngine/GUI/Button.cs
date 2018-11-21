using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine.Event;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
    public class Button : Widget
    {
        public override event EventHandler<WidgetEventArgs> WidgetEvent = (object sender, WidgetEventArgs e) => { };

        public Button() : this("")
        {

        }
        public Button(string str, WidgetSize size = WidgetSize.Wide) : base(size)
        {
            Text = str;
        }


        protected override void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            base.Window_MouseMoved(sender, e);

            if(isEntered)
            {
                if (isClicked)
                    state = WidgetState.notActive;
                else
                    state = WidgetState.selected;
            }
            else
            {
                state = WidgetState.active;
            }
        }
        protected override void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                if(isEntered)
                {
                    state = WidgetState.notActive;
                    isClicked = true;
                }
            }
        }
        protected override void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left )
            {
                if(isEntered && isClicked)
                {
                    state = WidgetState.selected;

                    WidgetEvent(this, new WidgetEventArgs(new WidgetEvent() { IsActive = isActive }));
                }

                isClicked = false;
            }
        }

        protected override void UpdateText()
        {
            text.Origin = new Vector2f(
                text.GetGlobalBounds().Width / 2f,
                text.GetGlobalBounds().Height / 2f);

            text.Position = new Vector2f(
                rect.GetGlobalBounds().Width / 2f,
                rect.GetGlobalBounds().Height / 2.5f);
        }

        protected override void NotActiveState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 0, 200, 20);
            else
                rect.FillColor = new Color(22, 192, 82);

            text.Color = Color.White;
        }
        protected override void ActiveState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 20, 200, 20);
            else
                rect.FillColor = new Color(52, 152, 219);

            text.Color = Color.White;
        }
        protected override void SelectedState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 40, 200, 20);
            else
                rect.FillColor = new Color(45, 107, 236);

            text.Color = new Color(255, 255, 130);
        }
    }
}
