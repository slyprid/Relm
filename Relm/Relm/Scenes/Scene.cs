using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Collections;
using Relm.Components;
using Relm.Components.Renderables.Sprites;
using Relm.Content;
using Relm.Entities;
using Relm.Events;
using Relm.Extensions;
using Relm.Graphics.PostProcessing;
using Relm.Graphics.Renderers;
using Relm.Graphics.Textures;
using Relm.Math;

namespace Relm.Scenes
{
	public class Scene
	{
		#region Fields / Properties

		public Camera Camera;
        public Color ClearColor = Color.CornflowerBlue;
        public Color LetterboxColor = Color.Black;
        public SamplerState SamplerState = RelmGame.DefaultSamplerState;
        public readonly RelmContentManager Content;
        public bool EnablePostProcessing = true;
        public readonly EntityList Entities;
        public readonly RenderableComponentList RenderableComponents;
        public Point SceneRenderTargetSize => new Point(_sceneRenderTarget.Bounds.Width, _sceneRenderTarget.Bounds.Height);
        public RenderTarget2D SceneRenderTarget => _sceneRenderTarget;
        public int PixelPerfectScale = 1;

		public IFinalRenderDelegate FinalRenderDelegate
		{
			set
			{
				_finalRenderDelegate?.Unload();
				_finalRenderDelegate = value;
				_finalRenderDelegate?.OnAddedToScene(this);
			}
			get => _finalRenderDelegate;
		}
        IFinalRenderDelegate _finalRenderDelegate;
		
		#region SceneResolutionPolicy private fields

		static Point _defaultDesignResolutionSize;

		static Point _defaultDesignBleedSize;

		static SceneResolutionPolicy _defaultSceneResolutionPolicy = SceneResolutionPolicy.None;

		SceneResolutionPolicy _resolutionPolicy;

		Point _designResolutionSize;

		Point _designBleedSize;

		Rectangle _finalRenderDestinationRect;

		#endregion
		
		RenderTarget2D _sceneRenderTarget;
		RenderTarget2D _destinationRenderTarget;
		Action<Texture2D> _screenshotRequestCallback;

		internal readonly FastList<SceneComponent> _sceneComponents = new FastList<SceneComponent>();
		internal readonly FastList<Renderer> _renderers = new FastList<Renderer>();
		internal readonly FastList<Renderer> _afterPostProcessorRenderers = new FastList<Renderer>();
		internal readonly FastList<PostProcessor> _postProcessors = new FastList<PostProcessor>();
		bool _didSceneBegin;

		#endregion

		public static void SetDefaultDesignResolution(int width, int height, SceneResolutionPolicy sceneResolutionPolicy, int horizontalBleed = 0, int verticalBleed = 0)
		{
			_defaultDesignResolutionSize = new Point(width, height);
			_defaultSceneResolutionPolicy = sceneResolutionPolicy;
			if (_defaultSceneResolutionPolicy == SceneResolutionPolicy.BestFit)
				_defaultDesignBleedSize = new Point(horizontalBleed, verticalBleed);
		}
		
		#region Scene creation helpers

		public static Scene CreateWithDefaultRenderer(Color? clearColor = null)
		{
			var scene = new Scene();

			if (clearColor.HasValue)
				scene.ClearColor = clearColor.Value;
			scene.AddRenderer(new DefaultRenderer());
			return scene;
		}

        #endregion


		public Scene()
		{
			Entities = new EntityList(this);
			RenderableComponents = new RenderableComponentList();
			Content = new RelmContentManager();

			var cameraEntity = CreateEntity("camera");
			Camera = cameraEntity.AddComponent(new Camera());

			_resolutionPolicy = _defaultSceneResolutionPolicy;
			_designResolutionSize = _defaultDesignResolutionSize;
			_designBleedSize = _defaultDesignBleedSize;

			Initialize();
		}


		#region Scene lifecycle

		public virtual void Initialize() { }
        public virtual void OnStart() { }
        public virtual void Unload() { }

