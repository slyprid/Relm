using Microsoft.Xna.Framework;
using Relm.Core.Managers;

namespace Relm.Core.States
{
	public class StateManager
		: Manager<State>
	{
		public State CurrentState { get; protected set; }

		public void ChangeState(string stateAlias)
		{
			if (CurrentState == null)
			{
				CurrentState = GetItem(stateAlias);
				return;
			}

			CurrentState.StartChange(() =>
			{
				CurrentState.EndChange(() =>
				{
					CurrentState = GetItem(stateAlias);
				});
			});
		}

		public void Add(State state)
		{
			Add(State.GenerateAlias(), state);
		}

		public void Update(GameTime gameTime)
		{
			if (CurrentState == null) return;
			if (!CurrentState.IsEnabled) return;
		}

		public void Draw(GameTime gameTime)
		{
			if (CurrentState == null) return;
			if (!CurrentState.IsVisible) return;
		}
	}
}