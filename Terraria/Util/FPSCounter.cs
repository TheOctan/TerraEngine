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
    public class FPSCounter : Drawable
    {
        public int Count => (int)fps;
        public string Text { get; set; }

        private Text text;

        private Clock delayTimer;
        private Clock fpsTimer;

        private float fps;
        private int frameCount;

        public FPSCounter()
        {
            fps = 0;
            frameCount = 0;

            text = new Text()
            {
                Position = new Vector2f(5, 5),
                Color = Color.White,
                Font = ResourceHolder.Fonts.Get("arial"),
                CharacterSize = 15
            };

            delayTimer = new Clock();
            fpsTimer = new Clock();
        }

        public void Update()
        {
            frameCount++;

            if(delayTimer.ElapsedTime.AsSeconds() > 0.2f)
            {
                fps = frameCount / fpsTimer.Restart().AsSeconds();
                frameCount = 0;
                delayTimer.Restart();
            }

            text.DisplayedString = "FPS " + ((int)fps).ToString() + Text;
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(text, states);
        }
    }
}
