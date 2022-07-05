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
            AddChild<BorderPiece>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakLeft))
                .WithPosition(0, 0);

            int xo;
            for (xo = 0; xo <= Width - (w * 2); xo += w)
            {
                // Center
                AddChild<BorderPiece>(TextureAtlas)
                    .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakCenter))
                    .WithPosition(w + xo, 0);
            }

            // Right
            AddChild<BorderPiece>(TextureAtlas)
                .WithAtlasRegionName(nameof(UserInterfaceRegions.HorizontalBreakRight))
                .WithPosition(w + xo, 0);
        }
    }
}