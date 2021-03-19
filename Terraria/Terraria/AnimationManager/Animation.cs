using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terraria.AnimationManager
{
    public class Animation
    {
        AnimationFrame[] frames;

        float timer;
        AnimationFrame currentFrame;
        int currentFrameIndex;

        public Animation(params AnimationFrame[] frames)
        {
            this.frames = frames;
            Reset();
        }

        public void Reset()
        {
            timer = 0f;
            currentFrameIndex = 0;
            currentFrame = frames[currentFrameIndex];
        }

        public void NextFrame()
        {
            timer = 0f;
            currentFrameIndex++;

            if (currentFrameIndex == frames.Length)
                currentFrameIndex = 0;

            currentFrame = frames[currentFrameIndex];
        }

        public AnimationFrame GetFrame(float speed)
        {
            timer += speed;

            if (timer >= currentFrame.time)
                NextFrame();

            return currentFrame;
        }
    }
}
