using System;
using GameEngine.Core;
using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace GameEngine.GUI
{
	public abstract class Widget : Transformable, Drawable
	{
		public enum WidgetState
		{
			notActive = 0,
			active,
			selected,
			warning
		};
		public enum WidgetSize
		{
			Narrow = 0,
			Small,
			Wide
		};

		public virtual string Text
		{
			get => text.DisplayedString;
			set
			{
				text.DisplayedString = value;
				UpdateText();
			}
		}
		public virtual Texture Texture
		{
			get => rect.Texture;
			set
			{
				rect.Texture = value;
				UpdateState();
			}
		}
		public virtual Color Color
		{
			get => rect.FillColor;
			set => rect.FillColor = value;
		}		

		public bool IsActive { get; private set; }
		public Vector2f Size
		{
			get => rect.Size;
			private set => rect.Size = value;
		}

		protected Vector2f localMousePos;
		protected Vector2f gloabalMousePos;
		protected RectangleShape rect;
		protected Text text;

		protected WidgetState state;
		protected bool isClicked;
		protected bool isEntered;

		protected float outlineThickness;
		protected readonly float[] dimensions = new float[]
		{
			88f,
			192f,
			400f
		};

		public IntRect NotActiveRect { get; set; } = new IntRect(0, 0, 200, 20);
		public IntRect ActiveRect { get; set; } = new IntRect(0, 20, 200, 20);
		public IntRect SelectedRect { get; set; } = new IntRect(0, 40, 200, 20);
		public Color NotActiveColor { get; set; } = new Color(170, 170, 170);
		public Color ActiveColor { get; set; } = Color.White;
		public Color SelectedColor { get; set; } = new Color(45, 107, 236);
		public Color NotActiveTextColor { get; set; } = Color.White;
		public Color ActiveTextColor { get; set; } = Color.White;
		public Color SelectedTextColor { get; set; } = new Color(255, 255, 130);

		public Widget(WidgetSize size)
		{
			state = WidgetState.active;
			gloabalMousePos = new Vector2f(-1, 0);
			localMousePos = new Vector2f(-1, 0);
			IsActive = true;
			isClicked = false;
			isEntered = false;
			outlineThickness = -2;

			rect = new RectangleShape()
			{
				Size = new Vector2f(dimensions[(int)size], 40),
				FillColor = ActiveColor,
				OutlineColor = Color.Black,
				OutlineThickness = outlineThickness
			};

			text = new Text()
			{
				CharacterSize = 25,
				FillColor = Color.White,
				Font = ResourceHolder.Fonts.Get("minecraft")
			};
		}

		public virtual void Subscribe()
		{
			Game.Window.MouseMoved += OnMouseMoved;
			Game.Window.MouseButtonPressed += OnMouseButtonPressed;
			Game.Window.MouseButtonReleased += OnMouseButtonReleased;

		}
		public virtual void Unsubscribe()
		{
			Game.Window.MouseMoved -= OnMouseMoved;
			Game.Window.MouseButtonPressed -= OnMouseButtonPressed;
			Game.Window.MouseButtonReleased -= OnMouseButtonReleased;
		}
		public virtual void Update()
		{
			UpdateState();
		}
		public void SetActive(bool flag)
		{
			IsActive = flag;
			state = IsActive ? WidgetState.active : WidgetState.notActive;
			UpdateState();
		}
		public virtual void Reset()
		{
			rect.FillColor = Color.White;
		}
		public FloatRect GetLocalBounds()
		{
			return rect.GetLocalBounds();
		}
		public FloatRect GetGlobalBounds()
		{
			return Transform.TransformRect(GetLocalBounds());
		}
		public void Draw(RenderTarget target, RenderStates states)
		{
			states.Transform *= Transform;
			localMousePos = states.Transform.GetInverse().TransformPoint(gloabalMousePos);

			target.Draw(rect, states);
			DrawResource(target, states);
			target.Draw(text, states);
		}

		protected void UpdateState()
		{
			switch (state)
			{
				case WidgetState.notActive:
					NotActiveState();
					break;
				case WidgetState.active:
					ActiveState();
					break;
				case WidgetState.selected:
					SelectedState();
					break;
				case WidgetState.warning:
					WarningState();
					break;
			}
		}
		protected abstract void UpdateText();
		protected virtual void DrawResource(RenderTarget target, RenderStates states)
		{
		}
		protected virtual void OnMouseMoved(object sender, MouseMoveEventArgs e)
		{
			gloabalMousePos = new Vector2f(e.X, e.Y);
			isEntered = rect.GetGlobalBounds().Contains(localMousePos.X, localMousePos.Y);
		}
		protected virtual void OnMouseButtonPressed(object sender, MouseButtonEventArgs e)
		{
		}
		protected virtual void OnMouseButtonReleased(object sender, MouseButtonEventArgs e)
		{
		}
		protected virtual void NotActiveState()
		{
			text.FillColor = NotActiveTextColor;
			if (rect.Texture != null)
			{
				rect.TextureRect = NotActiveRect;
			}
			else
			{
				rect.FillColor = NotActiveColor;
			}
		}
		protected virtual void ActiveState()
		{
			text.FillColor = ActiveTextColor;
			if (rect.Texture != null)
			{
				rect.TextureRect = ActiveRect;
			}
			else
			{
				rect.FillColor = ActiveColor;
			}
		}
		protected virtual void SelectedState()
		{
			text.FillColor = SelectedTextColor;
			if (rect.Texture != null)
			{
				rect.TextureRect = SelectedRect;
			}
			else
			{
				rect.FillColor = SelectedColor;
			}
		}
		protected virtual void WarningState()
		{
			throw new NotImplementedException();
		}
	}
}
