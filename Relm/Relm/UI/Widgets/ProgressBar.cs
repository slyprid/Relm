﻿using System;
using Relm.Extensions;
using Relm.Graphics;
using Relm.Input;
using Relm.Math;
using Relm.UI.Base;
using Relm.UI.Drawable;
using Relm.UI.Widgets.Styles;

namespace Relm.UI.Widgets
{
	public class ProgressBar : Element
	{
		#region properties and fields

		public event Action<float> OnChanged;

		public bool Disabled;

		public override float PreferredWidth
		{
			get
			{
				if (_vertical)
					return System.Math.Max(style.Knob == null ? 0 : style.Knob.MinWidth,
						style.Background != null ? style.Background.MinWidth : 0);
				else
					return 140;
			}
		}

		public override float PreferredHeight
		{
			get
			{
				if (_vertical)
					return 140;
				else
					return System.Math.Max(style.Knob == null ? 0 : style.Knob.MinHeight,
						style.Background != null ? style.Background.MinHeight : 0);
			}
		}

		public float Min { get; protected set; }
		public float Max { get; protected set; }

		public float StepSize
		{
			get => _stepSize;
			set => SetStepSize(value);
		}

		public float Value
		{
			get => _value;
			set => SetValue(value);
		}

		public float[] SnapValues;
		public float SnapThreshold;
		public bool ShiftIgnoresSnap;

		protected float _stepSize, _value;
		protected bool _vertical;
		protected float position;
		ProgressBarStyle style;

		#endregion


		public ProgressBar(float min, float max, float stepSize, bool vertical, ProgressBarStyle style)
		{
			Assert.IsTrue(min < max, "min must be less than max");
            Assert.IsTrue(stepSize > 0, "stepSize must be greater than 0");

			SetStyle(style);
			Min = min;
			Max = max;
			StepSize = stepSize;
			_vertical = vertical;
			_value = Min;

			SetSize(PreferredWidth, PreferredHeight);
		}

		public ProgressBar(float min, float max, float stepSize, bool vertical, Skin skin, string styleName = null) :
			this(min, max, stepSize, vertical, skin.Get<ProgressBarStyle>(styleName))
		{
		}

		public ProgressBar(Skin skin, string styleName = null) : this(0, 1, 0.01f, false, skin)
		{
		}


		public virtual void SetStyle(ProgressBarStyle style)
		{
			this.style = style;
			InvalidateHierarchy();
		}


		/// <summary>
		/// Returns the progress bar's style. Modifying the returned style may not have an effect until
		/// {@link #setStyle(ProgressBarStyle)} is called.
		/// </summary>
		/// <returns>The style.</returns>
		public ProgressBarStyle GetStyle()
		{
			return style;
		}


		/// <summary>
		/// Sets the progress bar position, rounded to the nearest step size and clamped to the minimum and maximum values.
		/// </summary>
		/// <param name="value">Value.</param>
		/// <param name="ignoreSnap">If set to <c>true</c> we ignore value snapping.</param>
		public ProgressBar SetValue(float value, bool ignoreSnap = false)
		{
			if ((ShiftIgnoresSnap && InputUtils.IsShiftDown()) || ignoreSnap)
			{
				value = Mathf.Clamp(value, Min, Max);
			}
			else
			{
				// if value is lower/higher than min/max then we're not rounding to avoid situation where we can't achieve those values
				if (value >= Max)
					value = Max;
				else if (value <= Min)
					value = Min;
				else
					value = Mathf.Clamp(Mathf.Round(value / StepSize) * StepSize, Min, Max);

				value = Snap(value);
			}

			if (value == _value)
				return this;

			_value = value;

			// fire changed event
			if (OnChanged != null)
				OnChanged(_value);

			return this;
		}


		public ProgressBar SetStepSize(float stepSize)
		{
			_stepSize = stepSize;
			return this;
		}


		public ProgressBar SetMinMax(float min, float max)
		{
            Assert.IsTrue(min < max, "min must be less than max");
			Min = min;
			Max = max;
			_value = Mathf.Clamp(_value, Min, Max);

			return this;
		}


		/// <summary>
		/// Sets stepSize to a value that will evenly divide this progress bar into specified amount of steps.
		/// </summary>
		/// <param name="totalSteps">Total amount of steps.</param>
		public ProgressBar SetTotalSteps(int totalSteps)
		{
            Assert.IsTrue(totalSteps != 0, "totalSteps cannot be equal to 0");
			_stepSize = System.Math.Abs((Min - Max) / totalSteps);
			return this;
		}


		protected virtual IDrawable GetKnobDrawable()
		{
			return (Disabled && style.DisabledKnob != null) ? style.DisabledKnob : style.Knob;
		}


