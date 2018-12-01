using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GUI
{
    public class StackMenu : Transformable, Drawable
    {
        public string Title { get => titleText.DisplayedString; set => titleText.DisplayedString = value; }
        public Texture Texture
        {
            get => backGround.Texture;
            set
            {
                Reset();
                backGround.Texture = value;
            }
        }
        public Color FillColor { get => backGround.FillColor; set => backGround.FillColor = value; }
        public Vector2f Size { get => backGround.Size; private set => backGround.Size = value; }
        
        private List<Widget> widgets;
        private RectangleShape backGround;

        private Vector2f    basePosition;
        private Vector2f    nextPosition;
        private Vector2f    baseSize;
        private Text        titleText;

        private float interval;
        private float offset;
        private float smallWidth;

        public StackMenu()
        {
            basePosition = new Vector2f(75, 50);
            nextPosition = new Vector2f(basePosition.X, basePosition.Y);
            baseSize = new Vector2f(basePosition.X * 2, basePosition.Y * 2);

            interval = 10f;
            offset = 16f;

            backGround = new RectangleShape()
            {
                Size = baseSize,
                FillColor = new Color(174, 184, 219),
                OutlineColor = Color.Black,
                OutlineThickness = -2
            };

            titleText = new Text()
            {
                Position = new Vector2f(0, basePosition.Y - 40),
                CharacterSize = 30,
                Color = Color.White,
                Font = ResourceHolder.Fonts.Get("minecraft")
            };

            widgets = new List<Widget>();
        }

        public void Subscribe()
        {
            foreach (var widget in widgets)
            {
                widget.Subscribe();
            }
        }

        public void Unsubscribe()
        {
            foreach (var widget in widgets)
            {
                widget.Unsubscribe();
            }
        }

        public void AddWidget(Widget widget, bool rightSide = false)
        {
            InitWidget  (widget, rightSide);
            widgets.Add (widget);
        }

        public void Update()
        {
            for (int i = 0; i < widgets.Count; i++)
            {
                widgets[i].Update();
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(backGround, states);
            target.Draw(titleText, states);

            foreach (var widget in widgets)
            {
                target.Draw(widget, states);
            }
        }

        private void InitWidget(Widget widget, bool rightSide)
        {
            if(widgets.Count == 0)
            {
                smallWidth = widget.GetGlobalBounds().Width;

                baseSize.X += widget.Size.X;
                baseSize.Y += widget.Size.Y;

                widget.Position = basePosition;

                nextPosition.X += widget.Size.X + offset;
                nextPosition.Y += widget.Size.Y + interval;
            }
            else
            {
                smallWidth = Math.Min(smallWidth, widget.GetGlobalBounds().Width);

                if(rightSide)
                {
                    widget.Position = new Vector2f(nextPosition.X, basePosition.Y);

                    nextPosition.X += widget.Size.X + offset;
                }
                else
                {
                    widget.Position = new Vector2f(basePosition.X, nextPosition.Y);

                    // reset
                    basePosition.Y = nextPosition.Y;
                    nextPosition.X = basePosition.X + widget.Size.X + offset;

                    nextPosition.Y  += widget.Size.Y + interval;
                    baseSize.Y      += widget.Size.Y + interval;
                }
            }

            var bounds = widget.GetGlobalBounds();
            while (bounds.Left + bounds.Width >= baseSize.X)
            {
                baseSize.X += smallWidth + offset;
            }

            backGround.Size = baseSize;

            UpdateText();
        }
        private void UpdateText()
        {
            titleText.Position = new Vector2f(
                backGround.GetGlobalBounds().Width / 2f - titleText.GetGlobalBounds().Width / 2f,
                titleText.Position.Y);
        }
        private void Reset()
        {
            backGround.FillColor = Color.White;
            backGround.OutlineColor = Color.Transparent;
            backGround.OutlineThickness = 0;
        }
    }
}
