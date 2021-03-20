using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;

namespace GameEngine.GUI
{
    public class Label : Transformable, Drawable
    {
        public string Text
        {
            get => message.DisplayedString;
            set
            {
                message.DisplayedString = value;
                UpdateText();
            }
        }
        public Vector2f Size { get => frame.Size; set => frame.Size = value; }

        private RectangleShape frame;
        private Text message;

        public Label()
        {
            message = new Text();
            message.Font = ResourceHolder.Fonts.Get("arial");
            message.CharacterSize = 15;

            frame = new RectangleShape(new Vector2f(90, 20));
            frame.FillColor = new Color(0, 0, 0, 100);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(frame, states);
            target.Draw(message, states);
        }

        private void UpdateText()
        {
            frame.Size = new Vector2f(message.GetGlobalBounds().Width + 20, frame.Size.Y);
            message.Origin = new Vector2f(message.GetGlobalBounds().Width / 2f, message.GetGlobalBounds().Height / 2f);
            message.Position = new Vector2f(frame.GetGlobalBounds().Width / 2f, frame.GetGlobalBounds().Height / 2.5f);
        }
    }
}
