using System.IO;
using Microsoft.Xna.Framework.Graphics;

namespace Relm.Helpers
{
    public static class ContentHelper
    {
        public static Texture2D LoadTextureFromFile(string filename)
        {
            var graphicsDevice = OldRelmGame.Instance.GraphicsDevice;
            using (var stream = new FileStream(filename, FileMode.Open))
            {
                return Texture2D.FromStream(graphicsDevice, stream);
            }
        }
    }
}