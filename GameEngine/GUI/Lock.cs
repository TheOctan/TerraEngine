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
    public class Lock : Widget
    {
        public override event EventHandler<WidgetEventArgs> WidgetEvent = (object sender, WidgetEventArgs e) => { };

        public bool Value { get => isLocked; set { isLocked = value; UpdateState(); } }

        private bool isLocked;
        private RectangleShape locker;

        public Lock() : this("")
        {

        }
        public Lock(string str, WidgetSize size = WidgetSize.Wide) : base(size)
        {
            locker = new RectangleShape()
            {
                Size = new Vector2f(40, 40),
                FillColor = new Color(52, 152, 219),
                OutlineColor = Color.Black,
                OutlineThickness = outlineThickness
            };

            Text = str;
        }

        public override Texture Texture
        {
            get => base.Texture;
            set
            {
                Reset();

                locker.Texture = value;
                rect.Texture = value;
                rect.TextureRect = new IntRect(0, 20 * 3, 200, 20);

                UpdateState();
            }
        }

        protected override void DrawResource(RenderTarget target, RenderStates states)
        {
            target.Draw(locker, states);
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
                state = WidgetState.active;
            }
        }
        protected override void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered)
                {
                    isLocked = isLocked ? false : true;

                    WidgetEvent(this, new WidgetEventArgs("", Convert.ToInt32(isLocked), isActive));
                }
            }
        }

        protected override void Reset()
        {
            locker.FillColor = Color.White;
            rect.FillColor = Color.White;
        }
        protected override void UpdateText()
        {
            text.Origin = new Vector2f(0, text.GetGlobalBounds().Height / 2f);
            text.Position = new Vector2f(locker.GetGlobalBounds().Width + 15f, locker.GetGlobalBounds().Height / 2.5f);
        }

        protected override void NotActiveState()
        {
            if (locker.Texture != null)
                locker.TextureRect = new IntRect(20 * Convert.ToInt32(isLocked), 0, 20, 20);
            else
                locker.FillColor = isLocked ? Color.Red : new Color(244, 183, 19);

            text.Color = Color.White;
        }
        protected override void ActiveState()
        {
            if (locker.Texture != null)
                locker.TextureRect = new IntRect(20 * Convert.ToInt32(isLocked), 20, 20, 20);
            else
                locker.FillColor = isLocked ? new Color(22, 192, 82) : new Color(52, 152, 219);

            text.Color = Color.White;
        }
        protected override void SelectedState()
        {
            if (locker.Texture != null)
                locker.TextureRect = new IntRect(20 * Convert.ToInt32(isLocked), 40, 20, 20);
            else
                locker.FillColor = isLocked ? new Color(50, 196, 19) : new Color(45, 107, 236);

            text.Color = new Color(255, 255, 130);
        }
    }
}
