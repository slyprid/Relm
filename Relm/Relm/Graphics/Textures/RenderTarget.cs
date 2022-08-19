using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core;

namespace Relm.Graphics.Textures
{
	public class RenderTarget 
        : GlobalManager
	{
		internal class TrackedRenderTarget2D : RenderTarget2D
		{
			public uint LastFrameUsed;

            public TrackedRenderTarget2D(int width, int height, SurfaceFormat preferredFormat, DepthFormat preferredDepthFormat) : base(RelmGame.GraphicsDevice, width, height, false, preferredFormat, preferredDepthFormat, 0, RenderTargetUsage.PreserveContents) { }
		}

		internal static RenderTarget instance;

		List<TrackedRenderTarget2D> _renderTargetPool = new List<TrackedRenderTarget2D>();
		
		public RenderTarget()
		{
			instance = this;
		}


		#region Temporary RenderTarget management

		public static RenderTarget2D GetTemporary(int width, int height)
		{
			return GetTemporary(width, height, Screen.PreferredDepthStencilFormat);
		}
		
		public static RenderTarget2D GetTemporary(int width, int height, DepthFormat depthFormat)
		{
			RenderTarget2D tempRenderTarget = null;
			int tempRenderTargetIndex = -1;
			for (var i = 0; i < instance._renderTargetPool.Count; i++)
			{
				var renderTarget = instance._renderTargetPool[i];
				if (renderTarget.Width == width && renderTarget.Height == height &&
					renderTarget.DepthStencilFormat == depthFormat)
				{
					tempRenderTarget = renderTarget;
					tempRenderTargetIndex = i;
					break;
				}
			}

			if (tempRenderTargetIndex >= 0)
			{
				instance._renderTargetPool.RemoveAt(tempRenderTargetIndex);
				return tempRenderTarget;
			}

			return new TrackedRenderTarget2D(width, height, SurfaceFormat.Color, depthFormat);
		}


		public static void ReleaseTemporary(RenderTarget2D renderTarget)
		{
			Assert.IsTrue(renderTarget is TrackedRenderTarget2D, "Attempted to release a temporary RenderTarget2D that is not managed by the system");

			var trackedRT = renderTarget as TrackedRenderTarget2D;
			trackedRT.LastFrameUsed = Time.FrameCount;
			instance._renderTargetPool.Add(trackedRT);
		}

		#endregion


		#region RenderTarget2D creation helpers

		public static RenderTarget2D Create()
		{
			return Create(Screen.Width, Screen.Height, Screen.BackBufferFormat, Screen.PreferredDepthStencilFormat);
		}
		
		public static RenderTarget2D Create(DepthFormat preferredDepthFormat)
		{
			return Create(Screen.Width, Screen.Height, Screen.BackBufferFormat, preferredDepthFormat);
		}
		
		public static RenderTarget2D Create(int width, int height)
		{
			return Create(width, height, Screen.BackBufferFormat, Screen.PreferredDepthStencilFormat);
		}
		
		public static RenderTarget2D Create(int width, int height, DepthFormat preferredDepthFormat)
		{
			return Create(width, height, Screen.BackBufferFormat, preferredDepthFormat);
		}
		
		public static RenderTarget2D Create(int width, int height, SurfaceFormat preferredFormat,
											DepthFormat preferredDepthFormat)
		{
			return new RenderTarget2D(RelmGame.GraphicsDevice, width, height, false, preferredFormat, preferredDepthFormat,
				0, RenderTargetUsage.PreserveContents);
		}

		#endregion

		public override void Update()
		{
			for (var i = _renderTargetPool.Count - 1; i >= 0; i--)
			{
				if (_renderTargetPool[i].LastFrameUsed + 2 < Time.FrameCount)
				{
					_renderTargetPool[i].Dispose();
					_renderTargetPool.RemoveAt(i);
				}
			}
		}
	}
}