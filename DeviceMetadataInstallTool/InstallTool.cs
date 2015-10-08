using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

namespace DeviceMetadataInstallTool
{

    class InstallTool
    {
        static void HandleMetadata(IEnumerable<string> files)
        {
            var store = new Sensics.DeviceMetadataInstaller.MetadataStore();
            foreach (var fn in files)
            {
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
                store.InstallPackage(pkg);
            }
        }

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                var assemblyDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                HandleMetadata(Sensics.DeviceMetadataInstaller.Util.GetMetadataFilesRecursive(assemblyDir));
            }
            else
            {
                HandleMetadata(args);
            }

        }
    }
}
