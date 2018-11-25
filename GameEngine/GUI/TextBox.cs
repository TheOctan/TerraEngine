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
    public class TextBox : Widget
    {
        public override event EventHandler<WidgetEventArgs> WidgetEvent = (object sender, WidgetEventArgs e) => { };

        private StringBuilder modString;

        public TextBox() : this("")
        {

        }
        public TextBox(string str) : base(WidgetSize.Wide)
        {
            modString = new StringBuilder();
            rect.Size = new Vector2f(dimensions[(int)WidgetSize.Wide], 64);

            Text = str;
        }

        protected override void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                if(isEntered)
                {
                    state = WidgetState.selected;
                }
                else
                {
                    state = WidgetState.active;
                }
            }
        }
        protected override void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Code == Keyboard.Key.Return)
                if(state == WidgetState.selected)
                {
                    state = WidgetState.active;
                    WidgetEvent(this, new WidgetEventArgs(modString.ToString(), 0, isActive));
                }
        }
        protected override void Window_TextEntered(object sender, TextEventArgs e)
        {
            if(state == WidgetState.selected)
            {
                string keyCode = e.Unicode;

                if(IsValidCharacter(keyCode))
                {
                    if(text.GetGlobalBounds().Width + 50 <= rect.GetGlobalBounds().Width)
                    {
                        modString.Append(keyCode);
                    }
                }
                else if(IsBackspace(keyCode))
                {
                    if(modString.Length > 0)
                        modString.Remove(modString.Length - 1, 1);
                }
                text.DisplayedString = modString.ToString();
            }
        }

        protected override void Reset()
        {
            rect.FillColor = Color.White;
            rect.OutlineColor = Color.Transparent;
            rect.OutlineThickness = 0;
        }
        protected override void UpdateText()
        {
            text.Position = new Vector2f(15, rect.GetGlobalBounds().Height / 2.5f);
            modString = new StringBuilder(text.DisplayedString);
        }

        protected override void NotActiveState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 0, 160, 32);
            else
                rect.FillColor = new Color(22, 192, 82);

            text.Color = Color.Black;
        }
        protected override void ActiveState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 32, 160, 32);
            else
                rect.FillColor = new Color(52, 152, 219);

            text.Color = Color.White;
        }
        protected override void SelectedState()
        {
            if (rect.Texture != null)
                rect.TextureRect = new IntRect(0, 64, 160, 32);
            else
                rect.FillColor = new Color(45, 107, 236);

            text.Color = Color.White;
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
