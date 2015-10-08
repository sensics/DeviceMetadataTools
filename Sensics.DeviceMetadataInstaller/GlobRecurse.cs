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
                foreach (var d in Directory.GetDirectories(dir))
                {
                    foreach (var f in Directory.GetFiles(d, pattern))
                    {
                        Files.Add(f);
                    }
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
