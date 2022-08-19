using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Scenes;

namespace Relm.Graphics.PostProcessing
{
	public interface IFinalRenderDelegate
    {
        void OnAddedToScene(Scene scene);
        void OnSceneBackBufferSizeChanged(int newWidth, int newHeight);
        void HandleFinalRender(RenderTarget2D finalRenderTarget, Color letterboxColor, RenderTarget2D source, Rectangle finalRenderDestinationRect, SamplerState samplerState);
        void Unload();
    }
}