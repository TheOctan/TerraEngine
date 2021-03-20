using System;
using GameEngine.Event;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
    public class Lock : Widget
    {
        public event EventHandler<LockEventArgs> PressedEvent = (object sender, LockEventArgs e) => { };

        public Vector2i UVoffset { get; set; }
        public Vector2i TextureSize { get; set; }
        public bool Value { get => isLocked; set { isLocked = value; UpdateState(); } }
        public override Texture Texture
        {
            get => base.Texture;
            set
            {
                locker.Texture = value;
                rect.Texture = value;
                UpdateState();
            }
        }

        private bool isLocked;
        private RectangleShape locker;

        public Color ActiveLockedColor { get; set; } = new Color(22, 192, 82);
        public Color SelectedLockedColor { get; set; } = new Color(50, 196, 19);

        public Lock() : this("")
        {}
        public Lock(string str, WidgetSize size = WidgetSize.Wide) : base(size)
        {
            TextureSize = new Vector2i(20, 20);
            UVoffset = new Vector2i(0, 20);
            NotActiveColor = new Color(244, 183, 19);
            rect.TextureRect = NotActiveRect;

            locker = new RectangleShape()
            {
                Size = new Vector2f(40, 40),
                FillColor = ActiveColor,
                OutlineColor = Color.Black,
                OutlineThickness = outlineThickness
            };

            Text = str;
        }

        public override void Reset()
        {
            base.Reset();
            locker.FillColor = Color.White;
        }

        protected override void DrawResource(RenderTarget target, RenderStates states)
        {
            target.Draw(locker, states);
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
                    isLocked = !isLocked;

                    PressedEvent(this, new LockEventArgs(isLocked));
                }
            }
        }
        protected override void UpdateText()
        {
            text.Origin = new Vector2f(0, text.GetGlobalBounds().Height / 2f);
            text.Position = new Vector2f(locker.GetGlobalBounds().Width + 15f, locker.GetGlobalBounds().Height / 2.5f);
        }
        protected override void NotActiveState()
        {
            text.FillColor = NotActiveTextColor;
            if (locker.Texture != null)
            {
                locker.TextureRect = GetRect(UVoffset, TextureSize, 0, isLocked);
            }
            else
            {
                locker.FillColor = isLocked ? Color.Red : NotActiveColor;
            }
        }
        protected override void ActiveState()
        {
            text.FillColor = ActiveTextColor;
            if (locker.Texture != null)
            {
                locker.TextureRect = GetRect(UVoffset, TextureSize, 1, isLocked);
            }
            else
            {
                locker.FillColor = isLocked ? ActiveLockedColor : ActiveColor;
            }
        }
        protected override void SelectedState()
        {
            text.FillColor = SelectedTextColor;
            if (locker.Texture != null)
            {
                locker.TextureRect = GetRect(UVoffset, TextureSize, 2, isLocked);
            }
            else
            {
                locker.FillColor = isLocked ? SelectedLockedColor : SelectedColor;
            }
        }

        private IntRect GetRect(Vector2i UV, Vector2i size, int number, bool isLocked)
        {
            return new IntRect(size.X * Convert.ToInt32(isLocked) + UV.X, size.Y * number + UV.Y, size.X, size.Y);
        }
    }
}
