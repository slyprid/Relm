using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Content;
using Relm.Entities;
using Relm.Graphics;
using Relm.Scenes;

namespace Relm.Screens
{
    public abstract class Screen
    {
        public abstract string Name { get; }
        
        public Scene Scene { get; set; }
        public ContentLibrary ContentLibrary => Scene.ContentLibrary;
        public SpriteBatch SpriteBatch => Scene.SpriteBatch;
        public ViewportAdapter ViewportAdapter => Scene.ViewportAdapter;
        public SpriteFont DefaultFont => Scene.DefaultFont;
        public RelmGame Input => Scene.Input;
        public bool HasFocus { get; set; }
        public EntityCollection Entities { get; set; }

        protected Screen()
        {
            Entities = new EntityCollection();
        }

        public virtual void OnScreenLoad() { }
        public virtual void OnScreenEnter() { }
        public virtual void OnScreenExit() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }
    }
}