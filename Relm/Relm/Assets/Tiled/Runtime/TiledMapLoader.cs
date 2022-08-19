using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;
using Relm.Math;

namespace Relm.Assets.Tiled
{
	public static class TiledMapLoader
	{
		#region TiledMap Loader

		public static TiledMap LoadTiledMap(this TiledMap map, string filepath)
		{
			using (var stream = TitleContainer.OpenStream(filepath))
			{
				var xDoc = XDocument.Load(stream);
				map.TiledDirectory = Path.GetDirectoryName(filepath);
				map.LoadTiledMap(xDoc);

				return map;
			}
		}

		public static TiledMap LoadTiledMap(this TiledMap map, XDocument xDoc)
		{
			var xMap = xDoc.Element("map");
			map.Version = (string)xMap.Attribute("version");
			map.TiledVersion = (string)xMap.Attribute("tiledversion");

			map.Width = (int)xMap.Attribute("width");
			map.Height = (int)xMap.Attribute("height");
			map.TileWidth = (int)xMap.Attribute("tilewidth");
			map.TileHeight = (int)xMap.Attribute("tileheight");
			map.HexSideLength = (int?)xMap.Attribute("hexsidelength");

			// enum parsing
			map.Orientation = ParseOrientationType((string)xMap.Attribute("orientation"));
			map.StaggerAxis = ParseStaggerAxisType((string)xMap.Attribute("staggeraxis"));
			map.StaggerIndex = ParseStaggerIndexType((string)xMap.Attribute("staggerindex"));
			map.RenderOrder = ParaseRenderOrderType((string)xMap.Attribute("renderorder"));

			map.NextObjectID = (int?)xMap.Attribute("nextobjectid");
			map.BackgroundColor = ParseColor(xMap.Attribute("backgroundcolor"));

			map.Properties = ParsePropertyDict(xMap.Element("properties"));

			// we keep a tally of the max tile size for the case of image tilesets with random sizes
			map.MaxTileWidth = map.TileWidth;
			map.MaxTileHeight = map.TileHeight;

			map.Tilesets = new TiledList<TiledTileset>();
			foreach (var e in xMap.Elements("tileset"))
			{
				var tileset = ParseTiledTileset(map, e, map.TiledDirectory);
				map.Tilesets.Add(tileset);

				UpdateMaxTileSizes(tileset);
			}

			map.Layers = new TiledList<ITiledLayer>();
			map.TileLayers = new TiledList<TiledLayer>();
			map.ObjectGroups = new TiledList<TiledObjectGroup>();
			map.ImageLayers = new TiledList<TiledImageLayer>();
			map.Groups = new TiledList<TiledGroup>();

			ParseLayers(map, xMap, map, map.Width, map.Height, map.TiledDirectory);

			return map;
		}

		private static void UpdateMaxTileSizes(TiledTileset tileset)
		{
			// we have to iterate the dictionary because tile.gid (the key) could be any number in any order
			foreach (var kvPair in tileset.Tiles)
			{
				var tile = kvPair.Value;
				if (tile.Image != null)
				{
					if (tile.Image.Width > tileset.Map.MaxTileWidth) tileset.Map.MaxTileWidth = tile.Image.Width;
					if (tile.Image.Height > tileset.Map.MaxTileHeight) tileset.Map.MaxTileHeight = tile.Image.Height;
				}
			}

			foreach (var kvPair in tileset.TileRegions)
			{
				var region = kvPair.Value;
				var width = (int)region.Width;
				var height = (int)region.Height;
				if (width > tileset.Map.MaxTileWidth) tileset.Map.MaxTileWidth = width;
				if (width > tileset.Map.MaxTileHeight) tileset.Map.MaxTileHeight = height;
			}
		}

		static OrientationType ParseOrientationType(string type)
		{
			if (type == "unknown")
				return OrientationType.Unknown;
			if (type == "orthogonal")
				return OrientationType.Orthogonal;
			if (type == "isometric")
				return OrientationType.Isometric;
			if (type == "staggered")
				return OrientationType.Staggered;
			if (type == "hexagonal")
				return OrientationType.Hexagonal;

			return OrientationType.Unknown;
		}

		static StaggerAxisType ParseStaggerAxisType(string type)
		{
			if (type == "y")
				return StaggerAxisType.Y;
			return StaggerAxisType.X;
		}

		static StaggerIndexType ParseStaggerIndexType(string type)
		{
			if (type == "even")
				return StaggerIndexType.Even;
			return StaggerIndexType.Odd;
		}

