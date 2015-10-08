using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DeviceMetadataInspector
{
    class Program
    {
        static void HandleMetadata(IEnumerable<string> files)
        {
            foreach (var fn in files)
            {
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
            }
        }
        static void Main(string[] args)
        {
            //Console.WriteLine("Args size is {0}", args.Length);
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