		public virtual void Begin()
		{
			if (_renderers.Length == 0)
			{
				AddRenderer(new DefaultRenderer());
				Debug.Warn("Scene has begun with no renderer. A DefaultRenderer was added automatically so that something is visible.");
			}

			RelmPhysics.Reset();

			UpdateResolutionScaler();
			RelmGame.GraphicsDevice.SetRenderTarget(_sceneRenderTarget);
			RelmGame.Emitter.AddObserver(CoreEvents.GraphicsDeviceReset, OnGraphicsDeviceReset);
			RelmGame.Emitter.AddObserver(CoreEvents.OrientationChanged, OnOrientationChanged);

			_didSceneBegin = true;
			OnStart();
		}

		public virtual void End()
		{
			_didSceneBegin = false;

			for (var i = 0; i < _renderers.Length; i++)
				_renderers.Buffer[i].Unload();

			for (var i = 0; i < _postProcessors.Length; i++)
				_postProcessors.Buffer[i].Unload();

			RelmGame.Emitter.RemoveObserver(CoreEvents.GraphicsDeviceReset, OnGraphicsDeviceReset);
			Entities.RemoveAllEntities();

			for (var i = 0; i < _sceneComponents.Length; i++)
				_sceneComponents.Buffer[i].OnRemovedFromScene();
			_sceneComponents.Clear();

			Camera = null;
			Content.Dispose();
			_sceneRenderTarget.Dispose();
			RelmPhysics.Clear();

			if (_destinationRenderTarget != null)
				_destinationRenderTarget.Dispose();

			Unload();
		}

		public virtual void Update()
		{
			RelmGame.GraphicsDevice.SetRenderTarget(_sceneRenderTarget);

			Entities.UpdateLists();

			for (var i = _sceneComponents.Length - 1; i >= 0; i--)
			{
				if (_sceneComponents.Buffer[i].Enabled)
					_sceneComponents.Buffer[i].Update();
			}

			Entities.Update();

			RenderableComponents.UpdateLists();
		}

		internal void Render()
		{
			if (_renderers.Length == 0)
			{
				Debug.Error("There are no Renderers in the Scene!");
				return;
			}

			if (_renderers[0].WantsToRenderToSceneRenderTarget)
			{
				RelmGame.GraphicsDevice.SetRenderTarget(_sceneRenderTarget);
				RelmGame.GraphicsDevice.Clear(ClearColor);
			}


			var lastRendererHadRenderTarget = false;
			for (var i = 0; i < _renderers.Length; i++)
			{
				if (lastRendererHadRenderTarget && _renderers.Buffer[i].WantsToRenderToSceneRenderTarget)
				{
					RelmGame.GraphicsDevice.SetRenderTarget(_sceneRenderTarget);
					RelmGame.GraphicsDevice.Clear(ClearColor);

					if (_renderers.Buffer[i].Camera != null)
						_renderers.Buffer[i].Camera.ForceMatrixUpdate();
					Camera.ForceMatrixUpdate();
				}

				_renderers.Buffer[i].Render(this);
				lastRendererHadRenderTarget = _renderers.Buffer[i].RenderTexture != null;
			}
		}

