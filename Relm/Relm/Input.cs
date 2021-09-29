using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;

namespace Relm
{
    public static class Input
    {
        private static TouchListener _touchListener;
        private static GamePadListener _gamePadListener;
        private static KeyboardListener _keyboardListener;
        private static MouseListener _mouseListener;

        private static Dictionary<string, EventHandler<KeyboardEventArgs>> _keyboardEvents;
        private static Dictionary<string, EventHandler<TouchEventArgs>> _touchEvents;
        private static Dictionary<string, EventHandler<GamePadEventArgs>> _gamePadEvents;
        private static Dictionary<string, EventHandler<MouseEventArgs>> _mouseEvents;
        
        #region Initialization

        static Input()
        {
            _keyboardEvents = new Dictionary<string, EventHandler<KeyboardEventArgs>>();
            _touchEvents = new Dictionary<string, EventHandler<TouchEventArgs>>();
            _gamePadEvents = new Dictionary<string, EventHandler<GamePadEventArgs>>();
            _mouseEvents = new Dictionary<string, EventHandler<MouseEventArgs>>();
        }
        
        public static void Register(Game game, GameComponentCollection components)
        {
            _keyboardListener = new KeyboardListener();
            _gamePadListener = new GamePadListener();
            _mouseListener = new MouseListener();
            _touchListener = new TouchListener();

            components.Add(new InputListenerComponent(game, _keyboardListener, _gamePadListener, _mouseListener, _touchListener));
        }

        #endregion

        #region Keyboard

        public static KeyboardStateExtended GetKeyboardState()
        {
            return KeyboardExtended.GetState();
        }

