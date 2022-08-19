using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Scenes;

namespace Relm.Graphics.PostProcessing
{
	public class PostProcessor 
        : IComparable<PostProcessor>
	{
		public bool Enabled;
        public readonly int ExecutionOrder = 0;
        public Effect Effect;
        public SamplerState SamplerState = RelmGame.DefaultSamplerState;
        public BlendState BlendState = BlendState.Opaque;
        protected internal Scene _scene;

        public PostProcessor(int executionOrder, Effect effect = null)
		{
			Enabled = true;
			ExecutionOrder = executionOrder;
			Effect = effect;
		}

		public virtual void OnAddedToScene(Scene scene)
		{
			_scene = scene;
		}

		public virtual void OnSceneBackBufferSizeChanged(int newWidth, int newHeight) { }

		public virtual void Process(RenderTarget2D source, RenderTarget2D destination)
		{
			DrawFullscreenQuad(source, destination, Effect);
		}

		public virtual void Unload()
		{
			if (Effect != null && Effect.Name != null)
			{
				_scene.Content.UnloadEffect(Effect);
				Effect = null;
			}

			_scene = null;
		}

		protected void DrawFullscreenQuad(Texture2D texture, RenderTarget2D renderTarget, Effect effect = null)
		{
			RelmGame.GraphicsDevice.SetRenderTarget(renderTarget);
			DrawFullscreenQuad(texture, renderTarget.Width, renderTarget.Height, effect);
		}

		protected void DrawFullscreenQuad(Texture2D texture, int width, int height, Effect effect)
		{
			RelmGraphics.Instance.SpriteBatch.Begin(BlendState, SamplerState, DepthStencilState.None, RasterizerState.CullNone, effect);
			RelmGraphics.Instance.SpriteBatch.Draw(texture, new Rectangle(0, 0, width, height), Color.White);
			RelmGraphics.Instance.SpriteBatch.End();
		}

		public int CompareTo(PostProcessor other)
		{
			return ExecutionOrder.CompareTo(other.ExecutionOrder);
		}
	}
}