		internal void PostRender(RenderTarget2D finalRenderTarget = null)
		{
			var enabledCounter = 0;
			if (EnablePostProcessing)
			{
				for (var i = 0; i < _postProcessors.Length; i++)
				{
					if (_postProcessors.Buffer[i].Enabled)
					{
						var isEven = Mathf.IsEven(enabledCounter);
						enabledCounter++;

						var source = isEven ? _sceneRenderTarget : _destinationRenderTarget;
						var destination = !isEven ? _sceneRenderTarget : _destinationRenderTarget;
						_postProcessors.Buffer[i].Process(source, destination);
					}
				}
			}

			for (var i = 0; i < _afterPostProcessorRenderers.Length; i++)
			{
				if (i == 0)
				{
					var currentRenderTarget = Mathf.IsEven(enabledCounter) ? _sceneRenderTarget : _destinationRenderTarget;
					RelmGame.GraphicsDevice.SetRenderTarget(currentRenderTarget);
				}

				if (_afterPostProcessorRenderers.Buffer[i].Camera != null)
					_afterPostProcessorRenderers.Buffer[i].Camera.ForceMatrixUpdate();
				_afterPostProcessorRenderers.Buffer[i].Render(this);
			}

			if (_screenshotRequestCallback != null)
			{
				var tex = new Texture2D(RelmGame.GraphicsDevice, _sceneRenderTarget.Width, _sceneRenderTarget.Height);
				var data = new int[tex.Bounds.Width * tex.Bounds.Height];

				var currentRenderTarget = Mathf.IsEven(enabledCounter) ? _sceneRenderTarget : _destinationRenderTarget;
				currentRenderTarget.GetData(data);
				tex.SetData(data);
				_screenshotRequestCallback(tex);

				_screenshotRequestCallback = null;
			}

			if (_finalRenderDelegate != null)
			{
				var currentRenderTarget = Mathf.IsEven(enabledCounter) ? _sceneRenderTarget : _destinationRenderTarget;
				_finalRenderDelegate.HandleFinalRender(finalRenderTarget, LetterboxColor, currentRenderTarget, _finalRenderDestinationRect, SamplerState);
			}
			else
			{
				var currentRenderTarget = Mathf.IsEven(enabledCounter) ? _sceneRenderTarget : _destinationRenderTarget;
				RelmGame.GraphicsDevice.SetRenderTarget(finalRenderTarget);
				RelmGame.GraphicsDevice.Clear(LetterboxColor);

				RelmGraphics.Instance.SpriteBatch.Begin(BlendState.Opaque, SamplerState, null, null);
				RelmGraphics.Instance.SpriteBatch.Draw(currentRenderTarget, _finalRenderDestinationRect, Color.White);
				RelmGraphics.Instance.SpriteBatch.End();
			}
		}

		void OnGraphicsDeviceReset() => UpdateResolutionScaler();
		void OnOrientationChanged() => UpdateResolutionScaler();

		#endregion


		#region Resolution Policy

		public void SetDesignResolution(int width, int height, SceneResolutionPolicy sceneResolutionPolicy,
			int horizontalBleed = 0, int verticalBleed = 0)
		{
			_designResolutionSize = new Point(width, height);
			_resolutionPolicy = sceneResolutionPolicy;
			if (_resolutionPolicy == SceneResolutionPolicy.BestFit)
				_designBleedSize = new Point(horizontalBleed, verticalBleed);
			UpdateResolutionScaler();
		}

