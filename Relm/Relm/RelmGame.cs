using System;
using System.Collections;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Assets.BitmapFonts;
using Relm.Collections;
using Relm.Content;
using Relm.Core;
using Relm.Events;
using Relm.Graphics.Textures;
using Relm.Graphics.Transitions;
using Relm.Graphics.Tweening;
using Relm.Scenes;
using Relm.Systems;
using Relm.Timers;

namespace Relm
{
	public class RelmGame 
        : Game
	{
        internal static RelmGame _instance;
        internal static long drawCalls;

		internal SceneTransition _sceneTransition;

		private TimeSpan _frameCounterElapsedTime = TimeSpan.Zero;
        private int _frameCounter = 0;
        private string _windowTitle;
        private Scene _scene;
        private Scene _nextScene;
        private ITimer _graphicsDeviceChangeTimer;
        private FastList<GlobalManager> _globalManagers = new();
        private CoroutineManager _coroutineManager = new();
		private TimerManager _timerManager = new();

		public static Emitter<CoreEvents> Emitter;
        public static bool ExitOnEscapeKeypress = true;
        public static bool PauseOnFocusLost = true;
        public static bool DebugRenderEnabled = false;
        public new static GraphicsDevice GraphicsDevice;
        public new static RelmContentManager Content;

        public static SamplerState DefaultWrappedSamplerState => DefaultSamplerState.Filter == TextureFilter.Point ? SamplerState.PointWrap : SamplerState.LinearWrap;
        public static SamplerState DefaultSamplerState = new() { Filter = TextureFilter.Point };
        public new static GameServiceContainer Services => ((Game)_instance).Services;
        public static RelmGame Instance => _instance;
		
		public static Scene Scene
		{
			get => _instance._scene;
			set
			{
				Assert.IsNotNull(value, "Scene cannot be null!");

				if (_instance._scene == null)
				{
					_instance._scene = value;
					_instance.OnSceneChanged();
					_instance._scene.Begin();
				}
				else
				{
					_instance._nextScene = value;
				}
			}
		}
		
		public RelmGame(int width = 1280, int height = 720, bool isFullScreen = false, string windowTitle = "Relm", string contentDirectory = "Content")
		{
            #if DEBUG
			    _windowTitle = windowTitle;
            #endif

			_instance = this;
			Emitter = new Emitter<CoreEvents>(new CoreEventsComparer());

			var graphicsManager = new GraphicsDeviceManager(this)
			{
				PreferredBackBufferWidth = width,
				PreferredBackBufferHeight = height,
				IsFullScreen = isFullScreen,
				SynchronizeWithVerticalRetrace = true,
				PreferHalfPixelOffset = true
			};
			graphicsManager.DeviceReset += OnGraphicsDeviceReset;
			graphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;

			Screen.Initialize(graphicsManager);
			Window.ClientSizeChanged += OnGraphicsDeviceReset;
			Window.OrientationChanged += OnOrientationChanged;

			base.Content.RootDirectory = contentDirectory;
			Content = new RelmGlobalContentManager(Services, base.Content.RootDirectory);
			IsMouseVisible = true;
			IsFixedTimeStep = false;

			// setup systems
			RegisterGlobalManager(_coroutineManager);
			RegisterGlobalManager(new TweenManager());
			RegisterGlobalManager(_timerManager);
			RegisterGlobalManager(new RenderTarget());
		}

		void OnOrientationChanged(object sender, EventArgs e)
		{
			Emitter.Emit(CoreEvents.OrientationChanged);
		}

		protected void OnGraphicsDeviceReset(object sender, EventArgs e)
		{
			if (_graphicsDeviceChangeTimer != null)
			{
				_graphicsDeviceChangeTimer.Reset();
			}
			else
			{
				_graphicsDeviceChangeTimer = Schedule(0.05f, false, this, t =>
				{
					(t.Context as RelmGame)._graphicsDeviceChangeTimer = null;
					Emitter.Emit(CoreEvents.GraphicsDeviceReset);
				});
			}
		}


		#region Passthroughs to Game

		public new static void Exit()
		{
			((Game)_instance).Exit();
		}

		#endregion


		#region Game overides

		protected override void Initialize()
		{
			base.Initialize();

			GraphicsDevice = base.GraphicsDevice;
            var name = typeof(BitmapFont).AssemblyQualifiedName;
            var name2 = typeof(BitmapFontReader).AssemblyQualifiedName;
			var font = Content.Load<BitmapFont>("relm://Relm.Content.NezDefaultBMFont.xnb");
			RelmGraphics.Instance = new RelmGraphics(font);
		}

		protected override void Update(GameTime gameTime)
		{
			if (PauseOnFocusLost && !IsActive)
			{
				SuppressDraw();
				return;
			}

			Time.Update((float)gameTime.ElapsedGameTime.TotalSeconds);
			RelmInput.Update();

			if (ExitOnEscapeKeypress && (RelmInput.IsKeyDown(Keys.Escape) || RelmInput.GamePads[0].IsButtonReleased(Buttons.Back)))
			{
				base.Exit();
				return;
			}

			if (_scene != null)
			{
				for (var i = _globalManagers.Length - 1; i >= 0; i--)
				{
					if (_globalManagers.Buffer[i].Enabled)
						_globalManagers.Buffer[i].Update();
				}

				if (_sceneTransition == null ||
					(_sceneTransition != null &&
					 (!_sceneTransition._loadsNewScene || _sceneTransition._isNewSceneLoaded)))
				{
					_scene.Update();
				}

				if (_nextScene != null)
				{
					_scene.End();

					_scene = _nextScene;
					_nextScene = null;
					OnSceneChanged();

					_scene.Begin();
				}
			}

			EndDebugUpdate();
        }

		protected override void Draw(GameTime gameTime)
		{
			if (PauseOnFocusLost && !IsActive)
				return;

			StartDebugDraw(gameTime.ElapsedGameTime);

			if (_sceneTransition != null)
				_sceneTransition.PreRender(RelmGraphics.Instance.SpriteBatch);

			if (_sceneTransition != null)
			{
				if (_scene != null && _sceneTransition.WantsPreviousSceneRender &&
					!_sceneTransition.HasPreviousSceneRender)
				{
					_scene.Render();
					_scene.PostRender(_sceneTransition.PreviousSceneRender);
					StartCoroutine(_sceneTransition.OnBeginTransition());
				}
				else if (_scene != null && _sceneTransition._isNewSceneLoaded)
				{
					_scene.Render();
					_scene.PostRender();
				}

				_sceneTransition.Render(RelmGraphics.Instance.SpriteBatch);
			}
			else if (_scene != null)
			{
				_scene.Render();

#if DEBUG
				if (DebugRenderEnabled)
					Debug.Render();
#endif

				_scene.PostRender();
			}

			EndDebugDraw();
		}

		protected override void OnExiting(object sender, EventArgs args)
		{
			base.OnExiting(sender, args);
			Emitter.Emit(CoreEvents.Exiting);
		}

		#endregion

		#region Debug Injection

		[Conditional("DEBUG")]
		void EndDebugUpdate()
		{
            #if DEBUG
			    DebugConsole.Instance.Update();
			    drawCalls = 0;
            #endif
		}

		[Conditional("DEBUG")]
		void StartDebugDraw(TimeSpan elapsedGameTime)
		{
            #if DEBUG
			    // fps counter
			    _frameCounter++;
			    _frameCounterElapsedTime += elapsedGameTime;
			    if (_frameCounterElapsedTime >= TimeSpan.FromSeconds(1))
			    {
				    var totalMemory = (GC.GetTotalMemory(false) / 1048576f).ToString("F");
				    Window.Title = string.Format("{0} {1} fps - {2} MB", _windowTitle, _frameCounter, totalMemory);
				    _frameCounter = 0;
				    _frameCounterElapsedTime -= TimeSpan.FromSeconds(1);
			    }
            #endif
		}

		[Conditional("DEBUG")]
		void EndDebugDraw()
		{
            #if DEBUG
			    DebugConsole.Instance.Render();
            #endif
		}

		#endregion

		void OnSceneChanged()
		{
			Emitter.Emit(CoreEvents.SceneChanged);
			Time.SceneChanged();
			GC.Collect();
		}

		public static T StartSceneTransition<T>(T sceneTransition) where T : SceneTransition
		{
			Assert.IsNull(_instance._sceneTransition, "You cannot start a new SceneTransition until the previous one has completed");
			_instance._sceneTransition = sceneTransition;
			return sceneTransition;
		}


		#region Global Managers

		public static void RegisterGlobalManager(GlobalManager manager)
		{
			_instance._globalManagers.Add(manager);
			manager.Enabled = true;
		}

		public static void UnregisterGlobalManager(GlobalManager manager)
		{
			_instance._globalManagers.Remove(manager);
			manager.Enabled = false;
		}

		public static T GetGlobalManager<T>() where T : GlobalManager
		{
			for (var i = 0; i < _instance._globalManagers.Length; i++)
			{
				if (_instance._globalManagers.Buffer[i] is T)
					return _instance._globalManagers.Buffer[i] as T;
			}

			return null;
		}

		#endregion


		#region Systems access

		public static ICoroutine StartCoroutine(IEnumerator enumerator)
		{
			return _instance._coroutineManager.StartCoroutine(enumerator);
		}

		public static ITimer Schedule(float timeInSeconds, bool repeats, object context, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, repeats, context, onTime);
		}

		public static ITimer Schedule(float timeInSeconds, object context, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, false, context, onTime);
		}

		public static ITimer Schedule(float timeInSeconds, bool repeats, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, repeats, null, onTime);
		}

		public static ITimer Schedule(float timeInSeconds, Action<ITimer> onTime)
		{
			return _instance._timerManager.Schedule(timeInSeconds, false, null, onTime);
		}

		#endregion
	}
}