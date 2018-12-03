using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria.Gameplay.NPC
{
    class Player : Npc
    {
        public const float PLAYER_MOVE_SPEED = 4f;
        public const float PLAYER_MOVE_SPEED_ACELERATION = 0.2f;

        public Color HairColor { get => asHair.Color; set => asHair.Color = value; }
        public Color BodyColor { get => asHead.Color; set { asHead.Color = value; asHands.Color = value; } }
        public Color ShirtColor { get => asShirt.Color; set { asShirt.Color = value; asUndershirt.Color = value; } }
        public Color LegsColor { get => asLegs.Color; set => asLegs.Color = value; }

        public Keyboard.Key Left { get; set; }
        public Keyboard.Key Rigt { get; set; }
        public Keyboard.Key Jump { get; set; }
        //public Keyboard.Key Attack { get; set; }

        private bool isMove;
        private bool isMoveLeft;
        private bool isMoveRight;
        private bool isJump;     


        RectangleShape rectDirection;
        AnimSprite asHair;              // Волосы
        AnimSprite asHead;              // Голова
        AnimSprite asShirt;             // Рубашка
        AnimSprite asUndershirt;        // Рукава
        AnimSprite asHands;             // Кисти
        AnimSprite asLegs;              // Ноги
        AnimSprite asShoes;             // Обувь

        public Player(World world) : base(world)
        {
            rect = new RectangleShape(new Vector2f(Tile.TILE_SIZE * 1.5f, Tile.TILE_SIZE * 2.8f));
            rect.Origin = new Vector2f(rect.Size.X / 2, 0);
            isRectVisible = false;

            rectDirection = new RectangleShape(new Vector2f(30, 1));
            rectDirection.FillColor = Color.Red;
            rectDirection.Position = new Vector2f(0, rect.Size.Y / 2);

            Left = Keyboard.Key.A;
            Rigt = Keyboard.Key.D;
            Jump = Keyboard.Key.W;

            var texPlayerHair       = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "hair");
            var texPlayerHead       = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "head");
            var texPlayerShirt      = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "shirt");
            var texPlayerUndershirt = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "undershirt");
            var texPlayerHands      = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "hands");
            var texPlayerLegs       = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "legs");
            var texPlayerShoes      = ResourceHolder.Textures.Get(Content.PLAYER_DIR + "shoes");

            #region Hair

            asHair = new AnimSprite(texPlayerHair, new SpriteSheet(1, 14, 0, (int)texPlayerHair.Size.X, (int)texPlayerHair.Size.Y));
            asHair.Position = new Vector2f(0, 19);
            asHair.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asHair.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asHair.AddAnimation("run", new Animation(
                new AnimationFrame(0, 0, 0.1f),
                new AnimationFrame(0, 1, 0.1f),
                new AnimationFrame(0, 2, 0.1f),
                new AnimationFrame(0, 3, 0.1f),
                new AnimationFrame(0, 4, 0.1f),
                new AnimationFrame(0, 5, 0.1f),
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f)
            ));

            #endregion

            #region Head

            asHead = new AnimSprite(texPlayerHead, new SpriteSheet(1, 20, 0, (int)texPlayerHead.Size.X, (int)texPlayerHead.Size.Y));
            asHead.Position = new Vector2f(0, 19);
            asHead.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asHead.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asHead.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            #region Shirt

            asShirt = new AnimSprite(texPlayerShirt, new SpriteSheet(1, 20, 0, (int)texPlayerShirt.Size.X, (int)texPlayerShirt.Size.Y));
            asShirt.Position = new Vector2f(0, 19);
            asShirt.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asShirt.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asShirt.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            #region Undershirt

            asUndershirt = new AnimSprite(texPlayerUndershirt, new SpriteSheet(1, 20, 0, (int)texPlayerUndershirt.Size.X, (int)texPlayerUndershirt.Size.Y));
            asUndershirt.Position = new Vector2f(0, 19);
            asUndershirt.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asUndershirt.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asUndershirt.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            #region Hands

            asHands = new AnimSprite(texPlayerHands, new SpriteSheet(1, 20, 0, (int)texPlayerHands.Size.X, (int)texPlayerHands.Size.Y));
            asHands.Position = new Vector2f(0, 19);
            asHands.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asHands.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asHands.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            #region Legs

            asLegs = new AnimSprite(texPlayerLegs, new SpriteSheet(1, 20, 0, (int)texPlayerLegs.Size.X, (int)texPlayerLegs.Size.Y));
            asLegs.Position = new Vector2f(0, 19);
            asLegs.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asLegs.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asLegs.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            #region Shoes

            asShoes = new AnimSprite(texPlayerShoes, new SpriteSheet(1, 20, 0, (int)texPlayerShoes.Size.X, (int)texPlayerShoes.Size.Y));
            asShoes.Position = new Vector2f(0, 19);
            asShoes.AddAnimation("idle", new Animation(
                new AnimationFrame(0, 0, 0.1f)
            ));
            asShoes.AddAnimation("jump", new Animation(
                new AnimationFrame(0, 5, 0.1f)
            ));
            asShoes.AddAnimation("run", new Animation(
                new AnimationFrame(0, 6, 0.1f),
                new AnimationFrame(0, 7, 0.1f),
                new AnimationFrame(0, 8, 0.1f),
                new AnimationFrame(0, 9, 0.1f),
                new AnimationFrame(0, 10, 0.1f),
                new AnimationFrame(0, 11, 0.1f),
                new AnimationFrame(0, 12, 0.1f),
                new AnimationFrame(0, 13, 0.1f),
                new AnimationFrame(0, 14, 0.1f),
                new AnimationFrame(0, 15, 0.1f),
                new AnimationFrame(0, 16, 0.1f),
                new AnimationFrame(0, 17, 0.1f),
                new AnimationFrame(0, 18, 0.1f),
                new AnimationFrame(0, 19, 0.1f)
            ));

            #endregion

            HairColor = new Color(255, 0, 0);
            BodyColor = new Color(255, 229, 186);
            ShirtColor = new Color(255, 255, 0);
            LegsColor = new Color(0, 76, 135);
            asShoes.Color = Color.Black;
        }

        public override void OnKill()
        {
            Spawn();
        }

        public override void OnWallCollided()
        {
            if (!isFly)
            {
                asHair.Play("idle");
                asHead.Play("idle");
                asShirt.Play("idle");
                asUndershirt.Play("idle");
                asHands.Play("idle");
                asLegs.Play("idle");
                asShoes.Play("idle");
            }
        }

        public override void UpdateNPC()
        {
            if (isJump && !isFly && onGround)
            {
                velocity = new Vector2f(0, -6);
                onGround = false;
            }

            if (isMove)
            {
                if (isMoveLeft)
                {
                    if (movement.X > 0)
                        movement.X = 0;

                    movement.X -= PLAYER_MOVE_SPEED_ACELERATION;
                    Direction = -1;
                }
                else if (isMoveRight)
                {
                    if (movement.X < 0)
                        movement.X = 0;

                    movement.X += PLAYER_MOVE_SPEED_ACELERATION;
                    Direction = 1;
                }

                if (movement.X > PLAYER_MOVE_SPEED)
                    movement.X = PLAYER_MOVE_SPEED;
                else if (movement.X < -PLAYER_MOVE_SPEED)
                    movement.X = -PLAYER_MOVE_SPEED;

                asHair.Play("run");
                asHead.Play("run");
                asShirt.Play("run");
                asUndershirt.Play("run");
                asHands.Play("run");
                asLegs.Play("run");
                asShoes.Play("run");
            }
            else
            {
                movement = new Vector2f();

                asHair.Play("idle");

                asHead.Play("idle");
                asShirt.Play("idle");
                asUndershirt.Play("idle");
                asHands.Play("idle");
                asLegs.Play("idle");
                asShoes.Play("idle");
            }

            if (isFly)
            {
                asHair.Play("jump");
                asHead.Play("jump");
                asShirt.Play("jump");
                asUndershirt.Play("jump");
                asHands.Play("jump");
                asLegs.Play("jump");
                asShoes.Play("jump");
            }
        }

        public override void DrawNPC(RenderTarget target, RenderStates states)
        {
            //target.Draw(rectDirection, states);
            target.Draw(asHead, states);
            target.Draw(asHair, states);
            target.Draw(asShirt, states);
            target.Draw(asUndershirt, states);
            target.Draw(asHands, states);
            target.Draw(asLegs, states);
            target.Draw(asShoes, states);
        }

        public void UpdateMovement()
        {
            isMoveLeft     = Keyboard.IsKeyPressed(Left);
            isMoveRight    = Keyboard.IsKeyPressed(Rigt);
            isJump         = Keyboard.IsKeyPressed(Jump);

            isMove         = isMoveLeft || isMoveRight;
        }

        public void Reset()
        {
            isMove = false;
            isJump = false;
        }

        protected override void UpdatePhysicsWall(FloatRect playerRect, int pX, int pY)
        {
            Tile[] walls = new Tile[]
            {
                world.GetTile(pX - 1, pY - 1),
                world.GetTile(pX - 1, pY - 2),
                world.GetTile(pX - 1, pY - 3),
                world.GetTile(pX + 1, pY - 1),
                world.GetTile(pX + 1, pY - 2),
                world.GetTile(pX + 1, pY - 3)
            };

            checkWall(playerRect, pX, pY, walls);
        }
    }
}
