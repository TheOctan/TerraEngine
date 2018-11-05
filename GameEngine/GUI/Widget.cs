using GameEngine.Event;

using SFML.Graphics;
using SFML.System;
using SFML.Window;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.GUI
{
    public abstract class Widget : Transformable, Drawable
    {
        public Widget()
        {
            #region Subscribing

            Program.window.MouseMoved += Window_MouseMoved;
            Program.window.MouseButtonPressed += Window_MouseButtonPressed;
            Program.window.MouseButtonReleased += Window_MouseButtonReleased;
            Program.window.KeyPressed += Window_KeyPressed;
            Program.window.TextEntered += Window_TextEntered;

            #endregion


            gloabalMousePos = new Vector2f(0, 0);
            state = WidgetState.active;
            dimensions = new float[]
            {
                88f,
                192f,
                400f
            };

            outlineThickness = -2;
        }

        private void Window_MouseMoved(object sender, MouseMoveEventArgs e)
        {
            gloabalMousePos = new Vector2f(e.X, e.Y);
            isEntered = rect.GetGlobalBounds().Contains(localMousePos.X, localMousePos.Y);

            if(isEntered)
            {
                if (isClicked)
                    state = WidgetState.notActive;
                else
                    state = WidgetState.selected;
            }
            else
            {
                state = WidgetState.active;
            }
        }
        private void Window_MouseButtonPressed(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                if(isEntered)
                {
                    state = WidgetState.notActive;
                    isClicked = true;
                }
            }
        }
        private void Window_MouseButtonReleased(object sender, MouseButtonEventArgs e)
        {
            if(e.Button == Mouse.Button.Left)
            {
                if(isEntered && isClicked)
                {
                    state = WidgetState.selected;
                }

                isClicked = false;
            }
        }
        private void Window_KeyPressed(object sender, KeyEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void Window_TextEntered(object sender, TextEventArgs e)
        {
            throw new NotImplementedException();
        }

        public virtual Texture Texture { private get; set; }
        public abstract int Value { get; set; }
        public String Text { get; set; }
        public bool Active { get; set; }
        public Vector2f Size {
            get         { return rect.Size; }
            private set { rect.Size = value; }
        }

        public FloatRect getLocalBounds()
        {
            return rect.GetLocalBounds();
        }                                                           // final
        public FloatRect getGlobalBounds()
        {
            return Transform.TransformRect(getLocalBounds());
        }                                                          // final

        protected virtual void Reset()
        {
            throw new NotImplementedException();
        }
        protected abstract void UpdteText();
        protected void UpdateState()
        {
            throw new NotImplementedException();
        }

        protected virtual void NotActiveState()
        {
            throw new NotImplementedException();
        }
        protected virtual void ActiveState()
        {
            throw new NotImplementedException();
        }
        protected virtual void SelectedState()
        {
            throw new NotImplementedException();
        }
        protected virtual void WarningState()
        {
            throw new NotImplementedException();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;
            localMousePos = states.Transform.GetInverse().TransformPoint(gloabalMousePos);


            target.Draw(rect, states);
            // draw() resources
            target.Draw(text, states);
        }

        public enum WidgetState
        {
            notActive = 0,
            active,
            selected,
            warning
        };                                                                     // final
        public enum WidgetSize
        {
            Narrow = 0,
            Small,
            Wide
        };                                                                      // final

        protected Vector2f          localMousePos;                                                     // final
        protected Vector2f          gloabalMousePos;                                                   // final
        protected RectangleShape    rect;                                                              // final
        protected Text              text;                                                              // final

        protected WidgetState       state;

        protected bool isActive;
        protected bool isClicked;
        protected bool isEntered;

        float[] dimensions;

        protected float outlineThickness;

        public event EventHandler<WidgetEventArgs>  WidgetClicked;
        //public event EventHandler                   MouseEntered;
        //public event EventHandler                   MouseLeft;
    }
}