		void UpdateResolutionScaler()
		{
			var designSize = _designResolutionSize;
			var screenSize = new Point(Screen.Width, Screen.Height);
			var screenAspectRatio = (float)screenSize.X / (float)screenSize.Y;

			var renderTargetWidth = screenSize.X;
			var renderTargetHeight = screenSize.Y;

			var resolutionScaleX = (float)screenSize.X / (float)designSize.X;
			var resolutionScaleY = (float)screenSize.Y / (float)designSize.Y;

			var rectCalculated = false;

			PixelPerfectScale = 1;
			if (_resolutionPolicy != SceneResolutionPolicy.None)
			{
				if ((float)designSize.X / (float)designSize.Y > screenAspectRatio)
					PixelPerfectScale = screenSize.X / designSize.X;
				else
					PixelPerfectScale = screenSize.Y / designSize.Y;

				if (PixelPerfectScale == 0)
					PixelPerfectScale = 1;
			}

			switch (_resolutionPolicy)
			{
				case SceneResolutionPolicy.None:
					_finalRenderDestinationRect.X = _finalRenderDestinationRect.Y = 0;
					_finalRenderDestinationRect.Width = screenSize.X;
					_finalRenderDestinationRect.Height = screenSize.Y;
					rectCalculated = true;
					break;
				case SceneResolutionPolicy.ExactFit:
					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;
					break;
				case SceneResolutionPolicy.NoBorder:
					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;

					resolutionScaleX = resolutionScaleY = System.Math.Max(resolutionScaleX, resolutionScaleY);
					break;
				case SceneResolutionPolicy.NoBorderPixelPerfect:
					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;

					PixelPerfectScale = 1;
					if ((float)designSize.X / (float)designSize.Y < screenAspectRatio)
					{
						var floatScale = (float)screenSize.X / (float)designSize.X;
						PixelPerfectScale = Mathf.CeilToInt(floatScale);
					}
					else
					{
						var floatScale = (float)screenSize.Y / (float)designSize.Y;
						PixelPerfectScale = Mathf.CeilToInt(floatScale);
					}

					if (PixelPerfectScale == 0)
						PixelPerfectScale = 1;

					_finalRenderDestinationRect.Width = Mathf.CeilToInt(designSize.X * PixelPerfectScale);
					_finalRenderDestinationRect.Height = Mathf.CeilToInt(designSize.Y * PixelPerfectScale);
					_finalRenderDestinationRect.X = (screenSize.X - _finalRenderDestinationRect.Width) / 2;
					_finalRenderDestinationRect.Y = (screenSize.Y - _finalRenderDestinationRect.Height) / 2;
					rectCalculated = true;

					break;
				case SceneResolutionPolicy.ShowAll:
					resolutionScaleX = resolutionScaleY = System.Math.Min(resolutionScaleX, resolutionScaleY);

					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;
					break;
				case SceneResolutionPolicy.ShowAllPixelPerfect:
					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;

					_finalRenderDestinationRect.Width = Mathf.CeilToInt(designSize.X * PixelPerfectScale);
					_finalRenderDestinationRect.Height = Mathf.CeilToInt(designSize.Y * PixelPerfectScale);
					_finalRenderDestinationRect.X = (screenSize.X - _finalRenderDestinationRect.Width) / 2;
					_finalRenderDestinationRect.Y = (screenSize.Y - _finalRenderDestinationRect.Height) / 2;
					rectCalculated = true;

					break;
				case SceneResolutionPolicy.FixedHeight:
					resolutionScaleX = resolutionScaleY;
					designSize.X = Mathf.CeilToInt(screenSize.X / resolutionScaleX);

					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;
					break;
				case SceneResolutionPolicy.FixedHeightPixelPerfect:
					renderTargetHeight = designSize.Y;

					_finalRenderDestinationRect.Width = Mathf.CeilToInt(designSize.X * resolutionScaleX);
					_finalRenderDestinationRect.Height = Mathf.CeilToInt(designSize.Y * PixelPerfectScale);
					_finalRenderDestinationRect.X = (screenSize.X - _finalRenderDestinationRect.Width) / 2;
					_finalRenderDestinationRect.Y = (screenSize.Y - _finalRenderDestinationRect.Height) / 2;
					rectCalculated = true;

					renderTargetWidth = (int)(designSize.X * resolutionScaleX / PixelPerfectScale);
					break;
				case SceneResolutionPolicy.FixedWidth:
					resolutionScaleY = resolutionScaleX;
					designSize.Y = Mathf.CeilToInt(screenSize.Y / resolutionScaleY);

					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;
					break;
				case SceneResolutionPolicy.FixedWidthPixelPerfect:
					renderTargetWidth = designSize.X;

					_finalRenderDestinationRect.Width = Mathf.CeilToInt(designSize.X * PixelPerfectScale);
					_finalRenderDestinationRect.Height = Mathf.CeilToInt(designSize.Y * resolutionScaleY);
					_finalRenderDestinationRect.X = (screenSize.X - _finalRenderDestinationRect.Width) / 2;
					_finalRenderDestinationRect.Y = (screenSize.Y - _finalRenderDestinationRect.Height) / 2;
					rectCalculated = true;

					renderTargetHeight = (int)(designSize.Y * resolutionScaleY / PixelPerfectScale);

					break;
				case SceneResolutionPolicy.BestFit:
					var safeScaleX = (float)screenSize.X / (designSize.X - _designBleedSize.X);
					var safeScaleY = (float)screenSize.Y / (designSize.Y - _designBleedSize.Y);

					var resolutionScale = MathHelper.Max(resolutionScaleX, resolutionScaleY);
					var safeScale = MathHelper.Min(safeScaleX, safeScaleY);

					resolutionScaleX = resolutionScaleY = MathHelper.Min(resolutionScale, safeScale);

					renderTargetWidth = designSize.X;
					renderTargetHeight = designSize.Y;

					break;
			}

			if (!rectCalculated)
			{
				var renderWidth = designSize.X * resolutionScaleX;
				var renderHeight = designSize.Y * resolutionScaleY;

				_finalRenderDestinationRect = RectangleExtensions.FromFloats((screenSize.X - renderWidth) / 2, (screenSize.Y - renderHeight) / 2, renderWidth, renderHeight);
			}


			var scaleX = renderTargetWidth / (float)_finalRenderDestinationRect.Width;
			var scaleY = renderTargetHeight / (float)_finalRenderDestinationRect.Height;

			RelmInput._resolutionScale = new Vector2(scaleX, scaleY);
			RelmInput._resolutionOffset = _finalRenderDestinationRect.Location;

			if (_sceneRenderTarget != null)
				_sceneRenderTarget.Dispose();
			_sceneRenderTarget = RenderTarget.Create(renderTargetWidth, renderTargetHeight);

			if (_destinationRenderTarget != null)
			{
				_destinationRenderTarget.Dispose();
				_destinationRenderTarget = RenderTarget.Create(renderTargetWidth, renderTargetHeight);
			}

			for (var i = 0; i < _renderers.Length; i++)
				_renderers.Buffer[i].OnSceneBackBufferSizeChanged(renderTargetWidth, renderTargetHeight);

			for (var i = 0; i < _afterPostProcessorRenderers.Length; i++)
				_afterPostProcessorRenderers.Buffer[i]
					.OnSceneBackBufferSizeChanged(renderTargetWidth, renderTargetHeight);

			for (var i = 0; i < _postProcessors.Length; i++)
				_postProcessors.Buffer[i].OnSceneBackBufferSizeChanged(renderTargetWidth, renderTargetHeight);

			if (_finalRenderDelegate != null)
				_finalRenderDelegate.OnSceneBackBufferSizeChanged(renderTargetWidth, renderTargetHeight);

			Camera.OnSceneRenderTargetSizeChanged(renderTargetWidth, renderTargetHeight);
		}

