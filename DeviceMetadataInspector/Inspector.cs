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

using System;
using System.Collections.Generic;
using System.IO;

namespace DeviceMetadataInspector
{
    internal class Inspector
    {
        private static void HandleMetadata(IEnumerable<string> files)
        {
            var cabFactory = new Sensics.CabTools.Shell32CabFileFactory();
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