		static RenderOrderType ParaseRenderOrderType(string type)
		{
			if (type == "right-up")
				return RenderOrderType.RightUp;
			if (type == "left-down")
				return RenderOrderType.LeftDown;
			if (type == "left-up")
				return RenderOrderType.LeftUp;
			return RenderOrderType.RightDown;
		}

		#endregion

		#region Parsers

		public static TiledTileset ParseTiledTileset(TiledMap map, XElement xTileset, string TiledDir)
		{
			// firstgid is always in Tiled, but not TSX
			var xFirstGid = xTileset.Attribute("firstgid");
			var firstGid = (int)xFirstGid;
			var source = (string)xTileset.Attribute("source");

			// source will be null if this is an embedded TiledTileset, i.e. not external
			if (source != null)
			{
				// Prepend the parent Tiled directory
				source = Path.Combine(TiledDir, source);

				// Everything else is in the TSX file
				using (var stream = TitleContainer.OpenStream(source))
				{
					var xDocTileset = XDocument.Load(stream);

					string tsxDir = Path.GetDirectoryName(source);
					var tileset = new TiledTileset().LoadTiledTileset(map, xDocTileset.Element("tileset"), firstGid, tsxDir);
					tileset.TiledDirectory = tsxDir;

					return tileset;
				}
			}

			return new TiledTileset().LoadTiledTileset(map, xTileset, firstGid, TiledDir);
		}

		public static Dictionary<string, string> ParsePropertyDict(XContainer xmlProp)
		{
			if (xmlProp == null)
				return null;

			var dict = new Dictionary<string, string>();
			foreach (var p in xmlProp.Elements("property"))
			{
				var pname = p.Attribute("name").Value;

				// Fallback to element value if no "value"
				var valueAttr = p.Attribute("value");
				var pval = valueAttr?.Value ?? p.Value;

				dict.Add(pname, pval);
			}
			return dict;
		}

		public static Color ParseColor(XAttribute xColor)
		{
			if (xColor == null)
				return Color.White;

			var colorStr = ((string)xColor).TrimStart("#".ToCharArray());
			return ColorExtensions.HexToColor(colorStr);
		}

		public static Vector2 ParsePoint(string s)
		{
			var pt = s.Split(',');
			var x = float.Parse(pt[0], NumberStyles.Float, CultureInfo.InvariantCulture);
			var y = float.Parse(pt[1], NumberStyles.Float, CultureInfo.InvariantCulture);
			return new Vector2(x, y);
		}

		public static Vector2[] ParsePoints(XElement xPoints)
		{
			var pointString = (string)xPoints.Attribute("points");
			var pointStringPair = pointString.Split(' ');
			var points = new Vector2[pointStringPair.Length];

			var index = 0;
			foreach (var s in pointStringPair)
				points[index++] = ParsePoint(s);
			return points;
		}

		public static TiledTileOffset ParseTiledTileOffset(XElement xTileOffset)
		{
			if (xTileOffset == null)
			{
				return new TiledTileOffset
				{
					X = 0,
					Y = 0
				};
			}

			return new TiledTileOffset
			{
				X = (int)xTileOffset.Attribute("x"),
				Y = (int)xTileOffset.Attribute("y")
			};
		}

		public static TiledTerrain ParseTiledTerrain(XElement xTerrain)
		{
			var terrain = new TiledTerrain();

			terrain.Name = (string)xTerrain.Attribute("name");
			terrain.Tile = (int)xTerrain.Attribute("tile");
			terrain.Properties = ParsePropertyDict(xTerrain.Element("properties"));

			return terrain;
		}

