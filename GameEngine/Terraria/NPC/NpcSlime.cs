using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;

namespace Terraria.Gameplay.NPC
{
    class NpcSlime : Npc
    {
        const float TIME_WAIT_JUMP = 1f;

        SpriteSheet spriteSheet;
        float waitTimer = 0f;

        public NpcSlime(World world) : base(world)
        {
            var texNpcSlime = ResourceHolder.Textures.Get(Content.NPC_DIR + "slime");

            spriteSheet = new SpriteSheet(1, 2, 0, (int)texNpcSlime.Size.X, (int)texNpcSlime.Size.Y);

            rect = new RectangleShape(new Vector2f(spriteSheet.SubWidth / 1.5f, spriteSheet.SubHeigt / 1.5f));
            rect.Origin = new Vector2f(rect.Size.X / 2, 0);
            rect.FillColor = new Color(0, 255, 0, 150);

            rect.Texture = texNpcSlime;
            rect.TextureRect = spriteSheet.GetTextureRect(0, 0);
        }

        public override void OnKill()
        {
            Spawn();
        }

        public override void OnWallCollided()
        {
            Direction *= -1;
            velocity = new Vector2f(-velocity.X * 0.8f, velocity.Y);
        }

        public override void UpdateNPC()
        {
            if (!isFly && onGround)
            {
                if (waitTimer >= TIME_WAIT_JUMP)
                {
                    velocity = new Vector2f(Direction * Program.Rand.Next(1, 10), -Program.Rand.Next(6, 9));
                    waitTimer = 0f;
                    onGround = false;
                }
                else
                {
                    waitTimer += 0.05f;
                    velocity.X = 0;
                }

                rect.TextureRect = spriteSheet.GetTextureRect(0, 0);
            }
            else
            {
                rect.TextureRect = spriteSheet.GetTextureRect(0, 1);
            }
        }

        public override void DrawNPC(RenderTarget target, RenderStates states)
        {

        }

        protected override void UpdatePhysicsWall(FloatRect playerRect, int pX, int pY)
        {
            Tile[] walls = new Tile[]
            {
                world.GetTile(pX - 1, pY - 1),
                world.GetTile(pX - 1, pY - 2),
                //world.GetTile(pX - 1, pY - 3),
                world.GetTile(pX + 1, pY - 1),
                world.GetTile(pX + 1, pY - 2),
                //world.GetTile(pX + 1, pY - 3)
            };

            checkWall(playerRect, pX, pY, walls);
        }
    }
}
