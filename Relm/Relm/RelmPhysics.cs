using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Relm.Components.Physics.Colliders;
using Relm.Math;
using Relm.Physics;

namespace Relm
{
	public static class RelmPhysics
	{
		static SpatialHash _spatialHash;

		public const int AllLayers = -1;

		public static Vector2 Gravity = new Vector2(0, 300f);

		public static int SpatialHashCellSize = 100;

		public static bool RaycastsHitTriggers = false;

		public static bool RaycastsStartInColliders = false;

		static RaycastHit[] _hitArray = new RaycastHit[1];

		static Collider[] _colliderArray = new Collider[1];


		public static void Reset()
		{
			_spatialHash = new SpatialHash(SpatialHashCellSize);
			_hitArray[0].Reset();
			_colliderArray[0] = null;
		}


		public static void Clear()
		{
			_spatialHash.Clear();
		}


		internal static void DebugDraw(float secondsToDisplay)
		{
			_spatialHash.DebugDraw(secondsToDisplay, 2f);
		}


		#region Collider management

		public static IEnumerable<Collider> GetAllColliders()
		{
			return _spatialHash.GetAllObjects();
		}


		public static void AddCollider(Collider collider)
		{
			_spatialHash.Register(collider);
		}


		public static void RemoveCollider(Collider collider)
		{
			_spatialHash.Remove(collider);
		}


		public static void UpdateCollider(Collider collider)
		{
			_spatialHash.Remove(collider);
			_spatialHash.Register(collider);
		}

		#endregion


		public static RaycastHit Linecast(Vector2 start, Vector2 end, int layerMask = AllLayers)
		{
			_hitArray[0].Reset();
			LinecastAll(start, end, _hitArray, layerMask);
			return _hitArray[0];
		}


		public static int LinecastAll(Vector2 start, Vector2 end, RaycastHit[] hits, int layerMask = AllLayers)
		{
			Assert.IsFalse(hits.Length == 0, "An empty hits array was passed in. No hits will ever be returned.");
			return _spatialHash.Linecast(start, end, hits, layerMask);
		}


		public static Collider OverlapRectangle(RectangleF rect, int layerMask = AllLayers)
		{
			return OverlapRectangle(ref rect, layerMask);
		}


		public static Collider OverlapRectangle(ref RectangleF rect, int layerMask = AllLayers)
		{
			_colliderArray[0] = null;
			_spatialHash.OverlapRectangle(ref rect, _colliderArray, layerMask);
			return _colliderArray[0];
		}


		public static int OverlapRectangleAll(ref RectangleF rect, Collider[] results, int layerMask = AllLayers)
		{
			Assert.IsFalse(results.Length == 0, "An empty results array was passed in. No results will ever be returned.");
			return _spatialHash.OverlapRectangle(ref rect, results, layerMask);
		}


		public static Collider OverlapCircle(Vector2 center, float radius, int layerMask = AllLayers)
		{
			_colliderArray[0] = null;
			_spatialHash.OverlapCircle(center, radius, _colliderArray, layerMask);
			return _colliderArray[0];
		}


		public static int OverlapCircleAll(Vector2 center, float radius, Collider[] results, int layerMask = AllLayers)
		{
			Assert.IsFalse(results.Length == 0, "An empty results array was passed in. No results will ever be returned.");
			return _spatialHash.OverlapCircle(center, radius, results, layerMask);
		}


		#region Broadphase methods

		public static HashSet<Collider> BoxcastBroadphase(RectangleF rect, int layerMask = AllLayers)
		{
			return _spatialHash.AabbBroadphase(ref rect, null, layerMask);
		}


		public static HashSet<Collider> BoxcastBroadphase(ref RectangleF rect, int layerMask = AllLayers)
		{
			return _spatialHash.AabbBroadphase(ref rect, null, layerMask);
		}


		public static HashSet<Collider> BoxcastBroadphaseExcludingSelf(Collider collider, int layerMask = AllLayers)
		{
			var bounds = collider.Bounds;
			return _spatialHash.AabbBroadphase(ref bounds, collider, layerMask);
		}


		public static HashSet<Collider> BoxcastBroadphaseExcludingSelf(Collider collider, ref RectangleF rect, int layerMask = AllLayers)
		{
			return _spatialHash.AabbBroadphase(ref rect, collider, layerMask);
		}


		public static HashSet<Collider> BoxcastBroadphaseExcludingSelf(Collider collider, float deltaX, float deltaY, int layerMask = AllLayers)
		{
			var colliderBounds = collider.Bounds;
			var sweptBounds = colliderBounds.GetSweptBroadphaseBounds(deltaX, deltaY);
			return _spatialHash.AabbBroadphase(ref sweptBounds, collider, layerMask);
		}

		#endregion
	}
}