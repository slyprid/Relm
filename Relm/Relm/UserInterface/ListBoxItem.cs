using Microsoft.Xna.Framework;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class ListBoxItem
        : BaseControl
    {
        public int Index { get; set; }

        public ListBoxItem(TextureAtlas skin) 
            : base(skin)
        {
            Initialize();
        }

        protected virtual void Initialize() { }

        #region Fluent Functions

        public ListBoxItem WithSize(Vector2 size)
        {
            Width = (int)size.X;
            Height = (int)size.Y;
            Initialize();
            return this;
        }

        public ListBoxItem WithSize(int x, int y)
        {
            return WithSize(new Vector2(x, y));
        }

        public ListBoxItem WithSize(float x, float y)
        {
            return WithSize(new Vector2(x, y));
        }

        #endregion
    }
}