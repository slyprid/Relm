using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core;
using Relm.Graphics.Tweening;
using Relm.Scenes;

namespace Relm.Graphics.Transitions
{
    public abstract class SceneTransition
    {
        private bool _hasPreviousSceneRender;
        
        internal bool _loadsNewScene;
        internal bool _isNewSceneLoaded;
        internal bool HasPreviousSceneRender
        {
            get
            {
                if (_hasPreviousSceneRender) return true;
                _hasPreviousSceneRender = true;
                return false;

            }
        }

        protected Func<Scene> SceneLoadAction { get; set; }

        public RenderTarget2D PreviousSceneRender { get; set; }
        public bool WantsPreviousSceneRender { get; set; }
        public bool LoadSceneOnBackgroundThread { get; set; }
        public Action OnScreenObscured { get; set; }
        public Action OnTransitionCompleted { get; set; }

        protected SceneTransition(bool wantsPreviousSceneRender = true) : this(null, wantsPreviousSceneRender) { }

        protected SceneTransition(Func<Scene> sceneLoadAction, bool wantsPreviousSceneRender = true)
        {
            SceneLoadAction = sceneLoadAction;
            WantsPreviousSceneRender = wantsPreviousSceneRender;
            _loadsNewScene = sceneLoadAction != null;

            if (wantsPreviousSceneRender)
                PreviousSceneRender = new RenderTarget2D(RelmGame.GraphicsDevice, Screen.Width, Screen.Height, false, Screen.BackBufferFormat, DepthFormat.None, 0, RenderTargetUsage.PreserveContents);
        }

		protected IEnumerator LoadNextScene()
		{
			OnScreenObscured?.Invoke();

			if (!_loadsNewScene)
			{
				_isNewSceneLoaded = true;
				yield break;
			}

			if (LoadSceneOnBackgroundThread)
			{
				Task.Run(() =>
				{
					var scene = SceneLoadAction();

					RelmGame.Schedule(0, false, null, timer =>
					{
                        RelmGame.Scene = scene;
						_isNewSceneLoaded = true;
					});
				});
			}
			else
			{
                RelmGame.Scene = SceneLoadAction();
				_isNewSceneLoaded = true;
			}

			while (!_isNewSceneLoaded) yield return null;
		}

		public virtual IEnumerator OnBeginTransition()
		{
			yield return null;
			yield return RelmGame.StartCoroutine(LoadNextScene());

			TransitionComplete();
		}

		public virtual void PreRender(SpriteBatch batcher) { }

		public virtual void Render(SpriteBatch batcher)
		{
            RelmGame.GraphicsDevice.SetRenderTarget(null);
			batcher.Begin(BlendState.Opaque, RelmGame.DefaultSamplerState, DepthStencilState.None, null);
			batcher.Draw(PreviousSceneRender, Vector2.Zero, Color.White);
			batcher.End();
		}

		protected virtual void TransitionComplete()
		{
            RelmGame.Instance._sceneTransition = null;

			if (PreviousSceneRender != null)
			{
				PreviousSceneRender.Dispose();
				PreviousSceneRender = null;
			}

			if (OnTransitionCompleted != null)
				OnTransitionCompleted();
		}

		public IEnumerator TickEffectProgressProperty(Effect effect, float duration, EaseType easeType = EaseType.ExpoOut, bool reverseDirection = false)
		{
			var start = reverseDirection ? 1f : 0f;
			var end = reverseDirection ? 0f : 1f;
			var progressParam = effect.Parameters["_progress"];

			var elapsed = 0f;
			while (elapsed < duration)
			{
				elapsed += Time.DeltaTime;
				var step = Lerps.Ease(easeType, start, end, elapsed, duration);
				progressParam.SetValue(step);

				yield return null;
			}
		}
	}
}