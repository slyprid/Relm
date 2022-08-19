using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Relm.Enumerations;
using Relm.Extensions;
using Relm.Physics;

namespace Relm.Math
{
    [DebuggerDisplay("{DebugDisplayString,nq}")]
	public struct RectangleF 
        : IEquatable<RectangleF>
	{
		static RectangleF emptyRectangle = new RectangleF();

		public float X;

		public float Y;

		public float Width;

		public float Height;


		#region Public Properties

		public static RectangleF Empty => emptyRectangle;

		public static RectangleF MaxRect => new RectangleF(float.MinValue / 2, float.MinValue / 2, float.MaxValue, float.MaxValue);

		public float Left => X;

		public float Right => (X + Width);

		public float Top => Y;

		public float Bottom => (Y + Height);

		public Vector2 Max => new Vector2(Right, Bottom);

		public bool IsEmpty => ((((Width == 0) && (Height == 0)) && (X == 0)) && (Y == 0));

		public Vector2 Location
		{
			get => new Vector2(X, Y);
			set
			{
				X = value.X;
				Y = value.Y;
			}
		}

		public Vector2 Size
		{
			get => new Vector2(Width, Height);
			set
			{
				Width = value.X;
				Height = value.Y;
			}
		}

		public Vector2 Center => new Vector2(X + (Width / 2), Y + (Height / 2));

		#endregion

		static Matrix2D _tempMat, _transformMat;


		internal string DebugDisplayString =>
			string.Concat(
				X, "  ",
				Y, "  ",
				Width, "  ",
				Height
			);


		public RectangleF(float x, float y, float width, float height)
		{
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}


		public RectangleF(Vector2 location, Vector2 size)
		{
			X = location.X;
			Y = location.Y;
			Width = size.X;
			Height = size.Y;
		}


		public static RectangleF FromMinMax(Vector2 min, Vector2 max)
		{
			return new RectangleF(min.X, min.Y, max.X - min.X, max.Y - min.Y);
		}


		public static RectangleF FromMinMax(float minX, float minY, float maxX, float maxY)
		{
			return new RectangleF(minX, minY, maxX - minX, maxY - minY);
		}


		public static RectangleF RectEncompassingPoints(Vector2[] points)
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

			return FromMinMax(minX, minY, maxX, maxY);
		}


		#region Public Methods

		public float GetSide(Edge edge)
		{
			switch (edge)
			{
				case Edge.Top:
					return Top;
				case Edge.Bottom:
					return Bottom;
				case Edge.Left:
					return Left;
				case Edge.Right:
					return Right;
				default:
					throw new ArgumentOutOfRangeException();
			}
		}


