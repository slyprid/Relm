using System.Collections.Generic;
using Relm.Collections;
using Relm.Components;
using Relm.Core;
using Relm.Entities;
using Relm.Graphics.Tweening.Interfaces;

namespace Relm.Graphics.Tweening
{
	public class TweenManager : GlobalManager
	{
		public static EaseType DefaultEaseType = EaseType.QuartIn;

		public static bool RemoveAllTweensOnLevelLoad = false;


		#region Caching rules

		public static bool CacheIntTweens = true;

		public static bool CacheFloatTweens = true;
		public static bool CacheVector2Tweens = true;
		public static bool CacheVector3Tweens;
		public static bool CacheVector4Tweens;
		public static bool CacheQuaternionTweens;
		public static bool CacheColorTweens = true;
		public static bool CacheRectTweens;

		#endregion


		FastList<ITweenable> _activeTweens = new FastList<ITweenable>();
		public static IReadOnlyList<ITweenable> ActiveTweens => _instance._activeTweens.Buffer;

		FastList<ITweenable> _tempTweens = new FastList<ITweenable>();

		bool _isUpdating;

		static TweenManager _instance;


		public TweenManager()
		{
			_instance = this;
		}


		public override void Update()
		{
			_isUpdating = true;

			for (var i = _activeTweens.Length - 1; i >= 0; --i)
			{
				var tween = _activeTweens.Buffer[i];
				if (tween.Tick())
					_tempTweens.Add(tween);
			}

			_isUpdating = false;

			for (var i = 0; i < _tempTweens.Length; i++)
			{
				_tempTweens.Buffer[i].RecycleSelf();
				_activeTweens.Remove(_tempTweens[i]);
			}

			_tempTweens.Clear();
		}


		#region Tween management

		public static void AddTween(ITweenable tween)
		{
			_instance._activeTweens.Add(tween);
		}
		
		public static void RemoveTween(ITweenable tween)
		{
			if (_instance._isUpdating)
			{
				_instance._tempTweens.Add(tween);
			}
			else
			{
				tween.RecycleSelf();
				_instance._activeTweens.Remove(tween);
			}
		}
		
		public static void StopAllTweens(bool bringToCompletion = false)
		{
			for (var i = _instance._activeTweens.Length - 1; i >= 0; --i)
				_instance._activeTweens.Buffer[i].Stop(bringToCompletion);
		}
		
		public static List<ITweenable> AllTweensWithContext(object context)
		{
			var foundTweens = new List<ITweenable>();

			for (var i = 0; i < _instance._activeTweens.Length; i++)
			{
				if (_instance._activeTweens.Buffer[i] is ITweenable &&
					(_instance._activeTweens.Buffer[i] as ITweenControl).Context == context)
					foundTweens.Add(_instance._activeTweens.Buffer[i]);
			}

			return foundTweens;
		}

        public static void StopAllTweensWithContext(object context, bool bringToCompletion = false)
		{
			for (var i = _instance._activeTweens.Length - 1; i >= 0; --i)
			{
				if (_instance._activeTweens.Buffer[i] is ITweenable &&
					(_instance._activeTweens.Buffer[i] as ITweenControl).Context == context)
					_instance._activeTweens.Buffer[i].Stop(bringToCompletion);
			}
		}

		public static List<ITweenable> AllTweensWithTarget(object target)
		{
			var foundTweens = new List<ITweenable>();

			for (var i = 0; i < _instance._activeTweens.Length; i++)
			{
				if (_instance._activeTweens[i] is ITweenControl)
				{
					var tweenControl = _instance._activeTweens.Buffer[i] as ITweenControl;
					if (tweenControl.GetTargetObject() == target)
						foundTweens.Add(_instance._activeTweens[i] as ITweenable);
				}
			}

			return foundTweens;
		}

		public static List<ITweenable> AllTweensWithTargetEntity(Entity target)
		{
			var foundTweens = new List<ITweenable>();

			for (var i = 0; i < _instance._activeTweens.Length; i++)
			{
				if (
					_instance._activeTweens[i] is ITweenControl tweenControl && (
						tweenControl.GetTargetObject() is Entity entity && entity == target ||
						tweenControl.GetTargetObject() is Component component && component.Entity == target ||
						tweenControl.GetTargetObject() is Transform transform && transform.Entity == target
					)
				)
					foundTweens.Add(_instance._activeTweens[i] as ITweenable);
			}

			return foundTweens;
		}

		public static void StopAllTweensWithTarget(object target, bool bringToCompletion = false)
		{
			for (var i = _instance._activeTweens.Length - 1; i >= 0; --i)
			{
				if (_instance._activeTweens[i] is ITweenControl)
				{
					var tweenControl = _instance._activeTweens.Buffer[i] as ITweenControl;
					if (tweenControl.GetTargetObject() == target)
						tweenControl.Stop(bringToCompletion);
				}
			}
		}

		#endregion
	}
}