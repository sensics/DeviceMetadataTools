using System;
using System.IO;
using Microsoft.Deployment.Compression.Cab;

namespace Sensics.DeviceMetadataInstaller
{
    internal class CabFile
    {
        public CabFile(string filename)
        {
            Filename = filename;
            Cab = new CabInfo(filename);
        }

        public TextReader OpenTextFile(string contained)
        {
            var files = Cab.GetFiles(contained);
            if (files.Count != 1)
            {
                throw new Exception(String.Format("Couldn't find exactly one file named '{0}' in the cab file '{1}!", contained, Filename));
            }
            return files[0].OpenText();
        }
        private string Filename;
        private CabInfo Cab;
    }
}
