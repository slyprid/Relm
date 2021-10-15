using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Media
{
    public class VideoPlayerPassThrough
    {
        public static dynamic VideoPlayer { get; set; }
        public static dynamic Video { get; set; }

        public static Action<ContentManager, string> OnLoadVideo { get; set; }
        public static Action UpdateVideoPlayer { get; set; }
        public static Action<SpriteBatch, Rectangle> DrawVideoPlayer { get; set; }
        public static Action OnComplete { get; set; }
        public static Action StopVideo { get; set; }

        public static void LoadVideo(ContentManager content, string assetName)
        {
            OnLoadVideo?.Invoke(content, assetName);
        }
    }
}