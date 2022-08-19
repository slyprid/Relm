using System.Collections.Generic;
using Relm.Core;
using Relm.Math;

namespace Relm.Assets.Tiled
{
	public class TiledTilesetTile
	{
		public TiledTileset Tileset;

		public int Id;
		public TiledTerrain[] TerrainEdges;
		public double Probability;
		public string Type;

		public Dictionary<string, string> Properties;
		public TiledImage Image;
		public TiledList<TiledObjectGroup> ObjectGroups;
		public List<TiledAnimationFrame> AnimationFrames;

		public int currentAnimationFrameGid => AnimationFrames[_animationCurrentFrame].Gid + Tileset.FirstGid;
		float _animationElapsedTime;
		int _animationCurrentFrame;

		public bool IsDestructable;

		public bool IsSlope;

		public bool IsOneWayPlatform;

		public int SlopeTopLeft;

		public int SlopeTopRight;

		public void ProcessProperties()
		{
			string value;
			if (Properties.TryGetValue("relm:isDestructable", out value))
				IsDestructable = bool.Parse(value);

			if (Properties.TryGetValue("relm:isSlope", out value))
				IsSlope = bool.Parse(value);

			if (Properties.TryGetValue("relm:isOneWayPlatform", out value))
				IsOneWayPlatform = bool.Parse(value);

			if (Properties.TryGetValue("relm:slopeTopLeft", out value))
				SlopeTopLeft = int.Parse(value);

			if (Properties.TryGetValue("relm:slopeTopRight", out value))
				SlopeTopRight = int.Parse(value);
		}

		public void UpdateAnimatedTiles()
		{
			if (AnimationFrames.Count == 0)
				return;

			_animationElapsedTime += Time.DeltaTime;

			if (_animationElapsedTime > AnimationFrames[_animationCurrentFrame].Duration)
			{
				_animationCurrentFrame = Mathf.IncrementWithWrap(_animationCurrentFrame, AnimationFrames.Count);
				_animationElapsedTime = 0;
			}
		}
	}
}