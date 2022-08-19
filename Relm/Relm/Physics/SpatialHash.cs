using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Extensions;
using Relm.Math;
using Relm.Physics.Shapes;

namespace Relm.Physics
{
	public class SpatialHash
	{
		public Rectangle GridBounds = new Rectangle();
        RaycastResultParser _raycastParser;

		int _cellSize;

		float _inverseCellSize;

		Box _overlapTestBox = new Box(0f, 0f);

		Circle _overlapTestCirce = new Circle(0f);

		IntIntDictionary _cellDict = new IntIntDictionary();

		HashSet<Collider> _tempHashset = new HashSet<Collider>();


		public SpatialHash(int cellSize = 100)
		{
			_cellSize = cellSize;
			_inverseCellSize = 1f / _cellSize;
			_raycastParser = new RaycastResultParser();
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Point CellCoords(int x, int y)
		{
			return new Point(Mathf.FloorToInt(x * _inverseCellSize), Mathf.FloorToInt(y * _inverseCellSize));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		Point CellCoords(float x, float y)
		{
			return new Point(Mathf.FloorToInt(x * _inverseCellSize), Mathf.FloorToInt(y * _inverseCellSize));
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		List<Collider> CellAtPosition(int x, int y, bool createCellIfEmpty = false)
		{
			List<Collider> cell = null;
			if (!_cellDict.TryGetValue(x, y, out cell))
			{
				if (createCellIfEmpty)
				{
					cell = new List<Collider>();
					_cellDict.Add(x, y, cell);
				}
			}

			return cell;
		}
		
		public void Register(Collider collider)
		{
			var bounds = collider.Bounds;
			collider.registeredPhysicsBounds = bounds;
			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			if (!GridBounds.Contains(p1)) RectangleExtensions.Union(ref GridBounds, ref p1, out GridBounds);

			if (!GridBounds.Contains(p2)) RectangleExtensions.Union(ref GridBounds, ref p2, out GridBounds);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					var c = CellAtPosition(x, y, true);
					c.Add(collider);
				}
			}
		}
		
		public void Remove(Collider collider)
		{
			var bounds = collider.registeredPhysicsBounds;
			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					var cell = CellAtPosition(x, y);
					Assert.IsNotNull(cell, "removing Collider [{0}] from a cell that it is not present in", collider);
					if (cell != null)
						cell.Remove(collider);
				}
			}
		}
		
		public void RemoveWithBruteForce(Collider obj)
		{
			_cellDict.Remove(obj);
		}


		public void Clear()
		{
			_cellDict.Clear();
		}
		
		public void DebugDraw(float secondsToDisplay, float textScale = 1f)
		{
			for (var x = GridBounds.X; x <= GridBounds.Right; x++)
			{
				for (var y = GridBounds.Y; y <= GridBounds.Bottom; y++)
				{
					var cell = CellAtPosition(x, y);
					if (cell != null && cell.Count > 0)
						DebugDrawCellDetails(x, y, cell.Count, secondsToDisplay, textScale);
				}
			}
		}


		void DebugDrawCellDetails(int x, int y, int cellCount, float secondsToDisplay = 0.5f, float textScale = 1f)
		{
			Debug.DrawHollowRect(new Rectangle(x * _cellSize, y * _cellSize, _cellSize, _cellSize), Color.Red, secondsToDisplay);

			if (cellCount > 0)
			{
				var textPosition = new Vector2((float)x * (float)_cellSize + 0.5f * _cellSize, (float)y * (float)_cellSize + 0.5f * _cellSize);
				Debug.DrawText(RelmGraphics.Instance.BitmapFont, cellCount.ToString(), textPosition, Color.DarkGreen, secondsToDisplay, textScale);
			}
		}
		
		public HashSet<Collider> GetAllObjects()
		{
			return _cellDict.GetAllObjects();
		}


		#region hash queries

