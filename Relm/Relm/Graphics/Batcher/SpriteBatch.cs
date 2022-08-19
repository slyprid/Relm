using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics.Textures;
using Relm.Math;
using SpriteEffect = Relm.Graphics.Effects.SpriteEffect;

namespace Relm.Graphics
{
	public class SpriteBatch 
        : GraphicsResource
	{
        public static bool UseFnaHalfPixelMatrix = false;
        static readonly float[] _cornerOffsetX = { 0.0f, 1.0f, 0.0f, 1.0f };
        static readonly float[] _cornerOffsetY = { 0.0f, 0.0f, 1.0f, 1.0f };
        static readonly short[] _indexData = GenerateIndexArray();
		const int MAX_SPRITES = 2048;
        const int MAX_VERTICES = MAX_SPRITES * 4;
        const int MAX_INDICES = MAX_SPRITES * 6;

		private bool _shouldIgnoreRoundingDestinations;
        private DynamicVertexBuffer _vertexBuffer;
        private IndexBuffer _indexBuffer;
        private VertexPositionColorTexture4[] _vertexInfo;
        private Texture2D[] _textureInfo;
        private SpriteEffect _spriteEffect;
        private EffectPass _spriteEffectPass;
        private bool _beginCalled;
        private BlendState _blendState;
        private SamplerState _samplerState;
        private DepthStencilState _depthStencilState;
        private RasterizerState _rasterizerState;
        private bool _disableBatching;
        private int _numSprites;
        private Matrix _transformMatrix;
        private Matrix _projectionMatrix;
        private Matrix _matrixTransformMatrix;
        private Effect _customEffect;

		public Matrix TransformMatrix => _transformMatrix;
        public bool ShouldRoundDestinations { get; set; } = true;
		
		public SpriteBatch(GraphicsDevice graphicsDevice)
		{
			Assert.IsTrue(graphicsDevice != null);

			GraphicsDevice = graphicsDevice;

			_vertexInfo = new VertexPositionColorTexture4[MAX_SPRITES];
			_textureInfo = new Texture2D[MAX_SPRITES];
			_vertexBuffer = new DynamicVertexBuffer(graphicsDevice, typeof(VertexPositionColorTexture), MAX_VERTICES, BufferUsage.WriteOnly);
			_indexBuffer = new IndexBuffer(graphicsDevice, IndexElementSize.SixteenBits, MAX_INDICES, BufferUsage.WriteOnly);
			_indexBuffer.SetData(_indexData);

			_spriteEffect = new SpriteEffect();
			_spriteEffectPass = _spriteEffect.CurrentTechnique.Passes[0];

			_projectionMatrix = new Matrix(
				0f, //(float)( 2.0 / (double)viewport.Width ) is the actual value we will use
				0.0f,
				0.0f,
				0.0f,
				0.0f,
				0f, //(float)( -2.0 / (double)viewport.Height ) is the actual value we will use
				0.0f,
				0.0f,
				0.0f,
				0.0f,
				1.0f,
				0.0f,
				-1.0f,
				1.0f,
				0.0f,
				1.0f
			);
		}

		protected override void Dispose(bool disposing)
		{
			if (!IsDisposed && disposing)
			{
				_spriteEffect.Dispose();
				_indexBuffer.Dispose();
				_vertexBuffer.Dispose();
			}

			base.Dispose(disposing);
		}

		public void SetIgnoreRoundingDestinations(bool shouldIgnore)
		{
			_shouldIgnoreRoundingDestinations = shouldIgnore;
		}

		#region Public begin/end methods

		public void Begin()
		{
			Begin(BlendState.AlphaBlend, RelmGame.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity, false);
		}

		public void Begin(Effect effect)
		{
			Begin(BlendState.AlphaBlend, RelmGame.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, effect, Matrix.Identity, false);
		}

		public void Begin(Material material)
		{
			Begin(material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullCounterClockwise, material.Effect);
		}

		public void Begin(Matrix transformationMatrix)
		{
			Begin(BlendState.AlphaBlend, RelmGame.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, transformationMatrix, false);
		}

		public void Begin(BlendState blendState)
		{
			Begin(blendState, RelmGame.DefaultSamplerState, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Matrix.Identity, false);
		}

		public void Begin(Material material, Matrix transformationMatrix)
		{
			Begin(material.BlendState, material.SamplerState, material.DepthStencilState, RasterizerState.CullCounterClockwise, material.Effect, transformationMatrix, false);
		}

		public void Begin(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState)
		{
			Begin(
				blendState,
				samplerState,
				depthStencilState,
				rasterizerState,
				null,
				Matrix.Identity,
				false
			);
		}

		public void Begin(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect)
		{
			Begin(
				blendState,
				samplerState,
				depthStencilState,
				rasterizerState,
				effect,
				Matrix.Identity,
				false
			);
		}

		public void Begin(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformationMatrix)
		{
			Begin(
				blendState,
				samplerState,
				depthStencilState,
				rasterizerState,
				effect,
				transformationMatrix,
				false
			);
		}

		public void Begin(BlendState blendState, SamplerState samplerState, DepthStencilState depthStencilState, RasterizerState rasterizerState, Effect effect, Matrix transformationMatrix, bool disableBatching)
		{
			Assert.IsFalse(_beginCalled, "Begin has been called before calling End after the last call to Begin. Begin cannot be called again until End has been successfully called.");
			_beginCalled = true;

			_blendState = blendState ?? BlendState.AlphaBlend;
			_samplerState = samplerState ?? RelmGame.DefaultSamplerState;
			_depthStencilState = depthStencilState ?? DepthStencilState.None;
			_rasterizerState = rasterizerState ?? RasterizerState.CullCounterClockwise;

			_customEffect = effect;
			_transformMatrix = transformationMatrix;
			_disableBatching = disableBatching;

			if (_disableBatching)
				PrepRenderState();
		}

		public void End()
		{
			Assert.IsTrue(_beginCalled, "End was called, but Begin has not yet been called. You must call Begin successfully before you can call End.");
			_beginCalled = false;

			if (!_disableBatching)
				FlushBatch();

			_customEffect = null;
		}

		#endregion

		#region Public draw methods

		public void Draw(Texture2D texture, Vector2 position)
		{
			CheckBegin();
			PushSprite(texture, null, position.X, position.Y, 1.0f, 1.0f, Color.White, Vector2.Zero, 0.0f, 0.0f, 0, false, 0, 0, 0, 0);
		}

		public void Draw(Texture2D texture, Vector2 position, Color color)
		{
			CheckBegin();
			PushSprite(texture, null, position.X, position.Y, 1.0f, 1.0f, color, Vector2.Zero, 0.0f, 0.0f, 0, false, 0, 0, 0, 0);
		}

		public void Draw(Texture2D texture, Rectangle destinationRectangle)
		{
			CheckBegin();
			PushSprite(texture, null, destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height, Color.White, Vector2.Zero, 0.0f, 0.0f, 0, true, 0, 0, 0, 0);
		}

		public void Draw(Texture2D texture, Rectangle destinationRectangle, Color color)
		{
			CheckBegin();
			PushSprite(texture, null, destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height, color, Vector2.Zero, 0.0f, 0.0f, 0, true, 0, 0, 0, 0);
		}


		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color)
		{
			CheckBegin();
			PushSprite(texture, sourceRectangle, destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height, color, Vector2.Zero, 0.0f, 0.0f, 0, true, 0, 0, 0, 0);
		}

