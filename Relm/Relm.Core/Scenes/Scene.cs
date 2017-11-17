using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Core.Input;
using Relm.Core.Managers;
using Relm.Core.States;

namespace Relm.Core.Scenes
{
    public abstract class Scene
        : IManaged<Scene>
    {
        public SpriteBatch SpriteBatch { get; set; }
        public ContentManager Content { get; set; }
        public bool IsLoaded { get; set; }
        public bool IsInitialized { get; set; }
        public InputManager Input { get; set; }

        public IManager<Scene> Manager { get; set; }
        public Stack<Screen> Screens { get; set; }

        public string MyAlias { get; set; }

        public bool IsEnabled { get; set; }
        public bool IsPaused { get; set; }
        public bool IsVisible { get; set; }
        public bool IsTransitioning { get; set; }

        protected Scene()
        {
            IsEnabled = true;
            IsPaused = false;
            IsVisible = true;

            Screens = new Stack<Screen>();

            Input = new InputManager();
        }

        public static string GenerateAlias()
        {
            return Guid.NewGuid().ToString();
        }

        public virtual void StartChange(Action onSuccess)
        {
            IsTransitioning = true;

            onSuccess?.Invoke();
        }

        public virtual void EndChange(Action onSuccess)
        {
            IsTransitioning = false;

            onSuccess?.Invoke();
        }

        public virtual void Pause()
        {
            IsPaused = !IsPaused;
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public virtual void Initialize()
        {
            IsInitialized = true;
        }
    }
}