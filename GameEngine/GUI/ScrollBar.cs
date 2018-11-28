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
    public class ScrollBar : Widget
    {
        public override event EventHandler<WidgetEventArgs> WidgetEvent = (object sender, WidgetEventArgs e) => { };

        public int Value
        {
            get => value;
            set { this.value = value >= maxValue ? maxValue : value; }
        }

        private RectangleShape bar;
        private string message;

        private int value;
        private readonly int minValue;
        private readonly int maxValue;
        private readonly int step;

        public ScrollBar() : this("")
        {

        }
        public ScrollBar(string str, int min = 0, int max = 100, int step = 1) : base(WidgetSize.Wide)
        {
            value = min * step;
            minValue = min;
            maxValue = max;
            this.step = step;

            bar = new RectangleShape()
            {
                Size = new Vector2f(16, 40),
                FillColor = new Color(255, 255, 255, 150),
                OutlineColor = Color.Black,
                OutlineThickness = outlineThickness
            };

            Text = str;
        }

        public override string Text
        {
            get => base.Text;
            set
            {
                message = value + " ";
                UpdateText();
            }
        }

        public override Texture Texture
        {
            get => base.Texture;
            set
            {
                Reset();

                bar.Texture = value;
                rect.Texture = value;
                rect.TextureRect = new IntRect(16, 0, 200, 20);

                UpdateState();
            }
        }

        protected override void DrawResource(RenderTarget target, RenderStates states)
        {
            target.Draw(bar, states);
        }

        protected override void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            base.Window_MouseMoved(sender, e);

            if (isEntered)
            {
                state = WidgetState.selected;
            }
            else
            {
                if (!isClicked)
                {
                    state = WidgetState.active;
                }
            }
        }
        protected override void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered)
                {
                    isClicked = true;
                }
            }
        }
        protected override void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                if(isClicked)
                {
                    if(isEntered)
                    {
                        state = WidgetState.selected;
                    }

                    WidgetEvent(this, new WidgetEventArgs("", value, isActive));
                }

                isClicked = false;
            }
        }

        protected override void Reset()
        {
            bar.FillColor = Color.White;
            rect.FillColor = Color.White;
        }
        protected override void UpdateText()
        {
            text.DisplayedString = message + value.ToString() + "/" + (maxValue * step).ToString();

            text.Origin = new Vector2f(
                text.GetGlobalBounds().Width / 2f,
                text.GetGlobalBounds().Height / 2f);

            text.Position = new Vector2f(
                rect.GetGlobalBounds().Width / 2f,
                rect.GetGlobalBounds().Height / 2.5f);
        }
        protected override void UpdateState()
        {
            base.UpdateState();

            UpdateBar();
            UpdateText();
        }
        private void UpdateBar()
        {
            float length = rect.GetGlobalBounds().Width - bar.GetGlobalBounds().Width;
            float range = maxValue - minValue;

            if (isClicked)
            {
                bar.Position = new Vector2f(localMousePos.X - bar.GetGlobalBounds().Width / 2f, 0);

                if (bar.Position.X < 0)
                {
                    bar.Position = new Vector2f(0, 0);
                }
                else if (bar.Position.X + bar.GetGlobalBounds().Width > rect.GetGlobalBounds().Width)
                {
                    bar.Position = new Vector2f(rect.GetGlobalBounds().Width - bar.GetLocalBounds().Width, 0);
                }

                float offset = length / range;
                value = ((int)((bar.Position.X + offset / 2f) * range / length) + minValue) * step;
            }
            else
            {
                bar.Position = new Vector2f((value / step - minValue) * length / range, 0);
            }
        }

        protected override void ActiveState()
        {
            if (bar.Texture != null)
                bar.TextureRect = new IntRect(0, 0, 8, 20);
            else
                bar.FillColor = new Color(255, 255, 255, 150);

            text.Color = Color.White;
        }
        protected override void SelectedState()
        {
            if (bar.Texture != null)
                bar.TextureRect = new IntRect(8, 0, 8, 20);
            else
                bar.FillColor = new Color(45, 107, 236);

            text.Color = new Color(255, 255, 130);
        }
    }
}
