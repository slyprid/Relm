using System;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Components;
using Relm.Graphics.Textures;
using Relm.Scenes;

namespace Relm.Graphics.Renderers
{
	public abstract class Renderer 
        : IComparable<Renderer>
	{
		public Material Material = Material.DefaultMaterial;
        public Camera Camera;
        public readonly int RenderOrder = 0;
        public RenderTexture RenderTexture;
        public Color RenderTargetClearColor = Color.Transparent;
        public bool ShouldDebugRender = true;
        public virtual bool WantsToRenderToSceneRenderTarget => RenderTexture == null;
        public bool WantsToRenderAfterPostProcessors;
        protected Material _currentMaterial;

        protected Renderer(int renderOrder) : this(renderOrder, null) { }

		protected Renderer(int renderOrder, Camera camera)
		{
			Camera = camera;
			RenderOrder = renderOrder;
		}

		public virtual void OnAddedToScene(Scene scene) { }
        public virtual void Unload() => RenderTexture?.Dispose();

		protected virtual void BeginRender(Camera cam)
		{
			if (RenderTexture != null)
			{
				RelmGame.GraphicsDevice.SetRenderTarget(RenderTexture);
				RelmGame.GraphicsDevice.Clear(RenderTargetClearColor);
			}

			_currentMaterial = Material;
            RelmGraphics.Instance.SpriteBatch.Begin(_currentMaterial, cam.TransformMatrix);
		}

		public abstract void Render(Scene scene);

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void RenderAfterStateCheck(IRenderable renderable, Camera cam)
		{
			if (renderable.Material != null && renderable.Material != _currentMaterial)
			{
				_currentMaterial = renderable.Material;
				if (_currentMaterial.Effect != null)
					_currentMaterial.OnPreRender(cam);
				FlushBatch(cam);
			}
			else if (renderable.Material == null && _currentMaterial != Material)
			{
				_currentMaterial = Material;
				FlushBatch(cam);
			}

			renderable.Render(RelmGraphics.Instance.SpriteBatch, cam);
		}

		void FlushBatch(Camera cam)
		{
			RelmGraphics.Instance.SpriteBatch.End();
			RelmGraphics.Instance.SpriteBatch.Begin(_currentMaterial, cam.TransformMatrix);
		}

		protected virtual void EndRender() => RelmGraphics.Instance.SpriteBatch.End();

		protected virtual void DebugRender(Scene scene, Camera cam)
		{
			RelmGraphics.Instance.SpriteBatch.End();
			RelmGraphics.Instance.SpriteBatch.Begin(cam.TransformMatrix);

			for (var i = 0; i < scene.Entities.Count; i++)
			{
				var entity = scene.Entities[i];
				if (entity.Enabled)
					entity.DebugRender(RelmGraphics.Instance.SpriteBatch);
			}
		}

		public virtual void OnSceneBackBufferSizeChanged(int newWidth, int newHeight) => RenderTexture?.OnSceneBackBufferSizeChanged(newWidth, newHeight);

		public int CompareTo(Renderer other) => RenderOrder.CompareTo(other.RenderOrder);
	}
}