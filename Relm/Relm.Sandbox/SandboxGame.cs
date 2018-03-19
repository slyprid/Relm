using Relm.Sandbox.States;

namespace Relm.Sandbox
{
	public class SandboxGame 
		: Core.Relm
	{
	    protected override void Initialize()
	    {
            ChangeResolution(1280, 720);
	        base.Initialize();
	    }

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