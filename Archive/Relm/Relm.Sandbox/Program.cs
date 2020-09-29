using System;

namespace Relm.Sandbox
{
#if WINDOWS || LINUX
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new Game()) game.Run();
        }
    }
#endif
}