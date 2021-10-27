using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace Relm.Components
{
    public class FpsComponent
        : SimpleDrawableGameComponent
    {
        private double _frames = 0;
        private double _updates = 0;
        private double _elapsed = 0;
        private double _last = 0;
        private double _now = 0;
        private double _msgFrequency = 1.0f;
        private string _msg = "";

        public RelmGame Game { get; set; }
        public SpriteFont Font { get; set; }
        public Vector2 Position { get; set; }
        public Color Color { get; set; }

        public FpsComponent()
        {
            Color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            _now = gameTime.TotalGameTime.TotalSeconds;
            _elapsed = (double)(_now - _last);
            if (_elapsed > _msgFrequency)
            {
                _msg = $" Fps: {(int)(_frames / _elapsed) + 1}\n Elapsed time: {(int)_elapsed}\n Updates: {_updates}\n Frames: {_frames}";
                _elapsed = 0;
                _frames = 0;
                _updates = 0;
                _last = _now;
            }
            _updates++;
        }

        public override void Draw(GameTime gameTime)
        {
            if (Font != null)
            {
                Game.SpriteBatch.Begin();
                Game.SpriteBatch.DrawString(Font, _msg, Position, Color);
                Game.SpriteBatch.End();
            }
            _frames++;
        }
    }
}