		public HashSet<Collider> AabbBroadphase(ref RectangleF bounds, Collider excludeCollider, int layerMask)
		{
			_tempHashset.Clear();

			var p1 = CellCoords(bounds.X, bounds.Y);
			var p2 = CellCoords(bounds.Right, bounds.Bottom);

			for (var x = p1.X; x <= p2.X; x++)
			{
				for (var y = p1.Y; y <= p2.Y; y++)
				{
					var cell = CellAtPosition(x, y);
					if (cell == null)
						continue;

					for (var i = 0; i < cell.Count; i++)
					{
						var collider = cell[i];

						if (collider == excludeCollider || !Flags.IsFlagSet(layerMask, collider.PhysicsLayer)) continue;

						if (bounds.Intersects(collider.Bounds)) _tempHashset.Add(collider);
					}
				}
			}

			return _tempHashset;
		}


		public int Linecast(Vector2 start, Vector2 end, RaycastHit[] hits, int layerMask)
		{
			var ray = new Ray2D(start, end);
			_raycastParser.Start(ref ray, hits, layerMask);

			var currentCell = CellCoords(start.X, start.Y);
			var lastCell = CellCoords(end.X, end.Y);

			var stepX = System.Math.Sign(ray.Direction.X);
			var stepY = System.Math.Sign(ray.Direction.Y);

			if (currentCell.X == lastCell.X) stepX = 0;
			if (currentCell.Y == lastCell.Y) stepY = 0;

			var xStep = stepX < 0 ? 0f : (float)stepX;
			var yStep = stepY < 0 ? 0f : (float)stepY;
			var nextBoundaryX = ((float)currentCell.X + xStep) * _cellSize;
			var nextBoundaryY = ((float)currentCell.Y + yStep) * _cellSize;

			var tMaxX = ray.Direction.X != 0 ? (nextBoundaryX - ray.Start.X) / ray.Direction.X : float.MaxValue;
			var tMaxY = ray.Direction.Y != 0 ? (nextBoundaryY - ray.Start.Y) / ray.Direction.Y : float.MaxValue;

			var tDeltaX = ray.Direction.X != 0 ? _cellSize / (ray.Direction.X * stepX) : float.MaxValue;
			var tDeltaY = ray.Direction.Y != 0 ? _cellSize / (ray.Direction.Y * stepY) : float.MaxValue;

			var cell = CellAtPosition(currentCell.X, currentCell.Y);

			if (cell != null && _raycastParser.CheckRayIntersection(currentCell.X, currentCell.Y, cell))
			{
				_raycastParser.Reset();
				return _raycastParser.HitCounter;
			}

			while (currentCell.X != lastCell.X || currentCell.Y != lastCell.Y)
			{
				if (tMaxX < tMaxY)
				{
					currentCell.X = (int)Mathf.Approach(currentCell.X, lastCell.X, System.Math.Abs(stepX));

					tMaxX += tDeltaX;
				}
				else
				{
					currentCell.Y = (int)Mathf.Approach(currentCell.Y, lastCell.Y, System.Math.Abs(stepY));

					tMaxY += tDeltaY;
				}

				cell = CellAtPosition(currentCell.X, currentCell.Y);
				if (cell != null && _raycastParser.CheckRayIntersection(currentCell.X, currentCell.Y, cell))
				{
					_raycastParser.Reset();
					return _raycastParser.HitCounter;
				}
			}

			_raycastParser.Reset();
			return _raycastParser.HitCounter;
		}


		public int OverlapRectangle(ref RectangleF rect, Collider[] results, int layerMask)
		{
			_overlapTestBox.UpdateBox(rect.Width, rect.Height);
			_overlapTestBox.position = rect.Location;

			var resultCounter = 0;
			var potentials = AabbBroadphase(ref rect, null, layerMask);
			foreach (var collider in potentials)
			{
				if (collider is BoxCollider)
				{
					results[resultCounter] = collider;
					resultCounter++;
				}
				else if (collider is CircleCollider)
				{
					if (Collisions.RectToCircle(ref rect, collider.Bounds.Center, collider.Bounds.Width * 0.5f))
					{
						results[resultCounter] = collider;
						resultCounter++;
					}
				}
				else if (collider is PolygonCollider)
				{
					if (collider.Shape.Overlaps(_overlapTestBox))
					{
						results[resultCounter] = collider;
						resultCounter++;
					}
				}
				else
				{
					throw new NotImplementedException(
						"overlapRectangle against this collider type is not implemented!");
				}

				if (resultCounter == results.Length) return resultCounter;
			}

			return resultCounter;
		}


