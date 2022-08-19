using System;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.Textures
{
	public class RenderTexture 
        : IDisposable
	{
		public enum RenderTextureResizeBehavior
		{
			None,
			SizeToSceneRenderTarget,
			SizeToScreen
		}

		public RenderTarget2D RenderTarget;

		public RenderTextureResizeBehavior ResizeBehavior = RenderTextureResizeBehavior.SizeToSceneRenderTarget;


		#region constructors

		public RenderTexture()
		{
			RenderTarget = Textures.RenderTarget.Create(Screen.Width, Screen.Height, Screen.BackBufferFormat, Screen.PreferredDepthStencilFormat);
		}


		public RenderTexture(DepthFormat preferredDepthFormat)
		{
			RenderTarget = Textures.RenderTarget.Create(Screen.Width, Screen.Height, Screen.BackBufferFormat, preferredDepthFormat);
		}


		public RenderTexture(int width, int height)
		{
			RenderTarget = Textures.RenderTarget.Create(width, height, Screen.BackBufferFormat, Screen.PreferredDepthStencilFormat);
		}


		public RenderTexture(int width, int height, DepthFormat preferredDepthFormat)
		{
			RenderTarget = Textures.RenderTarget.Create(width, height, Screen.BackBufferFormat, preferredDepthFormat);
		}


		public RenderTexture(int width, int height, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat)
		{
			RenderTarget = new RenderTarget2D(RelmGame.GraphicsDevice, width, height, false, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.PreserveContents);
		}

		#endregion


		public void OnSceneBackBufferSizeChanged(int newWidth, int newHeight)
		{
			switch (ResizeBehavior)
			{
				case RenderTextureResizeBehavior.None:
					break;
				case RenderTextureResizeBehavior.SizeToSceneRenderTarget:
					Resize(newWidth, newHeight);
					break;
				case RenderTextureResizeBehavior.SizeToScreen:
					Resize(Screen.Width, Screen.Height);
					break;
			}
		}


		public void ResizeToFitBackbuffer()
		{
			Resize(Screen.Width, Screen.Height);
		}


		public void Resize(int width, int height)
		{
			if (RenderTarget.Width == width && RenderTarget.Height == height && !RenderTarget.IsDisposed)
				return;

			var depthFormat = RenderTarget.DepthStencilFormat;

			Dispose();

			RenderTarget = Textures.RenderTarget.Create(width, height, depthFormat);
		}


		public void Dispose()
		{
			if (RenderTarget != null && !RenderTarget.IsDisposed)
			{
				RenderTarget.Dispose();
				RenderTarget = null;
			}
		}


		public static implicit operator RenderTarget2D(RenderTexture tex)
		{
			return tex.RenderTarget;
		}
	}
}