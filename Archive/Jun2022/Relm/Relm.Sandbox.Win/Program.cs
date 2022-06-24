using System;

namespace Relm.Sandbox.Win
{
    public static class Program
    {
        [STAThread]
        static void Main()
        {
            using (var game = new SandboxGame())
                game.Run();
        }
    }
}
