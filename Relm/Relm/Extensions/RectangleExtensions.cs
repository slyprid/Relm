using System;
using Microsoft.Xna.Framework;
using Relm.Enumerations;
using Relm.Math;
using Relm.Physics;

namespace Relm.Extensions
{
    public static class RectangleExtensions
	{
		public static int GetSide(this Rectangle rect, Edge edge)
		{
			switch (edge)
			{
				case Edge.Top:
					return rect.Top;
				case Edge.Bottom:
					return rect.Bottom;
				case Edge.Left:
					return rect.Left;
				case Edge.Right:
					return rect.Right;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static Rectangle GetHalfRect(this Rectangle rect, Edge edge)
		{
			switch (edge)
			{
				case Edge.Top:
					return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height / 2);
				case Edge.Bottom:
					return new Rectangle(rect.X, rect.Y + rect.Height / 2, rect.Width, rect.Height / 2);
				case Edge.Left:
					return new Rectangle(rect.X, rect.Y, rect.Width / 2, rect.Height);
				case Edge.Right:
					return new Rectangle(rect.X + rect.Width / 2, rect.Y, rect.Width / 2, rect.Height);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static Rectangle GetRectEdgePortion(this Rectangle rect, Edge edge, int size = 1)
		{
			switch (edge)
			{
				case Edge.Top:
					return new Rectangle(rect.X, rect.Y, rect.Width, size);
				case Edge.Bottom:
					return new Rectangle(rect.X, rect.Y + rect.Height - size, rect.Width, size);
				case Edge.Left:
					return new Rectangle(rect.X, rect.Y, size, rect.Height);
				case Edge.Right:
					return new Rectangle(rect.X + rect.Width - size, rect.Y, size, rect.Height);
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static void ExpandSide(ref Rectangle rect, Edge edge, int amount)
		{
			amount = System.Math.Abs(amount);

			switch (edge)
			{
				case Edge.Top:
					rect.Y -= amount;
					rect.Height += amount;
					break;
				case Edge.Bottom:
					rect.Height += amount;
					break;
				case Edge.Left:
					rect.X -= amount;
					rect.Width += amount;
					break;
				case Edge.Right:
					rect.Width += amount;
					break;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static void Contract(ref Rectangle rect, int horizontalAmount, int verticalAmount)
		{
			rect.X += horizontalAmount;
			rect.Y += verticalAmount;
			rect.Width -= horizontalAmount * 2;
			rect.Height -= verticalAmount * 2;
		}

		public static Rectangle FromFloats(float x, float y, float width, float height)
		{
			return new Rectangle((int)x, (int)y, (int)width, (int)height);
		}

		public static Rectangle FromMinMaxPoints(Point min, Point max)
		{
			return new Rectangle(min.X, min.Y, max.X - min.X, max.Y - min.Y);
		}

		public static void Union(ref Rectangle value1, ref Rectangle value2, out Rectangle result)
		{
			result.X = System.Math.Min(value1.X, value2.X);
			result.Y = System.Math.Min(value1.Y, value2.Y);
			result.Width = System.Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = System.Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}

		public static void Union(ref Rectangle first, ref Point point, out Rectangle result)
		{
			var rect = new Rectangle(point.X, point.Y, 0, 0);
			Union(ref first, ref rect, out result);
		}

		public static Rectangle BoundsFromPolygonPoints(Vector2[] points)
		{
			var minX = float.PositiveInfinity;
			var minY = float.PositiveInfinity;
			var maxX = float.NegativeInfinity;
			var maxY = float.NegativeInfinity;

			for (var i = 0; i < points.Length; i++)
			{
				var pt = points[i];

				if (pt.X < minX)
					minX = pt.X;
				if (pt.X > maxX)
					maxX = pt.X;

				if (pt.Y < minY)
					minY = pt.Y;
				if (pt.Y > maxY)
					maxY = pt.Y;
			}

			return FromMinMaxPoints(new Point((int)minX, (int)minY), new Point((int)maxX, (int)maxY));
		}

		public static void CalculateBounds(ref Rectangle rect, Vector2 parentPosition, Vector2 position, Vector2 origin,
										   Vector2 scale, float rotation, float width, float height)
		{
			if (rotation == 0f)
			{
				rect.X = (int)(parentPosition.X + position.X - origin.X * scale.X);
				rect.Y = (int)(parentPosition.Y + position.Y - origin.Y * scale.Y);
				rect.Width = (int)(width * scale.X);
				rect.Height = (int)(height * scale.Y);
			}
			else
			{
				var worldPosX = parentPosition.X + position.X;
				var worldPosY = parentPosition.Y + position.Y;

				Matrix2D tempMat;

				var transformMatrix = Matrix2D.CreateTranslation(-worldPosX - origin.X, -worldPosY - origin.Y);
				Matrix2D.CreateScale(scale.X, scale.Y, out tempMat); // scale ->
				Matrix2D.Multiply(ref transformMatrix, ref tempMat, out transformMatrix);
				Matrix2D.CreateRotation(rotation, out tempMat); // rotate ->
				Matrix2D.Multiply(ref transformMatrix, ref tempMat, out transformMatrix);
				Matrix2D.CreateTranslation(worldPosX, worldPosY, out tempMat); // translate back
				Matrix2D.Multiply(ref transformMatrix, ref tempMat, out transformMatrix);

				var topLeft = new Vector2(worldPosX, worldPosY);
				var topRight = new Vector2(worldPosX + width, worldPosY);
				var bottomLeft = new Vector2(worldPosX, worldPosY + height);
				var bottomRight = new Vector2(worldPosX + width, worldPosY + height);

				Vector2Extensions.Transform(ref topLeft, ref transformMatrix, out topLeft);
				Vector2Extensions.Transform(ref topRight, ref transformMatrix, out topRight);
				Vector2Extensions.Transform(ref bottomLeft, ref transformMatrix, out bottomLeft);
				Vector2Extensions.Transform(ref bottomRight, ref transformMatrix, out bottomRight);

				var minX = (int)Mathf.MinOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
				var maxX = (int)Mathf.MaxOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
				var minY = (int)Mathf.MinOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);
				var maxY = (int)Mathf.MaxOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);

				rect.Location = new Point(minX, minY);
				rect.Width = (int)(maxX - minX);
				rect.Height = (int)(maxY - minY);
			}
		}

		public static Rectangle Clone(this Rectangle rect) => new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);


		public static void Scale(ref Rectangle rect, Vector2 scale)
		{
			rect.X = (int)(rect.X * scale.X);
			rect.Y = (int)(rect.Y * scale.Y);
			rect.Width = (int)(rect.Width * scale.X);
			rect.Height = (int)(rect.Height * scale.Y);
		}

        public static Rectangle Scale(this Rectangle rect, Vector2 scale)
        {
            rect.X = (int)(rect.X * scale.X);
            rect.Y = (int)(rect.Y * scale.Y);
            rect.Width = (int)(rect.Width * scale.X);
            rect.Height = (int)(rect.Height * scale.Y);
            return rect;
        }

		public static void Translate(ref Rectangle rect, Vector2 vec) => rect.Location += vec.ToPoint();

		public static bool RayIntersects(ref Rectangle rect, ref Ray2D ray, out float distance)
		{
			distance = 0f;
			var maxValue = float.MaxValue;

			if (System.Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Start.X < rect.X) || (ray.Start.X > rect.X + rect.Width))
					return false;
			}
			else
			{
				var num11 = 1f / ray.Direction.X;
				var num8 = (rect.X - ray.Start.X) * num11;
				var num7 = (rect.X + rect.Width - ray.Start.X) * num11;
				if (num8 > num7)
				{
					var num14 = num8;
					num8 = num7;
					num7 = num14;
				}

				distance = MathHelper.Max(num8, distance);
				maxValue = MathHelper.Min(num7, maxValue);
				if (distance > maxValue)
					return false;
			}

			if (System.Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				if ((ray.Start.Y < rect.Y) || (ray.Start.Y > rect.Y + rect.Height))
				{
					return false;
				}
			}
			else
			{
				var num10 = 1f / ray.Direction.Y;
				var num6 = (rect.Y - ray.Start.Y) * num10;
				var num5 = (rect.Y + rect.Height - ray.Start.Y) * num10;
				if (num6 > num5)
				{
					var num13 = num6;
					num6 = num5;
					num5 = num13;
				}

				distance = MathHelper.Max(num6, distance);
				maxValue = MathHelper.Min(num5, maxValue);
				if (distance > maxValue)
					return false;
			}

			return true;
		}

		public static float? RayIntersects(this Rectangle rectangle, Ray ray)
		{
			var num = 0f;
			var maxValue = float.MaxValue;

			if (System.Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Position.X < rectangle.Left) || (ray.Position.X > rectangle.Right))
					return null;
			}
			else
			{
				float num11 = 1f / ray.Direction.X;
				float num8 = (rectangle.Left - ray.Position.X) * num11;
				float num7 = (rectangle.Right - ray.Position.X) * num11;
				if (num8 > num7)
				{
					float num14 = num8;
					num8 = num7;
					num7 = num14;
				}

				num = MathHelper.Max(num8, num);
				maxValue = MathHelper.Min(num7, maxValue);
				if (num > maxValue)
				{
					return null;
				}
			}

			if (System.Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				if ((ray.Position.Y < rectangle.Top) || (ray.Position.Y > rectangle.Bottom))
				{
					return null;
				}
			}
			else
			{
				float num10 = 1f / ray.Direction.Y;
				float num6 = (rectangle.Top - ray.Position.Y) * num10;
				float num5 = (rectangle.Bottom - ray.Position.Y) * num10;
				if (num6 > num5)
				{
					float num13 = num6;
					num6 = num5;
					num5 = num13;
				}

				num = MathHelper.Max(num6, num);
				maxValue = MathHelper.Min(num5, maxValue);
				if (num > maxValue)
					return null;
			}

			return new float?(num);
		}

