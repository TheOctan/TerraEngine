using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terraria.AnimationManager
{
    public class AnimSprite : Transformable, Drawable
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
