using Microsoft.Xna.Framework;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class ListBoxItem
        : BaseControl
    {
        public int Index { get; set; }

        public ListBoxItem(UserInterfaceSkin skin) 
            : base(skin)
        {
            
        }

        #region Fluent Functions

        public virtual ListBoxItem WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            return this;
        }

        public virtual ListBoxItem WithSize(int x, int y)
        {
            return WithSize(new Vector2(x, y));
        }

        public virtual ListBoxItem WithSize(float x, float y)
        {
            return WithSize(new Vector2(x, y));
        }

        #endregion
    }
}