		#endregion


		#region Utils

		public void RequestScreenshot(Action<Texture2D> callback) => _screenshotRequestCallback = callback;

		#endregion


		#region SceneComponent Management

		public T AddSceneComponent<T>() where T : SceneComponent, new() => AddSceneComponent(new T());

		public T AddSceneComponent<T>(T component) where T : SceneComponent
		{
			component.Scene = this;
			component.OnEnabled();
			_sceneComponents.Add(component);
			_sceneComponents.Sort();
			return component;
		}

		public T GetSceneComponent<T>() where T : SceneComponent
		{
			for (var i = 0; i < _sceneComponents.Length; i++)
			{
				var component = _sceneComponents.Buffer[i];
				if (component is T)
					return component as T;
			}

			return null;
		}

		public T GetOrCreateSceneComponent<T>() where T : SceneComponent, new()
		{
			var comp = GetSceneComponent<T>();
			if (comp == null)
				comp = AddSceneComponent<T>();

			return comp;
		}

		public bool RemoveSceneComponent<T>() where T : SceneComponent
		{
			var comp = GetSceneComponent<T>();
			if (comp != null)
			{
				RemoveSceneComponent(comp);
				return true;
			}

			return false;
		}

		public void RemoveSceneComponent(SceneComponent component)
		{
			Assert.IsTrue(_sceneComponents.Contains(component), "SceneComponent {0} is not in the SceneComponents list!", component);
			_sceneComponents.Remove(component);
			component.OnRemovedFromScene();
		}

		#endregion


		#region Renderer/PostProcessor Management

		public T AddRenderer<T>(T renderer) where T : Renderer
		{
			if (renderer.WantsToRenderAfterPostProcessors)
			{
				_afterPostProcessorRenderers.Add(renderer);
				_afterPostProcessorRenderers.Sort();
			}
			else
			{
				_renderers.Add(renderer);
				_renderers.Sort();
			}

			renderer.OnAddedToScene(this);

			if (_didSceneBegin)
				renderer.OnSceneBackBufferSizeChanged(_sceneRenderTarget.Width, _sceneRenderTarget.Height);

			return renderer;
		}

