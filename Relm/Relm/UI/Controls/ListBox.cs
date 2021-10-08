using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.TextureAtlases;
using Relm.UI.Configuration;
using Relm.UI.States;

namespace Relm.UI.Controls
{
    public class ListBox
        : Container
    {
        private string _id = Guid.NewGuid().ToString();
        private string _scrollBarName;
        private bool _isBaseConfigured;
        private bool _isChildrenConfigured;
        private int _topIndex;
        private int _bottomIndex;
        private int _visibleItemCount;
        private ScrollBar _scrollBar;

        public List<ListBoxItem> Items { get; set; }
        public TextureAtlas TextureAtlas { get; set; }
        public SpriteFont Font { get; set; }
        public Color ForegroundColor { get; set; }
        public int Count { get; private set; }
        public ListBoxItem SelectedItem { get; set; }
        public int SelectedIndex { get; set; }

        public ListBox()
        {
            Items = new List<ListBoxItem>();
            _scrollBarName = $"{_id}_scrollBarName";
            ForegroundColor = Color.Black;
            _topIndex = 0;
        }

        public override void Configure()
        {
            if (!_isChildrenConfigured && !_isBaseConfigured) return;

            var config = (ListBoxConfig)UserInterface.Skin.ControlConfigurations[typeof(ListBox)];
            TextureAtlas = new TextureAtlas(Guid.NewGuid().ToString(), UserInterface.Skin.Texture, config.Pieces);

            _scrollBar = Add<ScrollBar>(_scrollBarName, new Vector2(Width, 0), ParentScreen)
                .SetOrientation(ScrollBarOrientation.Vertical)
                .SetValues(0, 0, 0)
                .SetSize<ScrollBar>(TextureAtlas.First().Width, Height);

            _scrollBar.OnValueChanged = OnScroll;

            foreach (var item in Items)
            {
                item.ItemSelected = i =>
                {
                    SelectedItem = i;
                    SelectedIndex = Items.IndexOf(i);
                };
            }

            _isChildrenConfigured = true;

            base.Configure();
        }

        public override void Update(GameTime gameTime)
        {
            _isBaseConfigured = true;

            if (!_isChildrenConfigured) Configure();

            foreach (var item in Items)
            {
                if (item != SelectedItem) item.State = ListBoxItemState.Normal;
                item.Update(gameTime);
            }

            _scrollBar.MaximumValue = Count;

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            if (!_isBaseConfigured && !_isChildrenConfigured) return;

            DrawPanel(gameTime, spriteBatch);

            if (Items.Count < 0) return;

            _bottomIndex = CalculateDimensions();
            if (_bottomIndex - _topIndex > _visibleItemCount)
            {
                _visibleItemCount = _bottomIndex - _topIndex;
            }
            for (var i = _topIndex; i < _topIndex + _visibleItemCount; i++)
            {
                var item = Items[i];
                item.Draw(gameTime, spriteBatch);
            }

            base.Draw(gameTime, spriteBatch);
        }
        
        private void DrawPanel(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // Top Left
            var piece = TextureAtlas[PanelPiece.TopLeft.ToString()];
            var bounds = new Rectangle(X, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);

            // Top Right
            piece = TextureAtlas[PanelPiece.TopRight.ToString()];
            bounds = new Rectangle(X + Width - piece.Width, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);

            // Bottom Left
            piece = TextureAtlas[PanelPiece.BottomLeft.ToString()];
            bounds = new Rectangle(X, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);

            // Bottom Right
            piece = TextureAtlas[PanelPiece.BottomRight.ToString()];
            bounds = new Rectangle(X + Width - piece.Width, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);


            // Top Center
            piece = TextureAtlas[PanelPiece.Top.ToString()];
            var startX = X + piece.Width;
            var endX = X + Width - piece.Width;
            for (var x = startX; x < endX; x += piece.Width)
            {
                bounds = new Rectangle(x, Y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White);
            }

            // Bottom Center
            piece = TextureAtlas[PanelPiece.Bottom.ToString()];
            startX = X + piece.Width;
            endX = X + Width - piece.Width;
            for (var x = startX; x < endX; x += piece.Width)
            {
                bounds = new Rectangle(x, Y + Height - piece.Height, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White);
            }

            // Left Center
            piece = TextureAtlas[PanelPiece.Left.ToString()];
            var startY = Y + piece.Height;
            var endY = Y + Height - piece.Height;
            for (var y = startY; y < endY; y += piece.Height)
            {
                bounds = new Rectangle(X, y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White);
            }

            // Right Center
            piece = TextureAtlas[PanelPiece.Right.ToString()];
            startY = Y + piece.Height;
            endY = Y + Height - piece.Height;
            for (var y = startY; y < endY; y += piece.Height)
            {
                bounds = new Rectangle(X + Width - piece.Width, y, (int)(piece.Width * Scale.X), (int)(piece.Height * Scale.Y));
                spriteBatch.Draw(piece, bounds, Color.White);
            }

            // Center
            piece = TextureAtlas[PanelPiece.Center.ToString()];
            bounds = new Rectangle(X + piece.Width, Y + piece.Width, (int)((Width - (piece.Width * 2)) * Scale.X), (int)((Height - (piece.Height * 2)) * Scale.Y));
            spriteBatch.Draw(piece, bounds, Color.White);
        }

        private int CalculateDimensions()
        {
            var xOffset = 8;
            var yOffset = 8;
            for(var i = _topIndex; i < Items.Count; i++)
            {
                var item = Items[i];
                item.Font = Font;
                item.Color = ForegroundColor;
                item.Width = Width;
                item.Position = Position + new Vector2(xOffset, yOffset);
                item.XOffset = xOffset;
                item.CalculateDimensions();
                yOffset += item.Height;
                if (yOffset >= Height) return i;
            }

            return 0;
        }

        #region Events

        public void OnScroll(ScrollBar scrollBar)
        {
            _topIndex = scrollBar.Value;
            if (_topIndex >= scrollBar.MaximumValue - _visibleItemCount)
            {
                _topIndex = scrollBar.MaximumValue - _visibleItemCount;
            }
        }

        #endregion

        #region Fluent Functions

        public ListBox AddItem(object item)
        {
            Items.Add(new ListBoxItem
            {
                Content = item
            });

            Count = Items.Count;

            return this;
        }

        public ListBox UsingFont(string fontName)
        {
            Font = ContentLibrary.Fonts[fontName];
            return this;
        }

        public ListBox WithForegroundColor(Color color)
        {
            ForegroundColor = color;
            return this;
        }

        #endregion
    }
}