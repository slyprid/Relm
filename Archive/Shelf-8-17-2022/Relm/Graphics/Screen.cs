using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics
{
    public static class Screen
    {
        internal static GraphicsDeviceManager GraphicsDeviceManager;

        public static int Width
        {
            get => GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth;
            set => GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferWidth = value;
        }

        public static int Height
        {
            get => GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight;
            set => GraphicsDeviceManager.GraphicsDevice.PresentationParameters.BackBufferHeight = value;
        }

        public static int PreferredBackBufferWidth
        {
            get => GraphicsDeviceManager.PreferredBackBufferWidth;
            set => GraphicsDeviceManager.PreferredBackBufferWidth = value;
        }

        public static int PreferredBackBufferHeight
        {
            get => GraphicsDeviceManager.PreferredBackBufferHeight;
            set => GraphicsDeviceManager.PreferredBackBufferHeight = value;
        }

        public static bool IsFullscreen
        {
            get => GraphicsDeviceManager.IsFullScreen;
            set => GraphicsDeviceManager.IsFullScreen = value;
        }

        public static Vector2 Size => new Vector2(Width, Height);

        public static Vector2 Center => new Vector2(Width / 2f, Height / 2f);

        public static int MonitorWidth => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;

        public static int MonitorHeight => GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;

        public static void ApplyChanges() => GraphicsDeviceManager.ApplyChanges();

        internal static void Initialize(GraphicsDeviceManager graphicsManager) => GraphicsDeviceManager = graphicsManager;

        public static void Resize(int width, int height)
        {
            PreferredBackBufferWidth = width;
            PreferredBackBufferHeight = height;
            ApplyChanges();
        }
    }
}