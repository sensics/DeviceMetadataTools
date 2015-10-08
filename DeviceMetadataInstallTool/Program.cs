using System;
using System.IO;
using System.Collections;

namespace DeviceMetadataInstallTool
{
    /// <summary>
    /// see https://support.microsoft.com/en-us/kb/303974
    /// </summary>
    class MetadataFinder
    {
        public System.Collections.Generic.List<String> Files = new System.Collections.Generic.List<String>();

        public void SearchDirectory(string dir)
        {
            try
            {
                foreach (var d in Directory.GetDirectories(dir))
                {
                    foreach (var f in Directory.GetFiles(d, "*.devicemetadata-ms"))
                    {
                        Files.Add(f);
                    }
                    SearchDirectory(d);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
    class Program
    {
        static void Main(string[] args)
        {
            var files = new System.Collections.Generic.List<String>();
            //Console.WriteLine("Args size is {0}", args.Length);
            if (args.Length == 0)
            {
                var assemblyDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                var finder = new MetadataFinder();
                finder.SearchDirectory(assemblyDir);
                files = finder.Files;
            } else
            {
                files.AddRange(files);
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
