using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Relm.Media;
using Relm.Sandbox.Win.Models.Screens;
using Relm.Sandbox.Win.Models.UI;
using Relm.Sandbox.Win.Naming;

namespace Relm.Sandbox.Win
{
    public class SandboxGame 
        : RelmGame
    {
        private static MediaState _previousState;
        private static MediaState _currentState;
        private static bool _videoComplete;
        private static Texture2D _videoTexture;

        public SandboxGame()
            : base("Relm Sandbox", 1280, 720, 1280, 720)
        {
            VideoPlayerPassThrough.OnLoadVideo = OnLoadVideo;
            VideoPlayerPassThrough.UpdateVideoPlayer = UpdateVideoPlayer;
            VideoPlayerPassThrough.DrawVideoPlayer = DrawVideoPlayer;
            VideoPlayerPassThrough.StopVideo = StopVideo;
            VideoPlayerPassThrough.VideoPlayer = new VideoPlayer();
        }

        protected override void Initialize()
        {
            base.Initialize();

            //Input.OnKeyPressed(Keys.Escape, (sender, args) => { Exit(); });
            //Input.OnKeyPressed(Keys.Escape, (sender, args) => { UserInterface.Change(nameof(MainMenu), new FadeTransition(GraphicsDevice, Color.Black)); });
            //Input.OnKeyPressed(Keys.D1, (sender, args) => { Screens.Change(nameof(TestScreen1), new FadeTransition(GraphicsDevice, Color.Black)); });
            //Input.OnKeyPressed(Keys.D2, (sender, args) => { Screens.Change(nameof(TestScreen2), new FadeTransition(GraphicsDevice, Color.Black)); });
            //Input.OnKeyPressed(Keys.D3, (sender, args) => { Input.ClearKeyboardEvents(); });

            //Input.OnMouseDoubleClicked(MouseButton.Right, (sender, args) => { Exit(); });
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            ContentLibrary.Textures.Add(TextureNames.Test64);
            ContentLibrary.Textures.Add(TextureNames.Background);
            ContentLibrary.Textures.Add(TextureNames.UserInterfaceSkin);
            ContentLibrary.Textures.Add(TextureNames.Icons);
            ContentLibrary.Textures.Add(TextureNames.Fire);

            ContentLibrary.Fonts.Add(FontNames.Default, x => x.Texture.Name = FontNames.Default);
            var perpetuaFontSet = ContentLibrary.FontSets.NoLoadAdd(FontNames.Default, x => x.Name = FontNames.Default);
            perpetuaFontSet.Add(16, FontNames.Default);

            Screens.Add(new TestScreen1(this));
            Screens.Add(new TestScreen2(this));

            UserInterface.UseSkin(TextureNames.UserInterfaceSkin, FontNames.Default);
            UserInterface.Add(new MainMenu(this));
            UserInterface.Add(new Hud(this));

            Screens.Change(nameof(TestScreen1));

            Console.Initialize(FontNames.Default);
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private static void OnLoadVideo(ContentManager content, string assetName)
        {
            _videoComplete = false;
            _videoTexture = null;
            VideoPlayerPassThrough.Video = content.Load<Video>(assetName);
        }

        private static void UpdateVideoPlayer()
        {
            var player = VideoPlayerPassThrough.VideoPlayer;
            if (!_videoComplete && player.State == MediaState.Stopped && _previousState == MediaState.Stopped)
            {
                player.Play(VideoPlayerPassThrough.Video);
            }
            else if (player.State == MediaState.Stopped && _previousState == MediaState.Playing)
            {
                _videoComplete = true;
                VideoPlayerPassThrough.OnComplete?.Invoke();
            }

            _previousState = _currentState;
            _currentState = player.State;
        }

        private static void DrawVideoPlayer(SpriteBatch spriteBatch, Rectangle destRect)
        {
            var player = VideoPlayerPassThrough.VideoPlayer;
            if (player.State != MediaState.Stopped) _videoTexture = player.GetTexture();
            if (_videoTexture == null) return;
            spriteBatch.Begin();
            spriteBatch.Draw(_videoTexture, destRect, Color.White);
            spriteBatch.End();
        }

        private static void StopVideo()
        {
            var player = VideoPlayerPassThrough.VideoPlayer;
            player.Pause();
            _videoComplete = true;
        }
    }
}