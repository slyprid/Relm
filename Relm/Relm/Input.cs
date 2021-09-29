using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Input;
using MonoGame.Extended.Input.InputListeners;
using MonoGame.Extended.Screens;

namespace Relm
{
    public static class Input
    {
        private static TouchListener _touchListener;
        private static GamePadListener _gamePadListener;
        private static KeyboardListener _keyboardListener;
        private static MouseListener _mouseListener;

        private static readonly Dictionary<string, EventHandler<KeyboardEventArgs>> KeyboardEvents;
        private static readonly Dictionary<string, EventHandler<TouchEventArgs>> TouchEvents;
        private static readonly Dictionary<string, EventHandler<GamePadEventArgs>> GamePadEvents;
        private static readonly Dictionary<string, EventHandler<MouseEventArgs>> MouseEvents;
        
        #region Initialization

        static Input()
        {
            KeyboardEvents = new Dictionary<string, EventHandler<KeyboardEventArgs>>();
            TouchEvents = new Dictionary<string, EventHandler<TouchEventArgs>>();
            GamePadEvents = new Dictionary<string, EventHandler<GamePadEventArgs>>();
            MouseEvents = new Dictionary<string, EventHandler<MouseEventArgs>>();
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
            foreach (var kvp in KeyboardEvents)
            {
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyPressed))) _keyboardListener.KeyPressed -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyReleased))) _keyboardListener.KeyReleased -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_keyboardListener.KeyTyped))) _keyboardListener.KeyTyped -= kvp.Value;
            }

            KeyboardEvents.Clear();
        }

        private static void KeyPressed(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyPressed += eventHandler;
            KeyboardEvents.Add($"{nameof(_keyboardListener.KeyPressed)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyReleased(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyReleased += eventHandler;
            KeyboardEvents.Add($"{nameof(_keyboardListener.KeyReleased)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void KeyTyped(EventHandler<KeyboardEventArgs> eventHandler)
        {
            _keyboardListener.KeyTyped += eventHandler;
            KeyboardEvents.Add($"{nameof(_keyboardListener.KeyTyped)}_{Guid.NewGuid()}", eventHandler);
        }

        public static void OnKeyPressed(Keys key, Action<object, KeyboardEventArgs> action, GameScreen screen)
        {
            KeyPressed((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyReleased(Keys key, Action<object, KeyboardEventArgs> action, GameScreen screen)
        {
            KeyReleased((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnKeyTyped(Keys key, Action<object, KeyboardEventArgs> action, GameScreen screen)
        {
            KeyTyped((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Key != key) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Gamepad

        public static void ClearGamepadEvents()
        {
            foreach (var kvp in GamePadEvents)
            {
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonUp))) _gamePadListener.ButtonUp -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonDown))) _gamePadListener.ButtonDown -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ButtonRepeated))) _gamePadListener.ButtonRepeated -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.ThumbStickMoved))) _gamePadListener.ThumbStickMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_gamePadListener.TriggerMoved))) _gamePadListener.TriggerMoved -= kvp.Value;
            }

            GamePadEvents.Clear();
        }

        private static void ButtonDown(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonDown += eventHandler;
            GamePadEvents.Add($"{nameof(_gamePadListener.ButtonDown)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ButtonRepeated(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonRepeated += eventHandler;
            GamePadEvents.Add($"{nameof(_gamePadListener.ButtonRepeated)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ButtonUp(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ButtonUp += eventHandler;
            GamePadEvents.Add($"{nameof(_gamePadListener.ButtonUp)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void ThumbStickMoved(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.ThumbStickMoved += eventHandler;
            GamePadEvents.Add($"{nameof(_gamePadListener.ThumbStickMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TriggerMoved(EventHandler<GamePadEventArgs> eventHandler)
        {
            _gamePadListener.TriggerMoved += eventHandler;
            GamePadEvents.Add($"{nameof(_gamePadListener.TriggerMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        public static void OnButtonDown(Buttons button, Action<object, GamePadEventArgs> action, GameScreen screen)
        {
            ButtonDown((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnButtonRepeated(Buttons button, Action<object, GamePadEventArgs> action, GameScreen screen)
        {
            ButtonRepeated((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnButtonUp(Buttons button, Action<object, GamePadEventArgs> action, GameScreen screen)
        {
            ButtonUp((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnThumbStickMoved(Action<object, GamePadEventArgs> action, GameScreen screen)
        {
            ThumbStickMoved((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnTriggerMoved(Action<object, GamePadEventArgs> action, GameScreen screen)
        {
            TriggerMoved((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
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
            foreach (var kvp in MouseEvents)
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

            MouseEvents.Clear();
        }

        private static void MouseClicked(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseClicked += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDoubleClicked(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDoubleClicked += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseDoubleClicked)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDown(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDown += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseDown)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDrag(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDrag += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseDrag)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragEnd(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragEnd += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseDragEnd)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseDragStart(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseDragStart += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseDragStart)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseMoved(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseMoved += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void MouseUp(EventHandler<MouseEventArgs> eventHandler)
        {
            _mouseListener.MouseUp += eventHandler;
            MouseEvents.Add($"{nameof(_mouseListener.MouseUp)}_{Guid.NewGuid()}", eventHandler);
        }


        public static void OnMouseClicked(MouseButton button, Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseClicked((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDoubleClicked(MouseButton button, Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseDoubleClicked((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDown(MouseButton button, Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseDown((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDrag(Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseDrag((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragEnd(Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseDragEnd((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseDragStart(Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseDragStart((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseMoved(Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseMoved((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnMouseUp(MouseButton button, Action<object, MouseEventArgs> action, GameScreen screen)
        {
            MouseUp((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                if (args.Button != button) return;
                action?.Invoke(sender, args);
            });
        }

        #endregion

        #region Touch

        public static void ClearTouchEvents()
        {
            foreach (var kvp in TouchEvents)
            {
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchCancelled))) _touchListener.TouchCancelled -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchEnded))) _touchListener.TouchEnded -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchMoved))) _touchListener.TouchMoved -= kvp.Value;
                if (kvp.Key.StartsWith(nameof(_touchListener.TouchStarted))) _touchListener.TouchStarted -= kvp.Value;
            }

            TouchEvents.Clear();
        }

        private static void TouchCancelled(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchCancelled += eventHandler;
            TouchEvents.Add($"{nameof(_touchListener.TouchCancelled)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchEnded(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchEnded += eventHandler;
            TouchEvents.Add($"{nameof(_touchListener.TouchEnded)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchMoved(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchMoved += eventHandler;
            TouchEvents.Add($"{nameof(_touchListener.TouchMoved)}_{Guid.NewGuid()}", eventHandler);
        }

        private static void TouchStarted(EventHandler<TouchEventArgs> eventHandler)
        {
            _touchListener.TouchStarted += eventHandler;
            TouchEvents.Add($"{nameof(_touchListener.TouchStarted)}_{Guid.NewGuid()}", eventHandler);
        }


        public static void OnTouchCancelled(Action<object, TouchEventArgs> action, GameScreen screen)
        {
            TouchCancelled((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchEnded(Action<object, TouchEventArgs> action, GameScreen screen)
        {
            TouchEnded((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchMoved(Action<object, TouchEventArgs> action, GameScreen screen)
        {
            TouchMoved((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }

        public static void OnTouchStarted(Action<object, TouchEventArgs> action, GameScreen screen)
        {
            TouchStarted((sender, args) =>
            {
                if (screen != UserInterface.ActiveScreen && screen != Screens.ActiveScreen) return;
                action?.Invoke(sender, args);
            });
        }
        #endregion
    }
}