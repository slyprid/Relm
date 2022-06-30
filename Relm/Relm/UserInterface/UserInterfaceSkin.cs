using Microsoft.Xna.Framework.Graphics;
using Relm.Naming;
using Relm.Textures;

namespace Relm.UserInterface
{
    public class UserInterfaceSkin
        : TextureAtlas
    {
        public static int FrameRegionWidth = 32;
        public static int FrameRegionHeight = 32;
        public static int BorderOffsetX = 8;
        public static int BorderOffsetY = 8;

        public static int CursorWidth = 32;
        public static int CursorHeight = 32;

        public static int AdvanceButtonWidth = 32;
        public static int AdvanceButtonHeight = 32;

        public static int ControllerButtonWidth = 64;
        public static int ControllerButtonHeight = 64;

        public static int MouseWidth = 64;
        public static int MouseHeight = 64;

        public static int KeyboardKeyWidth = 64;
        public static int KeyboardKeyHeight = 64;


        public UserInterfaceSkin(string name, Texture2D texture)
            : base(name, texture)
        {
            CreateRegion(nameof(UserInterfaceRegions.FrameTopLeft), 0, 0, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameTop), 32, 0, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameTopRight), 64, 0, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameLeft), 0, 32, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameRight), 64, 32, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameBottomLeft), 0, 64, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameBottom), 32, 64, FrameRegionWidth, FrameRegionHeight);
            CreateRegion(nameof(UserInterfaceRegions.FrameBottomRight), 64, 64, FrameRegionWidth, FrameRegionHeight);

            CreateRegion(nameof(UserInterfaceRegions.Cursor), 96, 0, CursorWidth, CursorHeight);
            CreateRegion(nameof(UserInterfaceRegions.AdvanceLeft), 128, 0, AdvanceButtonWidth, AdvanceButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.AdvanceRight), 160, 0, AdvanceButtonWidth, AdvanceButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.AdvanceUp), 192, 0, AdvanceButtonWidth, AdvanceButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.AdvanceDown), 224, 0, AdvanceButtonWidth, AdvanceButtonHeight);

            CreateRegion(nameof(UserInterfaceRegions.XboxAButton), 0, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxBButton), 64, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxXButton), 128, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxYButton), 192, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxLTrigger), 256, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxRTrigger), 320, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxLShoulder), 384, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxRShoulder), 448, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxMenu), 512, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxView), 576, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxDPad), 640, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxDPadDown), 704, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxDPadLeft), 768, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxDPadRight), 832, 128, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.XboxDPadUp), 896, 128, ControllerButtonWidth, ControllerButtonHeight);

            CreateRegion(nameof(UserInterfaceRegions.PlaystationCrossButton), 0, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationCircleButton), 64, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationSquareButton), 128, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationTriangleButton), 192, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationL2), 256, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationR2), 320, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationL1), 384, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationR1), 448, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationOptionsButton), 512, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationTouchpad), 576, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationDPad), 640, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationDPadDown), 704, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationDPadLeft), 768, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationDPadRight), 832, 192, ControllerButtonWidth, ControllerButtonHeight);
            CreateRegion(nameof(UserInterfaceRegions.PlaystationDPadUp), 896, 192, ControllerButtonWidth, ControllerButtonHeight);

            CreateRegion(nameof(UserInterfaceRegions.MouseLeftButton), 0, 256, MouseWidth, MouseHeight);
            CreateRegion(nameof(UserInterfaceRegions.MouseRightButton), 64, 256, MouseWidth, MouseHeight);
            CreateRegion(nameof(UserInterfaceRegions.MouseWheel), 128, 256, MouseWidth, MouseHeight);
            CreateRegion(nameof(UserInterfaceRegions.KeyboardButton), 192, 256, KeyboardKeyWidth, KeyboardKeyHeight);
            CreateRegion(nameof(UserInterfaceRegions.Spacebar), 256, 256, KeyboardKeyWidth * 2, KeyboardKeyHeight);
        }
    }
}