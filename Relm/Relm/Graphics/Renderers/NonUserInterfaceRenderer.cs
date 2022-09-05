using Relm.Components;
using Relm.Scenes;

namespace Relm.Graphics.Renderers
{
    public class NonUserInterfaceRenderer
        : Renderer
    {
        public NonUserInterfaceRenderer(int renderOrder = 0, Camera camera = null) : base(renderOrder, camera) { }

        public override void Render(Scene scene)
        {
            var cam = Camera ?? scene.Camera;
            BeginRender(cam);

            for (var i = 0; i < scene.RenderableComponents.Count; i++)
            {
                var renderable = scene.RenderableComponents[i];
                var isUserInterface = renderable.GetType().GetInterface(nameof(IUserInterfaceRenderable)) != null;
                if (renderable.Enabled && renderable.IsVisibleFromCamera(cam) && !isUserInterface)
                    RenderAfterStateCheck(renderable, cam);
            }

            if (ShouldDebugRender && RelmGame.DebugRenderEnabled)
                DebugRender(scene, cam);

            EndRender();
        }
    }
}