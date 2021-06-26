using Microsoft.Xna.Framework;
using MonoGame.Extended.Screens;

namespace Relm.Models
{
    public abstract class RelmGameScreen
        : GameScreen
    {
        public abstract string Name { get; }
        protected RelmGameScreen(Game game) 
            : base(game)
        {
        }
    }
}