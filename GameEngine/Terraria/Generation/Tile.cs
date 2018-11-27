using GameEngine;
using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Terraria
{
    public enum TileType
    {
        None,
        Ground,
        Grass
    }

    public class Tile : Transformable, Drawable
    {
        public const int TILE_SIZE = 16;

        TileType type = TileType.Ground;
        RectangleShape rectShape;
        SpriteSheet spriteSheet;

        Tile upTile = null;
        Tile downTile = null;
        Tile leftTile = null;
        Tile rigthTile = null;

        public Tile UpTile
        {
            get
            {
                return upTile;
            }
            set
            {
                upTile = value;
                UpdateTile();
            }
        }
        public Tile DownTile
        {
            get
            {
                return downTile;
            }
            set
            {
                downTile = value;
                UpdateTile();
            }
        }
        public Tile LeftTile
        {
            get
            {
                return leftTile;
            }
            set
            {
                leftTile = value;
                UpdateTile();
            }
        }
        public Tile RigthTile
        {
            get
            {
                return rigthTile;
            }
            set
            {
                rigthTile = value;
                UpdateTile();
            }
        }

        public Tile(TileType type, Tile upTile, Tile downTile, Tile leftTile, Tile rigthTile)
        {
            this.type = type;

            if (upTile != null)
            {
                this.upTile = upTile;
                this.upTile.DownTile = this;
            }
            if (downTile != null)
            {
                this.downTile = downTile;
                this.downTile.UpTile = this;
            }
            if (leftTile != null)
            {
                this.leftTile = leftTile;
                this.leftTile.RigthTile = this;
            }
            if (rigthTile != null)
            {
                this.rigthTile = rigthTile;
                this.rigthTile.LeftTile = this;
            }

            rectShape = new RectangleShape(new Vector2f(TILE_SIZE, TILE_SIZE));

            switch (type)
            {
                case TileType.None:
                    break;

                case TileType.Ground:
                    rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_0");
                    break;

                case TileType.Grass:
                    rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_1");
                    break;
            }

            spriteSheet = new SpriteSheet(TILE_SIZE, TILE_SIZE, 1);

            UpdateTile();
        }

        public void UpdateTile()
        {
            int i = Program.Rand.Next(0, 3);

            if (upTile != null && downTile != null && leftTile != null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(1 + i, 1);
            }
            else if (upTile == null && downTile == null && leftTile == null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(9 + i, 3);
            }

            else if (upTile == null && downTile != null && leftTile != null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(1 + i, 0);
            }
            else if (upTile != null && downTile == null && leftTile != null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(1 + i, 2);
            }
            else if (upTile != null && downTile != null && leftTile == null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(0, i);
            }
            else if (upTile != null && downTile != null && leftTile != null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(4, i);
            }

            else if (upTile == null && downTile != null && leftTile == null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(0 + i * 2, 3);
            }
            else if (upTile == null && downTile != null && leftTile != null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(1 + i * 2, 3);
            }
            else if (upTile != null && downTile == null && leftTile == null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(0 + i * 2, 4);
            }
            else if (upTile != null && downTile == null && leftTile != null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(1 + i * 2, 4);
            }

            else if (upTile == null && downTile == null && leftTile != null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(6 + i, 4);
            }
            else if (upTile != null && downTile != null && leftTile == null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(5, i);
            }

            else if (upTile == null && downTile == null && leftTile == null && rigthTile != null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(9, i);
            }
            else if (upTile == null && downTile == null && leftTile != null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(12, i);
            }
            else if (upTile == null && downTile != null && leftTile == null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(6 + i, 0);
            }
            else if (upTile != null && downTile == null && leftTile == null && rigthTile == null)
            {
                rectShape.TextureRect = spriteSheet.GetTextureRect(6 + i, 3);
            }
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            states.Transform *= Transform;

            target.Draw(rectShape, states);
        }
    }
}
