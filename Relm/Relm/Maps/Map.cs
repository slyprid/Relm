using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Tiles;

namespace Relm.Maps
{
    public class Map
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Vector2 Size { get; set; }
        public Vector2 TileSize { get; set; }

        public List<MapLayer> Layers { get; set; }

        public Map()
        {
            Id = Guid.NewGuid();
            Size = new Vector2(25f, 25f);
            TileSize = new Vector2(32f, 32f);
            Layers = new List<MapLayer>();
        }

        public MapLayer AddLayer<T>()
            where T : MapLayer
        {
            if (typeof(T) == typeof(TileLayer))
            {
                var tileLayer = new TileLayer(Size, TileSize);
                Layers.Add(tileLayer);
                return tileLayer;
            }

            var ret = Activator.CreateInstance<T>();
            Layers.Add(ret);
            return ret;
        }
    }
}