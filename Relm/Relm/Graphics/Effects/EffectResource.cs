using System;
using System.IO;
using Microsoft.Xna.Framework;
using Relm.Extensions;

namespace Relm.Graphics.Effects
{
	public static class EffectResource
	{
		internal static byte[] SpriteBlinkEffectBytes => GetFileResourceBytes("Content/nez/effects/SpriteBlinkEffect.mgfxo");

		internal static byte[] SpriteLinesEffectBytes => GetFileResourceBytes("Content/nez/effects/SpriteLines.mgfxo");

		internal static byte[] SpriteAlphaTestBytes => GetFileResourceBytes("Content/nez/effects/SpriteAlphaTest.mgfxo");

		internal static byte[] CrosshatchBytes => GetFileResourceBytes("Content/nez/effects/Crosshatch.mgfxo");

		internal static byte[] NoiseBytes => GetFileResourceBytes("Content/nez/effects/Noise.mgfxo");

		internal static byte[] TwistBytes => GetFileResourceBytes("Content/nez/effects/Twist.mgfxo");

		internal static byte[] DotsBytes => GetFileResourceBytes("Content/nez/effects/Dots.mgfxo");

		internal static byte[] DissolveBytes => GetFileResourceBytes("Content/nez/effects/Dissolve.mgfxo");

		internal static byte[] BloomCombineBytes => GetFileResourceBytes("Content/nez/effects/BloomCombine.mgfxo");

		internal static byte[] BloomExtractBytes => GetFileResourceBytes("Content/nez/effects/BloomExtract.mgfxo");

		internal static byte[] GaussianBlurBytes => GetFileResourceBytes("Content/nez/effects/GaussianBlur.mgfxo");

		internal static byte[] VignetteBytes => GetFileResourceBytes("Content/nez/effects/Vignette.mgfxo");

		internal static byte[] LetterboxBytes => GetFileResourceBytes("Content/nez/effects/Letterbox.mgfxo");

		internal static byte[] HeatDistortionBytes => GetFileResourceBytes("Content/nez/effects/HeatDistortion.mgfxo");

		internal static byte[] SpriteLightMultiplyBytes => GetFileResourceBytes("Content/nez/effects/SpriteLightMultiply.mgfxo");

		internal static byte[] PixelGlitchBytes => GetFileResourceBytes("Content/nez/effects/PixelGlitch.mgfxo");

		internal static byte[] StencilLightBytes => GetFileResourceBytes("Content/nez/effects/StencilLight.mgfxo");

		internal static byte[] DeferredSpriteBytes => GetFileResourceBytes("Content/nez/effects/DeferredSprite.mgfxo");

		internal static byte[] DeferredLightBytes => GetFileResourceBytes("Content/nez/effects/DeferredLighting.mgfxo");

		internal static byte[] ForwardLightingBytes => GetFileResourceBytes("Content/nez/effects/ForwardLighting.mgfxo");

		internal static byte[] PolygonLightBytes => GetFileResourceBytes("Content/nez/effects/PolygonLight.mgfxo");

		internal static byte[] SquaresTransitionBytes => GetFileResourceBytes("Content/nez/effects/transitions/Squares.mgfxo");

        internal static byte[] SpriteEffectBytes => GetMonoGameEmbeddedResourceBytes("Microsoft.Xna.Framework.Graphics.Effect.Resources.SpriteEffect.ogl.mgfxo");

		internal static byte[] MultiTextureOverlayBytes => GetFileResourceBytes("Content/nez/effects/MultiTextureOverlay.mgfxo");

		internal static byte[] ScanlinesBytes => GetFileResourceBytes("Content/nez/effects/Scanlines.mgfxo");

		internal static byte[] ReflectionBytes => GetFileResourceBytes("Content/nez/effects/Reflection.mgfxo");

		internal static byte[] GrayscaleBytes => GetFileResourceBytes("Content/nez/effects/Grayscale.mgfxo");

		internal static byte[] SepiaBytes => GetFileResourceBytes("Content/nez/effects/Sepia.mgfxo");

		internal static byte[] PaletteCyclerBytes => GetFileResourceBytes("Content/nez/effects/PaletteCycler.mgfxo");


		static byte[] GetEmbeddedResourceBytes(string name)
		{
			var assembly = typeof(EffectResource).Assembly;
            using var stream = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }


		internal static byte[] GetMonoGameEmbeddedResourceBytes(string name)
		{
			var assembly = typeof(MathHelper).Assembly;
			if (!assembly.GetManifestResourceNames().Contains(name)) name = name.Replace(".Framework", ".Framework.Platform");

            using var stream = assembly.GetManifestResourceStream(name);
            using var ms = new MemoryStream();
            stream.CopyTo(ms);
            return ms.ToArray();
        }


		public static byte[] GetFileResourceBytes(string path)
		{
			byte[] bytes;
			try
            {
                using var stream = TitleContainer.OpenStream(path);
                if (stream.CanSeek)
                {
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                }
                else
                {
                    using var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                }
            }
			catch (Exception ex)
			{
				var txt = $"OpenStream failed to find file at path: {path}. Did you add it to the Content folder and set its properties to copy to output directory?";
				throw new Exception(txt, ex);
			}

			return bytes;
		}
	}
}