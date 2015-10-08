using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

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
#if true
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn, cabFactory);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
#else
                using (var cab = new TemporaryCabCopy(fn))
                {
                    var folder = sh.NameSpace(cab.CabPath);
                    var pi = folder.ParseName("PackageInfo.xml");
                    Console.WriteLine("Got item {0}", pi.Path);
                    var reader = new System.IO.StreamReader(pi.Path, System.Text.Encoding.UTF8);
                }
#endif
            }
        }

        private static void Main(string[] args)
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