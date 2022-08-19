using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;

namespace Relm.Extensions
{
    public static class TouchLocationExtensions
    {
        public static Vector2 ScaledPosition(this TouchLocation touchLocation)
        {
            return RelmInput.ScaledPosition(touchLocation.Position);
        }
    }
}