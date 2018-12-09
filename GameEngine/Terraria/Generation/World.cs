using GameEngine.Core;
using GameEngine.Resource;
using SFML.Graphics;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine.Terraria.Generation
{
    public class World : Transformable, Drawable
    {
        public const int WORLD_SIZE = 5;

        Chunk[][] chunks;
        RectangleShape sky;
        RectangleShape forest;
        RectangleShape field;

        string[] cells;

        public World()
        {
            sky = new RectangleShape(new Vector2f(Game.Window.Size.X, Game.Window.Size.Y));
            sky.Texture = ResourceHolder.Textures.Get(Content.BACKGROUND_DIR + "Background_0");

            forest = new RectangleShape(new Vector2f(Game.Window.Size.X, Game.Window.Size.Y / 2));
            forest.Position = new Vector2f(0, Game.Window.Size.Y / 4);
            forest.Texture = ResourceHolder.Textures.Get(Content.BACKGROUND_DIR + "Background_11");

            field = new RectangleShape(new Vector2f(Game.Window.Size.X, Game.Window.Size.Y / 3));
            field.Position = new Vector2f(0, Game.Window.Size.Y - field.Size.Y);
            field.Texture = ResourceHolder.Textures.Get(Content.BACKGROUND_DIR + "Background_17");

            Game.Window.Resized += UpdateBackgroud;

            ResetWorld();
        }

        private void UpdateBackgroud(object sender, SizeEventArgs e)
        {
            sky.Size = new Vector2f(e.Width, e.Height);
            forest.Size = new Vector2f(e.Width, e.Height / 2);
            field.Size = new Vector2f(e.Width, e.Height / 3);

            forest.Position = new Vector2f(0, e.Height / 4);
            field.Position = new Vector2f(0, e.Height - field.Size.Y);
        }

        public void GenerateWorld(string location)
        {
            if(LoadFromFile("Levels/" + location + ".level"))
            {
                ResetWorld();

                for (int y = 0; y < cells.Length; y++)
                {
                    for (int x = 0; x < cells[y].Length; x++)
                    {
                        switch (cells[y][x])
                        {
                            //case ' ': SetTile(TileType.None, x, y); break;
                            case '0': SetTile(TileType.Ground, x, y); break;
                            case '1': SetTile(TileType.Grass, x, y); break;
                            case '2': SetTile(TileType.DriedGrass, x, y); break;
                            case '3': SetTile(TileType.Hellstone, x, y); break;
                            case '4': SetTile(TileType.FrozenGrass, x, y); break;
                            case '5': SetTile(TileType.RawGrass, x, y); break;
                            case '6': SetTile(TileType.MarshGrass, x, y); break;
                            case '7': SetTile(TileType.Delta, x, y); break;
                            case '8': SetTile(TileType.Delta2, x, y); break;
                            case '9': SetTile(TileType.GeariteBrick, x, y); break;
                            case 'a': SetTile(TileType.Molybdenum, x, y); break;
                            case 'b': SetTile(TileType.PermafrostHellstone, x, y); break;
                            case 'c': SetTile(TileType.SampleOre3, x, y); break;
                            case 'd': SetTile(TileType.SampleOre4, x, y); break;
                            case 'e': SetTile(TileType.SoulIce, x, y); break;
                        }
                    }
                }
            }
            else
            {
                for (int x = 3; x <= 46; x++)
                    for (int y = 17; y <= 17; y++)
                        SetTile(TileType.Grass, x, y);

                for (int x = 3; x <= 46; x++)
                    for (int y = 18; y <= 32; y++)
                        SetTile(TileType.Ground, x, y);

                for (int x = 3; x <= 4; x++)
                    for (int y = 1; y <= 17; y++)
                        SetTile(TileType.Ground, x, y);

                for (int x = 45; x <= 46; x++)
                    for (int y = 1; y <= 17; y++)
                        SetTile(TileType.Ground, x, y);
            }
        }

        public void SetTile(TileType type, int x, int y)
        {
            var chunk = GetChunk(x, y);
            var tilePos = GetTilePosFromChunk(x, y);

            Tile upTile     = GetTile(x    , y - 1);
            Tile downTile   = GetTile(x    , y + 1);
            Tile leftTile   = GetTile(x - 1, y    );
            Tile rigthTile  = GetTile(x + 1, y    );

            chunk.SetTile(type, tilePos.X, tilePos.Y, upTile, downTile, leftTile, rigthTile);
        }

        public Tile GetTile(int x, int y)
        {
            var chunk = GetChunk(x, y);
            if (chunk == null) return null;

            var tilePos = GetTilePosFromChunk(x, y);

            return chunk.GetTile(tilePos.X, tilePos.Y);
        }

        public Chunk GetChunk(int x, int y)
        {
            int X = x / Chunk.CHUNK_SIZE;
            int Y = y / Chunk.CHUNK_SIZE;

            if (X >= WORLD_SIZE || Y >= WORLD_SIZE || X < 0 || Y < 0)
            {
                return null;
            }

            if(chunks[X][Y] == null)
            {
                chunks[X][Y] = new Chunk(new Vector2i(X, Y));
            }

            return chunks[X][Y];
        }

        public Vector2i GetTilePosFromChunk(int x, int y)
        {
            int X = x / Chunk.CHUNK_SIZE;
            int Y = y / Chunk.CHUNK_SIZE;

            return new Vector2i(x - X * Chunk.CHUNK_SIZE, y - Y * Chunk.CHUNK_SIZE);
        }

        public void Draw(RenderTarget target, RenderStates states)
        {
            target.Draw(sky);
            target.Draw(forest);
            target.Draw(field);

            // рисуем чанки
            for (int x = 0; x < WORLD_SIZE; x++)
            {
                for (int y = 0; y < WORLD_SIZE; y++)
                {
                    if (chunks[x][y] == null) continue;

                    target.Draw(chunks[x][y]);
                }
            }
        }

        private bool LoadFromFile(string fileName)
        {
            if (!File.Exists(fileName)) return false;

            cells = File.ReadAllLines(fileName);

            return true;
        }

        private void ResetWorld()
        {
            chunks = new Chunk[WORLD_SIZE][];
            for (int i = 0; i < WORLD_SIZE; i++)
                chunks[i] = new Chunk[WORLD_SIZE];
        }
    }
}
