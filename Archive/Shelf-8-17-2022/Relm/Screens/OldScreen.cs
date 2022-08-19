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
    public abstract class OldScreen
    {
        public abstract string Name { get; }
        
        public OldScene OldScene { get; set; }
        public ContentLibrary ContentLibrary => OldScene.ContentLibrary;
        public SpriteBatch SpriteBatch => OldScene.SpriteBatch;
        public ViewportAdapter ViewportAdapter => OldScene.ViewportAdapter;
        public SpriteFont DefaultFont => OldScene.DefaultFont;
        public OldRelmGame Input => OldScene.Input;
        public bool HasFocus { get; set; }
        public EntityCollection Entities { get; set; }
        public UserInterfaceSkin UserInterfaceSkin { get; set; }
        public List<OldSprite> Controls => Entities.Values.OfType<IControl>().Cast<OldSprite>().ToList<OldSprite>();

        protected OldScreen()
        {
            Entities = new EntityCollection();
        }

        public virtual void OnScreenLoad() { }
        public virtual void OnScreenEnter() { }
        public virtual void OnScreenExit() { }

        public virtual void Update(GameTime gameTime) { }
        public virtual void Draw(GameTime gameTime) { }

        #region OldEntity Functions / Methods

        public T AddEntity<T>(params object[] args)
            where T : OldEntity
        {
            return (T)Entities.Add<T>(args);
        }

        public void ClearControls()
        {
            Entities.Clear();
        }

        #endregion

        #region User Interface Functions / Methods

        public T AddControl<T>(params object[] args)
            where T : OldSprite
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