using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core;
using Relm.Graphics.Textures;

namespace Relm.Components.Renderables.Sprites
{
	public class ScrollingSpriteRenderer : TiledSpriteRenderer, IUpdateable
    {
        /// <summary>
        /// x speed of automatic scrolling in pixels/s
        /// </summary>
        public float ScrollSpeedX = 15;

        /// <summary>
        /// y speed of automatic scrolling in pixels/s
        /// </summary>
        public float ScrollSpeedY = 0;

        /// <summary>
        /// scale of the texture
        /// </summary>
        /// <value>The texture scale.</value>
        public override Vector2 TextureScale
        {
            get => _textureScale;
            set
            {
                _textureScale = value;

                // recalulcate our inverseTextureScale and the source rect size
                _inverseTexScale = new Vector2(1f / _textureScale.X, 1f / _textureScale.Y);
            }
        }

        // accumulate scroll in a separate float so that we can round it without losing precision for small scroll speeds
        float _scrollX, _scrollY;


        public ScrollingSpriteRenderer()
        {
        }

        public ScrollingSpriteRenderer(Sprite sprite) : base(sprite)
        {
        }

        public ScrollingSpriteRenderer(Texture2D texture) : this(new Sprite(texture))
        {
        }

        public virtual void Update()
        {
            _scrollX += ScrollSpeedX * Time.DeltaTime;
            _scrollY += ScrollSpeedY * Time.DeltaTime;
            _sourceRect.X = (int)_scrollX;
            _sourceRect.Y = (int)_scrollY;
        }
    }
}