		public static Rectangle GetSweptBroadphaseBounds(ref Rectangle rect, float deltaX, float deltaY) => GetSweptBroadphaseBounds(ref rect, (int)deltaX, (int)deltaY);

		public static Rectangle GetSweptBroadphaseBounds(ref Rectangle rect, int deltaX, int deltaY)
		{
			var broadphasebox = Rectangle.Empty;

			broadphasebox.X = deltaX > 0 ? rect.X : rect.X + deltaX;
			broadphasebox.Y = deltaY > 0 ? rect.Y : rect.Y + deltaY;
			broadphasebox.Width = deltaX > 0 ? deltaX + rect.Width : rect.Width - deltaX;
			broadphasebox.Height = deltaY > 0 ? deltaY + rect.Height : rect.Height - deltaY;

			return broadphasebox;
		}

		public static bool Intersect(ref Rectangle rect1, ref Rectangle rect2)
		{
			bool result;
			rect1.Intersects(ref rect2, out result);
			return result;
		}

		public static bool CollisionCheck(ref Rectangle rect, ref Rectangle other, out float moveX, out float moveY)
		{
			moveX = moveY = 0.0f;

			var l = other.X - (rect.X + rect.Width);
			var r = (other.X + other.Width) - rect.X;
			var t = other.Y - (rect.Y + rect.Height);
			var b = (other.Y + other.Height) - rect.Y;

			if (l > 0 || r < 0 || t > 0 || b < 0) return false;

			moveX = System.Math.Abs(l) < r ? l : r;
			moveY = System.Math.Abs(t) < b ? t : b;

			if (System.Math.Abs(moveX) < System.Math.Abs(moveY)) moveY = 0.0f;
			else moveX = 0.0f;

			return true;
		}