		public int OverlapCircle(Vector2 circleCenter, float radius, Collider[] results, int layerMask)
		{
			var bounds = new RectangleF(circleCenter.X - radius, circleCenter.Y - radius, radius * 2f, radius * 2f);

			_overlapTestCirce.Radius = radius;
			_overlapTestCirce.position = circleCenter;

			var resultCounter = 0;
			var potentials = AabbBroadphase(ref bounds, null, layerMask);
			foreach (var collider in potentials)
			{
				if (collider is BoxCollider)
				{
					if (collider.Shape.Overlaps(_overlapTestCirce))
					{
						results[resultCounter] = collider;
						resultCounter++;
					}
				}
				else if (collider is CircleCollider)
				{
					if (collider.Shape.Overlaps(_overlapTestCirce))
					{
						results[resultCounter] = collider;
						resultCounter++;
					}
				}
				else if (collider is PolygonCollider)
				{
					if (collider.Shape.Overlaps(_overlapTestCirce))
					{
						results[resultCounter] = collider;
						resultCounter++;
					}
				}
				else
				{
					throw new NotImplementedException("overlapCircle against this collider type is not implemented!");
				}

				if (resultCounter == results.Length) return resultCounter;
			}

			return resultCounter;
		}

		#endregion
	}


	class IntIntDictionary
	{
		Dictionary<long, List<Collider>> _store = new Dictionary<long, List<Collider>>();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		long GetKey(int x, int y)
		{
			return unchecked((long)x << 32 | (uint)y);
		}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add(int x, int y, List<Collider> list)
		{
			_store.Add(GetKey(x, y), list);
		}
		
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Remove(Collider obj)
		{
			foreach (var list in _store.Values)
			{
				if (list.Contains(obj))
					list.Remove(obj);
			}
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool TryGetValue(int x, int y, out List<Collider> list)
		{
			return _store.TryGetValue(GetKey(x, y), out list);
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public HashSet<Collider> GetAllObjects()
		{
			var set = new HashSet<Collider>();

			foreach (var list in _store.Values)
				set.UnionWith(list);

			return set;
		}

        public void Clear()
		{
			_store.Clear();
		}
	}


	class RaycastResultParser
	{
		public int HitCounter;

		static Comparison<RaycastHit> compareRaycastHits = (a, b) => { return a.Distance.CompareTo(b.Distance); };

		RaycastHit[] _hits;
		RaycastHit _tempHit;
		List<Collider> _checkedColliders = new List<Collider>();
		List<RaycastHit> _cellHits = new List<RaycastHit>();
		Ray2D _ray;
		int _layerMask;


		public void Start(ref Ray2D ray, RaycastHit[] hits, int layerMask)
		{
			_ray = ray;
			_hits = hits;
			_layerMask = layerMask;
			HitCounter = 0;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public bool CheckRayIntersection(int cellX, int cellY, List<Collider> cell)
		{
			float fraction;
			for (var i = 0; i < cell.Count; i++)
			{
				var potential = cell[i];

				if (_checkedColliders.Contains(potential))
					continue;

				_checkedColliders.Add(potential);

				if (potential.IsTrigger && !RelmPhysics.RaycastsHitTriggers)
					continue;

				if (!Flags.IsFlagSet(_layerMask, potential.PhysicsLayer))
					continue;

				var colliderBounds = potential.Bounds;
				if (colliderBounds.RayIntersects(ref _ray, out fraction) && fraction <= 1.0f)
				{
					if (potential.Shape.CollidesWithLine(_ray.Start, _ray.End, out _tempHit))
					{
						if (!RelmPhysics.RaycastsStartInColliders && potential.Shape.ContainsPoint(_ray.Start))
							continue;
						
						_tempHit.Collider = potential;
						_cellHits.Add(_tempHit);
					}
				}
			}

			if (_cellHits.Count == 0)
				return false;

			_cellHits.Sort(compareRaycastHits);
			for (var i = 0; i < _cellHits.Count; i++)
			{
				_hits[HitCounter] = _cellHits[i];

				HitCounter++;
				if (HitCounter == _hits.Length)
					return true;
			}

			return false;
		}


		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Reset()
		{
			_hits = null;
			_checkedColliders.Clear();
			_cellHits.Clear();
		}
	}
}