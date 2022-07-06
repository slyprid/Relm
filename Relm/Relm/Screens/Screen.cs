using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Relm.Content;
using Relm.Entities;
using Relm.Graphics;
using Relm.Scenes;
using Relm.Sprites;
using Relm.UserInterface;

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
        public UserInterfaceSkin UserInterfaceSkin { get; set; }
        public List<Sprite> Controls => Entities.Values.OfType<IControl>().Cast<Sprite>().ToList<Sprite>();

        protected Screen()
        {
            Entities = new EntityCollection();
        }

        public virtual void OnScreenLoad() { }
        public virtual void OnScreenEnter() { }
        public virtual void OnScreenExit() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        #region Entity Functions / Methods

        public T AddEntity<T>(params object[] args)
            where T : Entity
        {
            return (T)Entities.Add<T>(args);
        }

        #endregion

        #region User Interface Functions / Methods

        public T AddControl<T>(params object[] args)
            where T : Sprite
        {
            var entityArgs = new object[args.Length + 1];
            entityArgs[0] = UserInterfaceSkin;
            for (var i = 0; i < args.Length; i++)
            {
                entityArgs[i + 1] = args[i];
            }
            var control = (T)Entities.Add<T>(entityArgs);
            return (T)control;
        }

        #endregion
    }
}