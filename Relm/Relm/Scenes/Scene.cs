using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Relm.Graphics;

namespace Relm.Scenes
{
    public abstract class Scene
    {
        public abstract string Name { get; }
        public RelmGame Game { get; set; }

        public ContentManager Content => Game.Content;
        public SpriteBatch SpriteBatch => Game.SpriteBatch;
        public GraphicsDevice GraphicsDevice => Game.GraphicsDevice;
        public ViewportAdapter ViewportAdapter => Game.ViewportAdapter;
        public RelmGame Input => Game;

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);

        public virtual void OnSceneChange() { }
        public virtual void OnSceneEnter() { }
        public virtual void OnSceneExit() { }
    }
}