using Sensics.CabTools;
using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceMetadataInstallTool
{
    internal class InstallTool
    {
        private ICabFileFactory cabFactory;
        private Sensics.DeviceMetadataInstaller.MetadataStore store;

        private InstallTool()
        {
            cabFactory = new Sensics.CabTools.Shell32CabFileFactory();
            store = new Sensics.DeviceMetadataInstaller.MetadataStore();
        }

        private void RecurseMetadata(string directory)
        {
            HandleMetadata(Sensics.DeviceMetadataInstaller.Util.GetMetadataFilesRecursive(directory));
        }

        private void HandleMetadata(IEnumerable<string> files)
        {
            foreach (var fn in files)
            {
                HandleMetadata(fn);
            }
        }

        private void HandleMetadata(string fn)
        {
            var pkg = new Sensics.DeviceMetadataInstaller.MetadataPackage(fn, cabFactory);
            Console.WriteLine("- {0} - {1} - Default locale: {2}", pkg.ExperienceGUID, pkg.ModelName, pkg.DefaultLocale);
            store.InstallPackage(pkg);
        }

        private static void Main(string[] args)
        {
            var tool = new InstallTool();
            if (args.Length == 0)
            {
                var dir = Directory.GetCurrentDirectory();
                Console.WriteLine("Processing metadata recursively from current directory: {0}", dir);
                tool.RecurseMetadata(dir);
            }
            else
            {
                Console.WriteLine("Processing metadata and directories passed on the command line");
                foreach (var arg in args)
                {
                    if (Path.GetExtension(arg) == ".devicemetadata-ms")
                    {
                        Console.WriteLine("Processing single file by name: {0}", arg);
                        tool.HandleMetadata(arg);
                    }
                    else if (Directory.Exists(arg))
                    {
                        Console.WriteLine("Processing named directory recursively: {0}", arg);
                        tool.RecurseMetadata(arg);
                    }
                    else
                    {
                        Console.WriteLine("Warning: {0} passed but appears to be neither a metadata file nor a directory. Skipping.", arg);
                    }
                }
            }
        }
    }
}