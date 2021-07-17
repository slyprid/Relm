using Microsoft.Xna.Framework;
using Relm.Maps;

namespace Relm.Tiles
{
    public class TileLayer
        : MapLayer
    {
        public Vector2 TileSize { get; set; }

        public int TileWidth => (int) TileSize.X;
        public int TileHeight => (int) TileSize.Y;

        public Tile[,] Tiles { get; set; }

        public TileLayer(Vector2 size, Vector2 tileSize)
            : base(size)
        {
            TileSize = tileSize;
            Tiles = new Tile[Width, Height];
        }

        public override void Clear()
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Tiles[x, y] = new Tile();
                }
            }
        }

        public override void Update(GameTime gameTime)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Tiles[x, y].Update(gameTime);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Tiles[x, y].Draw(gameTime);
                }
            }
        }
    }
}