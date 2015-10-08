using System.Xml;

namespace Sensics.DeviceMetadataInstaller
{
    internal class XPathDoc
    {
        public XPathDoc(System.IO.TextReader stream, string prefix, string namespaceURI)
        {
            Document = new XmlDocument();
            Document.Load(stream);

            NamespaceManager = new XmlNamespaceManager(Document.NameTable);
            NamespaceManager.AddNamespace(prefix, namespaceURI);
        }

        public XmlNode SelectSingleNode(string xpath)
        {
            return Document.SelectSingleNode(xpath, NamespaceManager);
        }

        public XmlNodeList SelectNodes(string xpath)
        {
            return Document.SelectNodes(xpath, NamespaceManager);
        }

        public XmlDocument Document;
        public XmlNamespaceManager NamespaceManager;
    }
}
