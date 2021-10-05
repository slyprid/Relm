using MonoGame.Extended.Sprites;

namespace Relm.UI.Extensions
{
    public static class SpriteSheetAnimationCycleExtensions
    {
        public static SpriteSheetAnimationCycle AddFrame(this SpriteSheetAnimationCycle cycle, int index)
        {
            cycle.Frames.Add(new SpriteSheetAnimationFrame(index));
            return cycle;
        }

        public static SpriteSheetAnimationCycle AddFrames(this SpriteSheetAnimationCycle cycle, int startIndex, int endIndex)
        {
            for (var index = startIndex; index <= endIndex; index++)
            {
                cycle.Frames.Add(new SpriteSheetAnimationFrame(index));
            }

            return cycle;
        }
    }
}