		public override void Draw(SpriteBatch spriteBatch, float parentAlpha)
		{
			var knob = GetKnobDrawable();
			var bg = (Disabled && style.DisabledBackground != null) ? style.DisabledBackground : style.Background;
			var knobBefore = (Disabled && style.DisabledKnobBefore != null)
				? style.DisabledKnobBefore
				: style.KnobBefore;
			var knobAfter = (Disabled && style.DisabledKnobAfter != null) ? style.DisabledKnobAfter : style.KnobAfter;

			var x = this.x;
			var y = this.y;
			var width = this.width;
			var height = this.height;
			var knobHeight = knob == null ? 0 : knob.MinHeight;
			var knobWidth = knob == null ? 0 : knob.MinWidth;
			var percent = GetVisualPercent();
			var color = ColorExtensions.Create(this.color, (int)(this.color.A * parentAlpha));

			if (_vertical)
			{
				var positionHeight = height;

				float bgTopHeight = 0;
				if (bg != null)
				{
					bg.Draw(spriteBatch, x + (int)((width - bg.MinWidth) * 0.5f), y, bg.MinWidth, height, color);
					bgTopHeight = bg.TopHeight;
					positionHeight -= bgTopHeight + bg.BottomHeight;
				}

				float knobHeightHalf = 0;
				if (Min != Max)
				{
					if (knob == null)
					{
						knobHeightHalf = knobBefore == null ? 0 : knobBefore.MinHeight * 0.5f;
						position = (positionHeight - knobHeightHalf) * percent;
						position = System.Math.Min(positionHeight - knobHeightHalf, position);
					}
					else
					{
						var bgBottomHeight = bg != null ? bg.BottomHeight : 0;
						knobHeightHalf = knobHeight * 0.5f;
						position = (positionHeight - knobHeight) * percent;
						position = System.Math.Min(positionHeight - knobHeight, position) + bgBottomHeight;
					}

					position = System.Math.Max(0, position);
				}

				if (knobBefore != null)
				{
					float offset = 0;
					if (bg != null)
						offset = bgTopHeight;
					knobBefore.Draw(spriteBatch, x + ((width - knobBefore.MinWidth) * 0.5f), y + offset,
						knobBefore.MinWidth,
						(int)(position + knobHeightHalf), color);
				}

				if (knobAfter != null)
				{
					knobAfter.Draw(spriteBatch, x + ((width - knobAfter.MinWidth) * 0.5f), y + position + knobHeightHalf,
						knobAfter.MinWidth, height - position - knobHeightHalf, color);
				}

				if (knob != null)
					knob.Draw(spriteBatch, x + (int)((width - knobWidth) * 0.5f), (int)(y + position), knobWidth,
						knobHeight, color);
			}
			else
			{
				float positionWidth = width;

				float bgLeftWidth = 0;
				if (bg != null)
				{
					bg.Draw(spriteBatch, x, y + (int)((height - bg.MinHeight) * 0.5f), width, bg.MinHeight, color);
					bgLeftWidth = bg.LeftWidth;
					positionWidth -= bgLeftWidth + bg.RightWidth;
				}

				float knobWidthHalf = 0;
				if (Min != Max)
				{
					if (knob == null)
					{
						knobWidthHalf = knobBefore == null ? 0 : knobBefore.MinWidth * 0.5f;
						position = (positionWidth - knobWidthHalf) * percent;
						position = System.Math.Min(positionWidth - knobWidthHalf, position);
					}
					else
					{
						knobWidthHalf = knobWidth * 0.5f;
						position = (positionWidth - knobWidth) * percent;
						position = System.Math.Min(positionWidth - knobWidth, position) + bgLeftWidth;
					}

					position = System.Math.Max(0, position);
				}

				if (knobBefore != null)
				{
					float offset = 0;
					if (bg != null)
						offset = bgLeftWidth;
					knobBefore.Draw(spriteBatch, x + offset, y + (int)((height - knobBefore.MinHeight) * 0.5f),
						(int)(position + knobWidthHalf), knobBefore.MinHeight, color);
				}

				if (knobAfter != null)
				{
					knobAfter.Draw(spriteBatch, x + (int)(position + knobWidthHalf),
						y + (int)((height - knobAfter.MinHeight) * 0.5f),
						width - (int)(position + knobWidthHalf), knobAfter.MinHeight, color);
				}

				if (knob != null)
					knob.Draw(spriteBatch, (int)(x + position), (int)(y + (height - knobHeight) * 0.5f), knobWidth,
						knobHeight, color);
			}
		}


		public float GetVisualPercent()
		{
			return (_value - Min) / (Max - Min);
		}


		/// <summary>
		/// Returns a snapped value
		/// </summary>
		/// <param name="value">Value.</param>
		float Snap(float value)
		{
			if (SnapValues == null)
				return value;

			for (var i = 0; i < SnapValues.Length; i++)
			{
				if (System.Math.Abs(value - SnapValues[i]) <= SnapThreshold)
					return SnapValues[i];
			}

			return value;
		}
	}
}