        public static void ClearKeyboardEvents()
        {
            foreach (var kvp in _keyboardEvents)
            {
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyPressed))) _keyboardListener.KeyPressed -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyReleased))) _keyboardListener.KeyReleased -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyTyped))) _keyboardListener.KeyTyped -= kvp.Value;
            }

            _keyboardEvents.Clear();
        }

        private static void KeyPressed(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyPressed += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyPressed)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyReleased(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyReleased += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyReleased)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyTyped(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyTyped += eventHandler;
            _keyboardEvents.Add($"{nameof(_keyboardListener.KeyTyped)}_{Guid.NewGuid()}", eventHandler);
        }

        public static void OnKeyPressed(Keys key, Action<object, KeyboardEventArgs> action)
        {
            KeyPressed((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyReleased(Keys key, Action<object, KeyboardEventArgs> action)
        {
            KeyReleased((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyTyped(Keys key, Action<object, KeyboardEventArgs> action)
        {
            KeyTyped((sender, args) =>
            {
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Gamepad

        public static void ClearGamepadEvents()
        {
            foreach (var kvp in _gamePadEvents)
            {
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonUp))) _gamePadListener.ButtonUp -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonDown))) _gamePadListener.ButtonDown -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonRepeated))) _gamePadListener.ButtonRepeated -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ThumbStickMoved))) _gamePadListener.ThumbStickMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.TriggerMoved))) _gamePadListener.TriggerMoved -= kvp.Value;
            }

            _gamePadEvents.Clear();
        }

        private static void ButtonDown(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonDown += eventHandler;
            _gamePadEvents.Add($"{nameof(_gamePadListener.ButtonDown)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ButtonRepeated(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonRepeated += eventHandler;
            _gamePadEvents.Add($"{nameof(_gamePadListener.ButtonRepeated)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ButtonUp(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonUp += eventHandler;
            _gamePadEvents.Add($"{nameof(_gamePadListener.ButtonUp)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ThumbStickMoved(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ThumbStickMoved += eventHandler;
            _gamePadEvents.Add($"{nameof(_gamePadListener.ThumbStickMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TriggerMoved(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.TriggerMoved += eventHandler;
            _gamePadEvents.Add($"{nameof(_gamePadListener.TriggerMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        public static void OnButtonDown(Buttons button, Action<object, GamePadEventArgs> action)
        {
            ButtonDown((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnButtonRepeated(Buttons button, Action<object, GamePadEventArgs> action)
        {
            ButtonRepeated((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnButtonUp(Buttons button, Action<object, GamePadEventArgs> action)
        {
            ButtonUp((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnThumbStickMoved(Action<object, GamePadEventArgs> action)
        {
            ThumbStickMoved((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnTriggerMoved(Action<object, GamePadEventArgs> action)
        {
            TriggerMoved((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Mouse

        public static MouseStateExtended GetMouseState()
        {
            return MouseExtended.GetState();
        }

        public static void ClearMouseEvents()
        {
            foreach (var kvp in _mouseEvents)
            {
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseClicked))) _mouseListener.MouseClicked -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDoubleClicked))) _mouseListener.MouseDoubleClicked -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDown))) _mouseListener.MouseDown -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDrag))) _mouseListener.MouseDrag -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDragEnd))) _mouseListener.MouseDragEnd -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseDragStart))) _mouseListener.MouseDragStart -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseMoved))) _mouseListener.MouseMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseUp))) _mouseListener.MouseUp -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_mouseListener.MouseWheelMoved))) _mouseListener.MouseWheelMoved -= kvp.Value;
            }

            _mouseEvents.Clear();
        }

        private static void MouseClicked(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseClicked += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDoubleClicked(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDoubleClicked += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDoubleClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDown(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDown += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDown)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDrag(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDrag += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDrag)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragEnd(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragEnd += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDragEnd)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragStart(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragStart += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseDragStart)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseMoved(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseMoved += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseUp(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseUp += eventHandler;
            _mouseEvents.Add($"{nameof(_mouseListener.MouseUp)}_{Guid.NewGuid()}", eventHandler);
        }


        public static void OnMouseClicked(MouseButton button, Action<object, MouseEventArgs> action)
        {
            MouseClicked((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDoubleClicked(MouseButton button, Action<object, MouseEventArgs> action)
        {
            MouseDoubleClicked((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDown(MouseButton button, Action<object, MouseEventArgs> action)
        {
            MouseDown((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDrag(Action<object, MouseEventArgs> action)
        {
            MouseDrag((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragEnd(Action<object, MouseEventArgs> action)
        {
            MouseDragEnd((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragStart(Action<object, MouseEventArgs> action)
        {
            MouseDragStart((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseMoved(Action<object, MouseEventArgs> action)
        {
            MouseMoved((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseUp(MouseButton button, Action<object, MouseEventArgs> action)
        {
            MouseUp((sender, args) =>
            {
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Touch

        public static void ClearTouchEvents()
        {
            foreach (var kvp in _touchEvents)
            {
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchCancelled))) _touchListener.TouchCancelled -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchEnded))) _touchListener.TouchEnded -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchMoved))) _touchListener.TouchMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchStarted))) _touchListener.TouchStarted -= kvp.Value;
            }

            _touchEvents.Clear();
        }

        private static void TouchCancelled(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchCancelled += eventHandler;
            _touchEvents.Add($"{nameof(_touchListener.TouchCancelled)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchEnded(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchEnded += eventHandler;
            _touchEvents.Add($"{nameof(_touchListener.TouchEnded)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchMoved(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchMoved += eventHandler;
            _touchEvents.Add($"{nameof(_touchListener.TouchMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchStarted(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchStarted += eventHandler;
            _touchEvents.Add($"{nameof(_touchListener.TouchStarted)}_{Guid.NewGuid()}", eventHandler);
        }


        public static void OnTouchCancelled(Action<object, TouchEventArgs> action)
        {
            TouchCancelled((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchEnded(Action<object, TouchEventArgs> action)
        {
            TouchEnded((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchMoved(Action<object, TouchEventArgs> action)
        {
            TouchMoved((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchStarted(Action<object, TouchEventArgs> action)
        {
            TouchStarted((sender, args) =>
            {
                action?.Invoke(sender, args);
            });
        }
        #endregion
    }
}