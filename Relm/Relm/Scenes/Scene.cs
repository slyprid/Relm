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
    public abstract class Scene
    {
        private readonly List<string> _screensVisible = new List<string>();

        public abstract string Name { get; }
        public RelmGame Game { get; set; }
        public Dictionary<string, Screen> Screens { get; private set; }
        public ContentLibrary ContentLibrary { get; private set; }
        
        public ContentManager Content => Game.Content;
        public SpriteBatch SpriteBatch => Game.SpriteBatch;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public ViewportAdapter ViewportAdapter => Game.ViewportAdapter;
        public SpriteFont DefaultFont => Game.DefaultFont;
        public RelmGame Input => Game;

        protected Scene()
        {
            Screens = new Dictionary<string, Screen>();
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

        public Screen AddScreen<T>()
            where T : Screen
        {
            var screen = (Screen)Activator.CreateInstance<T>();
            screen.Scene = this;
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