		public void Draw(Texture2D texture, Rectangle destinationRectangle, Rectangle? sourceRectangle, Color color, SpriteEffects effects)
		{
			CheckBegin();
			PushSprite(texture, sourceRectangle, destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height, color, Vector2.Zero, 0.0f, 0.0f, (byte)(effects & (SpriteEffects)0x03), true, 0, 0, 0, 0);
		}

		public void Draw(
			Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			SpriteEffects effects,
			float layerDepth,
			float skewTopX, float skewBottomX, float skewLeftY, float skewRightY
		)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				destinationRectangle.X,
				destinationRectangle.Y,
				destinationRectangle.Width,
				destinationRectangle.Height,
				color,
				Vector2.Zero,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				true,
				skewTopX, skewBottomX, skewLeftY, skewRightY
			);
		}

		public void Draw(Texture2D texture, Vector2 position, Rectangle? sourceRectangle, Color color)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				position.X,
				position.Y,
				1.0f,
				1.0f,
				color,
				Vector2.Zero,
				0.0f,
				0.0f,
				0,
				false,
				0, 0, 0, 0
			);
		}

		public void Draw(
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			float scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				position.X,
				position.Y,
				scale,
				scale,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				false,
				0, 0, 0, 0
			);
		}

		public void Draw(
			Sprite sprite,
			Vector2 position,
			Color color,
			float rotation,
			Vector2 origin,
			float scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckBegin();
			PushSprite(
				sprite,
				position.X,
				position.Y,
				scale,
				scale,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				0, 0, 0, 0
			);
		}

		public void Draw(
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				position.X,
				position.Y,
				scale.X,
				scale.Y,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				false,
				0, 0, 0, 0
			);
		}

		public void Draw(
			Sprite sprite,
			Vector2 position,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckBegin();
			PushSprite(
				sprite,
				position.X,
				position.Y,
				scale.X,
				scale.Y,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				0, 0, 0, 0
			);
		}

		public void Draw(
			Texture2D texture,
			Vector2 position,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			Vector2 scale,
			SpriteEffects effects,
			float layerDepth,
			float skewTopX, float skewBottomX, float skewLeftY, float skewRightY
		)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				position.X,
				position.Y,
				scale.X,
				scale.Y,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				false,
				skewTopX, skewBottomX, skewLeftY, skewRightY
			);
		}

		public void Draw(
			Texture2D texture,
			Rectangle destinationRectangle,
			Rectangle? sourceRectangle,
			Color color,
			float rotation,
			Vector2 origin,
			SpriteEffects effects,
			float layerDepth
		)
		{
			CheckBegin();
			PushSprite(
				texture,
				sourceRectangle,
				destinationRectangle.X,
				destinationRectangle.Y,
				destinationRectangle.Width,
				destinationRectangle.Height,
				color,
				origin,
				rotation,
				layerDepth,
				(byte)(effects & (SpriteEffects)0x03),
				true,
				0, 0, 0, 0
			);
		}

		public unsafe void DrawRaw(Texture2D texture, Vector3[] verts, Vector2[] textureCoords, Color[] colors)
		{
			Assert.IsTrue(verts.Length == 4, "there must be only 4 verts");
			Assert.IsTrue(textureCoords.Length == 4, "there must be only 4 texture coordinates");
			Assert.IsTrue(colors.Length == 4, "there must be only 4 colors");

			if (_numSprites >= MAX_SPRITES) FlushBatch();

			fixed (VertexPositionColorTexture4* vertexInfo = &_vertexInfo[_numSprites])
			{
				vertexInfo->Position0 = verts[0];
				vertexInfo->Position1 = verts[1];
				vertexInfo->Position2 = verts[2];
				vertexInfo->Position3 = verts[3];

				vertexInfo->TextureCoordinate0 = textureCoords[0];
				vertexInfo->TextureCoordinate1 = textureCoords[1];
				vertexInfo->TextureCoordinate2 = textureCoords[2];
				vertexInfo->TextureCoordinate3 = textureCoords[3];

				vertexInfo->Color0 = colors[0];
				vertexInfo->Color1 = colors[1];
				vertexInfo->Color2 = colors[2];
				vertexInfo->Color3 = colors[3];
			}

			if (_disableBatching)
			{
				_vertexBuffer.SetData(0, _vertexInfo, 0, 1, VertexPositionColorTexture4.RealStride, SetDataOptions.None);
				DrawPrimitives(texture, 0, 1);
			}
			else
			{
				_textureInfo[_numSprites] = texture;
				_numSprites += 1;
			}
		}

		public unsafe void DrawRaw(Texture2D texture, Vector3[] verts, Vector2[] textureCoords, Color color)
		{
			Assert.IsTrue(verts.Length == 4, "there must be only 4 verts");
			Assert.IsTrue(textureCoords.Length == 4, "there must be only 4 texture coordinates");

			// we're out of space, flush
			if (_numSprites >= MAX_SPRITES)
				FlushBatch();

			fixed (VertexPositionColorTexture4* vertexInfo = &_vertexInfo[_numSprites])
			{
				vertexInfo->Position0 = verts[0];
				vertexInfo->Position1 = verts[1];
				vertexInfo->Position2 = verts[2];
				vertexInfo->Position3 = verts[3];

				vertexInfo->TextureCoordinate0 = textureCoords[0];
				vertexInfo->TextureCoordinate1 = textureCoords[1];
				vertexInfo->TextureCoordinate2 = textureCoords[2];
				vertexInfo->TextureCoordinate3 = textureCoords[3];

				vertexInfo->Color0 = color;
				vertexInfo->Color1 = color;
				vertexInfo->Color2 = color;
				vertexInfo->Color3 = color;
			}

			if (_disableBatching)
			{
				_vertexBuffer.SetData(0, _vertexInfo, 0, 1, VertexPositionColorTexture4.RealStride, SetDataOptions.None);
				DrawPrimitives(texture, 0, 1);
			}
			else
			{
				_textureInfo[_numSprites] = texture;
				_numSprites += 1;
			}
		}

		#endregion
		
		static short[] GenerateIndexArray()
		{
			var result = new short[MAX_INDICES];
			for (int i = 0, j = 0; i < MAX_INDICES; i += 6, j += 4)
			{
				result[i] = (short)(j);
				result[i + 1] = (short)(j + 1);
				result[i + 2] = (short)(j + 2);
				result[i + 3] = (short)(j + 3);
				result[i + 4] = (short)(j + 2);
				result[i + 5] = (short)(j + 1);
			}

			return result;
		}

        #region Methods

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		unsafe void PushSprite(Texture2D texture, Rectangle? sourceRectangle, float destinationX, float destinationY, float destinationW, float destinationH, Color color, Vector2 origin, float rotation, float depth, byte effects, bool destSizeInPixels, float skewTopX, float skewBottomX, float skewLeftY, float skewRightY)
		{
			if (_numSprites >= MAX_SPRITES) FlushBatch();

			if (!_shouldIgnoreRoundingDestinations && ShouldRoundDestinations)
			{
				destinationX = Mathf.Round(destinationX);
				destinationY = Mathf.Round(destinationY);
			}

			float sourceX, sourceY, sourceW, sourceH;
			float originX, originY;
			if (sourceRectangle.HasValue)
			{
				var inverseTexW = 1.0f / (float)texture.Width;
				var inverseTexH = 1.0f / (float)texture.Height;

				sourceX = sourceRectangle.Value.X * inverseTexW;
				sourceY = sourceRectangle.Value.Y * inverseTexH;
				sourceW = sourceRectangle.Value.Width * inverseTexW;
				sourceH = sourceRectangle.Value.Height * inverseTexH;

				originX = (origin.X / sourceW) * inverseTexW;
				originY = (origin.Y / sourceH) * inverseTexH;

				if (!destSizeInPixels)
				{
					destinationW *= sourceRectangle.Value.Width;
					destinationH *= sourceRectangle.Value.Height;
				}
			}
			else
			{
				sourceX = 0.0f;
				sourceY = 0.0f;
				sourceW = 1.0f;
				sourceH = 1.0f;

				originX = origin.X * (1.0f / texture.Width);
				originY = origin.Y * (1.0f / texture.Height);

				if (!destSizeInPixels)
				{
					destinationW *= texture.Width;
					destinationH *= texture.Height;
				}
			}

			float rotationMatrix1X;
			float rotationMatrix1Y;
			float rotationMatrix2X;
			float rotationMatrix2Y;
			if (!Mathf.WithinEpsilon(rotation))
			{
				var sin = Mathf.Sin(rotation);
				var cos = Mathf.Cos(rotation);
				rotationMatrix1X = cos;
				rotationMatrix1Y = sin;
				rotationMatrix2X = -sin;
				rotationMatrix2Y = cos;
			}
			else
			{
				rotationMatrix1X = 1.0f;
				rotationMatrix1Y = 0.0f;
				rotationMatrix2X = 0.0f;
				rotationMatrix2Y = 1.0f;
			}


			if (effects != 0)
			{
				skewTopX *= -1;
				skewBottomX *= -1;
				skewLeftY *= -1;
				skewRightY *= -1;
			}

			fixed (VertexPositionColorTexture4* vertexInfo = &_vertexInfo[_numSprites])
			{
				var cornerX = (_cornerOffsetX[0] - originX) * destinationW + skewTopX;
				var cornerY = (_cornerOffsetY[0] - originY) * destinationH - skewLeftY;
				vertexInfo->Position0.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position0.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[1] - originX) * destinationW + skewTopX;
				cornerY = (_cornerOffsetY[1] - originY) * destinationH - skewRightY;
				vertexInfo->Position1.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position1.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[2] - originX) * destinationW + skewBottomX;
				cornerY = (_cornerOffsetY[2] - originY) * destinationH - skewLeftY;
				vertexInfo->Position2.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position2.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[3] - originX) * destinationW + skewBottomX;
				cornerY = (_cornerOffsetY[3] - originY) * destinationH - skewRightY;
				vertexInfo->Position3.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position3.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				vertexInfo->TextureCoordinate0.X = (_cornerOffsetX[0 ^ effects] * sourceW) + sourceX;
				vertexInfo->TextureCoordinate0.Y = (_cornerOffsetY[0 ^ effects] * sourceH) + sourceY;
				vertexInfo->TextureCoordinate1.X = (_cornerOffsetX[1 ^ effects] * sourceW) + sourceX;
				vertexInfo->TextureCoordinate1.Y = (_cornerOffsetY[1 ^ effects] * sourceH) + sourceY;
				vertexInfo->TextureCoordinate2.X = (_cornerOffsetX[2 ^ effects] * sourceW) + sourceX;
				vertexInfo->TextureCoordinate2.Y = (_cornerOffsetY[2 ^ effects] * sourceH) + sourceY;
				vertexInfo->TextureCoordinate3.X = (_cornerOffsetX[3 ^ effects] * sourceW) + sourceX;
				vertexInfo->TextureCoordinate3.Y = (_cornerOffsetY[3 ^ effects] * sourceH) + sourceY;
				vertexInfo->Position0.Z = depth;
				vertexInfo->Position1.Z = depth;
				vertexInfo->Position2.Z = depth;
				vertexInfo->Position3.Z = depth;
				vertexInfo->Color0 = color;
				vertexInfo->Color1 = color;
				vertexInfo->Color2 = color;
				vertexInfo->Color3 = color;
			}

			if (_disableBatching)
			{
				_vertexBuffer.SetData(0, _vertexInfo, 0, 1, VertexPositionColorTexture4.RealStride, SetDataOptions.None);
				DrawPrimitives(texture, 0, 1);
			}
			else
			{
				_textureInfo[_numSprites] = texture;
				_numSprites += 1;
			}
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		unsafe void PushSprite(Sprite sprite, float destinationX, float destinationY, float destinationW, float destinationH, Color color, Vector2 origin, float rotation, float depth, byte effects, float skewTopX, float skewBottomX, float skewLeftY, float skewRightY)
		{
			if (_numSprites >= MAX_SPRITES) FlushBatch();

			var originX = (origin.X / sprite.Uvs.Width) / sprite.Texture2D.Width;
			var originY = (origin.Y / sprite.Uvs.Height) / sprite.Texture2D.Height;
			destinationW *= sprite.SourceRect.Width;
			destinationH *= sprite.SourceRect.Height;

			float rotationMatrix1X;
			float rotationMatrix1Y;
			float rotationMatrix2X;
			float rotationMatrix2Y;
			if (!Mathf.WithinEpsilon(rotation))
			{
				var sin = Mathf.Sin(rotation);
				var cos = Mathf.Cos(rotation);
				rotationMatrix1X = cos;
				rotationMatrix1Y = sin;
				rotationMatrix2X = -sin;
				rotationMatrix2Y = cos;
			}
			else
			{
				rotationMatrix1X = 1.0f;
				rotationMatrix1Y = 0.0f;
				rotationMatrix2X = 0.0f;
				rotationMatrix2Y = 1.0f;
			}


			if (effects != 0)
			{
				skewTopX *= -1;
				skewBottomX *= -1;
				skewLeftY *= -1;
				skewRightY *= -1;
			}

			fixed (VertexPositionColorTexture4* vertexInfo = &_vertexInfo[_numSprites])
			{
				var cornerX = (_cornerOffsetX[0] - originX) * destinationW + skewTopX;
				var cornerY = (_cornerOffsetY[0] - originY) * destinationH - skewLeftY;
				vertexInfo->Position0.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position0.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[1] - originX) * destinationW + skewTopX;
				cornerY = (_cornerOffsetY[1] - originY) * destinationH - skewRightY;
				vertexInfo->Position1.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position1.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[2] - originX) * destinationW + skewBottomX;
				cornerY = (_cornerOffsetY[2] - originY) * destinationH - skewLeftY;
				vertexInfo->Position2.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position2.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				cornerX = (_cornerOffsetX[3] - originX) * destinationW + skewBottomX;
				cornerY = (_cornerOffsetY[3] - originY) * destinationH - skewRightY;
				vertexInfo->Position3.X = (
					(rotationMatrix2X * cornerY) +
					(rotationMatrix1X * cornerX) +
					destinationX
				);
				vertexInfo->Position3.Y = (
					(rotationMatrix2Y * cornerY) +
					(rotationMatrix1Y * cornerX) +
					destinationY
				);

				vertexInfo->TextureCoordinate0.X =
					(_cornerOffsetX[0 ^ effects] * sprite.Uvs.Width) + sprite.Uvs.X;
				vertexInfo->TextureCoordinate0.Y =
					(_cornerOffsetY[0 ^ effects] * sprite.Uvs.Height) + sprite.Uvs.Y;
				vertexInfo->TextureCoordinate1.X =
					(_cornerOffsetX[1 ^ effects] * sprite.Uvs.Width) + sprite.Uvs.X;
				vertexInfo->TextureCoordinate1.Y =
					(_cornerOffsetY[1 ^ effects] * sprite.Uvs.Height) + sprite.Uvs.Y;
				vertexInfo->TextureCoordinate2.X =
					(_cornerOffsetX[2 ^ effects] * sprite.Uvs.Width) + sprite.Uvs.X;
				vertexInfo->TextureCoordinate2.Y =
					(_cornerOffsetY[2 ^ effects] * sprite.Uvs.Height) + sprite.Uvs.Y;
				vertexInfo->TextureCoordinate3.X =
					(_cornerOffsetX[3 ^ effects] * sprite.Uvs.Width) + sprite.Uvs.X;
				vertexInfo->TextureCoordinate3.Y =
					(_cornerOffsetY[3 ^ effects] * sprite.Uvs.Height) + sprite.Uvs.Y;
				vertexInfo->Position0.Z = depth;
				vertexInfo->Position1.Z = depth;
				vertexInfo->Position2.Z = depth;
				vertexInfo->Position3.Z = depth;
				vertexInfo->Color0 = color;
				vertexInfo->Color1 = color;
				vertexInfo->Color2 = color;
				vertexInfo->Color3 = color;
			}

			if (_disableBatching)
			{
				_vertexBuffer.SetData(0, _vertexInfo, 0, 1, VertexPositionColorTexture4.RealStride, SetDataOptions.None);
				DrawPrimitives(sprite, 0, 1);
			}
			else
			{
				_textureInfo[_numSprites] = sprite;
				_numSprites += 1;
			}
		}

		public unsafe void FlushBatch()
		{
			if (_numSprites == 0) return;

			var offset = 0;
			Texture2D curTexture = null;

			PrepRenderState();

			_vertexBuffer.SetData(0, _vertexInfo, 0, _numSprites, VertexPositionColorTexture4.RealStride, SetDataOptions.Discard);

			curTexture = _textureInfo[0];
			for (var i = 1; i < _numSprites; i += 1)
			{
				if (_textureInfo[i] != curTexture)
				{
					DrawPrimitives(curTexture, offset, i - offset);
					curTexture = _textureInfo[i];
					offset = i;
				}
			}

			DrawPrimitives(curTexture, offset, _numSprites - offset);

			_numSprites = 0;
		}

		public void EnableScissorTest(bool shouldEnable)
		{
			var currentValue = _rasterizerState.ScissorTestEnable;
			if (currentValue == shouldEnable) return;

			FlushBatch();

			_rasterizerState = new RasterizerState
			{
				CullMode = _rasterizerState.CullMode,
				DepthBias = _rasterizerState.DepthBias,
				FillMode = _rasterizerState.FillMode,
				MultiSampleAntiAlias = _rasterizerState.MultiSampleAntiAlias,
				SlopeScaleDepthBias = _rasterizerState.SlopeScaleDepthBias,
				ScissorTestEnable = shouldEnable
			};
		}

		void PrepRenderState()
		{
			GraphicsDevice.BlendState = _blendState;
			GraphicsDevice.SamplerStates[0] = _samplerState;
			GraphicsDevice.DepthStencilState = _depthStencilState;
			GraphicsDevice.RasterizerState = _rasterizerState;

			GraphicsDevice.SetVertexBuffer(_vertexBuffer);
			GraphicsDevice.Indices = _indexBuffer;

			var viewport = GraphicsDevice.Viewport;

			if (UseFnaHalfPixelMatrix)
			{
				_projectionMatrix.M11 = (float)(2.0 / (double)(viewport.Width / 2 * 2 - 1));
				_projectionMatrix.M22 = (float)(-2.0 / (double)(viewport.Height / 2 * 2 - 1));
			}
			else
			{
				_projectionMatrix.M11 = (float)(2.0 / (double)viewport.Width);
				_projectionMatrix.M22 = (float)(-2.0 / (double)viewport.Height);
			}

			_projectionMatrix.M41 = -1 - 0.5f * _projectionMatrix.M11;
			_projectionMatrix.M42 = 1 - 0.5f * _projectionMatrix.M22;

			Matrix.Multiply(ref _transformMatrix, ref _projectionMatrix, out _matrixTransformMatrix);
			_spriteEffect.SetMatrixTransform(ref _matrixTransformMatrix);

			_spriteEffectPass.Apply();
		}

		void DrawPrimitives(Texture texture, int baseSprite, int batchSize)
		{
			if (_customEffect != null)
			{
				foreach (var pass in _customEffect.CurrentTechnique.Passes)
				{
					pass.Apply();
					GraphicsDevice.Textures[0] = texture;
					GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, baseSprite * 4, 0, batchSize * 2);
				}
			}
			else
			{
				GraphicsDevice.Textures[0] = texture;
				GraphicsDevice.DrawIndexedPrimitives(PrimitiveType.TriangleList, baseSprite * 4, 0, batchSize * 2);
			}
		}

		[System.Diagnostics.Conditional("DEBUG")]
		void CheckBegin()
		{
			if (!_beginCalled) throw new InvalidOperationException("Begin has not been called. Begin must be called before you can draw");
		}

		#endregion


		#region Sprite Data Container Class

		[StructLayout(LayoutKind.Sequential, Pack = 1)]
		struct VertexPositionColorTexture4 : IVertexType
		{
			public const int RealStride = 96;

			VertexDeclaration IVertexType.VertexDeclaration => throw new NotImplementedException();

			public Vector3 Position0;
			public Color Color0;
			public Vector2 TextureCoordinate0;
			public Vector3 Position1;
			public Color Color1;
			public Vector2 TextureCoordinate1;
			public Vector3 Position2;
			public Color Color2;
			public Vector2 TextureCoordinate2;
			public Vector3 Position3;
			public Color Color3;
			public Vector2 TextureCoordinate3;
		}

		#endregion
	}
}