using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Naming;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class HorizontalBreak
        : BaseControl 
    {
        public HorizontalBreak(TextureAtlas skin) 
            : base(skin)
        {
            Width = 96;
            Height = 32;

            Initialize();
        }

        private void Initialize()
        {
            var w = UserInterfaceSkin.HorizontalBreakWidth;
            var h = UserInterfaceSkin.HorizontalBreakHeight;

            Children.Clear();

            // Left
            AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakLeft))
                .WithPosition(0, 0);

            int xo;
            for (xo = 0; xo <= Width - (w * 3); xo += w)
            {
                // Center
                AddChild<ChildControl>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakCenter))
                    .WithPosition(w + xo, 0);
            }

            // Right
            AddChild<ChildControl>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakRight))
                .WithPosition(w + xo, 0);
        }

        #region Fluent Functions

        public HorizontalBreak WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            return this;
        }

        public HorizontalBreak WithSize(int w, int h)
        {
            return WithSize(new Vector2(w, h));
        }

        public HorizontalBreak WithSize(float w, float h)
        {
            return WithSize(new Vector2(w, h));
        }

        #endregion
    }
}