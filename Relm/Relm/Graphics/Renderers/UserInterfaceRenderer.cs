using Relm.Components;
using Relm.Scenes;

namespace Relm.Graphics.Renderers
{
    public class UserInterfaceRenderer
        : Renderer
    {
        public UserInterfaceRenderer(int renderOrder = 0) : base(renderOrder, null)
        {
            WantsToRenderAfterPostProcessors = true;
        }

        public override void Render(Scene scene)
        {
            BeginRender(Camera);

            var components = scene.RenderableComponents.UserInterfaceComponents();
            for (var i = 0; i < components.Length; i++)
            {
                var renderable = components[i];
                if (renderable.Enabled && renderable.IsVisibleFromCamera(Camera))
                    RenderAfterStateCheck(renderable, Camera);
            }

            if (ShouldDebugRender && RelmGame.DebugRenderEnabled)
                DebugRender(scene, Camera);

            EndRender();
        }

        protected override void DebugRender(Scene scene, Camera cam)
        {
            RelmGraphics.Instance.SpriteBatch.End();
            RelmGraphics.Instance.SpriteBatch.Begin(cam.TransformMatrix);

            var components = scene.RenderableComponents.UserInterfaceComponents();
            for (var i = 0; i < components.Length; i++)
            {
                var renderable = components[i];
                if (renderable.Enabled)
                    renderable.DebugRender(RelmGraphics.Instance.SpriteBatch);
            }
        }

        public override void OnSceneBackBufferSizeChanged(int newWidth, int newHeight)
        {
            base.OnSceneBackBufferSizeChanged(newWidth, newHeight);

            // this is a bit of a hack. we maybe should take the Camera in the constructor
            if (Camera == null)
                Camera = RelmGame.Scene.CreateEntity("userInterface camera").AddComponent<Camera>();
        }
    }
}