using System;
using System.Text;
using GameEngine.Core;
using GameEngine.Event;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
    public class TextBox : Widget
    {
        public event EventHandler<TextBoxEventArgs> TextChangedEvent = (object sender, TextBoxEventArgs e) => { };

        private StringBuilder modString;

        public TextBox() : this("")
        {}
        public TextBox(string str) : base(WidgetSize.Wide)
        {
            modString = new StringBuilder();
            rect.Size = new Vector2f(dimensions[(int)WidgetSize.Wide], 64);
            Text = str;

            NotActiveRect = new IntRect(0, 0, 160, 32);
            ActiveRect = new IntRect(0, 32, 160, 32);
            SelectedRect = new IntRect(0, 64, 160, 32);

            NotActiveTextColor = Color.Black;
        }

        public override void Subscribe()
        {
            base.Subscribe();

            Game.Window.TextEntered += OnTextEntered;
            Game.Window.KeyPressed += OnKeyPressed;
        }
        public override void Unsubscribe()
        {
            base.Unsubscribe();

            Game.Window.TextEntered -= OnTextEntered;
            Game.Window.KeyPressed -= OnKeyPressed;
        }

        protected override void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if (!IsActive)
                return;

            if (e.Button == Mouse.Button.Left)
            {
                if (isEntered)
                {
                    state = WidgetState.selected;
                }
                else
                {
                    state = WidgetState.active;
                    TextChangedEvent(this, new TextBoxEventArgs(modString.ToString()));
                }
            }
        }
        protected void OnKeyPressed(object sender, KeyEventArgs e)
        {
            if (!IsActive)
                return;

            if (e.Code == Keyboard.Key.Enter)
            {
                if (state == WidgetState.selected)
                {
                    state = WidgetState.active;
                    TextChangedEvent(this, new TextBoxEventArgs(modString.ToString()));
                }
            }
        }
        protected void OnTextEntered(object sender, TextEventArgs e)
        {
            if (!IsActive)
                return;

            if (state == WidgetState.selected)
            {
                string keyCode = e.Unicode;

                if (IsValidCharacter(keyCode))
                {
                    if (text.GetGlobalBounds().Width + 50 <= rect.GetGlobalBounds().Width)
                    {
                        modString.Append(keyCode);
                    }
                }
                else if (IsBackspace(keyCode))
                {
                    if (modString.Length > 0)
                        modString.Remove(modString.Length - 1, 1);
                }
                text.DisplayedString = modString.ToString();
            }
        }
        public override void Reset()
        {
            base.Reset();
            rect.OutlineColor = Color.Transparent;
            rect.OutlineThickness = 0;
        }

        protected override void UpdateText()
        {
            text.Position = new Vector2f(15, rect.GetGlobalBounds().Height / 2.5f);
            modString = new StringBuilder(text.DisplayedString);
        }
        private bool IsValidCharacter(string keyCode)
        {
            return keyCode[0] >= 48 && keyCode[0] <= 57 ||    // Numbers
                keyCode[0] >= 65 && keyCode[0] <= 90 ||       // Uppercase EN
                keyCode[0] >= 97 && keyCode[0] <= 122 ||      // Lowercase EN
                keyCode[0] >= 1040 && keyCode[0] <= 1171 ||   // Uppercase RU
                keyCode[0] >= 1072 && keyCode[0] <= 1103 ||   // Lowercase RU
                keyCode[0] == 32 ||                           // Space
                keyCode[0] == 95 ||                           // _
                keyCode[0] == 40 || keyCode[0] == 41          // ( )
            ;
        }
        private bool IsBackspace(string keyCode)
        {
            return keyCode[0] == 8;
        }
    }
}
