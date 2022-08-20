using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Math;
using SpriteBatch = Relm.Graphics.SpriteBatch;

namespace Relm.Components.Renderables.Backgrounds
{
    public class GradientRenderer
        : RenderableComponent
    {
        private Texture2D _pixel;
        private readonly Color _startColor = Color.White;
        private readonly Color _endColor = Color.White;

        public GradientRenderer() { }

        public GradientRenderer(Color startColor, Color endColor)
        {
            _startColor = startColor;
            _endColor = endColor;
            _bounds = new RectangleF(0f, 0f, Screen.Width, Screen.Height);
            _areBoundsDirty = false;
        }

        public override void OnAddedToEntity()
        {
            base.OnAddedToEntity();

            _pixel = RelmGraphics.CreateSingleColorTexture(1, 1, Color.White);
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            for (var y = 0; y < Screen.Height; y++)
            {
                var p = ((float)y / (float)Screen.Height);
                var color = Color.Lerp(_startColor, _endColor, p);
                spriteBatch.Draw(_pixel, new Rectangle(0, y, Screen.Width, 1), color);
            }
        }
    }
}