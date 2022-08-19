using Relm.Graphics;
using Relm.UI.Base;
using Relm.UI.Drawable;
using Relm.UI.Widgets.Styles;

namespace Relm.UI.Widgets
{
	public class ImageButton : Button
	{
		Image image;
		ImageButtonStyle style;


		public ImageButton(ImageButtonStyle style) : base(style)
		{
			image = new Image();
			image.SetScaling(Scaling.Fit);
			Add(image);
			SetStyle(style);
			SetSize(PreferredWidth, PreferredHeight);
		}

		public ImageButton(Skin skin, string styleName = null) : this(skin.Get<ImageButtonStyle>(styleName))
		{ }


		public ImageButton(IDrawable imageUp) : this(new ImageButtonStyle(null, null, null, imageUp, null, null))
		{ }


		public ImageButton(IDrawable imageUp, IDrawable imageDown) : this(new ImageButtonStyle(null, null, null,
			imageUp, imageDown, null))
		{ }


		public ImageButton(IDrawable imageUp, IDrawable imageDown, IDrawable imageOver) : this(
			new ImageButtonStyle(null, null, null, imageUp, imageDown, imageOver))
		{ }


		public override void SetStyle(ButtonStyle style)
		{
			Assert.IsTrue(style is ImageButtonStyle, "style must be a ImageButtonStyle");

			base.SetStyle(style);
			this.style = (ImageButtonStyle)style;
			if (image != null)
				UpdateImage();
		}


		public new ImageButtonStyle GetStyle()
		{
			return style;
		}


		public Image GetImage()
		{
			return image;
		}


		public Cell GetImageCell()
		{
			return GetCell(image);
		}


		private void UpdateImage()
		{
			IDrawable drawable = null;
			if (_isDisabled && style.ImageDisabled != null)
				drawable = style.ImageDisabled;
			else if (_mouseDown && style.ImageDown != null)
				drawable = style.ImageDown;
			else if (IsChecked && style.ImageChecked != null)
				drawable = (style.ImageCheckedOver != null && _mouseOver) ? style.ImageCheckedOver : style.ImageChecked;
			else if (_mouseOver && style.ImageOver != null)
				drawable = style.ImageOver;
			else if (style.ImageUp != null) //
				drawable = style.ImageUp;

			image.SetDrawable(drawable);
		}


		public override void Draw(SpriteBatch spriteBatch, float parentAlpha)
		{
			UpdateImage();
			base.Draw(spriteBatch, parentAlpha);
		}
	}
}