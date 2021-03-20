using System;
using GameEngine.Event;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
    public class Button : Widget
    {
        public event EventHandler<ButtonEventArgs> PressedEvent = (object sender, ButtonEventArgs e) => { };

        public Button() : this("")
        {}
        public Button(string str, WidgetSize size = WidgetSize.Wide) : base(size)
        {
            Text = str;
        }

        protected override void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (!IsActive)
                return;

            base.OnMouseMoved(sender, e);

            if (isEntered)
            {
                if (isClicked)
                {
                    state = WidgetState.notActive;
                }
                else
                {
                    state = WidgetState.selected;
                }
            }
            else
            {
                state = WidgetState.active;
            }
        }
        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!IsActive)
                return;

            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered)
                {
                    state = WidgetState.notActive;
                    isClicked = true;
                }
            }
        }
        protected override void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (!IsActive)
                return;

            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered && isClicked)
                {
                    state = WidgetState.selected;

                    PressedEvent(this, ButtonEventArgs.Empty);
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
    }
}
