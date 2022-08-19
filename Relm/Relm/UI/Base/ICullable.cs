using Microsoft.Xna.Framework;

namespace Relm.UI.Base
{
    public interface ICullable
    {
        void SetCullingArea(Rectangle cullingArea);
    }
}