		public static Vector2 GetIntersectionDepth(ref Rectangle rectA, ref Rectangle rectB)
		{
			var halfWidthA = rectA.Width / 2.0f;
			var halfHeightA = rectA.Height / 2.0f;
			var halfWidthB = rectB.Width / 2.0f;
			var halfHeightB = rectB.Height / 2.0f;

			var centerA = new Vector2(rectA.Left + halfWidthA, rectA.Top + halfHeightA);
			var centerB = new Vector2(rectB.Left + halfWidthB, rectB.Top + halfHeightB);

			var distanceX = centerA.X - centerB.X;
			var distanceY = centerA.Y - centerB.Y;
			var minDistanceX = halfWidthA + halfWidthB;
			var minDistanceY = halfHeightA + halfHeightB;

			if (System.Math.Abs(distanceX) >= minDistanceX || System.Math.Abs(distanceY) >= minDistanceY)
				return Vector2.Zero;

			var depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
			var depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

			return new Vector2(depthX, depthY);
		}

		public static Vector2 GetClosestPointOnBoundsToOrigin(ref Rectangle rect)
		{
			var max = GetMax(ref rect);
			var minDist = System.Math.Abs(rect.Location.X);
			var boundsPoint = new Vector2(rect.Location.X, 0);

			if (System.Math.Abs(max.X) < minDist)
			{
				minDist = System.Math.Abs(max.X);
				boundsPoint.X = max.X;
				boundsPoint.Y = 0f;
			}

			if (System.Math.Abs(max.Y) < minDist)
			{
				minDist = System.Math.Abs(max.Y);
				boundsPoint.X = 0f;
				boundsPoint.Y = max.Y;
			}

			if (System.Math.Abs(rect.Location.Y) < minDist)
			{
				minDist = System.Math.Abs(rect.Location.Y);
				boundsPoint.X = 0;
				boundsPoint.Y = rect.Location.Y;
			}

			return boundsPoint;
		}