		public bool Contains(int x, int y)
		{
			return ((((X <= x) && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
		}


		public bool Contains(float x, float y)
		{
			return ((((X <= x) && (x < (X + Width))) && (Y <= y)) && (y < (Y + Height)));
		}


		public bool Contains(Point value)
		{
			return ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) &&
					(value.Y < (Y + Height)));
		}


		public void Contains(ref Point value, out bool result)
		{
			result = ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) &&
					  (value.Y < (Y + Height)));
		}


		public bool Contains(Vector2 value)
		{
			return ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) &&
					(value.Y < (Y + Height)));
		}


		public void Contains(ref Vector2 value, out bool result)
		{
			result = ((((X <= value.X) && (value.X < (X + Width))) && (Y <= value.Y)) &&
					  (value.Y < (Y + Height)));
		}


		public bool Contains(RectangleF value)
		{
			return ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) &&
					 (Y <= value.Y)) && ((value.Y + value.Height) <= (Y + Height)));
		}


		public void Contains(ref RectangleF value, out bool result)
		{
			result = ((((X <= value.X) && ((value.X + value.Width) <= (X + Width))) &&
					   (Y <= value.Y)) && ((value.Y + value.Height) <= (Y + Height)));
		}


		public void Inflate(int horizontalAmount, int verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}


		public void Inflate(float horizontalAmount, float verticalAmount)
		{
			X -= horizontalAmount;
			Y -= verticalAmount;
			Width += horizontalAmount * 2;
			Height += verticalAmount * 2;
		}


		public bool Intersects(RectangleF value)
		{
			return value.Left < Right &&
				   Left < value.Right &&
				   value.Top < Bottom &&
				   Top < value.Bottom;
		}


		public void Intersects(ref RectangleF value, out bool result)
		{
			result = value.Left < Right &&
					 Left < value.Right &&
					 value.Top < Bottom &&
					 Top < value.Bottom;
		}


		public bool Intersects(ref RectangleF other)
		{
			bool result;
			Intersects(ref other, out result);
			return result;
		}


		public bool RayIntersects(ref Ray2D ray, out float distance)
		{
			distance = 0f;
			var maxValue = float.MaxValue;

			if (System.Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Start.X < X) || (ray.Start.X > X + Width))
					return false;
			}
			else
			{
				var num11 = 1f / ray.Direction.X;
				var num8 = (X - ray.Start.X) * num11;
				var num7 = (X + Width - ray.Start.X) * num11;
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
				if ((ray.Start.Y < Y) || (ray.Start.Y > Y + Height))
				{
					return false;
				}
			}
			else
			{
				var num10 = 1f / ray.Direction.Y;
				var num6 = (Y - ray.Start.Y) * num10;
				var num5 = (Y + Height - ray.Start.Y) * num10;
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


		public float? RayIntersects(Ray ray)
		{
			var num = 0f;
			var maxValue = float.MaxValue;

			if (System.Math.Abs(ray.Direction.X) < 1E-06f)
			{
				if ((ray.Position.X < Left) || (ray.Position.X > Right))
					return null;
			}
			else
			{
				float num11 = 1f / ray.Direction.X;
				float num8 = (Left - ray.Position.X) * num11;
				float num7 = (Right - ray.Position.X) * num11;
				if (num8 > num7)
				{
					float num14 = num8;
					num8 = num7;
					num7 = num14;
				}

				num = MathHelper.Max(num8, num);
				maxValue = MathHelper.Min(num7, maxValue);
				if (num > maxValue)
					return null;
			}

			if (System.Math.Abs(ray.Direction.Y) < 1E-06f)
			{
				if ((ray.Position.Y < Top) || (ray.Position.Y > Bottom))
					return null;
			}
			else
			{
				float num10 = 1f / ray.Direction.Y;
				float num6 = (Top - ray.Position.Y) * num10;
				float num5 = (Bottom - ray.Position.Y) * num10;
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


		public Vector2 GetClosestPointOnBoundsToOrigin()
		{
			var max = Max;
			var minDist = System.Math.Abs(Location.X);
			var boundsPoint = new Vector2(Location.X, 0);

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

			if (System.Math.Abs(Location.Y) < minDist)
			{
				minDist = System.Math.Abs(Location.Y);
				boundsPoint.X = 0;
				boundsPoint.Y = Location.Y;
			}

			return boundsPoint;
		}


		public Vector2 GetClosestPointOnRectangleFToPoint(Vector2 point)
		{
			var res = new Vector2();
			res.X = MathHelper.Clamp(point.X, Left, Right);
			res.Y = MathHelper.Clamp(point.Y, Top, Bottom);

			return res;
		}


		public Vector2 GetClosestPointOnRectangleBorderToPoint(Vector2 point, out Vector2 edgeNormal)
		{
			edgeNormal = Vector2.Zero;

			var res = new Vector2();
			res.X = MathHelper.Clamp(point.X, Left, Right);
			res.Y = MathHelper.Clamp(point.Y, Top, Bottom);

			if (Contains(res))
			{
				var dl = res.X - Left;
				var dr = Right - res.X;
				var dt = res.Y - Top;
				var db = Bottom - res.Y;

				var min = Mathf.MinOf(dl, dr, dt, db);
				if (min == dt)
				{
					res.Y = Top;
					edgeNormal.Y = -1;
				}
				else if (min == db)
				{
					res.Y = Bottom;
					edgeNormal.Y = 1;
				}
				else if (min == dl)
				{
					res.X = Left;
					edgeNormal.X = -1;
				}
				else
				{
					res.X = Right;
					edgeNormal.X = 1;
				}
			}
			else
			{
				if (res.X == Left)
					edgeNormal.X = -1;
				if (res.X == Right)
					edgeNormal.X = 1;
				if (res.Y == Top)
					edgeNormal.Y = -1;
				if (res.Y == Bottom)
					edgeNormal.Y = 1;
			}

			return res;
		}


		public static RectangleF Intersect(RectangleF value1, RectangleF value2)
		{
			RectangleF rectangle;
			Intersect(ref value1, ref value2, out rectangle);
			return rectangle;
		}


		public static void Intersect(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			if (value1.Intersects(value2))
			{
				var right_side = System.Math.Min(value1.X + value1.Width, value2.X + value2.Width);
				var left_side = System.Math.Max(value1.X, value2.X);
				var top_side = System.Math.Max(value1.Y, value2.Y);
				var bottom_side = System.Math.Min(value1.Y + value1.Height, value2.Y + value2.Height);
				result = new RectangleF(left_side, top_side, right_side - left_side, bottom_side - top_side);
			}
			else
			{
				result = new RectangleF(0, 0, 0, 0);
			}
		}


		public void Offset(int offsetX, int offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}


		public void Offset(float offsetX, float offsetY)
		{
			X += offsetX;
			Y += offsetY;
		}


		public void Offset(Point amount)
		{
			X += amount.X;
			Y += amount.Y;
		}


		public void Offset(Vector2 amount)
		{
			X += amount.X;
			Y += amount.Y;
		}


		public static RectangleF Union(RectangleF value1, RectangleF value2)
		{
			var x = System.Math.Min(value1.X, value2.X);
			var y = System.Math.Min(value1.Y, value2.Y);
			return new RectangleF(x, y,
                System.Math.Max(value1.Right, value2.Right) - x,
                System.Math.Max(value1.Bottom, value2.Bottom) - y);
		}


		public static void Union(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			result.X = System.Math.Min(value1.X, value2.X);
			result.Y = System.Math.Min(value1.Y, value2.Y);
			result.Width = System.Math.Max(value1.Right, value2.Right) - result.X;
			result.Height = System.Math.Max(value1.Bottom, value2.Bottom) - result.Y;
		}


		public static RectangleF Overlap(RectangleF value1, RectangleF value2)
		{
			var x = System.Math.Max(System.Math.Max(value1.X, value2.X), 0);
			var y = System.Math.Max(System.Math.Max(value1.Y, value2.Y), 0);
			return new RectangleF(x, y,
                System.Math.Max(System.Math.Min(value1.Right, value2.Right) - x, 0),
                System.Math.Max(System.Math.Min(value1.Bottom, value2.Bottom) - y, 0));
		}


		public static void Overlap(ref RectangleF value1, ref RectangleF value2, out RectangleF result)
		{
			result.X = System.Math.Max(System.Math.Max(value1.X, value2.X), 0);
			result.Y = System.Math.Max(System.Math.Max(value1.Y, value2.Y), 0);
			result.Width = System.Math.Max(System.Math.Min(value1.Right, value2.Right) - result.X, 0);
			result.Height = System.Math.Max(System.Math.Min(value1.Bottom, value2.Bottom) - result.Y, 0);
		}


		public void CalculateBounds(Vector2 parentPosition, Vector2 position, Vector2 origin, Vector2 scale,
									float rotation, float width, float height)
		{
			if (rotation == 0f)
			{
				X = parentPosition.X + position.X - origin.X * scale.X;
				Y = parentPosition.Y + position.Y - origin.Y * scale.Y;
				Width = width * scale.X;
				Height = height * scale.Y;
			}
			else
			{
				var worldPosX = parentPosition.X + position.X;
				var worldPosY = parentPosition.Y + position.Y;

				Matrix2D.CreateTranslation(-worldPosX - origin.X, -worldPosY - origin.Y, out _transformMat);
				Matrix2D.CreateScale(scale.X, scale.Y, out _tempMat); // scale ->
				Matrix2D.Multiply(ref _transformMat, ref _tempMat, out _transformMat);
				Matrix2D.CreateRotation(rotation, out _tempMat); // rotate ->
				Matrix2D.Multiply(ref _transformMat, ref _tempMat, out _transformMat);
				Matrix2D.CreateTranslation(worldPosX, worldPosY, out _tempMat); // translate back
				Matrix2D.Multiply(ref _transformMat, ref _tempMat, out _transformMat);

				var topLeft = new Vector2(worldPosX, worldPosY);
				var topRight = new Vector2(worldPosX + width, worldPosY);
				var bottomLeft = new Vector2(worldPosX, worldPosY + height);
				var bottomRight = new Vector2(worldPosX + width, worldPosY + height);

				Vector2Extensions.Transform(ref topLeft, ref _transformMat, out topLeft);
				Vector2Extensions.Transform(ref topRight, ref _transformMat, out topRight);
				Vector2Extensions.Transform(ref bottomLeft, ref _transformMat, out bottomLeft);
				Vector2Extensions.Transform(ref bottomRight, ref _transformMat, out bottomRight);

				var minX = Mathf.MinOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
				var maxX = Mathf.MaxOf(topLeft.X, bottomRight.X, topRight.X, bottomLeft.X);
				var minY = Mathf.MinOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);
				var maxY = Mathf.MaxOf(topLeft.Y, bottomRight.Y, topRight.Y, bottomLeft.Y);

				Location = new Vector2(minX, minY);
				Width = maxX - minX;
				Height = maxY - minY;
			}
		}


		public RectangleF GetSweptBroadphaseBounds(float deltaX, float deltaY)
		{
			var broadphasebox = Empty;

			broadphasebox.X = deltaX > 0 ? X : X + deltaX;
			broadphasebox.Y = deltaY > 0 ? Y : Y + deltaY;
			broadphasebox.Width = deltaX > 0 ? deltaX + Width : Width - deltaX;
			broadphasebox.Height = deltaY > 0 ? deltaY + Height : Height - deltaY;

			return broadphasebox;
		}


		public bool CollisionCheck(ref RectangleF other, out float moveX, out float moveY)
		{
			moveX = moveY = 0.0f;

			var l = other.X - (X + Width);
			var r = (other.X + other.Width) - X;
			var t = other.Y - (Y + Height);
			var b = (other.Y + other.Height) - Y;

			if (l > 0 || r < 0 || t > 0 || b < 0) return false;

			moveX = System.Math.Abs(l) < r ? l : r;
			moveY = System.Math.Abs(t) < b ? t : b;

			if (System.Math.Abs(moveX) < System.Math.Abs(moveY)) moveY = 0.0f;
			else moveX = 0.0f;

			return true;
		}
		
		public static Vector2 GetIntersectionDepth(ref RectangleF rectA, ref RectangleF rectB)
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

			if (System.Math.Abs(distanceX) >= minDistanceX || System.Math.Abs(distanceY) >= minDistanceY) return Vector2.Zero;

			var depthX = distanceX > 0 ? minDistanceX - distanceX : -minDistanceX - distanceX;
			var depthY = distanceY > 0 ? minDistanceY - distanceY : -minDistanceY - distanceY;

			return new Vector2(depthX, depthY);
		}
		
		public override bool Equals(object obj)
		{
			return (obj is RectangleF) && this == ((RectangleF)obj);
		}
		
		public bool Equals(RectangleF other)
		{
			return this == other;
		}

        public override int GetHashCode()
		{
			return ((int)X ^ (int)Y ^ (int)Width ^ (int)Height);
		}
		
		public override string ToString()
		{
			return $"X:{X}, Y:{Y}, Width: {Width}, Height: {Height}";
		}

		#endregion


		#region Operators

		public static bool operator ==(RectangleF a, RectangleF b)
		{
			return ((a.X == b.X) && (a.Y == b.Y) && (a.Width == b.Width) && (a.Height == b.Height));
		}

		public static bool operator !=(RectangleF a, RectangleF b)
		{
			return !(a == b);
		}
		
		public static implicit operator Rectangle(RectangleF self)
		{
			return RectangleExtensions.FromFloats(self.X, self.Y, self.Width, self.Height);
		}

		#endregion
	}
}