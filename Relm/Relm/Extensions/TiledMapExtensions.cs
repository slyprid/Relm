using MonoGame.Extended.Tiled;

namespace Relm.Extensions
{
    public static class TiledMapExtensions
    {
        public static int GetCollisionTileIndex(this TiledMap map, string tilesetName)
        {
            var ret = 0;
            foreach (var tileset in map.Tilesets)
            {
                if (tileset.Name.Contains(tilesetName))
                {
                    ret += 1;
                }
                else
                {
                    ret += tileset.TileCount;
                }
            }
            return ret;
        }
    }
}