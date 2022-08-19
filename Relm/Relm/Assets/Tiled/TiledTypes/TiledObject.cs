using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Relm.Assets.Tiled
{
    public class TiledObject : ITiledElement
    {
        public int Id;
        public string Name { get; set; }
        public TiledObjectType ObjectType;
        public string Type;
        public float X;
        public float Y;
        public float Width;
        public float Height;
        public float Rotation;
        public TiledLayerTile Tile;
        public bool Visible;
        public TiledText Text;

        public Vector2[] Points;
        public Dictionary<string, string> Properties;
    }
}