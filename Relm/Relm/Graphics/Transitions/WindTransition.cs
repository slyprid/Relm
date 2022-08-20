using System;
using System.Collections;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Tweening;
using Relm.Scenes;

namespace Relm.Graphics.Transitions
{
    public class WindTransition : SceneTransition
    {
        /// <summary>
        /// how many wind segments should be used. Defaults to 100. (1 - 1000)
        /// </summary>
        /// <value>The wind segments.</value>
        public float WindSegments
        {
            set => _windEffect.Parameters["_windSegments"].SetValue(value);
        }

        /// <summary>
        /// size of the wind streaks. defaults to 0.3. (0.1 - 1)
        /// </summary>
        /// <value>The size.</value>
        public float Size
        {
            set => _windEffect.Parameters["_size"].SetValue(value);
        }

        /// <summary>
        /// duration for the wind transition
        /// </summary>
        public float Duration = 1f;

        /// <summary>
        /// ease equation to use for the animation
        /// </summary>
        public EaseType EaseType = EaseType.QuartOut;

        Effect _windEffect;
        Rectangle _destinationRect;


        public WindTransition(Func<Scene> sceneLoadAction) : base(sceneLoadAction, true)
        {
            _destinationRect = PreviousSceneRender.Bounds;

            // load Effect and set defaults
            _windEffect = RelmGame.Content.LoadEffect("Content/nez/effects/transitions/Wind.mgfxo");
            Size = 0.3f;
            WindSegments = 100;
        }


        public WindTransition() : this(null)
        {
        }


        public override IEnumerator OnBeginTransition()
        {
            // load up the new Scene
            yield return RelmGame.StartCoroutine(LoadNextScene());

            // wind to the new Scene
            yield return RelmGame.StartCoroutine(TickEffectProgressProperty(_windEffect, Duration, EaseType));

            TransitionComplete();

            // cleanup
            RelmGame.Content.UnloadEffect(_windEffect.Name);
        }


        public override void Render(SpriteBatch spriteBatch)
        {
            RelmGame.GraphicsDevice.SetRenderTarget(null);
            spriteBatch.Begin(BlendState.NonPremultiplied, RelmGame.DefaultSamplerState, DepthStencilState.None, null, _windEffect);
            spriteBatch.Draw(PreviousSceneRender, _destinationRect, Color.White);
            spriteBatch.End();
        }
    }
}