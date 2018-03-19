using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Relm.Core.Scenes;

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

		private SceneManager _scenes;

		#endregion

		#region Initialization

		public Relm()
		{
			Graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
		}

		protected override void Initialize()
		{
			_scenes = new SceneManager();

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

			var currentState = CurrentScene();
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

		#region Scene Management

		public T LoadScene<T>(string alias)
			where T : Scene
		{
			var scene = Activator.CreateInstance<T>();
		    scene.SpriteBatch = SpriteBatch;
			_scenes.Add(alias, scene);
			return scene;
		}

		public T LoadScene<T>()
			where T : Scene
		{
			var scene = Activator.CreateInstance<T>();
			_scenes.Add(scene);
			return scene;
		}

		public void UnloadScene(string alias)
		{
			_scenes.Remove(alias);
		}

		public Scene ChangeScene(string alias)
		{
			_scenes.ChangeState(alias);
			return _scenes.CurrentScene;
		}

		public Scene CurrentScene()
		{
			return _scenes.CurrentScene;
		}

		#endregion

		#region Utilities

		public SpriteBatch CreateSpriteBatch()
		{
			return new SpriteBatch(GraphicsDevice);
		}

	    public void ChangeResolution(int width, int height)
	    {
	        Graphics.PreferredBackBufferWidth = width;
	        Graphics.PreferredBackBufferHeight = height;
            Graphics.ApplyChanges();
	    }

		#endregion
	}
}