		/// <summary>
		/// parses all the layers in xEle putting them in the container
		/// </summary>
		public static void ParseLayers(object container, XElement xEle, TiledMap map, int width, int height, string TiledDirectory)
		{
			foreach (var e in xEle.Elements().Where(x => x.Name == "layer" || x.Name == "objectgroup" || x.Name == "imagelayer" || x.Name == "group"))
			{
				ITiledLayer layer;
				switch (e.Name.LocalName)
				{
					case "layer":
						var tileLayer = new TiledLayer().LoadTiledLayer(map, e, width, height);
						layer = tileLayer;

						if (container is TiledMap m)
							m.TileLayers.Add(tileLayer);
						else if (container is TiledGroup g)
							g.TileLayers.Add(tileLayer);
						break;
					case "objectgroup":
						var objectgroup = new TiledObjectGroup().LoadTiledObjectGroup(map, e);
						layer = objectgroup;

						if (container is TiledMap mm)
							mm.ObjectGroups.Add(objectgroup);
						else if (container is TiledGroup gg)
							gg.ObjectGroups.Add(objectgroup);
						break;
					case "imagelayer":
						var imagelayer = new TiledImageLayer().LoadTiledImageLayer(map, e, TiledDirectory);
						layer = imagelayer;

						if (container is TiledMap mmm)
							mmm.ImageLayers.Add(imagelayer);
						else if (container is TiledGroup ggg)
							ggg.ImageLayers.Add(imagelayer);
						break;
					case "group":
						var newGroup = new TiledGroup().LoadTiledGroup(map, e, width, height, TiledDirectory);
						layer = newGroup;

						if (container is TiledMap mmmm)
							mmmm.Groups.Add(newGroup);
						else if (container is TiledGroup gggg)
							gggg.Groups.Add(newGroup);
						break;
					default:
						throw new InvalidOperationException();
				}

				if (container is TiledMap mmmmm)
					mmmmm.Layers.Add(layer);
				else if (container is TiledGroup g)
					g.Layers.Add(layer);
			}
		}

		#endregion

		public static TiledLayer LoadTiledLayer(this TiledLayer layer, TiledMap map, XElement xLayer, int width, int height)
		{
			layer.Map = map;
			layer.Name = (string)xLayer.Attribute("name");
			layer.Opacity = (float?)xLayer.Attribute("opacity") ?? 1.0f;
			layer.Visible = (bool?)xLayer.Attribute("visible") ?? true;
			layer.OffsetX = (float?)xLayer.Attribute("offsetx") ?? 0.0f;
			layer.OffsetY = (float?)xLayer.Attribute("offsety") ?? 0.0f;
			layer.ParallaxFactorX = (float?)xLayer.Attribute("parallaxx") ?? 1.0f;
			layer.ParallaxFactorY = (float?)xLayer.Attribute("parallaxy") ?? 1.0f;

			// TODO: does the width/height passed in ever differ from the Tiled layer XML?
			layer.Width = (int)xLayer.Attribute("width");
			layer.Height = (int)xLayer.Attribute("height");

			var xData = xLayer.Element("data");
			var encoding = (string)xData.Attribute("encoding");

			layer.Tiles = new TiledLayerTile[width * height];
			if (encoding == "base64")
			{
				var decodedStream = new TiledBase64Data(xData);
				var stream = decodedStream.Data;

				var index = 0;
				using (var br = new BinaryReader(stream))
				{
					for (var j = 0; j < height; j++)
					{
						for (var i = 0; i < width; i++)
						{
							var gid = br.ReadUInt32();
							layer.Tiles[index++] = gid != 0 ? new TiledLayerTile(map, gid, i, j) : null;
						}
					}
				}
			}
			else if (encoding == "csv")
			{
				var csvData = xData.Value;
				int k = 0;
				foreach (var s in csvData.Split(','))
				{
					var gid = uint.Parse(s.Trim());
					var x = k % width;
					var y = k / width;

					layer.Tiles[k++] = gid != 0 ? new TiledLayerTile(map, gid, x, y) : null;
				}
			}
			else if (encoding == null)
			{
				int k = 0;
				foreach (var e in xData.Elements("tile"))
				{
					var gid = (uint?)e.Attribute("gid") ?? 0;

					var x = k % width;
					var y = k / width;

					layer.Tiles[k++] = gid != 0 ? new TiledLayerTile(map, gid, x, y) : null;
				}
			}
			else throw new Exception("TiledLayer: Unknown encoding.");

			layer.Properties = TiledMapLoader.ParsePropertyDict(xLayer.Element("properties"));

			return layer;
		}

