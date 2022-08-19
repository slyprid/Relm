using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Graphics
{
	public static class SpriteBatchDrawingExtensions
	{
		static Rectangle _tempRect;
		
		#region Line

		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color)
		{
			DrawLineAngle(spriteBatch, start, Mathf.AngleBetweenVectors(start, end), Vector2.Distance(start, end), color);
		}
		
		public static void DrawLine(this SpriteBatch spriteBatch, Vector2 start, Vector2 end, Color color, float thickness)
		{
			DrawLineAngle(spriteBatch, start, Mathf.AngleBetweenVectors(start, end), Vector2.Distance(start, end), color,
				thickness);
		}
		
		public static void DrawLine(this SpriteBatch spriteBatch, float x1, float y1, float x2, float y2, Color color)
		{
			DrawLine(spriteBatch, new Vector2(x1, y1), new Vector2(x2, y2), color);
		}
		
		public static void DrawPoints(this SpriteBatch spriteBatch, List<Vector2> points, Color color, float thickness = 1)
		{
			if (points.Count < 2)
				return;

			spriteBatch.SetIgnoreRoundingDestinations(true);
			for (int i = 1; i < points.Count; i++) DrawLine(spriteBatch, points[i - 1], points[i], color, thickness);
			spriteBatch.SetIgnoreRoundingDestinations(false);
		}
		
		public static void DrawPoints(this SpriteBatch spriteBatch, Vector2[] points, Color color, float thickness = 1)
		{
			if (points.Length < 2) return;

			spriteBatch.SetIgnoreRoundingDestinations(true);
			for (int i = 1; i < points.Length; i++) DrawLine(spriteBatch, points[i - 1], points[i], color, thickness);
			spriteBatch.SetIgnoreRoundingDestinations(false);
		}


		public static void DrawPoints(this SpriteBatch spriteBatch, Vector2 position, Vector2[] points, Color color, bool closePoly = true, float thickness = 1)
		{
			if (points.Length < 2) return;

			spriteBatch.SetIgnoreRoundingDestinations(true);
			for (int i = 1; i < points.Length; i++) DrawLine(spriteBatch, position + points[i - 1], position + points[i], color, thickness);

			if (closePoly) DrawLine(spriteBatch, position + points[points.Length - 1], position + points[0], color, thickness);
			spriteBatch.SetIgnoreRoundingDestinations(false);
		}


		public static void DrawPolygon(this SpriteBatch spriteBatch, Vector2 position, Vector2[] points, Color color, bool closePoly = true, float thickness = 1)
		{
			if (points.Length < 2) return;

			spriteBatch.SetIgnoreRoundingDestinations(true);
			for (int i = 1; i < points.Length; i++) DrawLine(spriteBatch, position + points[i - 1], position + points[i], color, thickness);

            if (closePoly) DrawLine(spriteBatch, position + points[points.Length - 1], position + points[0], color, thickness);
			spriteBatch.SetIgnoreRoundingDestinations(false);
		}

		#endregion


		#region Line Angle

		public static void DrawLineAngle(this SpriteBatch spriteBatch, Vector2 start, float radians, float length, Color color)
		{
			spriteBatch.Draw(RelmGraphics.Instance.PixelTexture, start, RelmGraphics.Instance.PixelTexture.SourceRect, color, radians, Vector2.Zero, new Vector2(length, 1), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
		}


		public static void DrawLineAngle(this SpriteBatch spriteBatch, Vector2 start, float radians, float length, Color color, float thickness)
		{
			spriteBatch.Draw(RelmGraphics.Instance.PixelTexture, start, RelmGraphics.Instance.PixelTexture.SourceRect, color, radians, new Vector2(0f, 0.5f), new Vector2(length, thickness), Microsoft.Xna.Framework.Graphics.SpriteEffects.None, 0);
		}


		public static void DrawLineAngle(this SpriteBatch spriteBatch, float startX, float startY, float radians, float length, Color color)
		{
			DrawLineAngle(spriteBatch, new Vector2(startX, startY), radians, length, color);
		}

		#endregion


		#region Circle

		public static void DrawCircle(this SpriteBatch spriteBatch, Vector2 position, float radius, Color color, float thickness = 1f, int resolution = 12)
		{
			var last = Vector2.UnitX * radius;
			var lastP = Vector2Extensions.Perpendicular(last);

			spriteBatch.SetIgnoreRoundingDestinations(true);
			for (int i = 1; i <= resolution; i++)
			{
				var at = Mathf.AngleToVector(i * MathHelper.PiOver2 / resolution, radius);
				var atP = Vector2Extensions.Perpendicular(at);

				DrawLine(spriteBatch, position + last, position + at, color, thickness);
				DrawLine(spriteBatch, position - last, position - at, color, thickness);
				DrawLine(spriteBatch, position + lastP, position + atP, color, thickness);
				DrawLine(spriteBatch, position - lastP, position - atP, color, thickness);

				last = at;
				lastP = atP;
			}

			spriteBatch.SetIgnoreRoundingDestinations(false);
		}


		public static void DrawCircle(this SpriteBatch spriteBatch, float x, float y, float radius, Color color, int thickness = 1, int resolution = 12)
		{
			DrawCircle(spriteBatch, new Vector2(x, y), radius, color, thickness, resolution);
		}

		#endregion


		#region Rect

		public static void DrawRect(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color)
		{
			_tempRect.X = (int)x;
			_tempRect.Y = (int)y;
			_tempRect.Width = (int)width;
			_tempRect.Height = (int)height;
			spriteBatch.Draw(RelmGraphics.Instance.PixelTexture, _tempRect, RelmGraphics.Instance.PixelTexture.SourceRect, color);
		}


		public static void DrawRect(this SpriteBatch spriteBatch, Vector2 position, float width, float height, Color color)
		{
			DrawRect(spriteBatch, position.X, position.Y, width, height, color);
		}


		public static void DrawRect(this SpriteBatch spriteBatch, Rectangle rect, Color color)
		{
			spriteBatch.Draw(RelmGraphics.Instance.PixelTexture, rect, RelmGraphics.Instance.PixelTexture.SourceRect, color);
		}

		#endregion


		#region Hollow Rect

		public static void DrawHollowRect(this SpriteBatch spriteBatch, float x, float y, float width, float height, Color color, float thickness = 1)
		{
			var tl = Vector2Extensions.Round(new Vector2(x, y));
			var tr = Vector2Extensions.Round(new Vector2(x + width, y));
			var br = Vector2Extensions.Round(new Vector2(x + width, y + height));
			var bl = Vector2Extensions.Round(new Vector2(x, y + height));

			spriteBatch.SetIgnoreRoundingDestinations(true);
			spriteBatch.DrawLine(tl, tr, color, thickness);
			spriteBatch.DrawLine(tr, br, color, thickness);
			spriteBatch.DrawLine(br, bl, color, thickness);
			spriteBatch.DrawLine(bl, tl, color, thickness);
			spriteBatch.SetIgnoreRoundingDestinations(false);
		}


		public static void DrawHollowRect(this SpriteBatch spriteBatch, Vector2 position, float width, float height, Color color, float thickness = 1)
		{
			DrawHollowRect(spriteBatch, position.X, position.Y, width, height, color, thickness);
		}


		public static void DrawHollowRect(this SpriteBatch spriteBatch, Rectangle rect, Color color, float thickness = 1)
		{
			DrawHollowRect(spriteBatch, rect.X, rect.Y, rect.Width, rect.Height, color, thickness);
		}


		public static void DrawHollowRect(this SpriteBatch spriteBatch, RectangleF rect, Color color, float thickness = 1)
		{
			DrawHollowRect(spriteBatch, rect.X, rect.Y, rect.Width, rect.Height, color, thickness);
		}

		#endregion


		#region Pixel

		public static void DrawPixel(this SpriteBatch spriteBatch, float x, float y, Color color, int size = 1)
		{
			DrawPixel(spriteBatch, new Vector2(x, y), color, size);
		}


		public static void DrawPixel(this SpriteBatch spriteBatch, Vector2 position, Color color, int size = 1)
		{
			var destRect = new Rectangle((int)position.X, (int)position.Y, size, size);
			if (size != 1)
			{
				destRect.X -= (int)(size * 0.5f);
				destRect.Y -= (int)(size * 0.5f);
			}

			spriteBatch.Draw(RelmGraphics.Instance.PixelTexture, destRect, RelmGraphics.Instance.PixelTexture.SourceRect, color);
		}

		#endregion
	}
}