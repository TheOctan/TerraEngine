using GameEngine.Resource;
using GameEngine.Terraria.AnimationManager;
using SFML.Graphics;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terraria.Generation
{
    public enum TileType
    {
        Ground,
        Grass,
        DriedGrass,
        Hellstone,
        FrozenGrass,
        RawGrass,
        MarshGrass,
        Delta,
        Delta2,
        GeariteBrick,
        Molybdenum,
        PermafrostHellstone,
        SampleOre3,
        SampleOre4,
        SoulIce,
        None
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
                case TileType.Ground:               rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_0"); break;
                case TileType.Grass:                rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_1"); break;
                case TileType.DriedGrass:           rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_2"); break;
                case TileType.Hellstone:            rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_3"); break;
                case TileType.FrozenGrass:          rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_4"); break;
                case TileType.RawGrass:             rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_5"); break;
                case TileType.MarshGrass:           rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Tiles_6"); break;
                case TileType.Delta:                rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Delta"); break;
                case TileType.Delta2:               rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Delta_2"); break;
                case TileType.GeariteBrick:         rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "GeariteBrick"); break;
                case TileType.Molybdenum:           rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Molybdenum"); break;
                case TileType.PermafrostHellstone:  rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "PermafrostHellstone"); break;
                case TileType.SampleOre3:           rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "SampleOre_3"); break;
                case TileType.SampleOre4:           rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "SampleOre_4"); break;
                case TileType.SoulIce:              rectShape.Texture = ResourceHolder.Textures.Get(Content.TILES_DIR + "Soul_Ice"); break;
                case TileType.None: break;
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
