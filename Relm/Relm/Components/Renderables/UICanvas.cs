﻿using Microsoft.Xna.Framework;
using Relm.Graphics;
using Relm.UI;
using Relm.UI.Drawable;
using Relm.UI.Widgets;
using Relm.UI.Widgets.Styles;

namespace Relm.Components.Renderables
{
	public class UICanvas : RenderableComponent, IUpdateable
	{
		public override float Width => Stage.GetWidth();

		public override float Height => Stage.GetHeight();

		public Stage Stage;

		/// <summary>
		/// if true, the rawMousePosition will be used else the scaledMousePosition will be used. If your UI is in screen space (using a 
		/// ScreenSpaceRenderer for example) then set this to true so input is not scaled.
		/// </summary>
		public bool IsFullScreen
		{
			get => Stage.IsFullScreen;
			set => Stage.IsFullScreen = value;
		}


		public UICanvas()
		{
			Stage = new Stage();
		}


		public override void OnAddedToEntity()
		{
			Stage.Entity = Entity;

			foreach (var child in Stage.GetRoot().children)
			{
				if (child is Window)
					(child as Window).KeepWithinStage();
			}
		}


		public override void OnRemovedFromEntity()
		{
			Stage.Entity = null;
			Stage.Dispose();
		}


		public virtual void Update()
		{
			Stage.Update();
		}


		public override void Render(SpriteBatch spriteBatch, Camera camera)
		{
			Stage.Render(spriteBatch, camera);
		}


		public override void DebugRender(SpriteBatch spriteBatch)
		{
			Stage.GetRoot().DebugRender(spriteBatch);
		}


		/// <summary>
		/// displays a simple dialog with a button to close it
		/// </summary>
		/// <returns>The dialog.</returns>
		/// <param name="title">Title.</param>
		/// <param name="messageText">Message text.</param>
		/// <param name="closeButtonText">Close button text.</param>
		public Dialog ShowDialog(string title, string messageText, string closeButtonText)
		{
			var skin = Skin.CreateDefaultSkin();

			var style = new WindowStyle
			{
				Background = new PrimitiveDrawable(new Color(50, 50, 50)),
				StageBackground = new PrimitiveDrawable(new Color(0, 0, 0, 150))
			};

			var dialog = new Dialog(title, style);
			dialog.GetTitleLabel().GetStyle().Background = new PrimitiveDrawable(new Color(55, 100, 100));
			dialog.Pad(20, 5, 5, 5);
			dialog.AddText(messageText);
			dialog.AddButton(new TextButton(closeButtonText, skin)).OnClicked += butt => dialog.Hide();
			dialog.Show(Stage);

			return dialog;
		}
	}
}