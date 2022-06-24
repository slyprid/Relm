using System;

namespace Relm.Data
{
    public abstract class GameState
    {
        public string Name { get; set; }
        public DateTime Timestamp { get; set; }
    }
}