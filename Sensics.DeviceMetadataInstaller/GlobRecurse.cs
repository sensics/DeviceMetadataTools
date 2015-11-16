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

namespace Sensics.DeviceMetadataInstaller
{
    /// <summary>
    /// see https://support.microsoft.com/en-us/kb/303974
    /// </summary>
    internal class GlobRecurse
    {
        public static List<string> SearchDirectory(string dir, string pattern)
        {
            var obj = new GlobRecurse();
            obj.DoSearchDirectory(dir, pattern);
            return obj.Files;
        }

        private List<string> Files = new List<string>();

        private void DoSearchDirectory(string dir, string pattern)
        {
            try
            {
                foreach (var f in Directory.GetFiles(dir, pattern))
                {
                    Files.Add(f);
                }
                foreach (var d in Directory.GetDirectories(dir))
                {
                    DoSearchDirectory(d, pattern);
                }
            }
            catch (System.Exception excpt)
            {
                Console.WriteLine(excpt.Message);
            }
        }
    }
}
