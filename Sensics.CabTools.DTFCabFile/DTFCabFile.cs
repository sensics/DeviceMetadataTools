#region copyright
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