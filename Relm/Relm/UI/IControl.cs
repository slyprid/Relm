using MonoGame.Extended.Input;
using Relm.Entities;
using Relm.UI.Configuration;

namespace Relm.UI
{
    public interface IControl
        : IDrawableEntity
    {
        bool UseExternalInputStates { get; set; }
        KeyboardStateExtended KeyboardState { get; set; }
        MouseStateExtended MouseState { get; set; }

        void Configure<T>(T config) where T : IConfig;
    }
}