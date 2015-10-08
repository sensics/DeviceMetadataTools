using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceMetadataInspector
{
    internal class Inspector
    {
        private static void HandleMetadata(IEnumerable<string> files)
        {
            var sh = new Sensics.SystemUtilities.Shell32InstanceWrapper();
            var cabFactory = new Sensics.DeviceMetadataInstaller.Shell32CabFileFactory(sh);
            foreach (var fn in files)
            {
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn, cabFactory);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
            }
        }

        private static void Main(string[] args)
        {
            //Console.WriteLine("Args size is {0}", args.Length);
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