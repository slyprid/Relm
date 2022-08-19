namespace Relm.UI.Base
{
	public interface IGamepadFocusable
    {
        bool ShouldUseExplicitFocusableControl { get; set; }
        IGamepadFocusable GamepadUpElement { get; set; }
        IGamepadFocusable GamepadDownElement { get; set; }
        IGamepadFocusable GamepadLeftElement { get; set; }
        IGamepadFocusable GamepadRightElement { get; set; }


        /// <summary>
        /// enables shouldUseExplicitFocusableControl and sets the elements corresponding to each direction
        /// </summary>
        /// <param name="upEle">Up ele.</param>
        /// <param name="downEle">Down ele.</param>
        /// <param name="leftEle">Left ele.</param>
        /// <param name="rightEle">Right ele.</param>
        void EnableExplicitFocusableControl(IGamepadFocusable upEle, IGamepadFocusable downEle,
            IGamepadFocusable leftEle, IGamepadFocusable rightEle);

        /// <summary>
        /// called only when the following conditions are met:
        /// - shouldUseExplicitFocusableControl is true
        /// - this Element is focused
        /// - a gamepad direction was pressed with a null gamepadDIRECTIONElement
        /// </summary>
        /// <param name="direction">Direction.</param>
        void OnUnhandledDirectionPressed(Direction direction);

        /// <summary>
        /// called when gamepad focuses on the Element
        /// </summary>
        void OnFocused();

        /// <summary>
        /// called when gamepad focus is removed from the Element
        /// </summary>
        void OnUnfocused();

        /// <summary>
        /// called when the action button is pressed while the Element is focused
        /// </summary>
        void OnActionButtonPressed();

        /// <summary>
        /// called when the action button is released while the Element is focused
        /// </summary>
        void OnActionButtonReleased();
    }
}