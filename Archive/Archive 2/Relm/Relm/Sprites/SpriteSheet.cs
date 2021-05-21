using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.States;

namespace Relm.Sprites
{
    public class SpriteSheet
    {
        public string TextureName { get; set; }
        public Texture2D Texture => GameState.Textures[TextureName];
        public int Width { get; set; }
        public int Height { get; set; }
        public List<Rectangle> Bounds { get; set; }
        public int Spacing { get; set; }

        public List<string> Names { get; set; }

        public Rectangle this[int index] => Bounds[index];
        public Rectangle this[string name] => Bounds[Names.IndexOf(name)];

        public SpriteSheet() { }

        public SpriteSheet(string textureName, int width, int height)
        {
            TextureName = textureName;
            Width = width;
            Height = height;
            Names = new List<string>();
            Initialize();
        }

        public SpriteSheet(string textureName, int width, int height, int spacing)
        {
            TextureName = textureName;
            Width = width;
            Height = height;
            Spacing = spacing;
            Names = new List<string>();
            Initialize();
        }

        public SpriteSheet(string textureName, Dictionary<string, Rectangle> atlas)
        {
            TextureName = textureName;
            Names = new List<string>();
            InitializeAtlas(atlas);
        }

        private void Initialize()
        {
            Bounds = new List<Rectangle>();
            for (var y = 0; y < Texture.Height; y += Height + Spacing)
            {
                for (var x = 0; x < Texture.Width; x += Width + Spacing)
                {
                    Bounds.Add(new Rectangle(x, y, Width, Height));
                }
            }
        }

        private void InitializeAtlas(Dictionary<string, Rectangle> atlas)
        {
            Bounds = new List<Rectangle>();
            foreach (var kvp in atlas)
            {
                Names.Add(kvp.Key);
                Bounds.Add(kvp.Value);
            }
        }

        public void AddNames(List<string> names)
        {
            Names = names;
        }
    }
}