		public T GetRenderer<T>() where T : Renderer
		{
			for (var i = 0; i < _renderers.Length; i++)
			{
				if (_renderers.Buffer[i] is T)
					return _renderers[i] as T;
			}

			for (var i = 0; i < _afterPostProcessorRenderers.Length; i++)
			{
				if (_afterPostProcessorRenderers.Buffer[i] is T)
					return _afterPostProcessorRenderers.Buffer[i] as T;
			}

			return null;
		}

		public void RemoveRenderer(Renderer renderer)
		{
			Assert.IsTrue(_renderers.Contains(renderer) || _afterPostProcessorRenderers.Contains(renderer));

			if (renderer.WantsToRenderAfterPostProcessors)
				_afterPostProcessorRenderers.Remove(renderer);
			else
				_renderers.Remove(renderer);
			renderer.Unload();
		}

		public T AddPostProcessor<T>(T postProcessor) where T : PostProcessor
		{
			_postProcessors.Add(postProcessor);
			_postProcessors.Sort();
			postProcessor.OnAddedToScene(this);

			if (_didSceneBegin)
				postProcessor.OnSceneBackBufferSizeChanged(_sceneRenderTarget.Width, _sceneRenderTarget.Height);

			if (_destinationRenderTarget == null)
			{
				if (_sceneRenderTarget != null)
					_destinationRenderTarget = RenderTarget.Create(_sceneRenderTarget.Width, _sceneRenderTarget.Height);
				else
					_destinationRenderTarget = RenderTarget.Create();
			}

			return postProcessor;
		}

		public T GetPostProcessor<T>() where T : PostProcessor
		{
			for (var i = 0; i < _postProcessors.Length; i++)
			{
				if (_postProcessors.Buffer[i] is T)
					return _postProcessors[i] as T;
			}

			return null;
		}

		public void RemovePostProcessor(PostProcessor postProcessor)
		{
			Assert.IsTrue(_postProcessors.Contains(postProcessor));

			_postProcessors.Remove(postProcessor);
			postProcessor.Unload();
		}

		#endregion


		#region Entity Management

		public Entity CreateEntity(string name)
		{
			var entity = new Entity(name);
			return AddEntity(entity);
		}

		public Entity CreateEntity(string name, Vector2 position)
		{
			var entity = new Entity(name);
			entity.Transform.Position = position;
			return AddEntity(entity);
		}

		public Entity AddEntity(Entity entity)
		{
			Assert.IsFalse(Entities.Contains(entity), "You are attempting to add the same entity to a scene twice: {0}", entity);
			Entities.Add(entity);
			entity.Scene = this;

			for (var i = 0; i < entity.Transform.ChildCount; i++)
				AddEntity(entity.Transform.GetChild(i).Entity);

			return entity;
		}

		public T AddEntity<T>(T entity) where T : Entity
		{
			Assert.IsFalse(Entities.Contains(entity), "You are attempting to add the same entity to a scene twice: {0}", entity);
			Entities.Add(entity);
			entity.Scene = this;
			return entity;
		}

		public void DestroyAllEntities()
		{
			for (var i = 0; i < Entities.Count; i++)
				Entities[i].Destroy();
		}

		public Entity FindEntity(string name) => Entities.FindEntity(name);

		public List<Entity> FindEntitiesWithTag(int tag) => Entities.EntitiesWithTag(tag);

		public List<T> EntitiesOfType<T>() where T : Entity => Entities.EntitiesOfType<T>();

		public T FindComponentOfType<T>() where T : Component => Entities.FindComponentOfType<T>();

		public List<T> FindComponentsOfType<T>() where T : Component => Entities.FindComponentsOfType<T>();

		#endregion

		#region Ease of Use

        public Entity CreateSpriteEntity(string entityName, string texturePath)
        {
            var texture = Content.LoadTexture(texturePath);
            var entity = CreateEntity(entityName);
            entity.AddComponent(new SpriteRenderer(texture));
            return entity;
        }

        public Entity CreateSpriteEntity(string entityName, string texturePath, Vector2 origin)
        {
            var texture = Content.LoadTexture(texturePath);
            var entity = CreateEntity(entityName);
            var renderer = entity.AddComponent(new SpriteRenderer(texture));
            renderer.SetOrigin(origin);
            return entity;
        }

        #endregion
    }
}