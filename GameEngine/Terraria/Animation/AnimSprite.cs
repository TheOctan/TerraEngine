using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria
{
    class AnimationFrame
    {
        public int i;
        public int j;

        public float time;

        public AnimationFrame(int i, int j, float time)
        {
            this.i = i;
            this.j = j;
            this.time = time;
        }
    }

    class Animation
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

    class AnimSprite : Transformable, Drawable
    {
        public float Speed = 0.05f;

        RectangleShape rectShape;
        SpriteSheet ss;
        SortedDictionary<string, Animation> animations = new SortedDictionary<string, Animation>();
        Animation currentAnimation;
        string currAnimName;

        public Color Color
        {
            get { return rectShape.FillColor; }
            set { rectShape.FillColor = value; }
        }

        public AnimSprite(Texture texture, SpriteSheet ss)
        {
            this.ss = ss;

            rectShape = new RectangleShape(new Vector2f(ss.SubWidth, ss.SubHeigt));
            rectShape.Origin = new Vector2f(ss.SubWidth / 2, ss.SubHeigt / 2);
            rectShape.Texture = texture;
        }

        public void AddAnimation(string name, Animation animation)
        {
            animations.Add(name, animation);
            currentAnimation = animation;
            currAnimName = name;
        }

        public void Play(string name)
        {
            if (currAnimName == name) return;

            currentAnimation = animations[name];
            currAnimName = name;
            currentAnimation.Reset();
        }

        public IntRect GetTextureRect()
        {
            var currFrame = currentAnimation.GetFrame(Speed);
            return ss.GetTextureRect(currFrame.i, currFrame.j);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            rectShape.TextureRect = GetTextureRect();

            states.Transform *= Transform;
            target.Draw(rectShape, states);
        }
    }
}
