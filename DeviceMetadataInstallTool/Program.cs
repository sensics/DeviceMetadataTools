using System;
using System.IO;
using System.Collections;

namespace DeviceMetadataInstallTool
{

    class Program
    {
        static void Main(string[] args)
        {
            var files = new System.Collections.Generic.List<String>();
            //Console.WriteLine("Args size is {0}", args.Length);
            if (args.Length == 0)
            {
                var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                files = Sensics.DeviceMetadataInstaller.Util.GetMetadataFilesRecursive(assemblyDir);
            }
            else
            {
                files.AddRange(args);
            }

            var store = new Sensics.DeviceMetadataInstaller.MetadataStore();

            foreach (var fn in files)
            {
                var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn);
                Console.WriteLine("{0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
                store.InstallPackage(pkg);
            }
        }
    }
}
