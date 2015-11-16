using Microsoft.Deployment.Compression.Cab;
using System;
using System.IO;

namespace Sensics.CabTools
{
    public sealed class DTFCabFile : ICabFile
    {
        public DTFCabFile(string filename)
        {
            Filename = filename;
            Cab = new CabInfo(filename);
        }

        public TextReader OpenTextFile(string contained)
        {
            var files = Cab.GetFiles(contained);
            if (files.Count != 1)
            {
                throw new FileNotFoundException(String.Format("Couldn't find exactly one file named '{0}' in the cab file '{1}!", contained, Filename));
            }
            return files[0].OpenText();
        }

        private string Filename;
        private CabInfo Cab;
    }
}