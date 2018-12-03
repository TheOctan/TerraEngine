using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Util
{
    public class Timer : Transformable, Drawable
    {
        public event EventHandler EndTime = (object sender, EventArgs e) => { };

        public int Minute => seconds / 60;
        public int Second => seconds % 60;
        public Color Color
        {
            get => point.Color;
            set
            {
                firstDischarge.Color = value;
                point.Color = value;
                secondDischarge.Color = value;
            }
        }

        private int seconds;

        private Text firstDischarge;
        private Text point;
        private Text secondDischarge;

        private Clock timer;

        private bool isUpdate;

        public Timer() : this(0)
        {

        }

        public Timer(int minute, int second = 0)
        {
            isUpdate = false;

            seconds = minute * 60 + second;

            timer = new Clock();

            firstDischarge = new Text()
            {
                DisplayedString = minute >= 10 ? minute.ToString() : "0" + minute.ToString(),
                Font = ResourceHolder.Fonts.Get("digifaw"),
                CharacterSize = 50
            };
            point = new Text()
            {
                DisplayedString = ".",
                Font = ResourceHolder.Fonts.Get("digifaw"),
                CharacterSize = 50,
                Position = new Vector2f(firstDischarge.GetGlobalBounds().Width + 20, 0)
            };
            secondDischarge = new Text()
            {
                DisplayedString = second >= 10 ? second.ToString() : "0" + second.ToString(),
                Font = ResourceHolder.Fonts.Get("digifaw"),
                CharacterSize = 50,
                Position = new Vector2f(point.Position.X + point.GetGlobalBounds().Width + 5, 0)
            };
        }

        public FloatRect GetLocalBounds()
        {
            return new FloatRect(
                firstDischarge.Position, 
                new Vector2f(
                    firstDischarge.GetLocalBounds().Width + 
                    point.GetLocalBounds().Width + 
                    secondDischarge.GetLocalBounds().Width + 25, 
                    firstDischarge.GetLocalBounds().Height)
                );
        }

        public FloatRect GetGlobalBounds()
        {
            return Transform.TransformRect(GetLocalBounds());
        }

        public void Start()
        {
            isUpdate = true;
            timer.Restart();
        }

        public void Stop()
        {
            isUpdate = false;
        }

        public void Reset(int minute, int second = 0)
        {
            isUpdate = false;
            seconds = minute * 60 + second;
            Display();
        }

        public void Update()
        {
            if (!isUpdate) return;

            if (seconds <= 10)
                Color = Color.Red;
            else
                Color = Color.White;

            if (timer.ElapsedTime.AsSeconds() >= 1)
            {
                if (seconds == 0)
                {
                    Stop();
                    EndTime(this, EventArgs.Empty);
                    return;
                }

                seconds--;

                Display();

                timer.Restart();
            }

        }

        private void Display()
        {
            int minute = seconds / 60;
            int second = seconds % 60;

            firstDischarge.DisplayedString = minute >= 10 ? minute.ToString() : "0" + minute.ToString();
            secondDischarge.DisplayedString = second >= 10 ? second.ToString() : "0" + second.ToString();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(firstDischarge, states);
            target.Draw(point, states);
            target.Draw(secondDischarge, states);
        }
    }
}
