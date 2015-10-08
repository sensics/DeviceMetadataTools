using System;
using System.Runtime.InteropServices;

namespace Sensics.DeviceMetadataInstaller
{
    internal class PathUtilities
    {
        // this marshalling based on http://stackoverflow.com/questions/25049875/getting-any-special-folder-path-in-powershell-using-folder-guid
        [DllImport("shell32.dll")]
        private static extern int SHGetKnownFolderPath(
             [MarshalAs(UnmanagedType.LPStruct)]
                 Guid rfid,
             uint dwFlags,
             IntPtr hToken,
             out IntPtr pszPath
         );

        private static readonly Guid DeviceMetadataStore = new Guid("5CE4A5E9-E4EB-479D-B89F-130C02886155");

        public static string GetKnownFolderPath(Guid rfid)
        {
            IntPtr pszPath;
            if (SHGetKnownFolderPath(rfid, 0, IntPtr.Zero, out pszPath) != 0)
                return ""; // add whatever error handling you fancy
            string path = Marshal.PtrToStringUni(pszPath);
            Marshal.FreeCoTaskMem(pszPath);
            return path;
        }

        /// <summary>
        /// Gets the root of the Device Metadata Store using SHGetKnownFolderPath and the GUID
        /// </summary>
        /// <returns>Path to the DeviceMetadataStore or an empty string if some error occurred</returns>
        internal static string GetDeviceMetadataStore()
        {
            return GetKnownFolderPath(DeviceMetadataStore);
        }
    }
}