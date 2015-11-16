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
using System.IO;

namespace Sensics.DeviceMetadataInstaller
{
    public class MetadataStore
    {
        String DeviceMetadataStorePath { get; set; }
        public MetadataStore()
        {
            DeviceMetadataStorePath = PathUtilities.GetDeviceMetadataStore();
        }

        public void InstallPackage(MetadataPackage pkg)
        {
            var locale = pkg.DefaultLocale; /// @todo is this actually how to choose the location?
            var localeDir = Path.Combine(DeviceMetadataStorePath, locale);
            Directory.CreateDirectory(localeDir);
            File.Copy(pkg.FullPath, Path.Combine(localeDir, pkg.FileName), true); // allow overwrite
        }
    }

}
