using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Core.States;

namespace Relm.Core
{
	public class Relm 
		: Game
	{
		#region Fields

		protected GraphicsDeviceManager Graphics;
		protected SpriteBatch SpriteBatch;

		#endregion

		#region Properties

		#endregion

		#region Managers

		private StateManager _states;

		#endregion

		#region Initialization

		public Relm()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			_states = new StateManager();

			InitializeCustomManagers();

			base.Initialize();
		}

		protected virtual void InitializeCustomManagers()
		{

		}

		#endregion

		#region Loading

		protected override void LoadContent()
		{
			SpriteBatch = new SpriteBatch(GraphicsDevice);

			LoadStates();
			LoadCustomContent();
		}

		protected virtual void LoadStates() { }

		protected virtual void LoadCustomContent() { }

		protected override void UnloadContent()
		{

		}

		#endregion

		#region Updating

		protected override void Update(GameTime gameTime)
		{
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
				Exit();

			var currentState = CurrentState();
			if (currentState != null)
			{
				if (currentState.IsEnabled && !currentState.IsPaused)
				{
					currentState.Update(gameTime);
				}
			}

			base.Update(gameTime);
		}

		#endregion

		#region Rendering

		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			base.Draw(gameTime);
		}

		#endregion

		#region State Management

		public T LoadState<T>(string alias)
			where T : State
		{
			var state = Activator.CreateInstance<T>();
			state.SpriteBatch = SpriteBatch;
			_states.Add(alias, state);
			return state;
		}

		public T LoadState<T>()
			where T : State
		{
			var state = Activator.CreateInstance<T>();
			_states.Add(state);
			return state;
		}

		public void UnloadState(string alias)
		{
			_states.Remove(alias);
		}

		public State ChangeState(string alias)
		{
			_states.ChangeState(alias);
			return _states.CurrentState;
		}

		public State CurrentState()
		{
			return _states.CurrentState;
		}

		#endregion

		#region Utilities

		public SpriteBatch CreateSpriteBatch()
		{
			return new SpriteBatch(GraphicsDevice);
		}

		#endregion
	}
}
