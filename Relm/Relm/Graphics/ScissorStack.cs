﻿using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Components;
using Relm.Math;

namespace Relm.Graphics
{
	public static class ScissorStack
	{
		static Stack<Rectangle> _scissors = new Stack<Rectangle>();


		public static bool PushScissors(Rectangle scissor)
		{
			if (_scissors.Count > 0)
			{
				// merge scissors
				var parent = _scissors.Peek();
				var minX = (int)System.Math.Max(parent.X, scissor.X);
				var maxX = (int)System.Math.Min(parent.X + parent.Width, scissor.X + scissor.Width);
				if (maxX - minX < 1)
					return false;

				var minY = (int)System.Math.Max(parent.Y, scissor.Y);
				var maxY = (int)System.Math.Min(parent.Y + parent.Height, scissor.Y + scissor.Height);
				if (maxY - minY < 1)
					return false;

				scissor.X = minX;
				scissor.Y = minY;
				scissor.Width = maxX - minX;
				scissor.Height = (int)System.Math.Max(1, maxY - minY);
			}

			_scissors.Push(scissor);
			RelmGame.GraphicsDevice.ScissorRectangle = scissor;

			return true;
		}


		/// <summary>
		/// Pops the current scissor rectangle from the stack and sets the new scissor area to the new top of stack rectangle.
		/// Any drawing should be flushed before popping scissors.
		/// </summary>
		/// <returns>The scissors.</returns>
		public static Rectangle PopScissors()
		{
			var scissors = _scissors.Pop();

			// reset the ScissorRectangle to the viewport bounds
			if (_scissors.Count == 0)
				RelmGame.GraphicsDevice.ScissorRectangle = RelmGame.GraphicsDevice.Viewport.Bounds;
			else
				RelmGame.GraphicsDevice.ScissorRectangle = _scissors.Peek();

			return scissors;
		}


		/// <summary>
		/// Calculates a screen space scissor rectangle using the given Camera. If the Camera is null than the scissor will
		/// be calculated only with the batchTransform
		/// </summary>
		/// <returns>The scissors.</returns>
		/// <param name="camera">Camera.</param>
		/// <param name="batchTransform">Batch transform.</param>
		/// <param name="scissor">Area.</param>
		public static Rectangle CalculateScissors(Camera camera, Matrix batchTransform, Rectangle scissor)
		{
			// convert the top-left point to screen space
			var tmp = new Vector2(scissor.X, scissor.Y);
			tmp = Vector2.Transform(tmp, batchTransform);

			if (camera != null)
				tmp = camera.WorldToScreenPoint(tmp / camera.RawZoom);

			var newScissor = new Rectangle();
			newScissor.X = (int)tmp.X;
			newScissor.Y = (int)tmp.Y;

			// convert the bottom-right point to screen space
			tmp.X = scissor.X + scissor.Width;
			tmp.Y = scissor.Y + scissor.Height;
			tmp = Vector2.Transform(tmp, batchTransform);

			if (camera != null)
				tmp = camera.WorldToScreenPoint(tmp / camera.RawZoom);
			newScissor.Width = (int)tmp.X - newScissor.X;
			newScissor.Height = (int)tmp.Y - newScissor.Y;


			return newScissor;
		}


		/// <summary>
		/// Calculates a screen space scissor rectangle using the given Camera. If the Camera is null than the scissor will
		/// be calculated only with the batchTransform
		/// </summary>
		/// <returns>The scissors.</returns>
		/// <param name="camera">Camera.</param>
		/// <param name="batchTransform">Batch transform.</param>
		/// <param name="scissor">Area.</param>
		public static Rectangle CalculateScissors(Camera camera, Matrix2D batchTransform, Rectangle scissor)
		{
			// convert the top-left point to screen space
			var tmp = new Vector2(scissor.X, scissor.Y);
			tmp = Vector2.Transform(tmp, batchTransform);

			if (camera != null)
				tmp = camera.WorldToScreenPoint(tmp);

			var newScissor = new Rectangle();
			newScissor.X = (int)tmp.X;
			newScissor.Y = (int)tmp.Y;

			// convert the bottom-right point to screen space
			tmp.X = scissor.X + scissor.Width;
			tmp.Y = scissor.Y + scissor.Height;
			tmp = Vector2.Transform(tmp, batchTransform);

			if (camera != null)
				tmp = camera.WorldToScreenPoint(tmp);
			newScissor.Width = (int)tmp.X - newScissor.X;
			newScissor.Height = (int)tmp.Y - newScissor.Y;

			return newScissor;
		}
	}
}