using System;
using Microsoft.Xna.Framework.Graphics;
using Relm.Extensions;

namespace Relm.Graphics
{
	public abstract class GraphicsResource 
        : IDisposable
	{
        private GraphicsDevice _graphicsDevice;
        private WeakReference _selfReference;

		public bool IsDisposed { get; private set; }

		public GraphicsDevice GraphicsDevice
		{
			get => _graphicsDevice;
			internal set
			{
				Assert.IsTrue(value != null);

				if (_graphicsDevice == value) return;

				if (_graphicsDevice != null)
				{
					UpdateResourceReference(false);
					_selfReference = null;
				}

				_graphicsDevice = value;

				_selfReference = new WeakReference(this);
				UpdateResourceReference(true);
			}
		}

        internal GraphicsResource() { }
		
		~GraphicsResource()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);

			GC.SuppressFinalize(this);
		}
		
		protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed) return;
            if (disposing) { }
            if (GraphicsDevice != null) UpdateResourceReference(false);

            _selfReference = null;
            _graphicsDevice = null;
            IsDisposed = true;
        }
		
		private void UpdateResourceReference(bool shouldAdd)
		{
			var method = shouldAdd ? "AddResourceReference" : "RemoveResourceReference";
			var methodInfo = typeof(GraphicsDevice).GetMethodInfo(method);
			methodInfo.Invoke(GraphicsDevice, new object[] { _selfReference });
		}
	}
}