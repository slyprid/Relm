using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core;
using Relm.Graphics.Tweening;
using Relm.Scenes;

namespace Relm.Graphics.Transitions
{
    public class CrossFadeTransition : SceneTransition
    {
        /// <summary>
        /// duration for the fade
        /// </summary>
        public float FadeDuration = 1f;

        /// <summary>
        /// ease equation to use for the cross fade
        /// </summary>
        public EaseType FadeEaseType = EaseType.QuartIn;

        Color _fromColor = Color.White;
        Color _toColor = Color.Transparent;
        Color _color = Color.White;


        public CrossFadeTransition(Func<Scene> sceneLoadAction) : base(sceneLoadAction, true)
        {
        }

        public CrossFadeTransition() : this(null)
        {
        }

        public override IEnumerator OnBeginTransition()
        {
            yield return null;

            // load up the new Scene
            yield return RelmGame.StartCoroutine(LoadNextScene());

            var elapsed = 0f;
            while (elapsed < FadeDuration)
            {
                elapsed += Time.DeltaTime;
                _color = Lerps.Ease(FadeEaseType, ref _fromColor, ref _toColor, elapsed, FadeDuration);

                yield return null;
            }

            TransitionComplete();
        }

        public override void Render(SpriteBatch spriteBatch)
        {
            RelmGame.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(BlendState.NonPremultiplied, RelmGame.DefaultSamplerState, DepthStencilState.None, null);
            spriteBatch.Draw(PreviousSceneRender, Vector2.Zero, _color);
            spriteBatch.End();
        }
    }
}