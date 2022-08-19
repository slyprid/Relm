using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm
{
	public static class Screen
	{
		internal static GraphicsDeviceManager _graphicsManager;

		internal static void Initialize(GraphicsDeviceManager graphicsManager) => _graphicsManager = graphicsManager;

		public static int Width
		{
			get => _graphicsManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
			set => _graphicsManager.GraphicsDevice.PresentationParameters.BackBufferWidth = value;
		}

		public static int Height
		{
			get => _graphicsManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
			set => _graphicsManager.GraphicsDevice.PresentationParameters.BackBufferHeight = value;
		}

		public static Vector2 Size => new Vector2(Width, Height);

		public static Vector2 Center => new Vector2(Width / 2, Height / 2);

		public static int PreferredBackBufferWidth
		{
			get => _graphicsManager.PreferredBackBufferWidth;
			set => _graphicsManager.PreferredBackBufferWidth = value;
		}

		public static int PreferredBackBufferHeight
		{
			get => _graphicsManager.PreferredBackBufferHeight;
			set => _graphicsManager.PreferredBackBufferHeight = value;
		}

		public static int MonitorWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

		public static int MonitorHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

		public static SurfaceFormat BackBufferFormat =>
			_graphicsManager.GraphicsDevice.PresentationParameters.BackBufferFormat;

		public static SurfaceFormat PreferredBackBufferFormat
		{
			get => _graphicsManager.PreferredBackBufferFormat;
			set => _graphicsManager.PreferredBackBufferFormat = value;
		}

		public static bool SynchronizeWithVerticalRetrace
		{
			get => _graphicsManager.SynchronizeWithVerticalRetrace;
			set => _graphicsManager.SynchronizeWithVerticalRetrace = value;
		}

		public static DepthFormat PreferredDepthStencilFormat
		{
			get => _graphicsManager.PreferredDepthStencilFormat;
			set => _graphicsManager.PreferredDepthStencilFormat = value;
		}

		public static bool IsFullscreen
		{
			get => _graphicsManager.IsFullScreen;
			set => _graphicsManager.IsFullScreen = value;
		}

		public static DisplayOrientation SupportedOrientations
		{
			get => _graphicsManager.SupportedOrientations;
			set => _graphicsManager.SupportedOrientations = value;
		}

		public static void ApplyChanges() => _graphicsManager.ApplyChanges();

		public static void SetSize(int width, int height)
		{
			PreferredBackBufferWidth = width;
			PreferredBackBufferHeight = height;
			ApplyChanges();
		}
	}
}