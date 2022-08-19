using Microsoft.Xna.Framework;

namespace Relm.Input
{
    public class GamePadSettings
    {
        public PlayerIndex PlayerIndex { get; set; }
        public int RepeatDelay { get; set; }
        public int RepeatInitialDelay { get; set; }
        public bool VibrationEnabled { get; set; }
        public float VibrationStrengthLeft { get; set; }
        public float VibrationStrengthRight { get; set; }
        public float TriggerDeltaThreshold { get; set; }
        public float ThumbstickDeltaThreshold { get; set; }
        public float TriggerDownThreshold { get; set; }
        public float ThumbstickDownThreshold { get; set; }
    }
}