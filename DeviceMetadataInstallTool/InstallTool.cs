#region copyright
// Copyright 2015 Sensics, Inc.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
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
            HandleMetadataFiles(Sensics.DeviceMetadataInstaller.Util.GetMetadataFilesRecursive(directory));
        }

        private void HandleMetadataFiles(IEnumerable<string> files)
        {
            foreach (var fn in files)
            {
                HandleMetadataFile(fn);
            }
        }

        private void HandleMetadataFile(string fn)
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
                        tool.HandleMetadataFile(arg);
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