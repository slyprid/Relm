using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

#if ANDROID
using Android.App;
using Android.OS;
using Android.Content;
#endif


namespace Relm.Input
{
    public class InputManager
    {
        public TouchManager Touch { get; set; }
        public GamePadManager GamePad { get; set; }
        public KeyboardManager Keyboard { get; set; }

        public MouseManager Mouse { get; set; }

#if ANDROID
        public static Android.OS.Vibrator Vibrator { get; set; }
#endif

        public InputManager()
        {
            Touch = new TouchManager();
            GamePad = new GamePadManager();
            Keyboard = new KeyboardManager();
            Mouse = new MouseManager();
        }

        public void Update(GameTime gameTime)
        {
            Touch.Update(gameTime);
            GamePad.Update(gameTime);
            Keyboard.Update(gameTime);
            Mouse.Update(gameTime);
        }

        public bool IsBackPressed()
        {
            return GamePad.IsButtonReleased(Buttons.Back);
        }

        public void Vibrate(int ms, int amp)
        {
#if ANDROID

            if ((BuildVersionCodes)Globals.AndroidBuildVersion >= BuildVersionCodes.O)
            {
                Vibrator?.Vibrate(VibrationEffect.CreateOneShot(ms, amp));
            }
            else
            {
                Vibrator?.Vibrate(150);
            }

#endif
        }

        public void OnClickOpenLink(Rectangle bounds, string url)
        {
#if ANDROID
            if (Touch.IsReleased(bounds))
            {
                var intent = new Intent(Intent.ActionView, Android.Net.Uri.Parse(url));
                intent.AddFlags(ActivityFlags.NewTask);
                Application.Context.StartActivity(intent);
            }
#elif WEB
#else
            if (Mouse.IsReleased(MouseButtons.Left, bounds))
            {
                Process.Start(url);
            }
#endif
        }
    }
}