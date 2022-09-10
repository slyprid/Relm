using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.Effects
{
    public class GrayscaleEffect : Effect
    {
        public GrayscaleEffect() 
            : base(RelmGame.GraphicsDevice, EffectResource.GrayscaleBytes)
        {
        }
    }
}