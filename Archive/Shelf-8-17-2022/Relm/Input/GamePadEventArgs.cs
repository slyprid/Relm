using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Relm.Input
{
    public class GamePadEventArgs
        : EventArgs
    {
        public PlayerIndex PlayerIndex { get; }
        public GamePadState CurrentState { get; }
        public GamePadState PreviousState { get; }
        public Buttons Button { get; }
        public TimeSpan ElapsedTime { get; }
        public float TriggerState { get; }
        public Vector2 ThumbStickState { get; }
        
        public GamePadEventArgs(GamePadState previousState, GamePadState currentState, TimeSpan elapsedTime, PlayerIndex playerIndex, Buttons? button = null, float triggerState = 0, Vector2? thumbStickState = null)
        {
            PlayerIndex = playerIndex;
            PreviousState = previousState;
            CurrentState = currentState;
            ElapsedTime = elapsedTime;
            if (button != null) Button = button.Value;
            TriggerState = triggerState;
            ThumbStickState = thumbStickState ?? Vector2.Zero;
        }
    }
}