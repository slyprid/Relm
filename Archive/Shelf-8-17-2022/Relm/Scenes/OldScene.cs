using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Content;
using Relm.Graphics;
using Relm.Screens;

namespace Relm.Scenes
{
    public abstract class OldScene
    {
        private readonly List<string> _screensVisible = new List<string>();

        public abstract string Name { get; }
        public OldRelmGame Game { get; set; }
        public Dictionary<string, OldScreen> Screens { get; private set; }
        public ContentLibrary ContentLibrary { get; private set; }
        
        public ContentManager Content => Game.Content;
        public SpriteBatch SpriteBatch => Game.SpriteBatch;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public ViewportAdapter ViewportAdapter => Game.ViewportAdapter;
        public SpriteFont DefaultFont => Game.DefaultFont;
        public OldRelmGame Input => Game;

        protected OldScene()
        {
            Screens = new Dictionary<string, OldScreen>();
        }
        
        public virtual void OnSceneChange() { }
        public virtual void OnSceneEnter() { }
        public virtual void OnSceneExit() { }

        public virtual void OnSceneLoad()
        {
            ContentLibrary = new ContentLibrary(Content);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var screen in _screensVisible.Select(screenName => Screens[screenName]))
            {
                screen.Update(gameTime);
            }
        }

        public virtual void Draw(GameTime gameTime)
        {
            foreach (var screen in _screensVisible.Select(screenName => Screens[screenName]))
            {
                screen.Draw(gameTime);
            }
        }

        #region Screen Methods / Functions

        public OldScreen AddScreen<T>()
            where T : OldScreen
        {
            var screen = (OldScreen)Activator.CreateInstance<T>();
            screen.OldScene = this;
            screen.OnScreenLoad();
            Screens.Add(screen.Name, screen);
            return screen;
        }

        public void ClearScreens()
        {
            Screens.Clear();
            _screensVisible.Clear();
        }

        public void PushScreen(string screenName)
        {
            if (_screensVisible.Contains(screenName)) return;
            var screen = Screens[screenName];
            screen.OnScreenEnter();
            _screensVisible.Add(screenName);
            UnfocusScreens();
            FocusOnScreen(screenName);
        }

        public void PopScreen(string screenName)
        {
            var screen = Screens[screenName];
            screen.OnScreenExit();
            _screensVisible.Remove(screenName);
        }

        public void ChangeScreen(string targetScreenName)
        {
            var screenNamesVisible = new List<string>();
            _screensVisible.ForEach(x => screenNamesVisible.Add(x));
            foreach (var screenName in screenNamesVisible)
            {
                PopScreen(screenName);
            }

            PushScreen(targetScreenName);
        }

        public void UnfocusScreens()
        {
            foreach (var screen in _screensVisible.Select(screenName => Screens[screenName]))
            {
                screen.HasFocus = false;
            }
        }

        public void FocusOnScreen(string screenName)
        {
            var screen = Screens[screenName];
            screen.HasFocus = true;
        }

        #endregion
    }
}