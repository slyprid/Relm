using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Entities;
using Relm.Tiles;

namespace Relm.Maps
{
    public class Map
        : DrawableEntity
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
                tileLayer.Clear();
                Layers.Add(tileLayer);
                return tileLayer;
            }

            var ret = Activator.CreateInstance<T>();
            Layers.Add(ret);
            return ret;
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var layer in Layers)
            {
                layer.Update(gameTime);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            foreach (var layer in Layers)
            {
                layer.Draw(gameTime, spriteBatch);
            }
        }
    }
}