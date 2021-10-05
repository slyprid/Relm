using Microsoft.Xna.Framework;
using MonoGame.Extended.Input;
using MonoGame.Extended.Screens;
using Relm.Entities;

namespace Relm.UI
{
    public interface IControl
        : IDrawableEntity
    {
        KeyboardStateExtended KeyboardState { get; set; }
        MouseStateExtended MouseState { get; set; }
        Vector2 Scale { get; set; }
        GameScreen ParentScreen { get; set; }

        void Configure();

        #region Fluent Functions

        T As<T>() where T : IControl;
        T SetPosition<T>(int x, int y) where T : IControl;
        T SetPosition<T>(Vector2 position) where T : IControl;
        T SetScale<T>(float scale) where T : IControl;
        T SetScale<T>(float scaleX, float scaleY) where T : IControl;
        T SetSize<T>(int width, int height) where T : IControl;
        T Offset<T>(int x, int y) where T : IControl;
        T Offset<T>(Vector2 offset) where T : IControl;

        #endregion
    }
}