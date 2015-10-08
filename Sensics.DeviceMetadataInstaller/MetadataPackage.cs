using System;
using System.IO;

namespace Sensics.DeviceMetadataInstaller
{
    

    public class MetadataPackage
    {
        private const string DeviceInfoNamespace = "http://schemas.microsoft.com/windows/DeviceMetadata/DeviceInfo/2007/11/";

        public string FullPath
        {
            get
            {
                return Path.GetFullPath(_filename);
            }
        }

        public string FileName
        {
            get
            {
                return Path.GetFileName(_filename);
            }
        }
        private string _filename;
        public MetadataPackage(string filename)
        {
            var cabFileFactory = new Shell32CabFileFactory();
            _filename = filename;
            Cab = cabFileFactory.OpenCab(filename);
        }
        public MetadataPackage(string filename, ICabFileFactory cabFileFactory)
        {
            _filename = filename;
            Cab = cabFileFactory.OpenCab(filename);
        }

        public string DefaultLocale
        {
            get
            {
                var node = PackageInfo.SelectSingleNode("descendant::pi:Locale[@default='true']");
                return node.InnerText;
            }
        }

        private XPathDoc OpenXMLFromCab(string filename, string prefix, string namespaceURI)
        {
            return new XPathDoc(Cab.OpenTextFile(filename), prefix, namespaceURI);
        }

        private ICabFile Cab;

        private XPathDoc PackageInfo
        {
            get
            {
                if (_PackageInfo == null)
                {
                    _PackageInfo = OpenXMLFromCab("PackageInfo.xml", "pi", "http://schemas.microsoft.com/windows/DeviceMetadata/PackageInfo/2007/11/");
                }
                return _PackageInfo;
            }
        }
        private XPathDoc _PackageInfo;

        public string ExperienceGUID
        {
            get
            {
                return PackageInfo.SelectSingleNode("descendant::pi:ExperienceID").InnerText;
            }
        }

        private string DeviceInfoDirectory
        {
            get
            {
                return PackageInfo.SelectSingleNode(String.Format("descendant::pi:Metadata[@MetadataID='{0}']", DeviceInfoNamespace)).InnerText;
            }
        }

        private XPathDoc DeviceInfo
        {
            get
            {
                if (_DeviceInfo == null)
                {
                    _DeviceInfo = OpenXMLFromCab(String.Format("{0}\\{1}\\DeviceInfo.xml", DeviceInfoDirectory, DefaultLocale), "di", DeviceInfoNamespace);
                }
                return _DeviceInfo;
            }
        }
        private XPathDoc _DeviceInfo;

        public string GetDefaultDeviceInfoString(string element)
        {
            return DeviceInfo.SelectSingleNode(String.Format("descendant::di:{0}", element)).InnerText;
        }

        public string ModelName
        {
            get
            {
                return GetDefaultDeviceInfoString("ModelName");
            }
        }
    }
}