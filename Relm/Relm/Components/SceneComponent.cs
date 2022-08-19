using System;
using Relm.Scenes;

namespace Relm.Components
{
	public class SceneComponent 
        : IComparable<SceneComponent>
	{
        bool _enabled = true;

		public Scene Scene;
        public int UpdateOrder { get; private set; } = 0;

		public bool Enabled
		{
			get => _enabled;
			set => SetEnabled(value);
		}

        #region SceneComponent Lifecycle

		public virtual void OnEnabled() { }
        public virtual void OnDisabled() { }
        public virtual void OnRemovedFromScene() { }
        public virtual void Update() { }

		#endregion


		#region Fluent setters

		public SceneComponent SetEnabled(bool isEnabled)
		{
			if (_enabled != isEnabled)
			{
				_enabled = isEnabled;

				if (_enabled)
					OnEnabled();
				else
					OnDisabled();
			}

			return this;
		}


		public SceneComponent SetUpdateOrder(int updateOrder)
		{
			if (UpdateOrder != updateOrder)
			{
				UpdateOrder = updateOrder;
				RelmGame.Scene._sceneComponents.Sort();
			}

			return this;
		}

		#endregion


		int IComparable<SceneComponent>.CompareTo(SceneComponent other)
		{
			return UpdateOrder.CompareTo(other.UpdateOrder);
		}
	}
}