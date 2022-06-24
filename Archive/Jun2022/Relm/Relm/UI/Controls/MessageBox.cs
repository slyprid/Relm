using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.Entities;
using Relm.Extensions;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class MessageBox
        : Control
    {
        private List<IControl> _controls;
        private Button _btnOk;
        private Button _btnCancel;
        private Button _btnAbort;
        private Button _btnIgnore;
        private Button _btnRetry;
        private Button _btnYes;
        private Button _btnNo;

        public Action<MessageBox> OnResult { get; set; }
        public DialogResult Result { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public MessageBoxButtons Buttons { get; set; }
        public int FontSize { get; set; }
        public bool HasTextShadow { get; set; }
        public Vector2 TextShadowOffset { get; set; }
        public Color TextColor { get; set; }
        public Vector2 TextOffset { get; set; }

        private IDrawableEntity _drawableEntity;

        public MessageBox()
        {
            Result = DialogResult.None;
            FontSize = 20;
            HasTextShadow = true;
            TextShadowOffset = new Vector2(2f, 2f);
            TextColor = Color.White;
            TextOffset = Vector2.Zero;
        }

        public override void Configure()
        {
            _controls = new List<IControl>();

            var config = (MessageBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(MessageBox)];
            Size = new Vector2(config.Width, config.Height);

            Configure((int)Size.X, (int)Size.Y);
        }

        public void Configure(int width, int height)
        {
            _controls = new List<IControl>();

            var config = (MessageBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(MessageBox)];
            Size = new Vector2(width, height);

            var headerPanel = new HeaderPanel()
                .SetPosition<HeaderPanel>(Layout.Centered((int)width, (int)height))
                .SetSize<HeaderPanel>((int)width, (int)height)
                .SetHeader(Title, 24, Color.White)
                .SetHeaderOffset(0, 4);

            headerPanel.ParentScreen = ParentScreen;
            headerPanel.Configure();

            headerPanel.SetSize<HeaderPanel>((int)width, (int)height);

            Position = headerPanel.Position;

            _controls.Add(headerPanel);

            ConfigureButtons();
        }

        private void ConfigureButtons()
        {
            var x = 114f;
            var y = 144f;
            switch (Buttons)
            {
                case MessageBoxButtons.OK:
                    _btnOk = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(0f, y))
                        .SetText("OK", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.OK;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnOk);
                    break;
                case MessageBoxButtons.OKCancel:
                    _btnOk = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(-x, y))
                        .SetText("OK", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.OK;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnOk);

                    _btnCancel = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(x, y))
                        .SetText("Cancel", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Cancel;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnCancel);
                    break;
                case MessageBoxButtons.AbortRetryIgnore:
                    _btnAbort = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(-(x * 2), y))
                        .SetText("Abort", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Abort;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnAbort);

                    _btnRetry = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(0f, y))
                        .SetText("Retry", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Retry;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnRetry);

                    _btnIgnore = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2((x * 2), y))
                        .SetText("Ignore", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Ignore;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnIgnore);
                    break;
                case MessageBoxButtons.YesNoCancel:
                    _btnYes = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(-(x * 2), y))
                        .SetText("Yes", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Yes;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnYes);

                    _btnNo = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(0f, y))
                        .SetText("No", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.No;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnNo);

                    _btnCancel = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2((x * 2), y))
                        .SetText("Cancel", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Cancel;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnCancel);
                    break;
                case MessageBoxButtons.YesNo:
                    _btnYes = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(-x, y))
                        .SetText("Yes", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Yes;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnYes);

                    _btnNo = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(x, y))
                        .SetText("No", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.No;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnNo);
                    break;
                case MessageBoxButtons.RetryCancel:
                    _btnCancel = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(-x, y))
                        .SetText("Cancel", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Cancel;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnCancel);

                    _btnRetry = new Button()
                        .SetPosition<Button>(Layout.Centered(227, 50) + new Vector2(x, y))
                        .SetText("Retry", Color.White)
                        .OnClick(btn =>
                        {
                            Result = DialogResult.Retry;
                            OnResult?.Invoke(this);
                        });
                    _controls.Add(_btnRetry);
                    break;
            }

            _btnOk?.Configure();
            _btnCancel?.Configure();
            _btnAbort?.Configure();
            _btnIgnore?.Configure();
            _btnRetry?.Configure();
            _btnYes?.Configure();
            _btnNo?.Configure();
        }

        public override void Update(GameTime gameTime)
        {
            foreach (var control in _controls)
            {
                control.Update(gameTime);
            }

            _drawableEntity?.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied);
                foreach (var control in _controls)
                {
                    control.Draw(gameTime, spriteBatch);
                }

                if (!string.IsNullOrEmpty(Message))
                {
                    var fontSet = UserInterface.Skin.FontSet2;
                    var font = fontSet[FontSize];
                    var textSize = font.MeasureString(Message);
                    var textPosition = Position + new Vector2((Width / 2) - (textSize.X / 2), (Height / 2) - (textSize.Y / 2)) + TextOffset;
                    if (HasTextShadow)
                    {
                        spriteBatch.DrawString(font, Message, textPosition + TextShadowOffset, Color.Black.WithOpacity(Opacity));
                    }
                    spriteBatch.DrawString(font, Message, textPosition, TextColor.WithOpacity(Opacity));
                }

            spriteBatch.End();

            _drawableEntity?.Draw(gameTime, spriteBatch);

            base.Draw(gameTime, spriteBatch);
        }

        #region Fluent Functions

        public MessageBox SetTextOffset(int x, int y)
        {
            TextOffset = new Vector2(x, y);
            return this;
        }

        #endregion

        #region Show / Hide Functions

        public static void Hide()
        {
            RelmGame.MessageBox = null;
        }

        public static MessageBox Show(string message, string title, MessageBoxButtons buttons, Action<MessageBox> onResult)
        {
            RelmGame.MessageBox = new MessageBox
            {
                OnResult = onResult,
                Message = message,
                Title = title,
                Buttons = buttons
            };
            RelmGame.MessageBox.Configure();

            return RelmGame.MessageBox;
        }

        public static MessageBox Show(string message, string title, IDrawableEntity drawableEntity, MessageBoxButtons buttons, Action<MessageBox> onResult)
        {
            RelmGame.MessageBox = new MessageBox
            {
                OnResult = onResult,
                Message = message,
                Title = title,
                Buttons = buttons,
                _drawableEntity = drawableEntity
            };
            RelmGame.MessageBox.Configure();

            return RelmGame.MessageBox;
        }

        public static MessageBox Show(string message, string title, int width, int height, MessageBoxButtons buttons, Action<MessageBox> onResult)
        {
            RelmGame.MessageBox = new MessageBox
            {
                OnResult = onResult,
                Message = message,
                Title = title,
                Buttons = buttons
            };
            RelmGame.MessageBox.Configure(width, height);

            return RelmGame.MessageBox;
        }

        public static MessageBox Show(string message, string title, int width, int height, IDrawableEntity drawableEntity, MessageBoxButtons buttons, Action<MessageBox> onResult)
        {
            RelmGame.MessageBox = new MessageBox
            {
                OnResult = onResult,
                Message = message,
                Title = title,
                Buttons = buttons,
                _drawableEntity = drawableEntity
            };
            RelmGame.MessageBox.Configure(width, height);

            return RelmGame.MessageBox;
        }

        #endregion
    }
}