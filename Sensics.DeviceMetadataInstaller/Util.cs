using System.Collections.Generic;

namespace Sensics.DeviceMetadataInstaller
{

    public class Util
    {
        public static List<string> GetMetadataFilesRecursive(string dir)
        {
            return GlobRecurse.SearchDirectory(dir, "*.devicemetadata-ms");
        }
    }
}
