using System;
using GameEngine.Event;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
    public class ScrollBar : Widget
    {
        public event EventHandler<WidgetEventArgs> ValueChangedEvent = (object sender, WidgetEventArgs e) => { };

        public int Value
        {
            get => value;
            set { this.value = value >= maxValue ? maxValue : value; }
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
                bar.Texture = value;
                rect.Texture = value;
                UpdateState();
            }
        }

        public IntRect ActiveBarRect { get; set; } = new IntRect(0, 20, 8, 20);
        public IntRect SelectedBarRect { get; set; } = new IntRect(8, 20, 8, 20);

        private RectangleShape bar;
        private string message;

        private int value;
        private readonly int minValue;
        private readonly int maxValue;
        private readonly int step;

        public ScrollBar() : this("")
        {}
        public ScrollBar(string str, int min = 0, int max = 100, int step = 1) : base(WidgetSize.Wide)
        {
            value = min * step;
            minValue = min;
            maxValue = max;
            this.step = step;

            rect.TextureRect = NotActiveRect;
            bar = new RectangleShape()
            {
                Size = new Vector2f(16, 40),
                FillColor = ActiveColor,
                OutlineColor = Color.Black,
                OutlineThickness = outlineThickness
            };
            Text = str;
        }

        public override void Update()
        {
            base.Update();

            UpdateBar();
            UpdateText();
        }
        public override void Reset()
        {
            base.Reset();
            bar.FillColor = Color.White;
        }

        protected override void DrawResource(RenderTarget target, RenderStates states)
        {
            target.Draw(bar, states);
        }
        protected override void OnMouseMoved(object sender, MouseMoveEventArgs e)
        {
			if (!IsActive)
                return;

            base.OnMouseMoved(sender, e);

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
        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!IsActive)
                return;

            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered)
                {
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
                if (isClicked)
                {
                    if (isEntered)
                    {
                        state = WidgetState.selected;
                    }

                    ValueChangedEvent(this, new WidgetEventArgs("", value, IsActive));
                }

                isClicked = false;
            }
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
        protected override void ActiveState()
        {
            text.FillColor = ActiveTextColor;
            if (bar.Texture != null)
            {
                bar.TextureRect = ActiveBarRect;
            }
            else
            {
                bar.FillColor = ActiveColor;
            }
        }
        protected override void SelectedState()
        {
            text.FillColor = SelectedTextColor;
            if (bar.Texture != null)
            {
                bar.TextureRect = SelectedBarRect;
            }
            else
            {
                bar.FillColor = SelectedColor;
            }
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
    }
}
