namespace Relm.UI.Configuration
{
    public class MessageBoxConfig
        : IConfig
    {
        public int Width { get; set; }
        public int Height { get; set; }
        
        public MessageBoxConfig SizeOf(int srcWidth, int srcHeight)
        {
            Width = srcWidth;
            Height = srcHeight;
            return this;
        }
    }
}