using Microsoft.Xna.Framework;
using Relm.Collections;
using Relm.Components;
using Relm.Graphics.Tweening.Interfaces;
using Relm.Graphics.Tweening.Tweens;

namespace Relm.Graphics.Tweening
{
	public class RenderableColorTween : ColorTween, ITweenTarget<Color>
    {
        RenderableComponent _renderable;


        public void SetTweenedValue(Color value)
        {
            _renderable.Color = value;
        }


        public Color GetTweenedValue()
        {
            return _renderable.Color;
        }


        public new object GetTargetObject()
        {
            return _renderable;
        }


        protected override void UpdateValue()
        {
            SetTweenedValue(Lerps.Ease(_easeType, _fromValue, _toValue, _elapsedTime, _duration));
        }


        public void SetTarget(RenderableComponent renderable)
        {
            _renderable = renderable;
        }

        public override void RecycleSelf()
        {
            if (_shouldRecycleTween)
            {
                _renderable = null;
                _target = null;
                _nextTween = null;
            }

            if (_shouldRecycleTween && TweenManager.CacheColorTweens)
                Pool<RenderableColorTween>.Free(this);
        }
    }
}