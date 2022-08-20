using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core;
using Relm.Graphics.Tweening;
using Relm.Scenes;
using Relm.Systems;

namespace Relm.Graphics.Transitions
{
	public class FadeTransition : SceneTransition
	{
		/// <summary>
		/// the color we will fade to/from
		/// </summary>
		public Color FadeToColor = Color.Black;

		/// <summary>
		/// duration to fade to fadeToColor
		/// </summary>
		public float FadeOutDuration = 0.4f;

		/// <summary>
		/// delay to start fading out
		/// </summary>
		public float DelayBeforeFadeInDuration = 0.1f;

		/// <summary>
		/// duration to fade from fadeToColor to the new Scene
		/// </summary>
		public float FadeInDuration = 0.6f;

		/// <summary>
		/// ease equation to use for the fade
		/// </summary>
		public EaseType FadeEaseType = EaseType.QuartOut;

		Color _fromColor = Color.White;
		Color _toColor = Color.Transparent;

		Texture2D _overlayTexture;
		Color _color = Color.White;
		Rectangle _destinationRect;


		public FadeTransition(Func<Scene> sceneLoadAction) : base(sceneLoadAction, true)
		{
			_destinationRect = PreviousSceneRender.Bounds;
		}

		public FadeTransition() : this(null)
		{ }

		public override IEnumerator OnBeginTransition()
		{
			// create a single pixel texture of our fadeToColor
			_overlayTexture = RelmGraphics.CreateSingleColorTexture(1, 1, FadeToColor);

			var elapsed = 0f;
			while (elapsed < FadeOutDuration)
			{
				elapsed += Time.DeltaTime;
				_color = Lerps.Ease(FadeEaseType, ref _toColor, ref _fromColor, elapsed, FadeOutDuration);

				yield return null;
			}

			// load up the new Scene
			yield return RelmGame.StartCoroutine(LoadNextScene());

			// dispose of our previousSceneRender. We dont need it anymore.
			PreviousSceneRender.Dispose();
			PreviousSceneRender = null;

			yield return Coroutine.WaitForSeconds(DelayBeforeFadeInDuration);

			elapsed = 0f;
			while (elapsed < FadeInDuration)
			{
				elapsed += Time.DeltaTime;
				_color = Lerps.Ease(EaseHelper.OppositeEaseType(FadeEaseType), ref _fromColor, ref _toColor, elapsed, FadeInDuration);

				yield return null;
			}

			TransitionComplete();
			_overlayTexture.Dispose();
		}

		public override void Render(SpriteBatch spriteBatch)
		{
			RelmGame.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(BlendState.NonPremultiplied, RelmGame.DefaultSamplerState, DepthStencilState.None, null);

			// we only render the previousSceneRender while fading to _color. It will be null after that.
			if (!_isNewSceneLoaded)
                spriteBatch.Draw(PreviousSceneRender, _destinationRect, Color.White);

            spriteBatch.Draw(_overlayTexture, new Rectangle(0, 0, Screen.Width, Screen.Height), _color);

            spriteBatch.End();
		}
	}
}