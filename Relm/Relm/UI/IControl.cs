using MonoGame.Extended.Input;
using Relm.Entities;
using Relm.UI.Configuration;

namespace Relm.UI
{
    public interface IControl
        : IDrawableEntity
    {
        KeyboardStateExtended KeyboardState { get; set; }
        MouseStateExtended MouseState { get; set; }
        bool IsConfigured { get; }

        void Configure();
    }
}