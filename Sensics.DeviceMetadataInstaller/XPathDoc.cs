﻿#region copyright
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
