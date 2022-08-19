using Relm.Graphics;
using Relm.Math;

namespace Relm.Components
{
	public interface IRenderable
	{
		RectangleF Bounds { get; }
        bool Enabled { get; set; }
        float LayerDepth { get; set; }
        int RenderLayer { get; set; }
        Material Material { get; set; }
        bool IsVisible { get; }


		T GetMaterial<T>() where T : Material;
        bool IsVisibleFromCamera(Camera camera);
        void Render(SpriteBatch spriteBatch, Camera camera);
        void DebugRender(SpriteBatch spriteBatch);
	}
}