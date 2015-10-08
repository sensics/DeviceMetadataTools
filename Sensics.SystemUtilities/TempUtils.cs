using System;
using System.IO;

namespace Sensics.SystemUtilities
{
    internal static class TempUtils
    {
        public static string GetFullUniquePath(string extension = ".tmp")
        {
            return GetFullPathToTempFile(Guid.NewGuid().ToString() + extension);
        }

        public static string GetFullPathToTempFile(string filename)
        {
            return Path.Combine(Path.GetTempPath(), filename);
        }
    }
}
