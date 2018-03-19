using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Sandbox.States;

namespace Relm.Sandbox
{
	public class SandboxGame 
		: Core.Relm
	{
		#region Loading

		protected override void LoadStates()
		{
			LoadScene<TitleScene>(TitleScene.Alias);

			ChangeScene(TitleScene.Alias);
		}

		protected override void LoadCustomContent()
		{
			base.LoadCustomContent();
		}

		#endregion
	}
}