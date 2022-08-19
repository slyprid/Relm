using Relm.Components;
using Relm.Scenes;

namespace Relm.Graphics.Renderers
{
    public class DefaultRenderer 
        : Renderer
    {
        public DefaultRenderer(int renderOrder = 0, Camera camera = null) : base(renderOrder, camera) { }

        public override void Render(Scene scene)
        {
            var cam = Camera ?? scene.Camera;
            BeginRender(cam);

            for (var i = 0; i < scene.RenderableComponents.Count; i++)
            {
                var renderable = scene.RenderableComponents[i];
                if (renderable.Enabled && renderable.IsVisibleFromCamera(cam))
                    RenderAfterStateCheck(renderable, cam);
            }

            if (ShouldDebugRender && RelmGame.DebugRenderEnabled)
                DebugRender(scene, cam);

            EndRender();
        }
    }
}