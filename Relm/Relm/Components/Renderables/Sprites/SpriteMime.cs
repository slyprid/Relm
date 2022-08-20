using Relm.Extensions;
using Relm.Graphics;
using Relm.Math;

namespace Relm.Components.Renderables.Sprites
{
    public class SpriteMime : RenderableComponent
    {
        public override float Width => _spriteToMime.Width;
        public override float Height => _spriteToMime.Height;
        public override RectangleF Bounds => _spriteToMime.Bounds;

        SpriteRenderer _spriteToMime;


        public SpriteMime()
        {
            Enabled = false;
        }

        public SpriteMime(SpriteRenderer spriteToMime)
        {
            _spriteToMime = spriteToMime;
        }

        public override void OnAddedToEntity()
        {
            if (_spriteToMime == null)
                _spriteToMime = this.GetComponent<SpriteRenderer>();
        }

        public override void Render(SpriteBatch spriteBatch, Camera camera)
        {
            spriteBatch.Draw(_spriteToMime.Sprite, Entity.Transform.Position + _localOffset, Color,
                Entity.Transform.Rotation, _spriteToMime.Origin, Entity.Transform.Scale, _spriteToMime.SpriteEffects,
                _layerDepth);
        }
    }
}