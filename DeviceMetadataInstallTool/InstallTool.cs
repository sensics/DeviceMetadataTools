using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceMetadataInstallTool
{
    internal class InstallTool
    {
        private static void HandleMetadata(IEnumerable<string> files)
        {
            var cabFactory = new Sensics.CabTools.Shell32CabFileFactory();
            var store = new Sensics.DeviceMetadataInstaller.MetadataStore();
            foreach (var fn in files)
            {
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn, cabFactory);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
                store.InstallPackage(pkg);
            }
        }

        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var dir = Directory.GetCurrentDirectory();
                HandleMetadata(Sensics.DeviceMetadataInstaller.Util.GetMetadataFilesRecursive(dir));
            }
            else
            {
                HandleMetadata(args);
            }
        }
    }
}