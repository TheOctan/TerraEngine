using GameEngine.Core;
using GameEngine.Util;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria.Gameplay.NPC
{
    abstract class Npc : Transformable, Drawable
    {
        public Vector2f startPosition;

        protected RectangleShape rect;
        protected Vector2f velocity;
        protected Vector2f movement;
        protected World world;
        protected bool isFly = true;
        protected bool onGround = false;
        protected bool isRectVisible = true;

        public int Direction
        {
            get
            {
                int dir = Scale.X >= 0 ? 1 : -1;
                return dir;
            }
            set
            {
                int dir = value >= 0 ? 1 : -1;
                Scale = new Vector2f(dir, 1);
            }
        }

        public Npc(World world)
        {
            this.world = world;
        }

        public void Spawn()
        {
            Position = startPosition;
            velocity = new Vector2f();
        }

        public void Update()
        {
            UpdateNPC();
            UpdatePhysics();

            Position += movement + velocity;

            if (Position.Y > Game.Window.Size.Y)
                OnKill();
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            if (isRectVisible)
                target.Draw(rect, states);

            DrawNPC(target, states);
        }

        private void UpdatePhysics()
        {
            bool isFall = true;

            velocity.X *= 0.99f;
            velocity.Y += 0.25f;

            Vector2f nextPos = Position + velocity + movement - rect.Origin;
            FloatRect playerRect = new FloatRect(nextPos, rect.Size);

            int pX = (int)((Position.X - rect.Origin.X + rect.Size.X / 2) / Tile.TILE_SIZE);
            int pY = (int)((Position.Y + rect.Size.Y) / Tile.TILE_SIZE);
            int pY2 = (int)(Position.Y / Tile.TILE_SIZE);

            Tile downTile = world.GetTile(pX, pY);
            Tile upTile = world.GetTile(pX, pY2);

            if (downTile != null)
            {
                FloatRect tileRect = new FloatRect(downTile.Position, new Vector2f(Tile.TILE_SIZE, Tile.TILE_SIZE));

                DebugRender.AddRectangle(tileRect, Color.Red);

                isFall = !playerRect.Intersects(tileRect);
                isFly = isFall;

                if(playerRect.Intersects(tileRect))
                {
                    Position = new Vector2f(Position.X, downTile.Position.Y - rect.Size.Y + 5);
                    onGround = true;
                }
            }
            else
            {
                onGround = false;
            }

            if (upTile != null)
            {
                FloatRect tileRect = new FloatRect(upTile.Position, new Vector2f(Tile.TILE_SIZE, Tile.TILE_SIZE));

                DebugRender.AddRectangle(tileRect, Color.Magenta);

                isFall = !playerRect.Intersects(tileRect);
                isFly = isFall;

                if (playerRect.Intersects(tileRect))
                {
                    Position = new Vector2f(Position.X, upTile.Position.Y + Tile.TILE_SIZE);
                }
            }

            if (!isFall)
            {
                velocity.Y = 0;
            }

            UpdatePhysicsWall(playerRect, pX, pY);
        }

        protected abstract void UpdatePhysicsWall(FloatRect playerRect, int pX, int pY);
        protected void checkWall(FloatRect playerRect, int pX, int pY, Tile[] walls)
        {
            foreach (Tile tile in walls)
            {
                if (tile == null) continue;

                FloatRect tileRect = new FloatRect(tile.Position, new Vector2f(Tile.TILE_SIZE, Tile.TILE_SIZE));
                DebugRender.AddRectangle(tileRect, Color.Yellow);

                if (playerRect.Intersects(tileRect))
                {
                    Vector2f offset = new Vector2f(playerRect.Left - tileRect.Left, 0);
                    offset.X /= Math.Abs(offset.X);

                    float speed = Math.Abs(movement.X);

                    if (offset.X > 0)
                    {
                        Position = new Vector2f((tileRect.Left + tileRect.Width) + playerRect.Width / 2, Position.Y);
                        movement.X = 0;
                    }
                    else if (offset.X < 0)
                    {
                        Position = new Vector2f(tileRect.Left - playerRect.Width / 2, Position.Y);
                        movement.X = 0;
                    }

                    OnWallCollided();
                }
            }
        }


        public abstract void OnKill();
        public abstract void OnWallCollided();
        public abstract void UpdateNPC();
        public abstract void DrawNPC(RenderTarget target, RenderStates states);
    }
}
