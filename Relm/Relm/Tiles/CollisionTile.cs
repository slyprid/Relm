using Microsoft.Xna.Framework;
using MonoGame.Extended.Tiled;
using Relm.Collisions;

namespace Relm.Tiles
{
    public class CollisionTile
        : ICollider
    {
        public int X { get; set; }
        public int Y { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Rectangle Bounds => new Rectangle((int)(X * Width), (int)(Y * Height), Width, Height);

        public static CollisionTile From(TiledMap map, TiledMapTile tile)
        {
            return new CollisionTile
            {
                X = tile.X,
                Y = tile.Y,
                Width = map.TileWidth,
                Height = map.TileHeight
            };
        }
    }
}