		public static TiledObjectGroup LoadTiledObjectGroup(this TiledObjectGroup group, TiledMap map, XElement xObjectGroup)
		{
			group.Map = map;
			group.Name = (string)xObjectGroup.Attribute("name") ?? string.Empty;
			group.Color = TiledMapLoader.ParseColor(xObjectGroup.Attribute("color"));
			group.Opacity = (float?)xObjectGroup.Attribute("opacity") ?? 1.0f;
			group.Visible = (bool?)xObjectGroup.Attribute("visible") ?? true;
			group.OffsetX = (float?)xObjectGroup.Attribute("offsetx") ?? 0.0f;
			group.OffsetY = (float?)xObjectGroup.Attribute("offsety") ?? 0.0f;
			group.ParallaxFactorX = (float?)xObjectGroup.Attribute("parallaxx") ?? 1.0f;
			group.ParallaxFactorY = (float?)xObjectGroup.Attribute("parallaxy") ?? 1.0f;

			var drawOrderDict = new Dictionary<string, DrawOrderType> {
				{"unknown", DrawOrderType.UnknownOrder},
				{"topdown", DrawOrderType.IndexOrder},
				{"index", DrawOrderType.TopDown}
			};

			var drawOrderValue = (string)xObjectGroup.Attribute("draworder");
			if (drawOrderValue != null)
				group.DrawOrder = drawOrderDict[drawOrderValue];

			group.Objects = new TiledList<TiledObject>();
			foreach (var e in xObjectGroup.Elements("object"))
				group.Objects.Add(new TiledObject().LoadTiledObject(map, e));

			group.Properties = ParsePropertyDict(xObjectGroup.Element("properties"));

			return group;
		}

		public static TiledObject LoadTiledObject(this TiledObject obj, TiledMap map, XElement xObject)
		{
			obj.Id = (int?)xObject.Attribute("id") ?? 0;
			obj.Name = (string)xObject.Attribute("name") ?? string.Empty;
			obj.X = (float)xObject.Attribute("x");
			obj.Y = (float)xObject.Attribute("y");
			obj.Width = (float?)xObject.Attribute("width") ?? 0.0f;
			obj.Height = (float?)xObject.Attribute("height") ?? 0.0f;
			obj.Type = (string)xObject.Attribute("type") ?? (string)xObject.Attribute("class") ?? string.Empty;
			obj.Visible = (bool?)xObject.Attribute("visible") ?? true;
			obj.Rotation = (float?)xObject.Attribute("rotation") ?? 0.0f;

			// Assess object type and assign appropriate content
			var xGid = xObject.Attribute("gid");
			var xEllipse = xObject.Element("ellipse");
			var xPolygon = xObject.Element("polygon");
			var xPolyline = xObject.Element("polyline");
			var xText = xObject.Element("text");
			var xPoint = xObject.Element("point");

			if (xGid != null)
			{
				obj.Tile = new TiledLayerTile(map, (uint)xGid, Convert.ToInt32(System.Math.Round(obj.X)), Convert.ToInt32(System.Math.Round(obj.Y)));
				obj.ObjectType = TiledObjectType.Tile;
			}
			else if (xEllipse != null)
			{
				obj.ObjectType = TiledObjectType.Ellipse;
			}
			else if (xPolygon != null)
			{
				obj.Points = ParsePoints(xPolygon);
				obj.ObjectType = TiledObjectType.Polygon;
			}
			else if (xPolyline != null)
			{
				obj.Points = ParsePoints(xPolyline);
				obj.ObjectType = TiledObjectType.Polyline;
			}
			else if (xText != null)
			{
				obj.Text = new TiledText().LoadTiledText(xText);
				obj.ObjectType = TiledObjectType.Text;
			}
			else if (xPoint != null)
			{
				obj.ObjectType = TiledObjectType.Point;
			}
			else
			{
				obj.ObjectType = TiledObjectType.Basic;
			}

			obj.Properties = ParsePropertyDict(xObject.Element("properties"));

			return obj;
		}

		public static TiledText LoadTiledText(this TiledText text, XElement xText)
		{
			text.FontFamily = (string)xText.Attribute("fontfamily") ?? "sans-serif";
			text.PixelSize = (int?)xText.Attribute("pixelsize") ?? 16;
			text.Wrap = (bool?)xText.Attribute("wrap") ?? false;
			text.Color = ParseColor(xText.Attribute("color"));
			text.Bold = (bool?)xText.Attribute("bold") ?? false;
			text.Italic = (bool?)xText.Attribute("italic") ?? false;
			text.Underline = (bool?)xText.Attribute("underline") ?? false;
			text.Strikeout = (bool?)xText.Attribute("strikeout") ?? false;
			text.Kerning = (bool?)xText.Attribute("kerning") ?? true;
			text.Alignment = new TiledAlignment().LoadTiledAlignment(xText);
			text.Value = xText.Value;

			return text;
		}

