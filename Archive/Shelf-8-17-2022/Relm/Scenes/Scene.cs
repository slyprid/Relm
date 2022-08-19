using System;
using Relm.Entities;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Components;
using Relm.Renderers.Renderables;

namespace Relm.Scenes
{
    public class Scene
    {
        #region Fields / Properties 

        private static Point _defaultDesignResolutionSize;
        private static Point _defaultDesignBleedSize;
        private static SceneResolutionPolicy _defaultSceneResolutionPolicy = SceneResolutionPolicy.None;

        private List<SceneComponent> _sceneComponents = new List<SceneComponent>();
        private List<Renderer> _renderers = new List<Renderer>();
        private RenderTarget2D _sceneRenderTarget;
        private RenderTarget2D _destinationRenderTarget;
        private SceneResolutionPolicy _resolutionPolicy;
        private Point _designResolutionSize;
        private Point _designBleedSize;
        private Rectangle _finalRenderDestinationRect;
        private Action<Texture2D> _screenshotRequestCallback;
        private bool _didSceneBegin;

        public EntityList Entities { get; }
        public RenderableComponentList RenderableComponents { get; }
        public SamplerState SamplerState { get; set; }
        public Camera Camera { get; set; }
        
        public Color ClearColor = Color.MediumSlateBlue;
        public Color LetterboxColor = Color.Black;
        public Point SceneRenderTargetSize => new Point(_sceneRenderTarget.Bounds.Width, _sceneRenderTarget.Bounds.Height);
        public RenderTarget2D SceneRenderTarget => _sceneRenderTarget;
        public int PixelPerfectScale = 1;

        #endregion

        #region Initialization

        public Scene()
        {
            Entities = new EntityList(this);
            RenderableComponents = new RenderableComponentList();
            SamplerState = new SamplerState { Filter = TextureFilter.Point };

            Camera = CreateEntity(nameof(Camera)).AddComponent(new Camera());

            _resolutionPolicy = _defaultSceneResolutionPolicy;
            _designResolutionSize = _defaultDesignResolutionSize;
            _designBleedSize = _defaultDesignBleedSize;

            Initialize();
        }

        #endregion
        
        #region Empty Virtuals

        public virtual void Initialize() { }
        public virtual void OnStart() { }
        public virtual void Unload() { }

        #endregion

        #region Statics

        public static void SetDefaultDesignResolution(int width, int height, SceneResolutionPolicy sceneResolutionPolicy = SceneResolutionPolicy.None, int horizontalBleed = 0, int verticalBleed = 0)
        {
            _defaultDesignResolutionSize = new Point(width, height);
            _defaultSceneResolutionPolicy = sceneResolutionPolicy;
            if (_defaultSceneResolutionPolicy == SceneResolutionPolicy.BestFit) _defaultDesignBleedSize = new Point(horizontalBleed, verticalBleed);
        }

        public static Scene CreateWithDefaultRenderer(Color? clearColor = null)
        {
            var scene = new Scene();
            if(clearColor.HasValue) scene.ClearColor = clearColor.Value;
            scene.AddRenderer(new DefaultRenderer());
            return scene;
        }

        #endregion

        #region Virtuals

        public virtual void Begin()
        {
            if (_renderers.Count == 0) AddRenderer(new DefaultRenderer());

            UpdateResolutionScaler();
            RelmGame.GraphicsDevice.SetRenderTarget(_sceneRenderTarget);

            _didSceneBegin = true;
            OnStart();
        }

        public virtual void End()
        {
            _didSceneBegin = false;

            foreach (var renderer in _renderers)
            {
                renderer.Unload();
            }
            Entities.RemoveAll();

            foreach (var component in _sceneComponents)
            {
                component.OnRemoveFromScene();
            }
            _sceneComponents.Clear();

            Camera = null;
            RelmGame.Content.Dispose();
        }

        #endregion
    }
}