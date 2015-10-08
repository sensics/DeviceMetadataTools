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