		public static TiledAlignment LoadTiledAlignment(this TiledAlignment alignment, XElement xText)
		{
			string FirstLetterToUpperCase(string str)
			{
				if (string.IsNullOrEmpty(str))
					return str;
				return str[0].ToString().ToUpper() + str.Substring(1);
			}

			var xHorizontal = (string)xText.Attribute("halign") ?? "Left";
			alignment.Horizontal = (TiledHorizontalAlignment)Enum.Parse(typeof(TiledHorizontalAlignment), FirstLetterToUpperCase(xHorizontal));

			var xVertical = (string)xText.Attribute("valign") ?? "Top";
			alignment.Vertical = (TiledVerticalAlignment)Enum.Parse(typeof(TiledVerticalAlignment), FirstLetterToUpperCase(xVertical));

			return alignment;
		}

		public static TiledImageLayer LoadTiledImageLayer(this TiledImageLayer layer, TiledMap map, XElement xImageLayer, string TiledDir = "")
		{
			layer.Map = map;
			layer.Name = (string)xImageLayer.Attribute("name");

			layer.Width = (int?)xImageLayer.Attribute("width");
			layer.Height = (int?)xImageLayer.Attribute("height");
			layer.Visible = (bool?)xImageLayer.Attribute("visible") ?? true;
			layer.Opacity = (float?)xImageLayer.Attribute("opacity") ?? 1.0f;
			layer.OffsetX = (float?)xImageLayer.Attribute("offsetx") ?? 0.0f;
			layer.OffsetY = (float?)xImageLayer.Attribute("offsety") ?? 0.0f;
			layer.ParallaxFactorX = (float?)xImageLayer.Attribute("parallaxx") ?? 1.0f;
			layer.ParallaxFactorY = (float?)xImageLayer.Attribute("parallaxy") ?? 1.0f;

			var xImage = xImageLayer.Element("image");
			if (xImage != null)
				layer.Image = new TiledImage().LoadTiledImage(xImage, TiledDir);

			layer.Properties = ParsePropertyDict(xImageLayer.Element("properties"));

			return layer;
		}

		public static TiledGroup LoadTiledGroup(this TiledGroup group, TiledMap map, XElement xGroup, int width, int height, string TiledDirectory)
		{
			group.map = map;
			group.Name = (string)xGroup.Attribute("name") ?? string.Empty;
			group.Opacity = (float?)xGroup.Attribute("opacity") ?? 1.0f;
			group.Visible = (bool?)xGroup.Attribute("visible") ?? true;
			group.OffsetX = (float?)xGroup.Attribute("offsetx") ?? 0.0f;
			group.OffsetY = (float?)xGroup.Attribute("offsety") ?? 0.0f;
			group.ParallaxFactorX = (float?)xGroup.Attribute("parallaxx") ?? 1.0f;
			group.ParallaxFactorY = (float?)xGroup.Attribute("parallaxy") ?? 1.0f;

			group.Properties = ParsePropertyDict(xGroup.Element("properties"));

			group.Layers = new TiledList<ITiledLayer>();
			group.TileLayers = new TiledList<TiledLayer>();
			group.ObjectGroups = new TiledList<TiledObjectGroup>();
			group.ImageLayers = new TiledList<TiledImageLayer>();
			group.Groups = new TiledList<TiledGroup>();

			ParseLayers(group, xGroup, map, width, height, TiledDirectory);

			return group;
		}