		public static Vector2 GetClosestPointOnRectangleToPoint(ref Rectangle rect, Vector2 point)
		{
			var res = new Vector2();
			res.X = MathHelper.Clamp(point.X, rect.Left, rect.Right);
			res.Y = MathHelper.Clamp(point.Y, rect.Top, rect.Bottom);

			return res;
		}

		public static Point GetClosestPointOnRectangleBorderToPoint(ref Rectangle rect, Vector2 point)
		{
			var res = new Point();
			res.X = Mathf.Clamp((int)point.X, rect.Left, rect.Right);
			res.Y = Mathf.Clamp((int)point.Y, rect.Top, rect.Bottom);

			if (rect.Contains(res))
			{
				var dl = res.X - rect.Left;
				var dr = rect.Right - res.X;
				var dt = res.Y - rect.Top;
				var db = rect.Bottom - res.Y;

				var min = Mathf.MinOf(dl, dr, dt, db);
				if (min == dt)
					res.Y = rect.Top;
				else if (min == db)
					res.Y = rect.Bottom;
				else if (min == dl)
					res.X = rect.Left;
				else
					res.X = rect.Right;
			}

			return res;
		}

		public static Vector2 GetCenter(ref Rectangle rect) => new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

		public static Vector2 GetCenter(this Rectangle rect) => new Vector2(rect.X + rect.Width / 2, rect.Y + rect.Height / 2);

		public static Vector2 GetHalfSize(this Rectangle rect) => new Vector2(rect.Width * 0.5f, rect.Height * 0.5f);

		public static Point GetMax(ref Rectangle rect) => new Point(rect.Right, rect.Bottom);

		public static Point GetSize(this Rectangle rect) => new Point(rect.Width, rect.Height);

		public static Vector2 GetPosition(ref Rectangle rect) => new Vector2(rect.X, rect.Y);
	}
}