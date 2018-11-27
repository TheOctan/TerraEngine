using GameEngine.Core;
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
    class World : Transformable, Drawable
    {
        public const int WORLD_SIZE = 5;

        Chunk[][] chunks;
        RectangleShape sky;
        RectangleShape forest;
        RectangleShape field;

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

            chunks = new Chunk[WORLD_SIZE][];
            for (int i = 0; i < WORLD_SIZE; i++)
                chunks[i] = new Chunk[WORLD_SIZE];
        }

        private void UpdateBackgroud(object sender, SFML.Window.SizeEventArgs e)
        {
            sky.Size = new Vector2f(e.Width, e.Height);
            forest.Size = new Vector2f(e.Width, e.Height / 2);
            field.Size = new Vector2f(e.Width, e.Height / 3);

            forest.Position = new Vector2f(0, e.Height / 4);
            field.Position = new Vector2f(0, e.Height - field.Size.Y);
        }

        public void GenerateWorld()
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
    }
}