		public static TiledTileset LoadTiledTileset(this TiledTileset tileset, TiledMap map, XElement xTileset, int firstGid, string tsxDir)
		{
			tileset.Map = map;
			tileset.FirstGid = firstGid;

			tileset.Name = (string)xTileset.Attribute("name");
			tileset.TileWidth = (int)xTileset.Attribute("tilewidth");
			tileset.TileHeight = (int)xTileset.Attribute("tileheight");
			tileset.Spacing = (int?)xTileset.Attribute("spacing") ?? 0;
			tileset.Margin = (int?)xTileset.Attribute("margin") ?? 0;
			tileset.Columns = (int?)xTileset.Attribute("columns");
			tileset.TileCount = (int?)xTileset.Attribute("tilecount");
			tileset.TileOffset = ParseTiledTileOffset(xTileset.Element("tileoffset"));

			var xImage = xTileset.Element("image");
			if (xImage != null)
				tileset.Image = new TiledImage().LoadTiledImage(xImage, tsxDir);

			var xTerrainType = xTileset.Element("terraintypes");
			if (xTerrainType != null)
			{
				tileset.Terrains = new TiledList<TiledTerrain>();
				foreach (var e in xTerrainType.Elements("terrain"))
					tileset.Terrains.Add(ParseTiledTerrain(e));
			}

			tileset.Tiles = new Dictionary<int, TiledTilesetTile>();
			foreach (var xTile in xTileset.Elements("tile"))
			{
				var tile = new TiledTilesetTile().LoadTiledTilesetTile(tileset, xTile, tileset.Terrains, tsxDir);
				tileset.Tiles[tile.Id] = tile;
			}

			tileset.Properties = ParsePropertyDict(xTileset.Element("properties"));

			// cache our source rects for each tile so we dont have to calculate them every time we render. If we have
			// an image this is a normal tileset, else its an image tileset
			tileset.TileRegions = new Dictionary<int, RectangleF>();
			if (tileset.Image != null)
			{
				var id = firstGid;
				for (var y = tileset.Margin; y < tileset.Image.Height - tileset.Margin; y += tileset.TileHeight + tileset.Spacing)
				{
					var column = 0;
					for (var x = tileset.Margin; x < tileset.Image.Width - tileset.Margin; x += tileset.TileWidth + tileset.Spacing)
					{
						tileset.TileRegions.Add(id++, new RectangleF(x, y, tileset.TileWidth, tileset.TileHeight));

						if (++column >= tileset.Columns)
							break;
					}
				}
			}
			else
			{
				foreach (var tile in tileset.Tiles.Values)
					tileset.TileRegions.Add(firstGid + tile.Id, new RectangleF(0, 0, tile.Image.Width, tile.Image.Height));
			}

			return tileset;
		}

		public static TiledTilesetTile LoadTiledTilesetTile(this TiledTilesetTile tile, TiledTileset tileset, XElement xTile, TiledList<TiledTerrain> Terrains, string TiledDir = "")
		{
			tile.Tileset = tileset;
			tile.Id = (int)xTile.Attribute("id");

			var strTerrain = (string)xTile.Attribute("terrain");
			if (strTerrain != null)
			{
				tile.TerrainEdges = new TiledTerrain[4];
				var index = 0;
				foreach (var v in strTerrain.Split(','))
				{
					var success = int.TryParse(v, out int result);

					TiledTerrain edge;
					if (success)
						edge = Terrains[result];
					else
						edge = null;
					tile.TerrainEdges[index++] = edge;
				}
			}

			tile.Probability = (double?)xTile.Attribute("probability") ?? 1.0;
			tile.Type = (string)xTile.Attribute("type");
			var xImage = xTile.Element("image");
			if (xImage != null)
				tile.Image = new TiledImage().LoadTiledImage(xImage, TiledDir);

			tile.ObjectGroups = new TiledList<TiledObjectGroup>();
			foreach (var e in xTile.Elements("objectgroup"))
				tile.ObjectGroups.Add(new TiledObjectGroup().LoadTiledObjectGroup(tileset.Map, e));

			tile.AnimationFrames = new List<TiledAnimationFrame>();
			if (xTile.Element("animation") != null)
			{
				foreach (var e in xTile.Element("animation").Elements("frame"))
					tile.AnimationFrames.Add(new TiledAnimationFrame().LoadTiledAnimationFrame(e));
			}

			tile.Properties = ParsePropertyDict(xTile.Element("properties"));

			if (tile.Properties != null)
				tile.ProcessProperties();

			return tile;
		}

		public static TiledAnimationFrame LoadTiledAnimationFrame(this TiledAnimationFrame frame, XElement xFrame)
		{
			frame.Gid = (int)xFrame.Attribute("tileid");
			frame.Duration = (float)xFrame.Attribute("duration") / 1000f;

			return frame;
		}

		public static TiledImage LoadTiledImage(this TiledImage image, XElement xImage, string TiledDir = "")
		{
			var xSource = xImage.Attribute("source");
			if (xSource != null)
			{
				// Append directory if present
				image.Source = Path.Combine(TiledDir, (string)xSource);

				using (var stream = TitleContainer.OpenStream(image.Source))
					image.Texture = Texture2D.FromStream(RelmGame.GraphicsDevice, stream);
			}
			else
			{
				image.Format = (string)xImage.Attribute("format");
				var xData = xImage.Element("data");
				var decodedStream = new TiledBase64Data(xData);
				image.Data = decodedStream.Data;
				throw new NotSupportedException("Stream Data loading is not yet supported");
			}

			image.Trans = TiledMapLoader.ParseColor(xImage.Attribute("trans"));
			image.Width = (int?)xImage.Attribute("width") ?? 0;
			image.Height = (int?)xImage.Attribute("height") ?? 0;

			return image;
		}
	}
}