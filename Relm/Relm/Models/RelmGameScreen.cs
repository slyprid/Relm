using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Relm.Models
{
    public abstract class RelmGameScreen
        : GameScreen
    {
        public abstract string Name { get; }
        public Microsoft.Xna.Framework.Graphics.SpriteBatch SpriteBatch => ((RelmGame)Game).SpriteBatch;

        protected RelmGameScreen(Game game) 
            : base(game)
        {
        }
    }
}