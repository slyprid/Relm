using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
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

        public Tile this[int x, int y]
        {
            get => Tiles[x, y];
            set => Tiles[x, y] = value;
        }

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
                    Tiles[x, y] = new Tile
                    {
                        Size = TileSize,
                        Position = new Vector2(x * TileWidth, y * TileHeight)
                    };
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

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    Tiles[x, y].Draw(gameTime, spriteBatch);
                }
            }
        }
    }
}