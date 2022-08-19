using Microsoft.Xna.Framework.Graphics;

namespace Relm.Graphics.PostProcessing
{
    public class PostProcessor<T> 
        : PostProcessor where T : Effect
    {
        public new T Effect;
        
        public PostProcessor(int executionOrder, T effect = null) : base(executionOrder, effect)
        {
            Effect = effect;
        }

        public override void Process(RenderTarget2D source, RenderTarget2D destination)
        {
            DrawFullscreenQuad(source, destination, Effect);
        }

        public override void Unload()
        {
            base.Unload();
        }
    }
}