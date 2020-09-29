using System;
using Microsoft.Xna.Framework;

namespace Relm.Components
{
    public class FrameCounter
    {
        public int FrameRate { get; set; }
        private int _frameCounter;
        private TimeSpan _elapsedTime;

        public FrameCounter()
        {
            _elapsedTime = TimeSpan.Zero;
        }

        public void Update(GameTime gameTime)
        {
            _elapsedTime += gameTime.ElapsedGameTime;

            if (_elapsedTime > TimeSpan.FromSeconds(1))
            {
                _elapsedTime -= TimeSpan.FromSeconds(1);
                FrameRate = _frameCounter;
                _frameCounter = 0;
            }
        }

        public void Draw(GameTime gameTime)
        {
            _frameCounter++;
        }
    }
}