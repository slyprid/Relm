using System;
using System.IO;

namespace Relm.ContentCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            var src = args[0];
            var dest = args[1];

            try
            {
                Copy(src, dest);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            if (sourceDirectory.Contains("\\bin") || sourceDirectory.Contains("\\obj")) return;

            var diSource = new DirectoryInfo(sourceDirectory);
            var diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            if (target.Exists)
            {
                foreach (var fi in target.GetFiles())
                {
                    if (fi.Name.EndsWith(".mgcb")) continue;
                    fi.Delete();
                }
            }

            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (var fi in source.GetFiles())
            {
                if (fi.Name.EndsWith(".mgcb")) continue;
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (var diSourceSubDir in source.GetDirectories())
            {
                var nextTargetSubDir = target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
