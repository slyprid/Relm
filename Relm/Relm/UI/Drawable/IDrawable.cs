using Microsoft.Xna.Framework;
using Relm.Graphics;

namespace Relm.UI.Drawable
{
    public interface IDrawable
    {
        float LeftWidth { get; set; }
        float RightWidth { get; set; }
        float TopHeight { get; set; }
        float BottomHeight { get; set; }
        float MinWidth { get; set; }
        float MinHeight { get; set; }


        void SetPadding(float top, float bottom, float left, float right);

        void Draw(SpriteBatch spriteBatch, float x, float y, float width, float height, Color color);
    }
}