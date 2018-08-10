using Microsoft.Xna.Framework;

namespace Relm.Interfaces
{
    public interface IUpdate
    {
        bool IsEnabled { get; set; }

        void Update(GameTime gameTime);
    }
}