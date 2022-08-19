using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.Effects
{
    public class SpriteEffect 
        : Effect
    {
        private readonly EffectParameter _matrixTransformParam;

        public SpriteEffect() 
            : base(RelmGame.GraphicsDevice, EffectResource.SpriteEffectBytes)
        {
            _matrixTransformParam = Parameters["MatrixTransform"];
        }


        public void SetMatrixTransform(ref Matrix matrixTransform)
        {
            _matrixTransformParam.SetValue(matrixTransform);
        }
    }
}