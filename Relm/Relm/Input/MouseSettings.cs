using Relm.Graphics;

namespace Relm.Input
{
    public class MouseSettings
    {
        public int DragThreshold { get; set; }
        public int DoubleClickSpeed { get; set; }
        public ViewportAdapter ViewportAdapter { get; set; }

        public MouseSettings()
        {
            DoubleClickSpeed = 500;
            DragThreshold = 2;
        }
    }
}